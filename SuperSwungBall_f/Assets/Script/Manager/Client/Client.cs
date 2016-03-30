using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

using System.Threading;
using System.Text;


public class Client {

	private const string HOST = "127.0.0.1";
	private const int PORT = 9734;
	private const int BUFFER_SIZE = 1024;
	private const string CLIENT_ACTION_AUTHENTICATE = "connect";
	private const string CLIENT_ACTION_DISCONNECT = "disconnect";

	private Socket _sock;
	private byte[] _buffer = new byte[BUFFER_SIZE];

	private readonly List<ClientListener> listeners = null;

	private bool isAuthenticate = false;
	private string username;
	private int id;

	public Client(ClientListener listener){
		this.listeners = new List<ClientListener> ();
		this.listeners.Add(listener);
		this.username = "hugo_082";
		this.id = 1;
	}

	/// <summary>
	/// Connexion au serveur
	/// </summary>
	/// <param name="host">Localisation du serveur</param>
	/// <param name="port">Id of friend</param>
	/// <param name="authenticate">Authentification automatique</param>
	public void Connect(string host = HOST, int port = PORT, bool authenticate = true){
		this._sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		try
		{
			_sock.Connect( host, port);
			_sock.BeginReceive(_buffer,0, BUFFER_SIZE,SocketFlags.None,new AsyncCallback(OnReceive),null);

			if(authenticate){
				this.Authenticate();
			}
		}
		catch (Exception e)
		{
			Debug.Log("Erreur de connexion : " + e.Message);
		}
	}

	/// <summary>
	/// Envoi d'un message
	/// </summary>
	/// <param name="message">Message a envoyer</param>
	public void Send(string message){
		byte[] messageData = System.Text.Encoding.UTF8.GetBytes(message);
		_sock.Send(messageData);
	}

	/// <summary>
	/// Authentification sur le serveur
	/// </summary>
	public void Authenticate(){
		if (this.isAuthenticate)
			return;
		string action = CLIENT_ACTION_AUTHENTICATE;

		string value = action + "~" + this.username + "~" + this.id;
		this.Send (value);
	}

	/// <summary>
	/// Deconnexion sur le serveur
	/// </summary>
	public void Disconnect(){
		string action = CLIENT_ACTION_DISCONNECT;

		string value = action + "~" + this.username + "~" + this.id;
		this.Send (value);
	}

	/// <summary>
	/// Provoque la deconnexion sur le serveur si nécéssaire et ferme le socket
	/// </summary>
	public void Quit(){
		string debug = "";
		if (this.isAuthenticate) {
			this.Disconnect ();
			debug += "Client is now Disconnected - ";
		}
		_sock.Close ();
		Debug.Log ( debug + "Socket is now closed");
	}

	/// <summary>
	/// A la reception d'un message
	/// Call automatique
	/// </summary>
	private void OnReceive(IAsyncResult ar){
		if (!ar.IsCompleted || !_sock.Connected)
			return;
		_sock.EndReceive(ar);

		string message = System.Text.Encoding.UTF8.GetString(_buffer);
		string[] parameters = message.Split('~');

		switch (parameters[0]) {
		case "friendConnected":
			foreach (var listener in this.listeners) {
				listener.OnFriendConnected (parameters [1], int.Parse(parameters [2]));
			}
			break;
		case "friendDisconnected":
			foreach (var listener in this.listeners) {
				listener.OnFriendDisconnected (parameters [1], int.Parse(parameters [2]));
			}
			break;
		case "Connected":
			this.isAuthenticate = true;
			foreach (var listener in this.listeners) {
				listener.OnAuthenticated ();
			}
			break;
		case "Rejected":
			this.isAuthenticate = false;
			foreach (var listener in this.listeners) {
				listener.OnRejected();
			}
			break;
		case "Disconnected":
			this.isAuthenticate = false;
			_sock.Close ();
			foreach (var listener in this.listeners) {
				listener.OnDisconnected ();
			}
			break;

		default:
			foreach (var listener in this.listeners) {
				listener.OnReceiveMessage (parameters [0]);
			}
			break;
		}

		_buffer = new byte[BUFFER_SIZE];
		_sock.BeginReceive(_buffer,0, BUFFER_SIZE,SocketFlags.None,new AsyncCallback(OnReceive),null);

	}

	public void AddListener(ClientListener listener){
		this.listeners.Add (listener);
	}
	public void RemoveListener(ClientListener listener){
		this.listeners.Remove (listener);
	}

	public bool IsAuthticate {
		get { return isAuthenticate; }
	}
	public string Host {
		get {
			if (_sock != null && _sock.Connected) {
				IPAddress ipAddress = ((IPEndPoint)_sock.RemoteEndPoint).Address;
				IPHostEntry hostEntry = Dns.GetHostEntry (ipAddress);
				return hostEntry.HostName;
			} else {
				return "NotConnected";
			}
		}
	}
}
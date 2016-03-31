using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

using System.Text;
using System.Threading;


public class Client {

	private const string HOST = "127.0.0.1";
	private const int PORT = 9734;
	private int BUFFERSIZE = 1024;
	private const int BUFFER_SIZE = 1024;
	private const string CLIENT_ACTION_AUTHENTICATE = "connect";
	private const string CLIENT_ACTION_DISCONNECT = "disconnect";

	private Socket _sock;
	private byte[] _buffer;
	private SocketState state;
	private readonly object thread_syncer = new object();

	private Thread run; // Thread du Receive

	private readonly List<IClientListener> listeners = null;

	// -- Identifiant d'Authentification
	private string username;
	private int id;
	// --

	public Client(IClientListener listener){
		this.listeners = new List<IClientListener> ();
		this.listeners.Add(listener);
		this.username = "hugo_082";
		this.id = 1;
		this.state = SocketState.DISCONNECTED;
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
			this.state = SocketState.CONNECTED;

			run = new Thread(new ThreadStart(ReceiveLoop));
			run.Name = "ReceiveThread";
			run.IsBackground = true;
			run.Start();

			if(authenticate)
				this.Authenticate();
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
		Debug.Log ("Sended : " + message);
	}

	/// <summary>
	/// Authentification sur le serveur
	/// </summary>
	public void Authenticate(){
		if (this.state == SocketState.AUTHENTICATED)
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
	/// Disconnects the socket.
	/// </summary>
	public void DisconnectSocket()
	{
		lock (this.thread_syncer)
		{
			if (this._sock != null)
			{
				try
				{
					this._sock.Close();
				}
				catch (Exception ex)
				{
					Debug.LogError ("Error on Close scoket : " + ex);
				}
				this._sock = null;
			}
		}
		this.state = SocketState.DISCONNECTED;
	}

	/// <summary>
	/// Provoque la deconnexion sur le serveur si nécéssaire et ferme le socket
	/// </summary>
	public void Quit(){
		string debug = "";
		if (this.state == SocketState.AUTHENTICATED) {
			this.Disconnect ();
			debug += "Client is now Disconnected - ";
		}
		this.DisconnectSocket ();
		if (run != null ) run.Abort ();
		this.state = SocketState.DISCONNECTED;
		Debug.Log ( debug + "Socket is now closed - Thread is now aborted");
	}

	/// <summary>Endless loop, run in Receive Thread.</summary>
	public void ReceiveLoop()
	{
		byte[] inBuffer = new byte[this.BUFFERSIZE];
		while (this.state != SocketState.DISCONNECTED)
		{
			try
			{
				this._sock.Receive(inBuffer);
				lock (this.thread_syncer)
				{
					_buffer = inBuffer;
				}
			}
			catch (Exception e)
			{
				Debug.LogError ("Socket Receive : " + e);
			}
		}
		this.DisconnectSocket();
	}

	/// <summary>
	/// A la reception d'un message, Call automatique
	/// </summary>
	private void OnReceive(byte[] buffer){
		string message = System.Text.Encoding.UTF8.GetString(buffer);
		string[] parameters = message.Split('~');

		Debug.Log ("OnReceive : " + message);

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
			foreach (var listener in this.listeners) {
				listener.OnAuthenticated ();
			}
			this.state = SocketState.AUTHENTICATED;
			break;
		case "Rejected":
			foreach (var listener in this.listeners) {
				listener.OnRejected();
			}
			this.state = SocketState.CONNECTED;
			break;
		case "Disconnected":
			this.state = SocketState.CONNECTED;
			this.DisconnectSocket ();
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
	}

	/// <summary>
	/// Call régulièrement pour recevoir lire le buffer
	/// </summary>
	public void Service(){
		if (this.state == SocketState.DISCONNECTED)
			return;
		byte[] buff = null;
		lock (this.thread_syncer)
		{
			buff = _buffer;
			if (buff != null) {
				this.OnReceive (buff);
				_buffer = null;
			}
		}
	}


	public void AddListener(IClientListener listener){
		this.listeners.Add (listener);
	}
	public void RemoveListener(IClientListener listener){
		this.listeners.Remove (listener);
	}

	public bool IsAuthticate {
		get { return this.state == SocketState.AUTHENTICATED; }
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

	public enum SocketState {
		CONNECTED,			// Socket connected
		DISCONNECTED,		// Socket disconnected
		AUTHENTICATED		// Socket connected and User autheticated
	}
}
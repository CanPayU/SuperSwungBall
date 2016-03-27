using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

using System.Threading;
using System.Text;


public class Client {

	private const int BUFFER_SIZE = 1024;
	private const string CLIENT_ACTION_AUTHENTICATE = "connect";
	private const string CLIENT_ACTION_DISCONNECT = "disconnect";

	private Socket _sock;
	private byte[] _buffer = new byte[BUFFER_SIZE];

	private readonly ClientListener listener = null;

	private bool isAuthenticate = false;
	private string username;
	private int id;

	public Client(ClientListener listener){
		this.listener = listener;
		this.username = "hugo_082";
		this.id = 1;
	}

	public void Connect(string host = "127.0.0.1", int port = 9734, bool authenticate = true){
		this._sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		try
		{
			_sock.Connect( "127.0.0.1", port);
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

	public void Send(string message){
		byte[] messageData = System.Text.Encoding.UTF8.GetBytes(message);
		_sock.Send(messageData);
	}

	public void Authenticate(){
		if (this.isAuthenticate)
			return;
		string action = CLIENT_ACTION_AUTHENTICATE;

		string value = action + "~" + this.username + "~" + this.id;
		this.Send (value);
	}

	public void Disconnect(){
		string action = CLIENT_ACTION_DISCONNECT;

		string value = action + "~" + this.username + "~" + this.id;
		this.Send (value);
	}

	public void Quit(){
		string debug = "";
		if (this.isAuthenticate) {
			this.Disconnect ();
			debug += "Client is now Disconnected - ";
		}
		_sock.Close ();
		Debug.Log ( debug + "Socket is now closed");
	}

	private void OnReceive(IAsyncResult ar){
		if (!ar.IsCompleted || !_sock.Connected)
			return;
		_sock.EndReceive(ar);

		string message = System.Text.Encoding.UTF8.GetString(_buffer);
		string[] parameters = message.Split('~');

		switch (parameters[0]) {
		case "friendConnected":
			this.listener.OnFriendConnected (parameters [1], int.Parse(parameters [2]));
			break;
		case "friendDisconnected":
			this.listener.OnFriendDisconnected (parameters [1], int.Parse (parameters [2]));
			break;
		case "Connected":
			this.isAuthenticate = true;
			this.listener.OnAuthenticated ();
			break;
		case "Rejected":
			this.isAuthenticate = false;
			this.listener.OnAuthenticated ();
			break;
		case "Disconnected":
			this.isAuthenticate = false;
			_sock.Close ();
			this.listener.OnDisconnected ();
			break;

		default:
			this.listener.OnReceiveMessage (parameters [0]);
			break;
		}

		_buffer = new byte[BUFFER_SIZE];
		_sock.BeginReceive(_buffer,0, BUFFER_SIZE,SocketFlags.None,new AsyncCallback(OnReceive),null);

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
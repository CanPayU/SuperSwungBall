using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ClientManager : MonoBehaviour, IClientListener {

	private Client client;
	private rslideController SlideController;

	public Client Client {
		get { return client; }
	}

	void Start () {
		this.client = new Client (this);
		this.client.Connect (); // Authenticate automatique


		if(Input.GetKeyDown(KeyCode.A)){
			client.Authenticate ();
		}
		if(Input.GetKeyDown(KeyCode.D)){
			client.Disconnect ();
		}
		if(Input.GetKeyDown(KeyCode.C)){
			client.Connect ();
		}
		if(Input.GetKeyDown(KeyCode.Z)){
			client.Send ("Bonjour la france !");
		}
	}

	// Update is called once per frame
	void Update () {
		this.client.Service ();
	}

	// -- Pour + d'info voir ClientListener.cs
	public void OnFriendDisconnected(string username, int id){
		Notification.Create (NotificationType.Box, "Ami déconnecte", content: username + " est maintenant déconnecté");
		Debug.Log ("Friend " + username + " - " + id + " is now Disconnected");
		User.Instance.Friends.IsOnline (username, false);
	}
	public void OnFriendConnected(string username, int id){
		Notification.Create (NotificationType.Box, "Ami connecte", content: username + " est maintenant connecté");
		Debug.Log ("Friend " + username + " - " + id + " is now Connected");
		User.Instance.Friends.IsOnline (username);
	}
	public void OnReceiveMessage(string message){
		Debug.Log ("Receive undefined method : " + message);
	}
	public void OnAuthenticated(){
		Notification.Create (NotificationType.Box, "Authentifie", content: "Vous êtes maintenant authentifié en tant que " + this.client.Username);
	}
	public void OnRejected(){
		Notification.Create (NotificationType.Box, "Erreur d'Authentification", content: "Impossible de vous authentifier en tant que " + this.client.Username);
		Debug.Log ("Authetification Rejected");
	}
	public void OnDisconnected(){
		Notification.Create (NotificationType.Box, "Déconnecté", content: "Vous êtes maintenant déconnecté de notre serveur");
	}

	void OnApplicationQuit() {
		client.Quit ();
	}
}

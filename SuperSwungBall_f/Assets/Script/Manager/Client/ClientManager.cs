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
	}

	// Update is called once per frame
	void Update () {
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

	// -- Pour + d'info voir ClientListener.cs
	public void OnFriendDisconnected(string username, int id){
		Debug.Log ("Friend " + username + " - " + id + " is now Disconnected");
		User.Instance.Friends.IsOnline (username, false);
	}
	public void OnFriendConnected(string username, int id){
		Debug.Log ("Friend " + username + " - " + id + " is now Connected");
		User.Instance.Friends.IsOnline (username);
	}
	public void OnReceiveMessage(string message){
		Debug.Log ("Receive undefined method : " + message);
	}
	public void OnAuthenticated(){
		Debug.Log ("Now authenticated");
	}
	public void OnRejected(){
		Debug.Log ("Authetification Rejected");
	}
	public void OnDisconnected(){
		Debug.Log ("Disconnected");
	}

	void OnApplicationQuit() {
		client.Quit ();
	}
}

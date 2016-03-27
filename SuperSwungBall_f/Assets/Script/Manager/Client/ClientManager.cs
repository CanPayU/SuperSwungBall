using UnityEngine;
using System.Collections;

public class ClientManager : MonoBehaviour, ClientListener {

	private Client client;


	public Client Client {
		get { return client; }
	}

	void Start () {
		client = new Client (this);
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

	public void OnFriendDisconnected(string username, int id){
		Debug.Log ("In Socket Controller : Friend " + username + " - " + id + " is now Disconnected");
	}
	public void OnFriendConnected(string username, int id){
		Debug.Log ("In Socket Controller : Friend " + username + " - " + id + " is now Connected");
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

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkController : MonoBehaviour {


	private string game_version_ = "0.1";
	private bool room_joined = false;
	private string room_name;

	[SerializeField]
	private Text info_network;
	[SerializeField]
	private Text info_users;
	[SerializeField]
	private string scene;

	private System.Random rand = new System.Random();

	void Start () {
		PhotonNetwork.playerName = "Guest-" + rand.Next(100);
		PhotonNetwork.ConnectUsingSettings (game_version_);
		room_name = "myusername-" + rand.Next(1000);
	}

	void OnConnectedToMaster(){
		PhotonNetwork.JoinRandomRoom(); // création de la room
	}
	void OnPhotonRandomJoinFailed(){
		RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 2 }; // isVisible Random can join or not (ici oui)
		PhotonNetwork.JoinOrCreateRoom(room_name, roomOptions, TypedLobby.Default);
	}
	void OnJoinedRoom(){
		room_joined = true;
		if (PhotonNetwork.playerList.Length > 1) {
			StartCoroutine(ChangeLevel());
		}
	}
	void OnPhotonPlayerConnected(PhotonPlayer other )
	{
		if (PhotonNetwork.playerList.Length > 1) {
			StartCoroutine(ChangeLevel());
		}

	}

	void OnGUI(){

		string info = PhotonNetwork.connectionStateDetailed.ToString ();
		if (room_joined)
			info += " - " + room_name;
		info_network.text = info;

		info = "";
		if (room_joined) {
			info = "Joueur trouve : " + (PhotonNetwork.room.playerCount - 1) + "\n";
			if (PhotonNetwork.playerList.Length > 1) {
				info += PhotonNetwork.playerList[0].name + " VS " + PhotonNetwork.playerList[1].name;
			}
		}
		info_users.text = info;
	}

	IEnumerator ChangeLevel()
	{
		float fadeTime = GameObject.Find("GM_Fade").GetComponent<Fading>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(scene);
	}

}

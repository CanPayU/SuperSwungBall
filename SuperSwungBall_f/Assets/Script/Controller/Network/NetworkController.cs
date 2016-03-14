using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Network {
	public class NetworkController : MonoBehaviour {

		private string game_version_ = "0.7";
		private bool room_joined = false;
		private string room_name;
		private User user;

		[SerializeField]
		private Text info_network;
		[SerializeField]
		private Text info_users;
		[SerializeField]
		private string scene;

		private System.Random rand = new System.Random();

		void Start () {
			user = User.Instance;
			if (user.is_connected) {
				PhotonNetwork.playerName = user.username;
				PhotonNetwork.ConnectUsingSettings (game_version_);
				room_name = user.username + "-" + rand.Next (1000);
			}
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
				FadingManager.I.Fade (scene);
			}
		}
		void OnPhotonPlayerConnected(PhotonPlayer other )
		{
			if (PhotonNetwork.playerList.Length > 1) {
				FadingManager.I.Fade (scene);
			}

		}

		void OnGUI(){

			string info = PhotonNetwork.connectionStateDetailed.ToString ();
			if (room_joined)
				info += " - " + room_name;
			info_network.text = info;

			info = "Connexion";
			if (room_joined) {
				info = "Joueur trouve : " + (PhotonNetwork.room.playerCount - 1) + "\n";
				if (PhotonNetwork.playerList.Length > 1) {
					info += PhotonNetwork.playerList[0].name + " VS " + PhotonNetwork.playerList[1].name;
				}
			}
			info_users.text = info;
		}
		/*
		IEnumerator ChangeLevel()
		{
			float fadeTime = GameObject.Find("GM_Fade").GetComponent<Fading>().BeginFade(1);
			yield return new WaitForSeconds(fadeTime);
			SceneManager.LoadScene(scene);
		}
*/
	}
}
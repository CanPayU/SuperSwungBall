using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PrivateGameController : MonoBehaviour
{
    private string game_version_ = "0.7";
    private bool join = false;
    private string room_name;
    private User user;

    [SerializeField]
    private InputField room_name_field; // join
    [SerializeField]
    private Text room_name_text; // create
    [SerializeField]
    private Text info_network_text; // create
    [SerializeField]
    private string scene;

    private System.Random rand = new System.Random();

    void Start()
    {
        room_name_text.text = "Room name : _";
        user = User.Instance;
        if (user.is_connected)
        {
            PhotonNetwork.playerName = user.username + "-" + rand.Next(100);
            PhotonNetwork.ConnectUsingSettings(game_version_);
            room_name = (rand.Next(1000, 9999)).ToString();
        }
    }

    void OnConnectedToMaster()
    {
        if (!join)
        {
            RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 2 }; // isVisible Random can join or not (ici non)
            PhotonNetwork.JoinOrCreateRoom(room_name, roomOptions, TypedLobby.Default);
        }
        else
            PhotonNetwork.JoinRoom(room_name_field.text);
    }
    void OnPhotonJoinRoomFailed()
    {
        Notification.Create(NotificationType.Slide, title: "Room introuvable - " + room_name_field.text);
    }
    void OnJoinedRoom()
    {
        if (PhotonNetwork.playerList.Length > 1)
        {
            FadingManager.I.Fade(scene);
        }
    }
    void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        if (PhotonNetwork.playerList.Length > 1)
        {
            FadingManager.I.Fade(scene);
        }

    }

    void OnGUI()
    {
        string info = PhotonNetwork.connectionStateDetailed.ToString();

        if (PhotonNetwork.inRoom)
        {
            info = "Joueur trouve : " + (PhotonNetwork.room.playerCount - 1);
            room_name_text.text = "Room name : " + room_name;
        }
        else
        {
            room_name_text.text = "Room name : _";
        }
        info_network_text.text = info;
    }
    /*
	IEnumerator ChangeLevel()
	{
		float fadeTime = GameObject.Find("GM_Fade").GetComponent<Fading>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(scene);
	}
*/
    public void join_game()
    {
        Debug.Log("On player want join game");
        PhotonNetwork.LeaveRoom();
        join = true;
    }
}

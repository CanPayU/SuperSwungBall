using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ClientManager : MonoBehaviour, IClientListener
{

    private Client client;
    private RslideController SlideController;

    public Client Client
    {
        get { return client; }
    }

    void Start()
    {
        this.client = new Client(this);
        this.client.Connect(); // Authenticate automatique


        if (Input.GetKeyDown(KeyCode.A))
        {
            client.Authenticate();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            client.Disconnect();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            client.Connect();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            client.Send("Bonjour la france !");
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.client.Service();
    }

    // -- Pour + d'info voir ClientListener.cs
    public void OnFriendDisconnected(string username, int id)
    {
        Notification.Create(NotificationType.Box, "Ami déconnecte", content: username + " est maintenant déconnecté");
        Debug.Log("Friend " + username + " - " + id + " is now Disconnected");
        User.Instance.Friends.IsOnline(username, false);
    }
    public void OnFriendConnected(string username, int id)
    {
        Notification.Create(NotificationType.Box, "Ami connecte", content: username + " est maintenant connecté");
        Debug.Log("Friend " + username + " - " + id + " is now Connected");
        User.Instance.Friends.IsOnline(username);
    }
    public void OnFriendJoinRoom(string username, int id, string roomID)
    {

        roomID = (roomID == "" || roomID == "null" || roomID == null) ? null : roomID;

        Debug.Log("MyFriend " + username + "have joined a room :" + roomID);
        User.Instance.Friends.SetRoom(username, roomID);
    }
    public void OnReceiveInvitation(string username, int id, string roomID)
    {
		Notification.Alert( "Invitation recus", "Tu as recus une invitation à jouer de " + username, (success) =>
        {
			if (success)
            {
                PlayerPrefs.SetInt("Net_State", 2);
                PlayerPrefs.SetString("Net_RoomID", roomID);
				FadingManager.Instance.Fade("network");
            }
            else
            {
                Debug.Log("Encore un homme sans couille");
                // Retouner InviteRejected ? 
            }
        });
    }
    public void OnReceiveMessage(string message)
    {
        Debug.Log("Receive undefined method : " + message);
    }
    public void OnAuthenticated(string[] connected_user)
    {
        Notification.Create(NotificationType.Box, "Authentifie", content: "Vous êtes maintenant authentifié en tant que " + this.client.Username);
        Friends friends = User.Instance.Friends;
        int len = connected_user.Length;
        for (int i = 1; i < len - 1; i++)
        { // len - 1 pour eviter le "~end"
            string userData = connected_user[i];
            string[] data = userData.Split('∏');

            User friend = friends.Get(data[0]); // username
            friend.is_connected = true;
            string room = data[2];
            if (room != "null")
                friend.room = room;
        }
    }
    public void OnRejected()
    {
        Notification.Create(NotificationType.Box, "Erreur d'Authentification", content: "Impossible de vous authentifier en tant que " + this.client.Username);
        Debug.Log("Authetification Rejected");
    }
    public void OnDisconnected()
    {
        Notification.Create(NotificationType.Box, "Déconnecté", content: "Vous êtes maintenant déconnecté de notre serveur");
    }

    void OnApplicationQuit()
    {
        client.Quit();
    }
}

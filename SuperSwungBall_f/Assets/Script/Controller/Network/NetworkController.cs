﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PhotonHastable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;

namespace Network
{
    public class NetworkController : MonoBehaviour
    {
        private string game_version_ = "2.0";
        private string room_name;
        private User user;

        [SerializeField]
        private Text info_network;
        [SerializeField]
        private Text info_users;
        [SerializeField]
        private string scene;

        private System.Random rand = new System.Random();

        private Client client; // Socket

        void Start()
        {
            this.client = GameObject.Find("Manager").GetComponent<ClientManager>().Client;
            user = User.Instance;
            if (user.is_connected)
            {
                PhotonNetwork.playerName = user.username;

                PhotonPeer.RegisterType(typeof(Team), (byte)'T', ObjectToByteArray, ByteToTeam);
                PhotonPeer.RegisterType(typeof(Composition), (byte)'C', ObjectToByteArray, ByteToComposition);
                PhotonPeer.RegisterType(typeof(Player), (byte)'P', ObjectToByteArray, ByteToPlayer);
                PhotonPeer.RegisterType(typeof(User), (byte)'U', ObjectToByteArray, ByteToUser);
                PhotonPeer.RegisterType(typeof(List<string>), (byte)'L', ObjectToByteArray, ByteToLS);

                PhotonHastable props = new PhotonHastable();

                props.Add("Team", Settings.Instance.Selected_Team);
                props.Add("User", user);

                PhotonNetwork.player.SetCustomProperties(props);

                PhotonNetwork.ConnectUsingSettings(game_version_);
                this.room_name = user.username + "-" + rand.Next(1000);
            }
        }

        void OnConnectedToMaster()
        {
            int state = PlayerPrefs.GetInt("Net_State", -1); // -1 => Default | 1 => InviteFirend | 2 => InvitedByFriend

            if (state == -1)
            {
                Debug.Log("JoinRandomRoom");
                PhotonNetwork.JoinRandomRoom(); // création de la room
                return;
            }
            Debug.Log("JoinNanRandomRoom : " + state);
            string RoomID = PlayerPrefs.GetString("Net_RoomID", "NULL");
            if (RoomID != "NULL")
                this.room_name = RoomID;
            else
            {
                Notification.Create(NotificationType.Box, "Erreur lors de l'invitation", content: "Une erreur est survenue lors de la création de la room.");
                return;
            }

            if (state == 1)
            {
                RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 2 }; // isVisible Random can join or not (ici non)
                PhotonNetwork.JoinOrCreateRoom(this.room_name, roomOptions, TypedLobby.Default);
            }
            if (state == 2)
            {
                PhotonNetwork.JoinRoom(this.room_name);
            }
            PlayerPrefs.DeleteAll();
            state = PlayerPrefs.GetInt("Net_State", -1);
            Debug.Log("JoinNanRandomRoom : " + state);

        }
        void OnPhotonJoinRoomFailed()
        {
            string roomName = PlayerPrefs.GetString("", "NULL");
            Notification.Create(NotificationType.Slide, title: "Room introuvable : " + roomName);
        }
        void OnPhotonRandomJoinFailed()
        {
            RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 2 }; // isVisible Random can join or not (ici oui)
            PhotonNetwork.JoinOrCreateRoom(room_name, roomOptions, TypedLobby.Default);
        }
        void OnJoinedRoom()
        {
            this.client.JoinRoom(this.room_name);
            if (PhotonNetwork.playerList.Length > 1)
            {
				FadingManager.Instance.Fade(scene);
            }
        }
        void OnPhotonPlayerConnected(PhotonPlayer other)
        {
            if (PhotonNetwork.playerList.Length > 1)
            {
				FadingManager.Instance.Fade(scene);
            }
        }

        void OnGUI()
        {
            string info = PhotonNetwork.connectionStateDetailed.ToString();
			if (PhotonNetwork.inRoom)
                info += " - " + room_name;
            info_network.text = info;

            info = "Connexion";
			if (PhotonNetwork.inRoom)
            {
                info = "Joueur trouve : " + (PhotonNetwork.room.playerCount - 1) + "\n";
                if (PhotonNetwork.playerList.Length > 1)
                {
                    info += PhotonNetwork.playerList[0].name + " VS " + PhotonNetwork.playerList[1].name;
                }
            }
            info_users.text = info;
        }

        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        private Team ByteToTeam(byte[] arrBytes)
        {

            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Team obj = (Team)binForm.Deserialize(memStream);

            return obj;
        }
        private Composition ByteToComposition(byte[] arrBytes)
        {

            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Composition obj = (Composition)binForm.Deserialize(memStream);

            return obj;
        }
        private Player ByteToPlayer(byte[] arrBytes)
        {

            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Player obj = (Player)binForm.Deserialize(memStream);

            return obj;
        }

        private User ByteToUser(byte[] arrBytes)
        {

            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            User obj = (User)binForm.Deserialize(memStream);

            return obj;
        }
        private List<string> ByteToLS(byte[] arrBytes)
        {

            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            List<string> obj = (List<string>)binForm.Deserialize(memStream);

            return obj;
        }
    }
}
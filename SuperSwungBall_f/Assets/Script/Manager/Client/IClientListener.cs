using UnityEngine;
using System.Collections;

public interface IClientListener {

	/// <summary>
	/// Confirmation de l'authentification par le serveur
	/// </summary>
	/// <param name="connectedUser">Information sur tous les amis connecté actuellement. <value>[0]</value> => Nom de l'action</param>
	void OnAuthenticated(string[] connectedUser);

	/// <summary>
	/// Rejet de l'Authentification par le serveur.
	/// Le socket est fermé
	/// </summary>
	void OnRejected();

	/// <summary>
	/// Deconnexion du serveur, fin de l'authetification
	/// Le socket est fermé
	/// </summary>
	void OnDisconnected();

	/// <summary>
	/// A friend is now Disconnected
	/// </summary>
	/// <param name="username">Username of friend</param>
	/// <param name="id">Id of friend</param>
	void OnFriendDisconnected(string username, int id);

	/// <summary>
	/// A friend is now Connected
	/// </summary>
	/// <param name="username">Username of friend</param>
	/// <param name="id">Id of friend</param>
	void OnFriendConnected(string username, int id);

	/// <summary>
	/// A friend have joined a room
	/// </summary>
	/// <param name="username">Username of friend</param>
	/// <param name="id">Id of friend</param>
	/// <param name="roomID">Room joined</param>
	void OnFriendJoinRoom(string username, int id, string roomID);

	/// <summary>
	/// A friend invited you
	/// </summary>
	/// <param name="username">Username of friend</param>
	/// <param name="id">Id of friend</param>
	/// <param name="roomID">Room to join</param>
	void OnReceiveInvitation(string username, int id, string roomID);

	/// <summary>
	/// Reception d'une action inconnue
	/// </summary>
	/// <param name="message">Content</param>
	void OnReceiveMessage(string message);
}

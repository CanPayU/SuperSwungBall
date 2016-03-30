using UnityEngine;
using System.Collections;

public interface ClientListener {

	/// <summary>
	/// Confirmation de l'authentification par le serveur
	/// </summary>
	void OnAuthenticated();

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
	/// Reception d'une action inconnue
	/// </summary>
	/// <param name="message">Content</param>
	void OnReceiveMessage(string message);
}

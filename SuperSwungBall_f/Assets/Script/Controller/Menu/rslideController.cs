using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class rslideController : MonoBehaviour {
	
	[SerializeField] private GameObject scroll_view;
	[SerializeField] private ScrollRect content_scroll_view;
	[SerializeField] private GameObject panel_friend;

	private Friends friends;
	private float scroll_view_heigth;
	private float actual_position;

	private Client client;

	// Use this for initialization
	void Start () {
		scroll_view_heigth = ((RectTransform)this.scroll_view.transform).sizeDelta.y;
		this.friends = User.Instance.Friends;

		foreach (KeyValuePair<string, User> friend in friends.All) {
			InstanciateFriend (friend.Value.username);
		}
	}
	
	// Update is called once per frame
	void Update () {
		#if DEBUG
		if (Input.GetKey (KeyCode.S))
			IsOnline ("romain");
		#endif
	}

	void OnEnable() {
		foreach (Transform friend in content_scroll_view.content.transform) {
			string username = friend.name;
			User friend_user = this.friends.Get (username);
			bool online = friend_user.is_connected;
			IsOnline (username, online);
		}

	}


	/// <summary>
	/// Modifie la couleur de la pastille
	/// </summary>
	/// <param name="username">Username of friend</param>
	/// <param name="online">true => vert | false => rouge</param>
	public void IsOnline(string username, bool online = true){
		Transform user = content_scroll_view.content.transform.Find (username);
		if (user == null) {
			Debug.LogWarning ("Username Not Found : " + username);
			return;
		}

		GameObject gm = user.Find ("Online").gameObject;
		Color color = new Color ();
		if (online)
			color = new Color (83f / 255f, 212f / 255f, 83f / 255f);
		else
			color = new Color (212f / 255f, 83f / 255f, 83f / 255f);
		
		gm.GetComponent<Image>().color = color;
	}

	/// <summary>
	/// Ajoute un amis a la liste
	/// </summary>
	/// <param name="username">Username of friend</param>
	public void InstanciateFriend(string username){

		Transform panel = Instantiate (panel_friend).transform as Transform;
		Transform content = panel.transform.Find ("Pseudo");
		Text content_text = content.GetComponent<Text> ();

		content_text.text = username;
		panel.name = username;

		float panel_heigth = ((RectTransform)panel).sizeDelta.y;

		RectTransform scroll_view = content_scroll_view.content.GetComponent<RectTransform> ();
		float new_scroll_view_heigth = scroll_view.sizeDelta.y + (panel_heigth + 5);

		scroll_view.sizeDelta = new Vector2(scroll_view.sizeDelta.x, new_scroll_view_heigth);

		if (new_scroll_view_heigth > this.scroll_view_heigth) 
			scroll_view.anchoredPosition = new Vector3(0, new_scroll_view_heigth - this.scroll_view_heigth);

		panel.transform.SetParent (content_scroll_view.content.transform);
		actual_position -= ((panel_heigth / 2) + 5);
		((RectTransform)panel).anchoredPosition = new Vector2 (0, actual_position);
		actual_position -= ((panel_heigth / 2));
	}
}
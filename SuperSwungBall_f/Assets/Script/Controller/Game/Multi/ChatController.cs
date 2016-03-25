using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.Chat;
using AuthValues = ExitGames.Client.Photon.Chat.AuthenticationValues;


public class ChatController : MonoBehaviour, IChatClientListener {

	[SerializeField] private GameObject scroll_view;
	[SerializeField] private ScrollRect content_scroll_view;
	[SerializeField] private GameObject panel_message;
	[SerializeField] private GameObject panel_event;

	private PhotonView pv;
	private PhotonPlayer photon_enemy;
	private User user_enemy;

	// Rect
	private float actual_position = 0;
	private int nb_message = 0;
	private float scroll_view_heigth;

	// Use this for initialization
	void Start () {
		pv = PhotonView.Get (this);
		//photon_enemy = PhotonNetwork.otherPlayers [0];
		//user_enemy = (User)photon_enemy.allProperties ["User"];

		scroll_view_heigth = ((RectTransform)this.scroll_view.transform).sizeDelta.y;


		ChatClient chatClient = new ChatClient (this);

		AuthValues authvalues = new AuthValues ();
		authvalues.UserId = "MyUserName";//User.Instance.username;
		chatClient.Connect( "36f3dfb1-5ab7-4277-8d95-176d0bae98ff", "1.0", authvalues);

		chatClient.Subscribe( new string[] { "MyNameRoom" } );
		chatClient.Service();
	}

	public void SendMessage(string message){
		pv.RPC ("ReceiveMessage", PhotonTargets.All, pv.viewID, message);
	}

	public void OnDisconnected(){
		Debug.Log ("OnDisconnected");
	}
	public void OnConnected(){
		Debug.Log ("OnConnected");
	}
	public void OnGetMessages(string channelName, string[] senders, object[] messages){
		Debug.Log ("OnGetMessages");
	}
	public void OnPrivateMessage(string sender, object message, string channelName){
		Debug.Log ("OnPrivateMessage:" + sender + " - " + message + " - " + channelName);
	}
	public void OnSubscribed(string[] channels, bool[] results){
		Debug.Log ("OnSubscribed:" + channels + " - " + results);
	}
	public void OnUnsubscribed(string[] channels){
		Debug.Log ("OnUnsubscribed");
	}
	public void OnStatusUpdate(string user, int status, bool gotMessage, object message){
		Debug.Log ("OnStatusUpdate");
	}
	public void OnChatStateChange(ChatState state){
		Debug.Log ("OnChatStateChange");
	}
	public void DebugReturn(DebugLevel level, string message){
		Debug.Log ("DebugReturn: " + level + " - " + message);
	}

	[PunRPC] public void ReceiveMessage(int viewID, string message){
		if (pv.viewID != pv.viewID)
			Debug.Log ("Enemy "+ user_enemy.username + " send : " + message);
		else
			Debug.Log ("I send : " + message);
	}


	public void InstanciateMessage(string text, Chat type){
		GameObject instance = new GameObject();
		switch (type) {
		case Chat.EVENT:
			instance = this.panel_event;
			break;
		case Chat.LOCAL_MESSAGE:
			instance = this.panel_message;
			break;
		case Chat.SERVER_MESSAGE:
			instance = this.panel_message;
			break;
		default:
			break;
		}

		Transform panel = Instantiate (instance).transform as Transform;
		Transform content = panel.transform.Find ("Content");
		Text content_text = content.GetComponent<Text> ();

		content_text.text = text;
		float content_height = LayoutUtility.GetPreferredHeight(((RectTransform)content.transform));
		float content_width = ((RectTransform)content).sizeDelta.x;
		float panel_heigth = content_height + 10;
		float panel_width = content_width + 10;

		((RectTransform)content).sizeDelta = new Vector2 (content_width, content_height);
		((RectTransform)panel).sizeDelta = new Vector2 (panel_width, panel_heigth);

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

	public enum Chat
	{
		LOCAL_MESSAGE,
		SERVER_MESSAGE,
		EVENT
	}
}
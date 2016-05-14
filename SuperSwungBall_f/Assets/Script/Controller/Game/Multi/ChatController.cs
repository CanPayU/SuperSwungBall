using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.Chat;
using AuthValues = ExitGames.Client.Photon.Chat.AuthenticationValues;

using GameKit;

namespace GameScene {
		
	public class ChatController : GameBehavior, IChatClientListener
	{

	    [SerializeField]
	    private GameObject scroll_view;
	    [SerializeField]
	    private ScrollRect content_scroll_view;
	    [SerializeField]
	    private GameObject panel_my_message;
	    [SerializeField]
	    private GameObject panel_other_message;
	    [SerializeField]
	    private GameObject panel_event;
	    [SerializeField]
	    private InputField inputField;

	    private PhotonPlayer photon_enemy;
	    private User user_enemy;
	    private System.Random rand = new System.Random();

	    // -- Rect
	    private float actual_position = 0;
	    private float scroll_view_heigth;

	    // -- Chat
	    private ChatClient chatClient;
	    private const string APP_ID = "36f3dfb1-5ab7-4277-8d95-176d0bae98ff";
	    private const string APP_VERSION = "1.6";
	    private string UserName;
	    private string channel_name;

		public ChatController(){
			this.eventType = GameKit.EventType.External;
		}

	    // Use this for initialization
	    void Start()
	    {

	        Application.runInBackground = true;
	        scroll_view_heigth = ((RectTransform)this.scroll_view.transform).sizeDelta.y;

	        photon_enemy = PhotonNetwork.otherPlayers[0];
	        user_enemy = (User)photon_enemy.allProperties["User"];
	        string txt_chat = "Vous jouez contre : " + user_enemy.username;
	        InstanciateMessage(txt_chat, Chat.EVENT);

	        channel_name = PhotonNetwork.room.name;
	        int alea = rand.Next(1000);
	        UserName = User.Instance.username + alea.ToString();

	        chatClient = new ChatClient(this);
	        chatClient.Connect(APP_ID, APP_VERSION, new AuthValues(UserName));

	        inputField.onEndEdit.AddListener(val =>
	            {
	                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
	                {
	                    SendMessageToOtherClient(inputField.text);
	                    inputField.text = "";
	                }
	            });
	    }

	    void Update()
	    {
	        this.chatClient.Service();
	    }


		// ===
		public override void OnSucceedAttack(Player p){
			InstanciateMessage (p.Name + " réussit son attaque !", ChatController.Chat.EVENT);
		}
		public override void OnSucceedEsquive(Player p){
			InstanciateMessage (p.Name + " réussit son esquive !", ChatController.Chat.EVENT);
		}
		public override void OnFailedAttack(Player p){
			InstanciateMessage (p.Name + " rate son attaque !", ChatController.Chat.EVENT);
		}
		public override void OnFailedEsquive(Player p){
			InstanciateMessage (p.Name + " rate son esquive !", ChatController.Chat.EVENT);
		}
		// ===

	    public void SendMessageToOtherClient(string message)
	    {
	        Debug.Log("send : " + message);
	        this.chatClient.PublishMessage(this.channel_name, message);
	    }

	    public void OnDisconnected()
	    {
	        Debug.Log("OnDisconnected");
	    }
	    public void OnConnected()
	    {
	        this.chatClient.Subscribe(new string[] { channel_name });
	        this.chatClient.AddFriends(new string[] { "tobi" });          // Add some users to the server-list to get their status updates
	        this.chatClient.SetOnlineStatus(ChatUserStatus.Online);
	        InstanciateMessage("Connected", Chat.EVENT);
	    }
	    public void OnGetMessages(string channelName, string[] senders, object[] messages)
	    {
	        for (int i = 0; i < messages.Length; i++)
	        {
	            if (senders[i] != UserName)
	                InstanciateMessage((string)messages[i], Chat.SERVER_MESSAGE);
	            else
	                InstanciateMessage((string)messages[i], Chat.LOCAL_MESSAGE);
	        }

	    }
	    public void OnPrivateMessage(string sender, object message, string channelName)
	    {
	        Debug.Log("OnPrivateMessage:" + sender + " - " + message + " - " + channelName);
	    }
	    public void OnSubscribed(string[] channels, bool[] results)
	    {
	        for (int i = 0; i < channels.Length; i++)
	        {
	            if (!results[i])
	                InstanciateMessage("Impossible de rejoindre " + channels[i], Chat.EVENT);
	        }
	    }
	    public void OnUnsubscribed(string[] channels)
	    {
	        foreach (var channel in channels)
	        {
	            InstanciateMessage("Vous avez quitté " + channel, Chat.EVENT);
	        }
	    }
	    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
	    {
	        Debug.Log("OnStatusUpdate");
	    }
	    public void OnChatStateChange(ChatState state)
	    {
	        Debug.Log("ChatState - " + state);
	    }
	    public void DebugReturn(DebugLevel level, string message)
	    {
	        Debug.Log("DebugReturn: " + level + " - " + message + " - " + chatClient.State);
	    }

	    public void InstanciateMessage(string text, Chat type)
	    {
	        GameObject instance = new GameObject();
	        switch (type)
	        {
	            case Chat.EVENT:
	                instance = this.panel_event;
	                break;
	            case Chat.LOCAL_MESSAGE:
	                instance = this.panel_my_message;
	                break;
	            case Chat.SERVER_MESSAGE:
	                instance = this.panel_other_message;
	                break;
	            default:
	                break;
	        }

	        Transform panel = Instantiate(instance).transform as Transform;
	        Transform content = panel.transform.Find("Content");
	        Text content_text = content.GetComponent<Text>();

	        content_text.text = text;
	        float content_height = LayoutUtility.GetPreferredHeight(((RectTransform)content.transform));
	        float content_width = ((RectTransform)content).sizeDelta.x;
	        float panel_heigth = content_height + 10;
	        float panel_width = content_width + 10;

	        ((RectTransform)content).sizeDelta = new Vector2(content_width, content_height);
	        ((RectTransform)panel).sizeDelta = new Vector2(panel_width, panel_heigth);

	        RectTransform scroll_view = content_scroll_view.content.GetComponent<RectTransform>();
	        float new_scroll_view_heigth = scroll_view.sizeDelta.y + (panel_heigth + 5);

	        scroll_view.sizeDelta = new Vector2(scroll_view.sizeDelta.x, new_scroll_view_heigth);

	        if (new_scroll_view_heigth > this.scroll_view_heigth)
	            scroll_view.anchoredPosition = new Vector3(0, new_scroll_view_heigth - this.scroll_view_heigth);

	        panel.transform.SetParent(content_scroll_view.content.transform);
	        actual_position -= ((panel_heigth / 2) + 5);
	        ((RectTransform)panel).anchoredPosition = new Vector2(0, actual_position);
	        actual_position -= ((panel_heigth / 2));
	    }

	    public enum Chat
	    {
	        LOCAL_MESSAGE,
	        SERVER_MESSAGE,
	        EVENT
	    }
	}
}
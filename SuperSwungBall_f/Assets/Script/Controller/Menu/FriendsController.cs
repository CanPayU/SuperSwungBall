using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class FriendsController : MonoBehaviour
{

    [SerializeField]
    private GameObject scroll_view;
    [SerializeField]
    private ScrollRect content_scroll_view;
    [SerializeField]
    private GameObject panel_friend;

    private Friends friends;
    private float scroll_view_heigth;
    private float actual_position;

    private Client client;
    private System.Random rand = new System.Random();

    // Use this for initialization
    void Start()
    {
        scroll_view_heigth = ((RectTransform)this.scroll_view.transform).sizeDelta.y;
        this.friends = User.Instance.Friends;
        this.client = GameObject.Find("Manager").GetComponent<ClientManager>().Client;

        foreach (KeyValuePair<string, User> friend in friends.All)
        {
            InstanciateFriend(friend.Value);
        }
    }

    void OnEnable()
    {
        foreach (Transform friend in content_scroll_view.content.transform)
        {
            string username = friend.name;
            User friend_user = this.friends.Get(username);
            bool online = friend_user.is_connected;
            IsOnline(username, online);

            // Setup update
            Transform information = friend.Find("Information");
            Update_Information(information, friend_user);
        }

    }


    /// <summary>
    /// Modifie la couleur de la pastille
    /// </summary>
    /// <param name="username">Username of friend</param>
    /// <param name="online">true => vert | false => rouge</param>
    public void IsOnline(string username, bool online = true)
    {
        Transform user = content_scroll_view.content.transform.Find(username);
        if (user == null)
        {
            Debug.LogWarning("Username Not Found : " + username);
            return;
        }



        GameObject gm = user.Find("Online").gameObject;
        Color color = new Color();
        if (online)
            color = new Color(83f / 255f, 212f / 255f, 83f / 255f);
        else
            color = new Color(212f / 255f, 83f / 255f, 83f / 255f);

        gm.GetComponent<Image>().color = color;
    }

    /// <summary>
    /// Ajoute un amis a la liste
    /// </summary>
    /// <param name="username">Username of friend</param>
    public void InstanciateFriend(User user)
    {

        Transform panel = Instantiate(panel_friend).transform as Transform;
        Transform content = panel.transform.Find("Pseudo");
        Text content_text = content.GetComponent<Text>();

        string username = user.username;
        content_text.text = username;
        panel.name = username;
        Setup_Information(panel, user); // Action btn inviter + info text

        float panel_heigth = ((RectTransform)panel).sizeDelta.y;

        RectTransform scroll_view = content_scroll_view.content.GetComponent<RectTransform>();
        float new_scroll_view_heigth = scroll_view.sizeDelta.y + (panel_heigth + 5);

        scroll_view.sizeDelta = new Vector2(scroll_view.sizeDelta.x, new_scroll_view_heigth);

        if (new_scroll_view_heigth > this.scroll_view_heigth)
            scroll_view.anchoredPosition = new Vector3(0, new_scroll_view_heigth - this.scroll_view_heigth);

        panel.transform.SetParent(content_scroll_view.content.transform, false);
        actual_position -= ((panel_heigth / 2) + 5);
        ((RectTransform)panel).anchoredPosition = new Vector2(0, actual_position);
        actual_position -= ((panel_heigth / 2));
    }


    private void Setup_Information(Transform gm, User user)
    {
        string username = user.username;
        User friend = friends.Get(username);
        Transform information = gm.Find("Information");
        Text text = information.Find("Text").GetComponent<Text>();
        Button invite = information.Find("Invite").GetComponent<Button>();

        if (friend.room != null)
            text.text = "Joue en ligne";
        else if (friend.is_connected)
            text.text = "Navigue dans le menu";
        else
            text.text = "Deconnecte";

        Debug.Log(friend.room);

        invite.onClick.AddListener(delegate ()
            {
                Update_Information(information, friend);
                bool autorised = friend.is_connected && friend.room == null;
                Debug.Log("Send invitation to : " + username + " -- Autorised : " + autorised);
                if (autorised)
                {
                    string RoomID = (rand.Next(1000, 9999)).ToString();
                    ApplicationModel.NetState = Network.NetSate.InviteFriend;
                    ApplicationModel.RoomID = RoomID;
                    client.InviteFriend(friend, RoomID);
                    FadingManager.Instance.Fade("network");
                }
                else
                    Notification.Create(NotificationType.Box, "Erreur lors de l'invitation", content: "Impossible d'inviter " + username + ".\nIl est possible que l'utilisateur soit déjà occupé");
            });
    }

    private void Update_Information(Transform information, User friend)
    {
        Text text = information.Find("Text").GetComponent<Text>();
        Button invite = information.Find("Invite").GetComponent<Button>();

        //float information_heigth = ((RectTransform)information).sizeDelta.y;
        //float btn_heigth = ((RectTransform)invite).sizeDelta.y;

        if (friend.room != null)
        {
            text.text = "Joue en ligne";
            invite.interactable = false;
        }
        else if (friend.is_connected)
        {
            text.text = "Navigue dans le menu";
            invite.interactable = true;
        }
        else
        {
            text.text = "Deconnecte";
            invite.interactable = false;
        }
    }
}

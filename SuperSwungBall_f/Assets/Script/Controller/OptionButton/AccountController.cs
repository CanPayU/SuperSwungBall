using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Globalization;


public class AccountController : MonoBehaviour
{
    private Text username;
    private Text score;
    private Text phi;

    // Use this for initialization
    void Start()
    {
        Transform panel = transform.Find("Panel");

        this.username = panel.Find("Username").GetComponent<Text>();
        this.score = panel.Find("Score").GetComponent<Text>();
        this.phi = panel.Find("Phi").Find("Value").GetComponent<Text>();
        SetUp();
    }

    void Update()
    {
        SetUp();
    }

    public void OnDisconnect()
    {
        Debug.Log("OnDisconnect");
        SaveLoad.reset_user();
        FadingManager.Instance.Fade("standing");
    }

    public void OnViewReplay()
    {
        var replays = Resources.Load("Prefabs/OptionButton/Replay/View") as GameObject;
        var gm = GameObject.Instantiate(replays);
        Transform Canvas = GameObject.FindObjectOfType<Canvas>().transform;
        gm.transform.SetParent(Canvas, false);
    }

    private void SetUp()
    {
        var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        nfi.NumberGroupSeparator = " ";

        this.username.text = User.Instance.username;
        this.score.text = "Score : " + User.Instance.score.ToString("#,0", nfi);
        this.phi.text = User.Instance.phi.ToString("#,0", nfi);
    }

    public void MorePhi()
    {
        PhiManager.Instance.More();
    }
}
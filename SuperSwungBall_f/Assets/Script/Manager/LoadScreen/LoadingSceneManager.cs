using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour
{

    private Text valueText;
    public Image value;
    private float speed = 0.5f;

    private static AsyncOperation async;
    private static bool followAsync = true;
    private static float currentAmount = 0f;

    // Use this for initialization
    void Start()
    {
        var loadView = GameObject.Find("LoadView").transform;
        this.valueText = loadView.Find("Text").GetComponent<Text>();
        this.value = loadView.Find("Value").GetComponent<Image>();

        this.valueText.text = "0 %";
        this.value.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (followAsync && async != null)
            currentAmount = async.progress;
        if (currentAmount > this.value.fillAmount)
        {
            this.valueText.text = (this.value.fillAmount * 100).ToString("##0") + " %";
            this.value.fillAmount += this.speed * Time.deltaTime;
        }
    }

    public static void Complete()
    {
        followAsync = false;
        currentAmount = 1f;
    }

    public static void UnloadLoadingScene()
    {
        //SceneManager.UnloadScene ("LoadingScreen");
    }

    public static AsyncOperation Async
    {
        set
        {
            async = value;
            followAsync = true;
        }
    }
}

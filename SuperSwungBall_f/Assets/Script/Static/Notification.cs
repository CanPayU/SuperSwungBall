using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;


public static class Notification
{
    private static MonoBehaviour MonoB = GameObject.FindObjectOfType<MonoBehaviour>();
    private static GameObject Canvas = GameObject.FindObjectOfType<Canvas>().gameObject;

	private static GameObject Text_Prefab = Resources.Load("Prefabs/Notification/Text") as GameObject;
	private static GameObject SimpleAlert_Prefab = Resources.Load("Prefabs/Notification/SimpleAlert") as GameObject;
    private static GameObject Alert_Prefab = Resources.Load("Prefabs/Notification/Alert") as GameObject;
    private static GameObject Box_Prefab = Resources.Load("Prefabs/Notification/Box") as GameObject;
    private static GameObject Slide_Prefab = Resources.Load("Prefabs/Notification/Slide") as GameObject;

    private const float DELAY = 4;
    private static float delay;

    /// <summary>
    /// Create the specified type, title, Delay and content.
    /// </summary>
    /// <param name="type">Type de la notifi</param>
    /// <param name="title">Titre de la notif.</param>
    /// <param name="Delay">Delai avant disparition. <=0 pour infinie</param>
    /// <param name="content">Content, pour Box et Alert</param>
    /// <param name="force">Force la notification (Seulement pour le système)</param>
    public static void Create(NotificationType type, string title, float Delay = DELAY, string content = null, Action<bool, string> completion = null, bool force = false)
    {
		if (!IsAutorised (type, force)) {
			Debug.Log ("Notification rejected");
			return;
		}
        Canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
        delay = Delay;
        switch (type)
		{
			case NotificationType.Text:
				if (content == null && completion == null)
					return;
				Instanciate_Text(title, content, completion);
				break;
			case NotificationType.SimpleAlert:
				if (content == null && completion == null)
					return;
				Instanciate_SimpleAlert(title, content, completion);
				break;
            case NotificationType.Alert:
                if (content == null && completion == null)
                    return;
                Instanciate_Alert(title, content, completion);
                break;
            case NotificationType.Box:
                if (content == null)
                    return;
                Instanciate_Box(title, content);
                break;
            case NotificationType.Slide:
                Instanciate_Slide(title);
                break;
            default:
                break;
        }
	}
	/// <summary>
	/// Instanciates the alert.
	/// </summary>
	/// <param name="title">Title.</param>
	/// <param name="content">Content.</param>
	/// <param name="completion">Execute on click button</param>
	private static void Instanciate_SimpleAlert(string title, string content, Action<bool, string> completion)
	{
		GameObject gm = MonoBehaviour.Instantiate(SimpleAlert_Prefab);
		gm.name = "NotificationSimpleAlert";
		gm.transform.SetParent(Canvas.transform, false);
		Text textComponent = gm.transform.Find("Title").GetComponent<Text>();
		textComponent.text = title;
		textComponent = gm.transform.Find("Content").GetComponent<Text>();
		textComponent.text = content;

		Button accepted = gm.transform.Find("Send").GetComponent<Button>();
		accepted.onClick.AddListener(delegate ()
			{
				completion(true, null);
				gm.SetActive(false);
				MonoBehaviour.Destroy(gm);
			});
	}
	/// <summary>
	/// Instanciates the alert.
	/// </summary>
	/// <param name="title">Title.</param>
	/// <param name="content">Content.</param>
	/// <param name="completion">Execute on click button <c>true</c></param>
	private static void Instanciate_Text(string title, string content, Action<bool, string> completion)
	{
		GameObject gm = MonoBehaviour.Instantiate(Text_Prefab);
		gm.name = "NotificationText";
		gm.transform.SetParent(Canvas.transform, false);
		Text textComponent = gm.transform.Find("Title").GetComponent<Text>();
		textComponent.text = title;
		textComponent = gm.transform.Find("Content").GetComponent<Text>();
		textComponent.text = content;

		InputField text = gm.transform.Find("Value").GetComponent<InputField>();
		Button accepted = gm.transform.Find("Send").GetComponent<Button>();
		accepted.onClick.AddListener(delegate ()
			{
				if (text.text == null || text.text == "")
					return;
				completion(true, text.text);
				gm.SetActive(false);
				MonoBehaviour.Destroy(gm);
			});
	}
    /// <summary>
    /// Instanciates the alert.
    /// </summary>
    /// <param name="title">Title.</param>
    /// <param name="content">Content.</param>
    /// <param name="completion">Execute on click button : <c>true</c> Accepted or  <c>false</c> Refused</param>
    private static void Instanciate_Alert(string title, string content, Action<bool, string> completion)
    {
        GameObject gm = MonoBehaviour.Instantiate(Alert_Prefab);
        gm.name = "NotificationAlert";
        gm.transform.SetParent(Canvas.transform, false);
        Text textComponent = gm.transform.Find("Title").GetComponent<Text>();
        textComponent.text = title;
        textComponent = gm.transform.Find("Content").GetComponent<Text>();
        textComponent.text = content;

        Transform action = gm.transform.Find("Action");
        Button refused = action.Find("btn_1").GetComponent<Button>();
        Button accepted = action.Find("btn_2").GetComponent<Button>();

        accepted.onClick.AddListener(delegate ()
        {
            completion(true, null);
            gm.SetActive(false);
            MonoBehaviour.Destroy(gm);
        });

        refused.onClick.AddListener(delegate ()
        {
            completion(false, null);
            gm.SetActive(false);
            MonoBehaviour.Destroy(gm);
        });
    }
    /// <summary>
    /// Instanciates the box.
    /// </summary>
    /// <param name="title">Title.</param>
    /// <param name="content">Content.</param>
    private static void Instanciate_Box(string title, string content)
    {
        GameObject gm = MonoBehaviour.Instantiate(Box_Prefab);
        gm.name = "NotificationBox";
        gm.transform.SetParent(Canvas.transform, false);
        Text textComponent = gm.transform.Find("Title").GetComponent<Text>();
        textComponent.text = title;
        textComponent = gm.transform.Find("Content").GetComponent<Text>();
        textComponent.text = content;
        Coroutine(gm);
    }
    /// <summary>
    /// Instanciates the slide.
    /// </summary>
    /// <param name="title">Title.</param>
    private static void Instanciate_Slide(string title)
    {
        GameObject gm = MonoBehaviour.Instantiate(Slide_Prefab);
        gm.name = "NotificationSlide";
        gm.transform.SetParent(Canvas.transform, false);
        Text textComponent = gm.transform.Find("Title").GetComponent<Text>();
        textComponent.text = title;
        Coroutine(gm);
    }

    /// <summary>
    /// Determines if notfication is autorised.
    /// </summary>
    /// <returns><c>true</c> if is autorised; otherwise, <c>false</c>.</returns>
    private static bool IsAutorised(NotificationType type, bool force)
    {
        NotificationState state = Settings.Instance.NotificationState;
        return (force || (int)state >= (int)type);
    }

    private static void Coroutine(GameObject gm)
    {
        if (delay <= 0)
            return;
        if (MonoB == null)
            MonoB = GameObject.FindObjectOfType<MonoBehaviour>();
        MonoB.StartCoroutine(Disable(gm));
    }

    private static IEnumerator Disable(GameObject gm)
    {
        yield return new WaitForSeconds(delay);
        gm.SetActive(false);
        MonoBehaviour.Destroy(gm);
    }
}
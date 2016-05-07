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

	///
	/// Create.
	///

	/// <summary>
	/// Create a notification of type Box or Slide.
	/// </summary>
	public static void Create (NotificationType type, string title, string content, float Delay = DELAY, bool force = false)
	{
		if (!IsAutorised (type, force))
			return;
		
		Canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
		delay = Delay;
		if (type == NotificationType.Box)
			instanciateBox(title, content);
		if (type == NotificationType.Slide)
			instanciateSlide(title);
	}

	/// <summary>
	/// Create a notification of type SimpleAlert.
	/// </summary>
	public static void SimpleAlert (string title, string content, Action completion = null, bool force = false)
	{
		var type = NotificationType.SimpleAlert;
		if (!IsAutorised (type, force)) 
			return;
		
		Canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
		if (type == NotificationType.SimpleAlert)
			instanciateSimpleAlert(title, content, completion);
	}

	/// <summary>
	/// Create a notification of type Text
	/// </summary>
	public static void Alert (string title, string content, Action<bool> completion, bool force = false)
	{
		var type = NotificationType.Alert;
		if (!IsAutorised (type, force))
			return;
		Canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
		instanciateAlert(title, content, completion);
	}

	/// <summary>
	/// Create a notification of type Text
	/// </summary>
	public static void Text (string title, string content, Action<string> completion, bool force = false)
	{
		var type = NotificationType.Text;
		if (!IsAutorised (type, force))
			return;
		
		Canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
		instanciateText(title, content, completion);
	}

	///
	/// Instanciate.
	///

	/// <summary>
	/// Instanciate the Text notification.
	/// </summary>
	/// <param name="completion">Execute on click button ok.</param>
	private static void instanciateText(string title, string content, Action<string> completion)
	{
		GameObject gm = globalSetUp (title, content, Text_Prefab);

		InputField text = gm.transform.Find("Value").GetComponent<InputField>();
		Button accepted = gm.transform.Find("Send").GetComponent<Button>();
		accepted.onClick.AddListener(delegate ()
			{
				if (text.text == null || text.text == "")
					return;
				completion(text.text);
				gm.SetActive(false);
				MonoBehaviour.Destroy(gm);
			});
	}
	/// <summary>
	/// Instanciate the Alert notification.
	/// </summary>
	/// <param name="completion">Execute on click button : <c>true</c> Accepted or  <c>false</c> Refused</param>
	private static void instanciateAlert(string title, string content, Action<bool> completion)
	{
		GameObject gm = globalSetUp (title, content, Alert_Prefab);

		Transform action = gm.transform.Find("Action");
		Button refused = action.Find("btn_1").GetComponent<Button>();
		Button accepted = action.Find("btn_2").GetComponent<Button>();

		accepted.onClick.AddListener(delegate ()
			{
				completion(true);
				gm.SetActive(false);
				MonoBehaviour.Destroy(gm);
			});

		refused.onClick.AddListener(delegate ()
			{
				completion(false);
				gm.SetActive(false);
				MonoBehaviour.Destroy(gm);
			});
	}

	/// <summary>
	/// Instanciate the Box notification.
	/// </summary>
	private static void instanciateBox(string title, string content)
	{
		GameObject gm = globalSetUp (title, content, Box_Prefab);
		Coroutine(gm);
	}


	/// <summary>
	/// Instanciate the Slide notification.
	/// </summary>
	private static void instanciateSlide(string title)
	{
		GameObject gm = globalSetUp (title, null, Slide_Prefab);
		Coroutine(gm);
	}

	///
	/// Global.
	///

	/// <summary>
	/// Instanciate the SimpleAlert notification.
	/// </summary>
	/// <param name="completion">Execute on click button ok.</param>
	private static void instanciateSimpleAlert(string title, string content, Action completion)
	{
		GameObject gm = globalSetUp (title, content, SimpleAlert_Prefab);

		Button accepted = gm.transform.Find("Send").GetComponent<Button>();
		accepted.onClick.AddListener(delegate ()
			{
				if (completion != null)
					completion();
				gm.SetActive(false);
				MonoBehaviour.Destroy(gm);
			});

		Button close = gm.transform.Find("Close").GetComponent<Button>();
		close.onClick.AddListener(delegate ()
			{
				gm.SetActive(false);
				MonoBehaviour.Destroy(gm);
			});
	}

	/// <summary>
	/// Set up the notification.
	/// </summary>
	private static GameObject globalSetUp(string title, string content, GameObject instanceGm){
		GameObject gm = MonoBehaviour.Instantiate(instanceGm);
		gm.name = "Notification"+instanceGm.name;
		gm.transform.SetParent(Canvas.transform, false);
		if (title != null) {
			Text textComponent = gm.transform.Find("Title").GetComponent<Text>();
			textComponent.text = title;
		}
		if (content != null) {
			Text textComponent = gm.transform.Find("Content").GetComponent<Text>();
			textComponent.text = content;
		}
		return gm;
	}

    /// <summary>
    /// Determines if notfication is autorised.
    /// </summary>
    /// <returns><c>true</c> if is autorised; otherwise, <c>false</c>.</returns>
    private static bool IsAutorised(NotificationType type, bool force)
    {
        NotificationState state = Settings.Instance.NotificationState;
		if (force || (int)state >= (int)type)
			return true;
		else {
			Debug.Log ("Notification " + type + " rejected. State : " + state);
			return false;
		}
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
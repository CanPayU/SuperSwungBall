using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Notification {

	private static MonoBehaviour _mb = GameObject.FindObjectOfType<MonoBehaviour>();
	private static GameObject prefab = Resources.Load("Prefabs/Notification") as GameObject;
	private static float delay = 4;

	public static void success (string text, float time = 4) {
		Text txt = prefab.transform.Find("Panel/Text").GetComponent<Text>();
		txt.text = text;
		txt.color = new Color (92f / 255f, 184f / 255f, 92f / 255f);
		delay = time;
		Display ();
	}

	public static void danger (string text, float time = 4) {
		Text txt = prefab.transform.Find("Panel/Text").GetComponent<Text>();
		txt.text = text;
		txt.color = new Color (212f / 255f, 85f / 255f, 83f / 255f);
		delay = time;
		Display ();
	}

	private static void Display(){
		GameObject gm = MonoBehaviour.Instantiate(prefab);
		gm.name = "notification";
		_mb.StartCoroutine(Disable (gm));
	}

	private static IEnumerator  Disable(GameObject gm)
	{
		yield return new WaitForSeconds(delay);
		gm.SetActive (false);
		MonoBehaviour.Destroy (gm);

	}

}

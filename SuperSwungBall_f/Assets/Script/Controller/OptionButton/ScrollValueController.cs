using UnityEngine;
using UnityEngine.UI;

public class ScrollValueController : MonoBehaviour {

	private Text title;
	private Text textValue;
	private Scrollbar scroll;
	private Image image;

	// Use this for initialization
	void Start () {
		this.scroll = transform.Find("Value").GetComponent<Scrollbar>();
		this.image = scroll.transform.Find ("Mask").Find ("Image").GetComponent<Image>();
		this.textValue = transform.Find("Text").GetComponent<Text>();
		this.title = transform.Find("Title").GetComponent<Text>();
	}

	/// <summary>
	/// Update automatiquement la couleur.
	/// <c>Rouge</c> value > 1, <c>Bleu</c> sinon.
	/// </summary>
	public void valueWithColor(float value, string text = ""){
		if (value > 1f)
			this.Color = Colors.Normal.Red;
		else
			this.Color = Colors.Normal.Blue;
		this.Value = value;
		if (text != "")
			this.textValue.text = text;
	}

	/// <summary>
	/// Multiplie la value par le multiplier et l'ajoute en text
	/// </summary>
	/// <param name="multiplier">Multiplier.</param>
	public void updateText(int multiplier) {
		this.TextValue = (this.Value * multiplier).ToString ();
	}

	public string Title {
		set { this.title.text = value; }
	}
	public string TextValue {
		set { this.textValue.text = value; }
	}
	public float Value {
		get { return this.scroll.size; }
		set { this.scroll.size = value; }
	}
	public Color Color {
		set { this.image.color = value; }
	}
}

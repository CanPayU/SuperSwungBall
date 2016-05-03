using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace OptionButton
{
	
	public class QuitController : MonoBehaviour 
	{
		private Button btn;
		// Use this for initialization
		void Start () {
			this.btn = gameObject.GetComponent<Button> ();
			if (this.btn != null)
			{
				this.btn.onClick.AddListener(delegate ()
					{
						OnQuit();
					});
			}
		}
		
		// Update is called once per frame
		private void OnQuit () {
			Application.Quit ();
		}
	}
}
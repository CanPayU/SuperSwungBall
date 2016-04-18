using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Gestion {
	
	public class PlayerController : MonoBehaviour {

		private PhiManager manager;
		private Player player;
		private Text name_text;
		private Text price_text;
		private Button buy;

		// Use this for initialization
		void Start () {
			manager = GameObject.Find ("Manager").GetComponent<PhiManager> ();
			if (this.player == null)
				return;
			this.name_text = transform.Find ("Name").GetComponent<Text>();
			this.price_text = transform.Find ("Price").Find("Text").GetComponent<Text>();
			this.name_text.text = this.player.Name;
			this.price_text.text = this.player.Price.ToString();

			GameObject playerGm = Resources.Load("Prefabs/Resources/"+ this.player.UID) as GameObject;
			Transform player = Instantiate (playerGm).transform;
			player.SetParent (transform, false);

			RectTransform rect = player.GetComponent<RectTransform> ();
			rect.localScale = new Vector3 (200f, 200f, 200f);
			rect.anchoredPosition3D = new Vector3 (0, -35f, -20f);
			rect.rotation = new Quaternion (0, -180f, 0, 0);

			this.buy = transform.Find ("Buy").GetComponent<Button> ();
			this.buy.onClick.AddListener(delegate() {
				OnBuy();
			});
		}

		/// <summary>
		/// Lorsque l'on clic sur acheter
		/// </summary>
		public void OnBuy(){
			if (manager.BuyPlayer (player)) {
				Settings.Instance.BuyPlayer (player.UID);
				SaveLoad.save_setting ();
				BoughtButton ();
			} else
				Notification.Create (NotificationType.Box, "Achat impossible", content: "Vous n'avez pas assé de Phi ou vous possédez déjà ce SwungMen", force: true);
		}

		/// <summary> Configure le button une fois que le SwungMen à été acheté </summary>
		private void BoughtButton(){
			this.buy.interactable = false;
			this.buy.image.color = new Color (0f / 255f, 173f / 255f, 0f / 255f);
		}

		public Player Player {
			get { return this.player; }
			set { this.player = value; }
		}
	}
}
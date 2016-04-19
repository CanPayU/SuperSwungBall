using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Gestion {
	
	public class ChestController : MonoBehaviour {
		
		private PhiManager manager;
		private Text name_text;
		private Text price_text;
		private Button buy;
		private bool opened = false;

		// Use this for initialization
		void Start()
		{
			manager = GameObject.Find("Manager").GetComponent<PhiManager>();

			this.name_text = transform.Find("Name").GetComponent<Text>();
			this.price_text = transform.Find("Price").Find("Text").GetComponent<Text>();
			this.name_text.text = "Coffre";
			this.price_text.text = "30 000";

//			GameObject playerGm = Resources.Load("Prefabs/Resources/" + this.player.UID) as GameObject;
//			Transform player = Instantiate(playerGm).transform;
//			player.SetParent(transform, false);
//
//			RectTransform rect = player.GetComponent<RectTransform>();
//			rect.localScale = new Vector3(200f, 200f, 200f);
//			rect.anchoredPosition3D = new Vector3(0, -35f, -20f);
//			rect.rotation = new Quaternion(0, -180f, 0, 0);

			this.buy = transform.Find("Buy").GetComponent<Button>();
			this.buy.onClick.AddListener(delegate ()
			{
					OnBuy();
			});
		}

		/// <summary>
		/// Lorsque l'on clic sur acheter
		/// </summary>
		public void OnBuy()
		{
			if (manager.BuyChest())//&& !opened)
			{
				Player p = Open ();
				string title;
				if (manager.BuyPlayer (p)) {
					Settings.Instance.BuyPlayer(p.UID, PlayerType.Secret);
					SaveLoad.save_setting();
					title = "Nouveau SwungMen !";
				} else {
					title = "Contenu du coffre";
				}
				string content = "Vous avez obtenu " + p.Name + " !";
				Notification.Create(NotificationType.Box, title, content: content, force: true);
			}
			else
				Notification.Create(NotificationType.Box, "Achat impossible", content: "Vous n'avez pas assé de Phi", force: true);
		}

		/// <summary> Configure le button une fois que le SwungMen à été acheté </summary>
		private void BoughtButton()
		{
			this.buy.interactable = false;
			this.buy.image.color = new Color(0f / 255f, 173f / 255f, 0f / 255f);
		}

		/// <summary> Ouvre le coffre </summary>
		private Player Open()
		{
			this.opened = true;
			return Settings.Instance.Random_Secret_Player;
		}
	}
}
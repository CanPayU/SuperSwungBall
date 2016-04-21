using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Gestion {
	
	public class ChestController : MonoBehaviour {
		
		private PhiManager manager;

		private Animator animator;
		private GameObject chest;
		private GameObject info;
		private Text name_text;
		private Text price_text;
		private Button buy;

		private bool info_displayed = false;

		// Use this for initialization
		void Start()
		{
			this.chest = transform.Find ("Chest").gameObject;
			this.info = transform.Find ("Info").gameObject;
			this.manager = GameObject.Find("Manager").GetComponent<PhiManager>();
			this.name_text = transform.Find("Name").GetComponent<Text>();
			this.price_text = transform.Find("Price").Find("Text").GetComponent<Text>();
			this.name_text.text = "Coffre";
			this.price_text.text = "30 000";

			this.animator = transform.Find("Chest").Find ("Groupe").GetComponent<Animator> ();

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

		/// <summary> Affiche les infos sur le SwungMens </summary>
		public void DisplayInformation()
		{
			this.info_displayed = !this.info_displayed;
			this.info.SetActive (this.info_displayed);
			this.chest.SetActive (!this.info_displayed);
		}

		/// <summary> Ouvre le coffre </summary>
		private Player Open()
		{
			this.animator.Play ("Open");
			return Settings.Instance.Random_Secret_Player;
		}
	}
}
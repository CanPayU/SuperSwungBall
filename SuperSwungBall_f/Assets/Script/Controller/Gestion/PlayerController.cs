using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Gestion
{
    public class PlayerController : MonoBehaviour
    {
        private PhiManager manager;
        private Player player;

		private GameObject swungMen;
		private GameObject stats_panel;
        private Text name_text;
        private Text price_text;
        private Button buy;

		private bool stats_displayed = false;

        // Use this for initialization
        void Start()
        {
            if (this.player == null)
                return;
			this.stats_panel = transform.Find("Stats").gameObject;
            this.name_text = transform.Find("Name").GetComponent<Text>();
            this.price_text = transform.Find("Price").Find("Text").GetComponent<Text>();
            this.name_text.text = this.player.Name;
			this.price_text.text = this.player.Price.ToString();
			this.manager = GameObject.Find("Manager").GetComponent<PhiManager>();

			InstantiateSwungMen ();
			SetUpStats ();

            this.buy = transform.Find("Buy").GetComponent<Button>();
            this.buy.onClick.AddListener(delegate ()
            {
                OnBuy();
            });

			if (Settings.Instance.Default_player.ContainsKey(this.player.UID))
				BoughtButton();
        }

        /// <summary>
        /// Lorsque l'on clic sur acheter
        /// </summary>
        public void OnBuy()
        {
            if (manager.BuyPlayer(player))
            {
                Settings.Instance.BuyPlayer(player.UID);
                SaveLoad.save_setting();
                BoughtButton();
            }
            else
                Notification.Create(NotificationType.Box, "Achat impossible", content: "Vous n'avez pas assé de Phi ou vous possédez déjà ce SwungMen", force: true);
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
			this.stats_displayed = !this.stats_displayed;
			this.stats_panel.SetActive (this.stats_displayed);
			this.swungMen.SetActive (!this.stats_displayed);
//			/*Left*/ rectTransform.offsetMin.x;
//			/*Right*/ rectTransform.offsetMax.x;
//			/*Top*/ rectTransform.offsetMax.y;
//			/*Bottom*/ rectTransform.offsetMin.y;
		}

		private void SetUpStats(){
			this.stats_panel.SetActive (false);
			Transform t_p = this.stats_panel.transform;
			Transform passe = t_p.Find ("Passe").Find ("Value");
			Transform course = t_p.Find ("Course").Find ("Value");
			Transform esquive = t_p.Find ("Esquive").Find ("Value");
			Transform tacle = t_p.Find ("Tacle").Find ("Value");
			Player p = this.player;
			SetStatsValue (passe, this.player.PasseBase);
			SetStatsValue (course, this.player.SpeedBase);
			SetStatsValue (esquive, this.player.EsquiveBase);
			SetStatsValue (tacle, this.player.TacleBase);
		}

		private void SetStatsValue(Transform t, float value){
			RectTransform r = t.GetComponent<RectTransform> ();
			var size = r.rect.size.x - r.offsetMax.x; // + car value -
			var val = size - ((size / 10) * value);
			r.offsetMax = new Vector2 (-val , r.offsetMax.y);
		}

		private void InstantiateSwungMen(){
			GameObject gm = Resources.Load("Prefabs/Resources/" + this.player.UID) as GameObject;
			this.swungMen = Instantiate(gm);
			Transform player = this.swungMen.transform;
			player.SetParent(transform, false);

			RectTransform rect = player.GetComponent<RectTransform>();
			rect.localScale = new Vector3(200f, 200f, 200f);
			rect.anchoredPosition3D = new Vector3(0, -35f, -20f);
			rect.rotation = new Quaternion(0, -180f, 0, 0);
		}

        public Player Player
        {
            get { return this.player; }
            set { this.player = value; }
        }
    }
}
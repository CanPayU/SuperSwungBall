using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Gestion
{
	public class ChallengeController : MonoBehaviour
	{
		private PhiManager manager;
		private Player player;

		private GameObject swungMen;
		private GameObject stats_panel;
		private GameObject undefined_panel;
		private Text name_text;
		private Text defi_text;
		private Button buy;

		private bool stats_displayed = false;
		private bool unlocked = false;

		// Use this for initialization
		void Start()
		{
			if (this.player == null)
				return;
			this.stats_panel = transform.Find("Stats").gameObject;
			this.undefined_panel = transform.Find("Undefined").gameObject;
			this.name_text = transform.Find("Name").GetComponent<Text>();
			this.defi_text = transform.Find("Defi").Find("Text").GetComponent<Text>();
			this.name_text.text = this.player.Name;
			this.defi_text.text = "Defi";
			this.manager = GameObject.Find("Manager").GetComponent<PhiManager>();

			InstantiateSwungMen ();
			SetUpStats ();

			this.buy = transform.Find("Buy").GetComponent<Button>();
			this.unlocked = Settings.Instance.Default_player.ContainsKey (this.player.UID);
			if (this.unlocked) {
				this.undefined_panel.SetActive (false);
				BoughtButton ();
			} else {
				this.swungMen.SetActive (false);
			}
		}

		/// <summary> Configure le button une fois que le SwungMen à été débloqué </summary>
		private void BoughtButton()
		{
			this.buy.interactable = false;
			this.buy.transform.Find ("Text").GetComponent<Text> ().text = "Debloque";
		}

		/// <summary> Affiche les infos sur le SwungMan </summary>
		public void DisplayInformation()
		{
			this.stats_displayed = !this.stats_displayed;
			this.stats_panel.SetActive (this.stats_displayed);
			this.swungMen.SetActive ((!this.stats_displayed) && this.unlocked);
			this.undefined_panel.SetActive ((!this.stats_displayed) && (!this.unlocked));
		}

		private void SetUpStats(){
			this.stats_panel.SetActive (false);
			Transform t_p = this.stats_panel.transform;
			ScrollValueController passe = t_p.Find ("Passe").GetComponent<ScrollValueController>();
			ScrollValueController course = t_p.Find ("Course").GetComponent<ScrollValueController>();
			ScrollValueController esquive = t_p.Find ("Esquive").GetComponent<ScrollValueController>();
			ScrollValueController tacle = t_p.Find ("Tacle").GetComponent<ScrollValueController>();

			passe.Value = this.player.PasseBase;
			course.Value = this.player.SpeedBase;
			esquive.Value = this.player.EsquiveBase;
			tacle.Value = this.player.TacleBase;
		}

		private void SetStatsValue(Transform t, float value){
			RectTransform r = t.GetComponent<RectTransform> ();
			var size = r.rect.size.x - r.offsetMax.x; // + car value -
			var val = size - ((size / 10) * value);
			r.offsetMax = new Vector2 (-val , r.offsetMax.y);
		}

		private void InstantiateSwungMen(){
			GameObject gm = Resources.Load("Prefabs/Resources/" + this.player.UID) as GameObject;
			if (gm == null)
				gm = Resources.Load("Prefabs/Resources/IdPlayer") as GameObject;
			this.swungMen = Instantiate(gm);
			Transform player = this.swungMen.transform;
			player.SetParent(transform, false);

			Transform rect = player.GetComponent<Transform>();
			rect.localScale = new Vector3(20f, 20f, 200f);
			rect.localPosition = new Vector3(0, -35f, -20f);
			rect.localRotation = new Quaternion(0, -180f, 0, 0);
		}

		public Player Player
		{
			get { return this.player; }
			set { this.player = value; }
		}
	}
}
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

using GameKit;

namespace GameScene
{
	public class InfoJoueurController : GameBehavior
    {

        Vector3 open;
        Vector3 close;
        Vector3 moveTo;

        private Dictionary<string, GameObject[]> Stats;
        private Text playername;
        private float scaleStats;


        private RaycastHit hit;

        float speed;
        bool movement;


		public InfoJoueurController(){
			this.eventType = GameKit.EventType.Global;
		}

        void Start()
        {
            open = new Vector3(0, 0, 0);
            close = new Vector3(-transform.lossyScale.x * 413, 0, 0);
            speed = 1500;
            movement = false;
            moveTo = close;
            transform.position = close;
            // Initialisation du Dictionnaire
            Stats = new Dictionary<string, GameObject[]>();
            initializeDictionary("sprint", 0, 4);
            initializeDictionary("passe", 4, 7);
            initializeDictionary("tacle", 7, 10);
            initializeDictionary("esquive", 10, 13);
            playername = transform.GetChild(13).gameObject.GetComponent<Text>();
            scaleStats = 1f;
        }
        void Update()
        {
            if (movement)
            {
                transform.position = Vector3.MoveTowards(transform.position, moveTo, speed * Time.deltaTime);
                if (transform.position == moveTo)
                {
                    movement = false;
                }
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 100);
            if (Input.GetMouseButtonDown(0) && !hit.Equals(null) && hit.collider.gameObject.tag != "Player") //s'active au clic en dehors d'un player
            {
                Close();
            }
        }

        public void Open()
        {
            movement = true;
            moveTo = open;
        }
        public void Close()
        {
            movement = true;
            moveTo = close;
        }

        public void DisplayAlly(Player ally) // affiche les stats d'un allié
        {
            SetStats(ally);
            List<string> boutons = ally.Button_Values;
            int nbSprint = 0;
            int nbPasse = 0;
            int nbTacle = 0;
            int nbEsquive = 0;
            foreach (string str in boutons)
            {
                switch (str)
                {
                    case "esquive":
                        nbEsquive++;
                        break;
                    case "tacle":
                        nbTacle++;
                        break;
                    case "passe":
                        nbPasse++;
                        break;
                    default:
                        nbSprint++;
                        break;
                }
            }
            SetAlpha("sprint", nbSprint + 1);
            SetAlpha("passe", nbPasse);
            SetAlpha("tacle", nbTacle);
            SetAlpha("esquive", nbEsquive);
            Open();
        }
        public void DisplayEnnemy(Player ennemy) // affiche les stats de base d'un adversaire
        {
            SetStats(ennemy);
            Stats["passe"][0].GetComponent<CanvasRenderer>().SetAlpha(1);
            Stats["tacle"][0].GetComponent<CanvasRenderer>().SetAlpha(1);
            Stats["esquive"][0].GetComponent<CanvasRenderer>().SetAlpha(1);
            Open();
        }

        private void SetStats(Player p) // set la taille et la position des images de stat
        {
            float speed = p.SpeedBase * scaleStats;
            float passe = p.PasseBase * scaleStats;
            float tacle = p.TacleBase * scaleStats;
            float esquive = p.EsquiveBase * scaleStats;

            Stats["sprint"][0].transform.localScale = new Vector3(speed, Stats["sprint"][0].transform.localScale.y, Stats["sprint"][0].transform.localScale.z);
            Stats["sprint"][0].transform.localPosition = new Vector3(20, Stats["sprint"][0].transform.localPosition.y, Stats["sprint"][0].transform.localPosition.z);
            for (int i = 0; i < 3; i++)
            {
                Stats["sprint"][i + 1].transform.localScale = new Vector3(speed, Stats["sprint"][i].transform.localScale.y, Stats["sprint"][i].transform.localScale.z);
                Stats["sprint"][i + 1].transform.localPosition = new Vector3(20 + (i + 1) * (speed * 100 + 5), Stats["sprint"][i].transform.localPosition.y, Stats["sprint"][i].transform.localPosition.z);
                Stats["sprint"][i + 1].GetComponent<CanvasRenderer>().SetAlpha(0.3f);
                Stats["passe"][i].transform.localScale = new Vector3(passe, Stats["passe"][i].transform.localScale.y, Stats["passe"][i].transform.localScale.z);
                Stats["passe"][i].transform.localPosition = new Vector3(20 + i * (passe * 100 + 5), Stats["passe"][i].transform.localPosition.y, Stats["passe"][i].transform.localPosition.z);
                Stats["passe"][i].GetComponent<CanvasRenderer>().SetAlpha(0.3f);
                Stats["tacle"][i].transform.localScale = new Vector3(tacle, Stats["tacle"][i].transform.localScale.y, Stats["tacle"][i].transform.localScale.z);
                Stats["tacle"][i].transform.localPosition = new Vector3(20 + i * (tacle * 100 + 5), Stats["tacle"][i].transform.localPosition.y, Stats["tacle"][i].transform.localPosition.z);
                Stats["tacle"][i].GetComponent<CanvasRenderer>().SetAlpha(0.3f);
                Stats["esquive"][i].transform.localScale = new Vector3(esquive, Stats["esquive"][i].transform.localScale.y, Stats["esquive"][i].transform.localScale.z);
                Stats["esquive"][i].transform.localPosition = new Vector3(20 + i * (esquive * 100 + 5), Stats["esquive"][i].transform.localPosition.y, Stats["esquive"][i].transform.localPosition.z);
                Stats["esquive"][i].GetComponent<CanvasRenderer>().SetAlpha(0.3f);
            }
            playername.text = p.Name;
        }
        private void SetAlpha(string stat, int nb)
        {
            for (int i = 0; i < nb; i++)
            {
                Stats[stat][i].GetComponent<CanvasRenderer>().SetAlpha(1);
            }
        }
        private void initializeDictionary(string key, int min, int max) // recupere les gameObject enfant du menu de "min" (compris) à "max" (exclu) et les ajoute au dictionnaire "stats"
        {
            GameObject[] component = new GameObject[max - min];
            for (int i = min; i < max; ++i)
                component[i - min] = transform.GetChild(i).gameObject;
            Stats.Add(key, component);
        }

		public override void OnStartAnimation(){
			this.Close ();
		}
    }
}
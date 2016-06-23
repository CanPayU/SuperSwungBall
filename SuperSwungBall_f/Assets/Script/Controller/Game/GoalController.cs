using UnityEngine;
using System.Collections;

namespace GameScene
{
    public class GoalController : MonoBehaviour
    {
        [SerializeField]
        private int team_id;
        public int Team
        {
            set { team_id = value; }
            get { return team_id; }
        }

        private GameObject main;

        // Use this for initialization
        void Start()
        {
            main = GameObject.Find("Main");
        }

        public void goal()
        {
            Debug.Log("GOAAL RPC of " + team_id);
            if (PhotonNetwork.inRoom)
            {
                Game.Instance.goal(team_id);
                main.GetComponent<Multi.MainController>().update_score();
            }
            else
            {
                Game.Instance.goal(team_id);
                main.GetComponent<Didacticiel.MainController>().update_score();
            }
        }
    }
}
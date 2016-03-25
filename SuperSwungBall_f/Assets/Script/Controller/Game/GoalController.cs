using UnityEngine;
using System.Collections;
using GameScene.Multi;
using GameScene.Solo;

namespace GameScene
{
    public class GoalController : MonoBehaviour
    {

        [SerializeField] private int team_id;
        public int Team
        {	set { team_id = value; } }

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
				main.GetComponent<MainController>().update_score();
            }
            else {
                Game.Instance.goal(team_id);
                main.GetComponent<Main_Controller>().update_score();
            }
        }
    }
}


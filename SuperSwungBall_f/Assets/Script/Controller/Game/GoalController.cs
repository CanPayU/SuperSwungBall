using UnityEngine;
using System.Collections;
using GameScene.Multi;
using GameScene.Solo;

namespace GameScene
{
    public class GoalController : MonoBehaviour
    {

        [SerializeField]
        private int team_id;
        public int Team
        {
            set
            {
                team_id = value;
            }
        }

        private GameObject main;

        // Use this for initialization
        void Start()
        {
            main = GameObject.Find("Main");
        }

        public void goal()
        {


            Debug.Log("GOAAL RPC");
            if (PhotonNetwork.inRoom)
            {
                PhotonView pv = PhotonView.Get(this); // Impossible car GoalController n'a pas de PhotonView
                Debug.Log(pv);
                Debug.Log(team_id);
                pv.RPC("Goal_RPC", PhotonTargets.All, team_id);
            }
            else {
                Game.Instance.goal(team_id);
                main.GetComponent<Main_Controller>().update_score();
            }


        }

        [PunRPC]
        private void Goal_RPC(int teamID) //event collison
        {
            Debug.Log("GOAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAL");
            Game.Instance.goal(team_id);
            main.GetComponent<MainController>().update_score();
        }
    }
}


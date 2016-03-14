using UnityEngine;
using System.Collections;

namespace GameScene.Solo
{
    public class GoalController : MonoBehaviour
    {

        [SerializeField]
        private int team_id;

        private GameObject main;

        // Use this for initialization
        void Start()
        {
            main = GameObject.Find("Main");
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void goal()
        {
            Debug.Log("GOAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAL");
            Game.Instance.goal(team_id);
			Main_Controller controller = main.GetComponent<Main_Controller>();
            controller.update_score();
        }
    }
}


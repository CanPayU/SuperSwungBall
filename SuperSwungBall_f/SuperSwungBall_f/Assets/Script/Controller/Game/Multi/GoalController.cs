using UnityEngine;
using System.Collections;

namespace GameScene.Multi
{
    public class GoalController : MonoBehaviour
    {
        private int team_id;
		public int Team {
			set {
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
            Game.Instance.goal(team_id);
            MainController controller = main.GetComponent<MainController>();
            controller.update_score();
        }
    }
}


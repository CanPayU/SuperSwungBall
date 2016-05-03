using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace GameScene.Multi
{
    public class EndController : MonoBehaviour
    {

        [SerializeField]
        private Text status;
        [SerializeField]
        private Text points;
        [SerializeField]
        private Text content;
        [SerializeField]
        private GameObject panel;
        [SerializeField]
        private GameObject btn_quit;
        [SerializeField]
        private string scene;

        private string status_text = "";
        private string points_text = "Points : ";
        private string content_text = "";
        private Color status_color;
        private bool win = false;
        private int score = 0;

        private PhotonPlayer local_player;
        private PhotonPlayer other_player;

		private User ennemy;

        // Use this for initialization
        void Start()
        {
            panel.SetActive(false);
            other_player = PhotonNetwork.otherPlayers[0];
            local_player = PhotonNetwork.player;
			ennemy = (User)other_player.allProperties ["User"];
        }

        public void on_end(End type)
        {
            Game.Instance.isFinish = true;
            switch (type)
            {
			case End.ABANDON:
				OnAbandon();
				break;
			case End.TIME:
				OnPointMax();
                break;
            }

            if (PhotonNetwork.room.visible)
			{
				calculatePoint ();
				if (this.win)
					Sync(type != End.ABANDON);
                points_text += "" + score;
            }
            else
            {
                points_text = "Partie privee";
            }

            status.text = status_text;
            points.text = points_text;
            content.text = content_text;

            status.color = status_color;
            panel.SetActive(true);
        }

		private void calculatePoint(){
			//int newPhi = User.Instance.phi;
			if (!win)
			{
				if (ennemy.score > User.Instance.score) {
					this.score = -50; //newPhi += 600;
				}
				else {
					this.score = -150; //newPhi += 250;
				}
			}
			else
			{
				if (ennemy.score > User.Instance.score) {
					this.score = 200; //newPhi += 1200;
				} else {
					this.score = 60; //newPhi += 800;
				}
			}
		}

		private void Sync(bool respondAtEnnemy)
        {
			HTTP.WinGame (Game.Instance.MyTeam.Points, ennemy.username, Game.Instance.EnnemyTeam.Points, (success) => {
				Debug.Log("HTTP : " + success);
				if(respondAtEnnemy) {
					PhotonView pv = PhotonView.Get(this);
					pv.RPC("OnServerUpdated", PhotonTargets.Others);
					DestroyPlayers();
					Debug.Log("RPC sended and player destroyed");
				}
				btn_quit.SetActive(true);
			});
        }

		[PunRPC]
		private void OnServerUpdated()
		{
			Debug.Log("RPC received and player destroyed");
			HTTP.SyncUser ((success) => {
				if (!success)
					Debug.Log("Error Sync");
				Debug.Log("User Synced");
				btn_quit.SetActive(true);
			});
			DestroyPlayers();
		}

        void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            if (!Game.Instance.isFinish)
            {
                on_end(End.ABANDON);
            }
        }

		private void OnAbandon() {
			this.win = true;
			this.status_text = "Victoire";
			this.status_color = new Color(92f / 255f, 184f / 255f, 92f / 255f);
			this.content_text = User.Instance.username + "\n VS \n" + this.ennemy.username + " - " + this.ennemy.score + " - Abandon";
		}

        private void OnPointMax()
        {
            int myScore = Game.Instance.MyTeam.Points;
            int otherScore = Game.Instance.EnnemyTeam.Points;
            if (myScore > otherScore)
            {
				this.win = true;
				this.status_text = "Victoire";
				this.status_color = new Color(92f / 255f, 184f / 255f, 92f / 255f);
            }
            else
            {
				this.status_text = "Defaite";
				this.status_color = new Color(212f / 255f, 85f / 255f, 83f / 255f);
            }
			this.content_text = User.Instance.username + "\n VS \n" + this.ennemy.username + " - " + this.ennemy.score + " - Abandon";
        }

        private void DestroyPlayers()
        {
            foreach (var item in GameObject.FindGameObjectsWithTag("Player"))
            {
                Destroy(item);
            }
        }

        public void exit()
        {
			FadingManager.Instance.Fade(scene);
        }
    }
}
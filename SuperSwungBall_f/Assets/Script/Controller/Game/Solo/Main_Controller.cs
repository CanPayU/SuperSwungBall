using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using GameKit;
using GameScene.Replay;

namespace GameScene.Solo
{
	public class Main_Controller : GameScene.BasicMainController
    {

		public Main_Controller(){
			this.eventType = GameKit.EventType.Global;
		}

		// Use this for initialization
		protected override void Start()
		{
			this.playerController = typeof(GameScene.Solo.PlayerController);
			base.Start ();
		}

        // Update is called once per frame
        protected override void Update()
		{
			base.Update ();
			updateTimerText ();
        }

		private void updateTimerText(){
			if (this.annim_started) {
				this.timerText.text = string.Empty;
				return;
			}
			this.timerText.text = (this.time.Time_remaining).ToString ("0");
			if (this.time.Time_remaining < 10f)
				this.timerAnimator.Play ("EndTimer");
			else
				this.timerAnimator.Play ("Empty");
		}
    }

}

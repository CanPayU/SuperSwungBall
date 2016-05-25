using UnityEngine;
using System;


namespace GameScene.Instantiator {


	public class InstantiatorController : MonoBehaviour
	{
		[SerializeField]
		private InstanciatorType type;

		private Type solo;
		private Type multi;
		private Type replay;

		private bool instanciated = false;

		private bool gameIsReplay = false;
		private bool gameIsSolo = true;
		private bool gameIsMulti = true;

		// Use this for initialization
		public void Awake () {
			if (instanciated)
				return;

			initialiseType();

			if (gameIsReplay) 
				gameObject.AddComponent (this.replay);
			else if (gameIsSolo)
				gameObject.AddComponent (this.solo);
			else if (gameIsMulti)
				gameObject.AddComponent (this.multi);

		}

		private void initialiseType(){
			if (type == InstanciatorType.MainController) {
				this.solo = typeof(GameScene.Solo.Main_Controller);
				this.multi = typeof(GameScene.Multi.MainController);
				this.replay = typeof(GameScene.Replay.MainController);
			}
//			else if (type == InstanciatorType.PlayerController) {
//				this.instanciated = true;
//				this.solo = typeof(GameScene.Solo.PlayerController);
//				this.multi = typeof(GameScene.Multi.PlayerController);
//				this.replay = typeof(GameScene.Replay.PlayerController);
//			}
		}

	}

	public enum InstanciatorType {
		MainController,
		PlayerController
	}

}
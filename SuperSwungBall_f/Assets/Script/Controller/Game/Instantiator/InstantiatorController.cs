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

//		private bool gameIsReplay = false;
//		private bool gameIsSolo = false;
//		private bool gameIsMulti = true;

		// Use this for initialization
		public void Awake () {
			if (instanciated)
				return;

			initialiseType();

			if (type == InstanciatorType.Replay) 
				gameObject.AddComponent (this.replay);
			else if (type == InstanciatorType.Solo) 
				gameObject.AddComponent (this.solo);
			else if (type == InstanciatorType.Multi) 
				gameObject.AddComponent (this.multi);

		}

		private void initialiseType(){
			this.solo = typeof(GameScene.Solo.Main_Controller);
			this.multi = typeof(GameScene.Multi.MainController);
			this.replay = typeof(GameScene.Replay.MainController);

		}

	}

	public enum InstanciatorType {
		Replay,
		Solo,
		Multi
	}

}
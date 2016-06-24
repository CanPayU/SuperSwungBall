using UnityEngine;
using System;

using GameScene;

namespace GameScene.Instantiator
{


    public class InstantiatorController : MonoBehaviour
    {
        [SerializeField]
        private GameType type;

        private Type solo;
        private Type multi;
        private Type replay;

		private Multi.ChatController chat;
		private Multi.EndController end;

        private bool instanciated = false;

        //		private bool gameIsReplay = false;
        //		private bool gameIsSolo = false;
        //		private bool gameIsMulti = true;

        // Use this for initialization
        public void Awake()
        {
            if (instanciated)
                return;

            initialiseType();

            //if (ApplicationModel.TypeToInstanciate != null)
            instanciateMain(ApplicationModel.TypeToInstanciate);
            //else
            //	instanciateMain (type);

        }

        private void initialiseType()
        {
            this.solo = typeof(Solo.Main_Controller);
            this.multi = typeof(Multi.MainController);
            this.replay = typeof(Replay.MainController);
        }

        private void instanciateMain(GameType type)
        {
			type = GameType.Solo;
			if (type == GameType.Replay) {
				gameObject.AddComponent (this.replay);
				this.end = GetComponent<Multi.EndController> ();
				this.chat = GetComponent<Multi.ChatController> ();
				end.enabled = false;
				chat.enabled = false;
			}
			else if (type == GameType.Solo)
				gameObject.AddComponent (this.solo);
			else if (type == GameType.Multi) {
				gameObject.AddComponent (this.multi);
				//gameObject.AddComponent (typeof(GameScene.Multi.EndController));
				//gameObject.AddComponent (typeof(GameScene.Multi.ChatController));
			}

            this.instanciated = true;
        }

    }
}
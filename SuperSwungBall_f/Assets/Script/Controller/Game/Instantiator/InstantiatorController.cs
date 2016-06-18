using UnityEngine;
using System;


namespace GameScene.Instantiator
{


    public class InstantiatorController : MonoBehaviour
    {
        [SerializeField]
        private GameType type;

        private Type solo;
        private Type multi;
        private Type replay;

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
            this.solo = typeof(GameScene.Solo.Main_Controller);
            this.multi = typeof(GameScene.Multi.MainController);
            this.replay = typeof(GameScene.Replay.MainController);
        }

        private void instanciateMain(GameType type)
        {
            if (type == GameType.Replay)
                gameObject.AddComponent(this.replay);
            else if (type == GameType.Solo)
                gameObject.AddComponent(this.solo);
            else if (type == GameType.Multi)
                gameObject.AddComponent(this.multi);

            this.instanciated = true;
        }

    }
}
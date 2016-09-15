using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using GameKit;

namespace GameScene.Replay
{

    public class MainController : GameScene.BasicMainController
    {

        private Replay replay;

        private GameObject ballPrefab;

        public MainController()
        {
            this.eventType = GameKit.EventType.Global;
        }

        // Use this for initialization
        protected override void Start()
        {
            this.ballPrefab = Resources.Load("Prefabs/Resources/Ball") as GameObject;
            this.playerController = typeof(GameScene.Replay.PlayerController);
            this.replay = SaveLoad.load_replay(ApplicationModel.replayName);
            this.replay.resetActualValue();
            Game.Instance = this.replay.Game;
            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.K))
                Debug.Log(replay);
        }

        protected override void end_time()
        {
            this.time.reset();
            Caller.StartAnimation();
        }

        protected override void instantiate_team()
        {
            base.instantiate_team();
            GameObject ball = Instantiate(ballPrefab, new Vector3(0, 0.5F, -0), Quaternion.identity) as GameObject;
            ball.name = "Ball";
        }

        // ------- Event
        public override void OnStartAnimation()
        {
            this.replay.nextRound();
            base.OnStartAnimation();
        }
        public override void OnStartReflexion() { }


        public PlayerAction getPlayerAction(Player player)
        {
            return this.replay.ActualRound.getPlayerAction(player);
        }
    }
}
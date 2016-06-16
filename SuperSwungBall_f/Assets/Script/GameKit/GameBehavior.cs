using UnityEngine;
using System.Collections;

using GameScene;

namespace GameKit
{

    public class GameBehavior : MonoBehaviour, IGameListener
    {

        /// <summary> Permet d'appeler un évennement. </summary>
        internal Call Caller = null;

        private ListenerManager listener;

        internal EventType eventType = EventType.Local;

        void Awake()
        {
            this.listener = new ListenerManager(this, this.eventType, this.gameObject, true);
            this.Caller = new GameBehavior.Call(this);
        }

        void OnDisable()
        {
            if (listener.Listen)
                this.listener.StopListening();
        }

        void OnEnable()
        {
            if (!listener.Listen)
                this.listener.StartListening();
        }

        ///
        /// Event
        ///

        public virtual void OnStartAnimation() { }
        public virtual void OnStartReflexion() { }
        public virtual void OnGoal(GoalController g) { }
        public virtual void OnEndGame(GameScene.Multi.End type) { }

        public virtual void OnSucceedAttack(Player pl) { }
        public virtual void OnSucceedEsquive(Player pl) { }
        public virtual void OnFailedAttack(Player pl) { }
        public virtual void OnFailedEsquive(Player pl) { }


        ///
        /// Call Event
        ///

        internal class Call
        {

            private GameBehavior parent;

            public Call(GameBehavior parent)
            {
                this.parent = parent;
            }

            internal void StartAnimation()
            {
                callListeners("OnStartAnimation", null, EventType.Global);
            }

            internal void StartReflexion()
            {
                callListeners("OnStartReflexion", null, EventType.Global);
            }

            internal void Goal(GoalController g)
            {
                callListeners("OnGoal", new object[] { g }, EventType.Global);
            }

            // ---

            internal void EndGame(GameScene.Multi.End type)
            {
                callListeners("OnEndGame", new object[] { type }, EventType.Global);
            }

            internal static void OnEndGame(GameScene.Multi.End type)
            {
                CallListeners("OnEndGame", new object[] { type }, EventType.Global);
            }

            internal void SuccessEsquive(Player p, bool external = true)
            {
                var obj = new object[] { p };
                if (external)
                    callListeners("OnSucceedEsquive", obj, EventType.External);
                callListeners("OnSucceedEsquive", obj, EventType.Local);
            }

            internal void SuccessAttack(Player p, bool external = true)
            {
                var obj = new object[] { p };
                if (external)
                    callListeners("OnSucceedAttack", obj, EventType.External);
                callListeners("OnSucceedAttack", obj, EventType.Local);
            }

            internal void FailedAttack(Player p, bool external = false)
            {
                var obj = new object[] { p };
                if (external)
                    callListeners("OnFailedAttack", obj, EventType.External);
                callListeners("OnFailedAttack", obj, EventType.Local);
            }

            internal void FailedEsquive(Player p, bool external = false)
            {
                var obj = new object[] { p };
                if (external)
                    callListeners("OnFailedEsquive", obj, EventType.External);
                callListeners("OnFailedEsquive", obj, EventType.Local);
            }

            private void callListeners(string methodName, object[] parameters, EventType type)
            {
                foreach (var l in ListenerManager.getListeners(type, this.parent.gameObject))
                {
                    typeof(IGameListener).GetMethod(methodName).Invoke(l, parameters);
                }
            }

            private static void CallListeners(string methodName, object[] parameters, EventType type)
            {
                foreach (var l in ListenerManager.getListeners(type, null))
                {
                    typeof(IGameListener).GetMethod(methodName).Invoke(l, parameters);
                }
            }
        }

    }

}
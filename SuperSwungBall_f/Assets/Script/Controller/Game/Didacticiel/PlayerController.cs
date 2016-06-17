using UnityEngine;
using System.Collections.Generic;

using GameKit;

namespace GameScene.Didacticiel
{
    public class PlayerController : BasicPlayerController
    {
        public PlayerController()
        {
            this.eventType = GameKit.EventType.All;
        }
        public MenuController menucontroller
        {
            get { return menuController; }
        }
    }
}
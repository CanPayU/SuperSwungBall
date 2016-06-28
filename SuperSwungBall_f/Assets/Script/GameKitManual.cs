using UnityEngine;
using System.Collections;

namespace Manual
{


    /* 			COMMENT UTILISER GAMEKIT
	 * 
	 * Introduction
	 * 1 - Local Event
	 * 2 - Global Event
	 * 3 - Listen Event
	 * 4 - Call Event
	 * 5 - Tout les évennements
	 * Kiss
	 * 
	 */


    // --- INTRODUCTION

    // Il existe 2 types d'events :  Lacal et Global
    // Ils sont symbolisés par un enum :
    // 		EventType :
    //			- All		Ecoute Global et Local
    // 			- Local
    // 			- Global
    // Pour qu'une class gère les events, elle doit hériter de GameBehavior et non de MonoBehaviour. (Oui j'ai oublié le u ^^)
    // Pensez à importer GameKit avec : using GameKit;

    // Exemple :
    using GameKit;
    class MyClass : GameBehavior
    {
        // Je peux utiliser mes events
    }




    // 1.
    // --- Local Event

    // Les LocalEvents sont des évennemets interne au gameObject
    // Si tu déclenches un LocalEvent, il sera appelé uniquement dans les scripts du gameObject
    // Concrètement :
    // 		Un player contient plusieurs scripts : CollisionController, PlayerController, ...
    //		Si dans le CollisionController tu appels l'évennement "Tacle Réussi", seul les scripts du gameObject qui a réussi le tacle recoivent l'évennement

    // Exemple :
    class CollisionController : GameBehavior
    {
        // J'appel "Tacle Réussi"
    }
    class PlayerController : GameBehavior
    {
        void TacleReussi()
        {
            // J'ai réussi mon tacle
        }
    }




    // 2.
    // --- Global Event

    // Les GlobalEvents sont des évennemets globaux (logique ...)
    // Si tu déclenches un GlobalEvent, il sera appelé sur tout les scripts qui ecoutent les GlobalEvents
    // Pour qu'un script écoute les GlobalEvents, il faut lui spécifier

    // Exemple :
    class MyGlobalEventScript : GameBehavior
    {
        public MyGlobalEventScript()
        { // Constructor
            this.eventType = GameKit.EventType.Global; // All pour écouter les deux
        }
        // Le script écoute maintenant les GlobalEvents.
    }




    // 3.
    // --- Listen Event

    // Pour écouter un évennement il suffit d'Override l'event.

    // Exemple :
    class MyClass2 : GameBehavior
    {
        public override void OnFailedEsquive(Player p)
        {
            // Executé lorsque l'event est call
        }
    }




    // 4.
    // --- Call Event

    // Pour déclencher un event au près des autres scripts
    // Utilisez le Caller.

    // Exemple :
    class MyClass3 : GameBehavior
    {
        void myMethod()
        {
            Caller.FailedEsquive(null); // Declenche l'evennement OnFailedEsquive
        }
    }




    // 5.
    // --- Tout les évennements

    // Tout les evennements sont disponible et commenté dans Assets/Script/GameKit/IGameListener
    // Il est spécifié si c'est un GlobalEvent ou LocalEvent.



    // 		KISS
}
using UnityEngine;
using System.Collections;

public enum KeyboardAction
{
    Passe
}

public enum NotificationState
{
    // Les notifications système ne sont pas concernées (force = true)
    All = 5,        // Tout
    Private = 2,    // Tout sauf invitation
    Nothing = 0		// Rien
}

public enum NotificationType
{
    Text = 5,  		// Title, Content, Completion<bool, string>
    Alert = 4,      // Title, Content, Completion<bool, null>
    SimpleAlert = 3,// Title, Content, Completion<null, null>
    Box = 2,    	// Title, Content
    Slide = 1 		// Title
}

public enum GameType
{
    Replay,
    Multi,
    Solo
}

public enum PlayerType
{
    Buy,            // Purchasable
    Secret,         // In Chest
    Challenge		// Challenge permanent
}

public enum SoundState
{
    All,            // Tout
    Effect,         // Effet
    Musique,        // Musique
    Nothing			// Rien
}
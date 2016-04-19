using UnityEngine;
using System.Collections;

public enum KeyboardAction
{
    Passe
}

public enum NotificationState
{
	// Les notifications système ne sont pas concernées (force = true)
    All = 3,        // Tout
    Private = 2,    // Tout sauf invitation
    Nothing = 0		// Rien
}

public enum NotificationType
{
    Alert = 3,  	// Title, Content, Completion
    Box = 2,    	// Title, Content
    Slide = 1 		// Title
}

public enum PlayerType
{
	Buy,			// Purchasable
	Secret			// In Chest
}
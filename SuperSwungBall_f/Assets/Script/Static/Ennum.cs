using UnityEngine;
using System.Collections;

public enum KeyboardAction {
	Passe
}

public enum NotificationState : int {
	// Les notifications système ne sont pas concernées
	All = 10,		// Tout
	Private = 2,	// Tout sauf invitation
	Nothing = 0		// Rien
}

public enum NotificationType : int {
	Alert = 3, 	// Title, Content, Completion
	Box = 2, 	// Title, Content
	Slide = 1 	// Title
}
using UnityEngine;
using System.Collections;

namespace GameKit {

	public enum EventType {
		All = 3,		// Ecoute tout les évennements
		Global = 2,	// Ecoute seulement les évennement globaux
		Local = 1		// Ecoute seulement les évennement du gameObject
	}

}

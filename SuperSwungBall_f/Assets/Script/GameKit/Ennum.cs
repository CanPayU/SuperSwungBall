using UnityEngine;
using System.Collections;

namespace GameKit
{

    public enum EventType
    {
        All = 4,        // Ecoute tout les locaux et globaux
        Local = 3,      // Ecoute seulement les évennement du gameObject
        Global = 2,     // Ecoute seulement les évennement globaux
        External = 1    // Ecoute les evennement locaux de tous les gameObject
    }

}

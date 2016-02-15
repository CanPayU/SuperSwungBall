using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    class Player
    {
        private Dictionary<string, int> defaultStats = new Dictionary<string, int>();
        private List<string> buttonsValues = new List<string> { "esquive","esquive","esquive" };
        private Dictionary<string, int> finalStats = new Dictionary<string, int> { { "esquive", 0 }, { "tacle", 0 }, { "passe", 0 }, { "course", 0 } };

        public Player(int tacle, int esquive, int passe, int course)
        {
            defaultStats.Add("esquive", esquive);
            defaultStats.Add("tacle", tacle);
            defaultStats.Add("passe", passe);
            defaultStats.Add("course", course);
            initialize_finaleStats();
        }

        public void updateValues(string value)
        {
            buttonsValues.Add(value);
            buttonsValues.RemoveAt(0);
            computeStats();
        }
        public void computeStats()
        {
            initialize_finaleStats();
            foreach (string s in buttonsValues)
            {
                finalStats[s] += defaultStats[s];
            }
        }
         private void initialize_finaleStats()
        {
            finalStats["tacle"] = 0;
            finalStats["esquive"] = 0;
            finalStats["passe"] = 0;
            finalStats["course"] = defaultStats["course"] + 10;
        }


        public float Speed
        {
            get { return (float)finalStats["course"] / 10; }
        }
        public float ZoneDeplacement
        {
            get { return (float)finalStats["course"] / 10; }
        }
        public float ZonePasse
        {
            get { return (float)finalStats["passe"] / 10; }
        }

    }
}

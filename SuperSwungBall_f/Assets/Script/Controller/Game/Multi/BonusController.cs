using UnityEngine;
using System.Collections;
using GameKit;

namespace GameScene.Multi
{
    public class BonusController : GameBehavior
    {

        [SerializeField]
        GameObject tacle;
        [SerializeField]
        GameObject esquive;
        [SerializeField]
        GameObject passe;
        [SerializeField]
        GameObject course;

        public string[] setBonus()
        {
            string[] bonusNames = new string[3];
            for (int i = 0; i < 3; i++)
            {
                bonusNames[i] = addBonus(i + 1);
            }
            return bonusNames;
        }

        private string addBonus(int place)
        {
            int r = (int)(Random.value * 4);
            GameObject bonus = new GameObject();
            string bonusName = "";
            Debug.Log("r = " + r);
            switch (r)
            {
                case 0:
                    bonus = Instantiate(tacle);
                    bonusName = "tacle";
                    break;
                case 1:
                    bonus = Instantiate(esquive);
                    bonusName = "equive";
                    break;
                case 2:
                    bonus = Instantiate(passe);
                    bonusName = "passe";
                    break;
                case 3:
                    bonus = Instantiate(course);
                    bonusName = "course";
                    break;
                default:
                    break;
            }
            bonus.transform.SetParent(transform.FindChild("bonus" + place));
            bonus.transform.localPosition = new Vector3(0, 0, 0);
            bonus.transform.localEulerAngles = new Vector3(0, 0, 0);
            bonus.transform.localScale = new Vector3(1, 1, 1);
            return bonusName;
        }

        public void destroyBonus(int place)
        {
            if(transform.FindChild("bonus" + place).childCount != 0)
            {
                Destroy(transform.FindChild("bonus" + place).GetChild(0).gameObject);
            }
        }
    }
}
using UnityEngine;
using System.Collections;
using GameKit;
namespace GameScene.Multi
{
    public class MainBonusController : GameBehavior
    {
        [SerializeField]
        GameObject tripleBonus;
        BonusController myBonusController;

        GameObject bonusInstance;

        //string[] bonus;
        int count; //nb de bonus utilisées
        bool onReflexion;
        public MainBonusController()
        {
            this.eventType = GameKit.EventType.All;
        }

        void Start()
        {
            count = 3;
            //bonus = new string[3];
            onReflexion = false;
            bonusInstance = Instantiate(tripleBonus);
            myBonusController = bonusInstance.GetComponent<BonusController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(1) && onReflexion && count < 3)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit, 100);
                if (hit.collider.tag == "Player")
                {
                    BasicPlayerController myPlayerController = hit.collider.gameObject.GetComponent<BasicPlayerController>();
                    if (myPlayerController.IsMine)
                    {
                        //myPlayerController.bonus(bonus[count]);
                        count++;
                        myBonusController.destroyBonus(count);
                    }
                }
            }
        }

        public override void OnStartReflexion()
        {
            count = 0;
            onReflexion = true;
            //bonus = myBonusController.setBonus();
        }
        public override void OnStartAnimation()
        {
            for (int i = 1; i < 4; i++)
            {
                myBonusController.destroyBonus(i);
            }
            onReflexion = false;
        }
    }
}

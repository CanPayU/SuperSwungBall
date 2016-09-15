using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;


namespace Gestion
{
    public class MainController : MonoBehaviour
    {
        private SlideController slide;

        // Use this for initialization
        void Start()
        {
            this.slide = GameObject.Find("ScrollView").GetComponent<SlideController>();
            Settings.Instance.ResetSpecialPlayer();
            HTTP.SwungMens((success, json) =>
            {
                if (success)
                    Instanciate(json);
                else
                    Debug.LogError("Impossible de charger les SungMens");
            });
        }

        private void Instanciate(JSONArray json)
        {
            slide.InstanciateChest();
            foreach (JSONValue swungMen in json)
            {
                JSONObject obj = swungMen.Obj;
                Player player = new Player(obj);
                Settings.Instance.AddOrUpdate_Player(player);
                if (player.Type == PlayerType.Buy)
                    slide.InstanciatePlayer(player);
                else if (player.Type == PlayerType.Challenge)
                    slide.InstanciateChallenge(player);
            }
        }
    }
}
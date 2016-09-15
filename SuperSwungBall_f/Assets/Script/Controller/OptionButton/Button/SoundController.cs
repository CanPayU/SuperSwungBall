using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

using Extension;
using TranslateKit;


namespace OptionButton
{

    public class SoundController : MonoBehaviour
    {

        private Button btn;
        private SoundState actualSoundState;

        private int pressId = 0;
        private Dictionary<string, string> transKeys = new Dictionary<string, string>()
        {
            {"default", Language.GetValue(TradValues.General.Song)},
            {"state1", Language.GetValue(TradValues.General.All)},
            {"state2", Language.GetValue(TradValues.General.Music)},
            {"state3", Language.GetValue(TradValues.General.Effect)},
            {"state4", Language.GetValue(TradValues.General.Nothing)}
        };

        // Use this for initialization
        void Start()
        {
            this.btn = gameObject.GetComponent<Button>();
            actualSoundState = Settings.Instance.SoundState;
            if (this.btn != null)
            {
                this.btn.onClick.AddListener(delegate ()
                    {
                        OnChangeSoundState();
                    });
                updateView();
                this.btn.EditText(this.transKeys["default"]);
            }
        }

        void OnDisable()
        {
            if (this.btn != null)
                this.btn.EditText(this.transKeys["default"]);
        }

        private void OnChangeSoundState()
        {
            this.pressId++;
            actualSoundState = Next<SoundState>(actualSoundState);
            Settings.Instance.SoundState = actualSoundState;
            SaveLoad.save_setting();
            updateView();
            StartCoroutine(defaultSetUp(this.pressId));
        }

        private IEnumerator defaultSetUp(int id)
        {
            yield return new WaitForSeconds(5);
            if (id == this.pressId)
                this.btn.EditText(this.transKeys["default"]);
        }

        private void updateView()
        {
            switch (this.actualSoundState)
            {
                case SoundState.All:
                    this.btn.colors = Colors.Block.Green;
                    this.btn.EditText(this.transKeys["state1"]);
                    AudioListener.pause = false;
                    break;
                case SoundState.Effect:
                    this.btn.colors = Colors.Block.Orange;
                    this.btn.EditText(this.transKeys["state2"]);
                    break;
                case SoundState.Musique:
                    this.btn.colors = Colors.Block.Orange;
                    this.btn.EditText(this.transKeys["state3"]);
                    break;
                case SoundState.Nothing:
                    this.btn.colors = Colors.Block.Red;
                    this.btn.EditText(this.transKeys["state4"]);
                    AudioListener.pause = true;
                    break;
                default:
                    break;
            }
        }

        private static T Next<T>(T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argumnent {0} is not an Enum", typeof(T).FullName));
            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }
    }
}



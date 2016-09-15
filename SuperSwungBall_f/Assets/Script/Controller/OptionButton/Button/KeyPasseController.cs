using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Extension;
using TranslateKit;

namespace OptionButton
{

    public class KeyPasseController : MonoBehaviour
    {

        private Button btn;
        private KeyCode actualKeyCode;
        private KeyboardAction action = KeyboardAction.Passe;
        private Animator animator;

        private int pressId = 0;
        private Dictionary<string, string> transKeys = new Dictionary<string, string>()
        {
            {"default", Language.GetValue(TradValues.Menu.TouchPass)},
            {"cancel", Language.GetValue(TradValues.General.Cancel)},
            {"sync", Language.GetValue(TradValues.General.Sync)}
        };
        private bool listening = false;

        // Use this for initialization
        void Start()
        {
            this.animator = GetComponent<Animator>();
            this.btn = GetComponent<Button>();
            this.actualKeyCode = Settings.Instance.Keyboard[this.action];
            if (this.btn != null)
            {
                this.btn.onClick.AddListener(delegate ()
                    {
                        OnChange();
                    });
                this.btn.EditText(this.transKeys["default"]);
            }
        }

        void OnDisable()
        {
            if (this.btn != null)
                this.btn.EditText(this.transKeys["default"]);
            this.listening = false;
        }

        void OnGUI()
        {
            if (!this.listening)
                return;
            Event e = Event.current;
            if (e.isKey && e.keyCode != KeyCode.None)
            {
                endListenning(e.keyCode);
            }
        }

        private void endListenning(KeyCode code)
        {
            this.animator.Play("Empty");
            this.listening = false;
            if (code != KeyCode.None)
            {
                this.actualKeyCode = code;
                this.btn.EditText(actualKeyCode.ToString());
                Settings.Instance.UpdateKeyboard(this.action, this.actualKeyCode);
                SaveLoad.save_setting();
            }
            else
                this.btn.EditText(this.transKeys["cancel"]);
            this.pressId++;
            StartCoroutine(defaultSetUp(this.pressId));
        }

        private void OnChange()
        {
            this.listening = !this.listening;
            if (!this.listening)
            {
                endListenning(KeyCode.None);
            }
            else
            {
                this.animator.Play("LoadColor");
                this.btn.EditText(this.transKeys["sync"]);
            }
        }

        private IEnumerator defaultSetUp(int id)
        {
            yield return new WaitForSeconds(5);
            if (id == this.pressId)
                this.btn.EditText(this.transKeys["default"]);
        }
    }
}

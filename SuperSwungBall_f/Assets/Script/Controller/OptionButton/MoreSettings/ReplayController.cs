using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;


namespace OptionButton.MoreSettings
{

    public class ReplayController : MonoBehaviour
    {

        private SwitchController switchReplay;
        private Button emptyBackup;

        // Use this for initialization
        void Start()
        {
            //Transform content = transform.Find ("Scroll View").Find ("Viewport").Find ("Content");
            this.switchReplay = transform.Find("Container").Find("SaveEnabled").Find("Switch").GetComponent<SwitchController>();
            this.switchReplay.AddListener(OnSwitchChange);
            this.switchReplay.Value = Settings.Instance.AllowReplayBackup;
            this.emptyBackup = transform.Find("Container").Find("EmptyBackup").Find("Button").GetComponent<Button>();
            this.emptyBackup.onClick.AddListener(delegate ()
            {
                OnEmptyBackup();
            });
        }

        private void OnEmptyBackup()
        {
            Debug.Log("Empty backups");
            this.emptyBackup.interactable = false;
            var filesPath = Directory.GetFiles(Application.persistentDataPath + "/replay/");
            foreach (var filePath in filesPath)
            {
                File.Delete(filePath);
            }
        }

        private void OnSwitchChange(bool value)
        {
            Settings.Instance.AllowReplayBackup = value;
            SaveLoad.save_setting();
        }

    }
}
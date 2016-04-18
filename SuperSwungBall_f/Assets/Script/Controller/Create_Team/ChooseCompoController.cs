using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChooseCompoController : MonoBehaviour
{
    [SerializeField]
    private Dropdown dropdown;
    private Composition[] compo_;

    // Use this for initialization
    void Start()
    {

        Dictionary<string, Composition> dict = Settings.Instance.Default_compo;
        compo_ = new Composition[dict.Count];
        dropdown.options.Clear();

        int i = 0;
        foreach (KeyValuePair<string, Composition> compo in dict)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = compo.Value.Name });
            compo_[i] = compo.Value;
            i++;
        }

        dropdown.value = 0;
        dropdown.captionText.text = compo_[dropdown.value].Name;
    }

    public Composition GetChoice()
    {
        return compo_[dropdown.value];
    }
}
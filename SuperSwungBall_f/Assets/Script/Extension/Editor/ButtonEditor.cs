using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(UnityEngine.UI.Button))]
public class ButtonEditor : Editor
{

    public override void OnInspectorGUI()
    {
        Debug.Log("resetted");
        GUILayout.Label(" Grid Width ");
        if (GUILayout.Button("Reset"))
        {
            Debug.Log("resetted");
        }

        // Draw the default inspector first.
        //DrawDefaultInspector();

        if (GUI.changed)
        {
            OnModified();
        }
    }
    private void OnModified()
    {
        Debug.Log("Inspector modified: " + target.name);
    }
}




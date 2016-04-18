using UnityEngine;
using UnityEditor;

namespace GameScene.Multi
{

    [CustomEditor(typeof(ChatController))]
    public class ChatCEditor : Editor
    {

        private string value = "";
        private ChatController.Chat type;// = ChatController.Chat.EVENT;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();


            ChatController myTarget = (ChatController)target;
            EditorGUILayout.Separator();

            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel("Content");
                value = EditorGUILayout.TextField(value);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel("Type");
                type = (ChatController.Chat)EditorGUILayout.EnumPopup((System.Enum)type);
            }
            GUILayout.EndHorizontal();




            if (GUILayout.Button("Send Notification"))
            {
                myTarget.InstanciateMessage(value, type);
            }


            EditorGUILayout.Separator();

            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel("Content");
                value = EditorGUILayout.TextField(value);
            }
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Send Message Chat"))
            {
                myTarget.SendMessageToOtherClient(value);
            }
        }
    }
}

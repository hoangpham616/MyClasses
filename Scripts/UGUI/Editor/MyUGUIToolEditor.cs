/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIToolEditor (version 2.0)
 */

using UnityEngine;
using UnityEditor;

namespace MyClasses.UI.Tool
{
    public class MyUGUIToolEditor
    {
        /// <summary>
        /// Open UI Config ID.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Setup/Config ID")]
        public static void OpenConfigID()
        {
            EditorWindow.GetWindow(typeof(MyUGUIConfigIDEditorWindow));
        }

        /// <summary>
        /// Open UI Config Scene.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Setup/Config Scene")]
        public static void OpenConfigScene()
        {
            EditorWindow.GetWindow(typeof(MyUGUIConfigSceneEditorWindow));
        }

        /// <summary>
        /// Open UI Config Popup.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Setup/Config Popup")]
        public static void OpenConfigPopup()
        {
            EditorWindow.GetWindow(typeof(MyUGUIConfigPopupEditorWindow));
        }

        /// <summary>
        /// Create a game object with MyUGUIBooter attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Setup/Create MyUGUIBooter")]
        public static void CreateMyUGUIConfig()
        {
            GameObject obj = new GameObject("MyUGUIBooter");
            obj.AddComponent<MyUGUIBooter>();
            EditorGUIUtility.PingObject(obj);
            Selection.activeGameObject = obj.gameObject;
                     
            Debug.Log("[MyClasses] MyUGUIBooter was created.");
        }
    }
}
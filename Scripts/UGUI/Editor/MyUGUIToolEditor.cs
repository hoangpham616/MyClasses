/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIToolEditor (version 2.6)
 */

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MyClasses.UI.Tool
{
    public class MyUGUIToolEditor
    {
        #region ----- Config -----

        /// <summary>
        /// Open UI Config ID.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Config/Config ID", false, 1)]
        public static void OpenConfigID()
        {
            EditorWindow.GetWindow(typeof(MyUGUIConfigIDEditorWindow));
        }

        /// <summary>
        /// Open UI Config Scene.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Config/Config Scene", false, 12)]
        public static void OpenConfigScene()
        {
            EditorWindow.GetWindow(typeof(MyUGUIConfigSceneEditorWindow));
        }

        /// <summary>
        /// Open UI Config Popup.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Config/Config Popup", false, 13)]
        public static void OpenConfigPopup()
        {
            EditorWindow.GetWindow(typeof(MyUGUIConfigPopupEditorWindow));
        }

        /// <summary>
        /// Create a game object with MyUGUIBooter attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Config/Create MyUGUIBooter", false, 24)]
        public static void CreateMyUGUIConfig()
        {
            GameObject obj = new GameObject("MyUGUIBooter");

            obj.AddComponent<MyUGUIBooter>();

            EditorGUIUtility.PingObject(obj);
            Selection.activeGameObject = obj.gameObject;

            Debug.Log("[MyClasses] MyUGUIBooter was created.");
        }

        #endregion

        #region ----- GameObject -----

        /// <summary>
        /// Create a game object with MyUGUIButton attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/GameObject/Button", false, 1)]
        public static void CreateMyUGUIButton()
        {
            MyUGUIButton.CreateTemplate();

            Debug.Log("[MyClasses] Button was created.");
        }

        /// <summary>
        /// Create a game object with MyUGUIToggleButton attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/GameObject/Toggle Button", false, 2)]
        public static void CreateMyUGUIToggleButton()
        {
            MyUGUIToggleButton.CreateTemplate();

            Debug.Log("[MyClasses] Toggle Button was created.");
        }

        /// <summary>
        /// Create a game object with MyUGUIReusableListView attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/GameObject/Reusable List View", false, 13)]
        public static void CreateMyUGUIReusableListView()
        {
            MyUGUIReusableListView.CreateTemplate();

            Debug.Log("[MyClasses] ReusableListView was created.");
        }

        /// <summary>
        /// Create a game object with MyUGUIPieChart attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/GameObject/Pie Chart", false, 24)]
        public static void CreateMyUGUIPieChart()
        {
            MyUGUIPieChart.CreateTemplate();

            Debug.Log("[MyClasses] ReusableListView was created.");
        }

        /// <summary>
        /// Create a game object with MyUGUIRadarChart attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/GameObject/Radar Chart", false, 25)]
        public static void CreateMyUGUIRadarChart()
        {
            MyUGUIRadarChart.CreateTemplate();

            Debug.Log("[MyClasses] RadarChart was created.");
        }

        #endregion
    }
}

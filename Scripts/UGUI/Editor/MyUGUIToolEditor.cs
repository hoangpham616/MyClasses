/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIToolEditor (version 2.1)
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
            GameObject canvas = MyUtilities.FindObjectInRoot("Canvas");

            GameObject obj = new GameObject("Button");
            if (canvas != null)
            {
                obj.transform.SetParent(canvas.transform, false);
            }

            RectTransform contentRectTransform = obj.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref contentRectTransform, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 300, 100, 0, 0);
            obj.AddComponent<Image>();
            obj.AddComponent<MyUGUIButton>();

            GameObject text = new GameObject("Text");
            text.transform.SetParent(obj.transform, false);

            RectTransform textRectTransform = text.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref textRectTransform, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);
            Text textText = text.AddComponent<Text>();
            textText.text = "Button";
            textText.fontSize = 40;
            textText.supportRichText = false;
            textText.alignment = TextAnchor.MiddleCenter;
            textText.color = Color.black;
            textText.raycastTarget = false;

            EditorGUIUtility.PingObject(obj);
            Selection.activeGameObject = obj.gameObject;

            Debug.Log("[MyClasses] Button was created.");
        }

        /// <summary>
        /// Create a game object with MyUGUIReusableListView attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/GameObject/Reusable List View", false, 2)]
        public static void CreateMyUGUIReusableListView()
        {
            GameObject canvas = MyUtilities.FindObjectInRoot("Canvas");

            GameObject obj = new GameObject("ReusableListView");
            if (canvas != null)
            {
                obj.transform.SetParent(canvas.transform, false);
            }

            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(obj.transform, false);

            GameObject content = new GameObject("Content");
            content.transform.SetParent(viewport.transform, false);

            RectTransform contentRectTransform = content.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref contentRectTransform, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

            RectTransform viewportRectTransform = viewport.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref viewportRectTransform, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);
            viewport.AddComponent<Mask>();
            Image viewportImage = viewport.AddComponent<Image>();
            viewportImage.color = new Color(0, 0, 0, 100f / 255f);

            RectTransform objRectTransform = obj.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref objRectTransform, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 600, 400, 0, 0);
            ScrollRect scrollRect = obj.AddComponent<ScrollRect>();
            scrollRect.content = contentRectTransform;
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.viewport = viewportRectTransform;
            obj.AddComponent<MyUGUIReusableListView>();

            EditorGUIUtility.PingObject(obj);
            Selection.activeGameObject = obj.gameObject;

            Debug.Log("[MyClasses] ReusableListView was created.");
        }

        /// <summary>
        /// Create a game object with MyUGUIPieChart attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/GameObject/Pie Chart", false, 13)]
        public static void CreateMyUGUIPieChart()
        {
            GameObject canvas = MyUtilities.FindObjectInRoot("Canvas");

            GameObject obj = new GameObject("PieChart");
            if (canvas != null)
            {
                obj.transform.SetParent(canvas.transform, false);
            }

            obj.AddComponent<MyUGUIPieChart>();

            EditorGUIUtility.PingObject(obj);
            Selection.activeGameObject = obj.gameObject;

            Debug.Log("[MyClasses] ReusableListView was created.");
        }

        /// <summary>
        /// Create a game object with MyUGUIRadarChart attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/GameObject/Radar Chart", false, 14)]
        public static void CreateMyUGUIRadarChart()
        {
            GameObject canvas = MyUtilities.FindObjectInRoot("Canvas");

            GameObject obj = new GameObject("RadarChart");
            if (canvas != null)
            {
                obj.transform.SetParent(canvas.transform, false);
            }

            obj.AddComponent<MyUGUIRadarChart>();

            EditorGUIUtility.PingObject(obj);
            Selection.activeGameObject = obj.gameObject;

            Debug.Log("[MyClasses] RadarChart was created.");
        }

        #endregion
    }
}
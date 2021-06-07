/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIToolEditor (version 2.28)
 */

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MyClasses.UI.Tool
{
    public class MyUGUIToolEditor
    {
        #region ----- Setup -----

        /// <summary>
        /// Create a game object with Event System attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Setup/Create EventSystem (Menu Bar->GameObject->UI->EventSystem)", false, 1)]
        public static void CreateEventSystem()
        {
            Debug.Log("[MyClasses] Menu Bar -> GameObject -> UI -> EventSystem.");
        }

        /// <summary>
        /// Create a game object with Camera attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Setup/Create UICamera", false, 2)]
        public static void CreateUICamera()
        {
            GameObject goCamera = new GameObject("UICamera", typeof(Camera));
            goCamera.AddComponent<Camera>();
            goCamera.transform.localPosition = new Vector3(0, 0, -10);
            Camera camera = goCamera.GetComponent<Camera>();
            camera.clearFlags = CameraClearFlags.Depth;
            camera.cullingMask |= LayerMask.GetMask("UI");
            goCamera.AddComponent<AudioListener>();

            EditorGUIUtility.PingObject(goCamera);
            Selection.activeGameObject = goCamera;

            Debug.Log("[MyClasses] UICamera was created.");
        }

        /// <summary>
        /// Create portrait Canvases.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Setup/Create Portrait Canvases (Screen Space - Overlay)", false, 3)]
        public static void CreatePortraitCanvasesOverlay()
        {
            GameObject goCanvas = new GameObject("Canvas", typeof(Canvas));
            goCanvas.layer = LayerMask.NameToLayer("UI");
            Canvas canvasCanvas = goCanvas.GetComponent<Canvas>();
            canvasCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasCanvas.sortingOrder = -1000;
            CanvasScaler canvasCanvasScaler = goCanvas.AddComponent<CanvasScaler>();
            canvasCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasCanvasScaler.referenceResolution = new Vector2(1080, 2280);
            canvasCanvasScaler.matchWidthOrHeight = 1;
            GraphicRaycaster canvasGraphicRaycaster = goCanvas.AddComponent<GraphicRaycaster>();
            canvasGraphicRaycaster.ignoreReversedGraphics = true;
            canvasGraphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;

            GameObject goCanvasOnTop = GameObject.Instantiate(goCanvas);
            goCanvasOnTop.name = "CanvasOnTop";
            Canvas canvasOnTopCanvas = goCanvasOnTop.GetComponent<Canvas>();
            canvasOnTopCanvas.sortingOrder = 1000;

            GameObject goCanvasSceneFading = GameObject.Instantiate(goCanvas);
            goCanvasSceneFading.name = "CanvasSceneFading";
            Canvas canvasSceneFadingCanvas = goCanvasSceneFading.GetComponent<Canvas>();
            canvasSceneFadingCanvas.sortingOrder = 10000;
            CanvasScaler canvasSceneFadingCanvasScaler = goCanvasSceneFading.GetComponent<CanvasScaler>();
            canvasSceneFadingCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;

            EditorGUIUtility.PingObject(goCanvas);
            Selection.activeGameObject = goCanvas.gameObject;

            Debug.Log("[MyClasses] Portrait Canvases (Screen Space - Overlay) was created.");
        }

        /// <summary>
        /// Create landscape Canvases.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Setup/Create Landscape Canvases (Screen Space - Overlay)", false, 4)]
        public static void CreateLandscapeCanvasesOverlay()
        {
            GameObject goCanvas = new GameObject("Canvas", typeof(Canvas));
            goCanvas.layer = LayerMask.NameToLayer("UI");
            Canvas canvasCanvas = goCanvas.GetComponent<Canvas>();
            canvasCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasCanvas.sortingOrder = -1000;
            CanvasScaler canvasCanvasScaler = goCanvas.AddComponent<CanvasScaler>();
            canvasCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasCanvasScaler.referenceResolution = new Vector2(2280, 1080);
            canvasCanvasScaler.matchWidthOrHeight = 1;
            GraphicRaycaster canvasGraphicRaycaster = goCanvas.AddComponent<GraphicRaycaster>();
            canvasGraphicRaycaster.ignoreReversedGraphics = true;
            canvasGraphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;

            GameObject goCanvasOnTop = GameObject.Instantiate(goCanvas);
            goCanvasOnTop.name = "CanvasOnTop";
            Canvas canvasOnTopCanvas = goCanvasOnTop.GetComponent<Canvas>();
            canvasOnTopCanvas.sortingOrder = 1000;

            GameObject goCanvasSceneFading = GameObject.Instantiate(goCanvas);
            goCanvasSceneFading.name = "CanvasSceneFading";
            Canvas canvasSceneFadingCanvas = goCanvasSceneFading.GetComponent<Canvas>();
            canvasSceneFadingCanvas.sortingOrder = 10000;
            CanvasScaler canvasSceneFadingCanvasScaler = goCanvasSceneFading.GetComponent<CanvasScaler>();
            canvasSceneFadingCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;

            EditorGUIUtility.PingObject(goCanvas);
            Selection.activeGameObject = goCanvas.gameObject;

            Debug.Log("[MyClasses] Landscape Canvases (Screen Space - Overlay) was created.");
        }

        /// <summary>
        /// Create portrait Canvases.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Setup/Create Portrait Canvases (Screen Space - Camera)", false, 5)]
        public static void CreatePortraitCanvasesCamera()
        {
            GameObject goCamera = MyUtilities.FindObjectInRoot("UICamera");
            Camera camera = goCamera != null ? goCamera.GetComponent<Camera>() : null;
            if (camera != null)
            {
                camera.transform.position = new Vector3(540, 1140, -935.3079f);
            }

            GameObject goCanvas = new GameObject("Canvas", typeof(Canvas));
            goCanvas.layer = LayerMask.NameToLayer("UI");
            Canvas canvasCanvas = goCanvas.GetComponent<Canvas>();
            canvasCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvasCanvas.worldCamera = camera;
            canvasCanvas.planeDistance = 935.3079f;
            canvasCanvas.sortingOrder = -1000;
            CanvasScaler canvasCanvasScaler = goCanvas.AddComponent<CanvasScaler>();
            canvasCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasCanvasScaler.referenceResolution = new Vector2(1080, 2280);
            canvasCanvasScaler.matchWidthOrHeight = 1;
            GraphicRaycaster canvasGraphicRaycaster = goCanvas.AddComponent<GraphicRaycaster>();
            canvasGraphicRaycaster.ignoreReversedGraphics = true;
            canvasGraphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;

            GameObject goCanvasOnTop = GameObject.Instantiate(goCanvas);
            goCanvasOnTop.name = "CanvasOnTop";
            Canvas canvasOnTopCanvas = goCanvasOnTop.GetComponent<Canvas>();
            canvasOnTopCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvasOnTopCanvas.worldCamera = camera;
            canvasOnTopCanvas.planeDistance = 935.3079f;
            canvasOnTopCanvas.sortingOrder = 1000;

            GameObject goCanvasSceneFading = GameObject.Instantiate(goCanvas);
            goCanvasSceneFading.name = "CanvasSceneFading";
            Canvas canvasSceneFadingCanvas = goCanvasSceneFading.GetComponent<Canvas>();
            canvasSceneFadingCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvasSceneFadingCanvas.worldCamera = camera;
            canvasSceneFadingCanvas.planeDistance = 935.3079f;
            canvasSceneFadingCanvas.sortingOrder = 10000;
            CanvasScaler canvasSceneFadingCanvasScaler = goCanvasSceneFading.GetComponent<CanvasScaler>();
            canvasSceneFadingCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;

            EditorGUIUtility.PingObject(goCanvas);
            Selection.activeGameObject = goCanvas.gameObject;

            Debug.Log("[MyClasses] Portrait Canvases (Screen Space - Camera) was created.");
        }

        /// <summary>
        /// Create landscape Canvases.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Setup/Create Landscape Canvases (Screen Space - Camera)", false, 6)]
        public static void CreateLandscapeCanvasesCamera()
        {
            GameObject goCamera = MyUtilities.FindObjectInRoot("UICamera");
            Camera camera = goCamera != null ? goCamera.GetComponent<Camera>() : null;
            if (camera != null)
            {
                camera.transform.position = new Vector3(1140, 540, -935.3079f);
            }

            GameObject goCanvas = new GameObject("Canvas", typeof(Canvas));
            goCanvas.layer = LayerMask.NameToLayer("UI");
            Canvas canvasCanvas = goCanvas.GetComponent<Canvas>();
            canvasCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvasCanvas.worldCamera = camera;
            canvasCanvas.planeDistance = 935.3079f;
            canvasCanvas.sortingOrder = -1000;
            CanvasScaler canvasCanvasScaler = goCanvas.AddComponent<CanvasScaler>();
            canvasCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasCanvasScaler.referenceResolution = new Vector2(2280, 1080);
            canvasCanvasScaler.matchWidthOrHeight = 1;
            GraphicRaycaster canvasGraphicRaycaster = goCanvas.AddComponent<GraphicRaycaster>();
            canvasGraphicRaycaster.ignoreReversedGraphics = true;
            canvasGraphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;

            GameObject goCanvasOnTop = GameObject.Instantiate(goCanvas);
            goCanvasOnTop.name = "CanvasOnTop";
            Canvas canvasOnTopCanvas = goCanvasOnTop.GetComponent<Canvas>();
            canvasOnTopCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvasOnTopCanvas.worldCamera = camera;
            canvasOnTopCanvas.planeDistance = 935.3079f;
            canvasOnTopCanvas.sortingOrder = 1000;

            GameObject goCanvasSceneFading = GameObject.Instantiate(goCanvas);
            goCanvasSceneFading.name = "CanvasSceneFading";
            Canvas canvasSceneFadingCanvas = goCanvasSceneFading.GetComponent<Canvas>();
            canvasSceneFadingCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvasSceneFadingCanvas.worldCamera = camera;
            canvasSceneFadingCanvas.planeDistance = 935.3079f;
            canvasSceneFadingCanvas.sortingOrder = 10000;
            CanvasScaler canvasSceneFadingCanvasScaler = goCanvasSceneFading.GetComponent<CanvasScaler>();
            canvasSceneFadingCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;

            EditorGUIUtility.PingObject(goCanvas);
            Selection.activeGameObject = goCanvas.gameObject;

            Debug.Log("[MyClasses] Landscape Canvases (Screen Space - Camera) was created.");
        }

        /// <summary>
        /// Open UI Config ID.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Setup/Config ID", false, 15)]
        public static void OpenConfigID()
        {
            EditorWindow.GetWindow(typeof(MyUGUIConfigIDEditorWindow));
        }

        /// <summary>
        /// Open UI Config Scene.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Setup/Config Scene", false, 16)]
        public static void OpenConfigScene()
        {
            EditorWindow.GetWindow(typeof(MyUGUIConfigSceneEditorWindow));
        }

        /// <summary>
        /// Open UI Config Popup.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Setup/Config Popup", false, 17)]
        public static void OpenConfigPopup()
        {
            EditorWindow.GetWindow(typeof(MyUGUIConfigPopupEditorWindow));
        }

        /// <summary>
        /// Create a game object with MyUGUIBooter attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Setup/Create MyUGUIBooter", false, 28)]
        public static void CreateMyUGUIConfig()
        {
            GameObject obj = new GameObject("MyUGUIBooter");

            obj.AddComponent<MyUGUIBooter>();

            EditorGUIUtility.PingObject(obj);
            Selection.activeGameObject = obj.gameObject;

            Debug.Log("[MyClasses] " + typeof(MyUGUIBooter) + " was created.");
        }

        #endregion

        #region ----- Create -----

        /// <summary>
        /// Create a game object with MyUGUIButton & Text attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Create/Text Button (Color Tint)", false, 1)]
        public static void CreateMyUGUIButtonColorTint()
        {
            GameObject go = MyUGUIButton.CreateTextTemplate();

            MyUGUIButton button = go.GetComponent<MyUGUIButton>();
            button.Button.transition = Selectable.Transition.ColorTint;

            Debug.Log("[MyClasses] Text Button (Color Tint) was created.");
        }

        /// <summary>
        /// Create a game object with MyUGUIButton & Text attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Create/Text Button (Scale Animation)", false, 2)]
        public static void CreateMyUGUIButtonScaleAnimation()
        {
            GameObject go = MyUGUIButton.CreateTextTemplate();

            MyUGUIButton button = go.GetComponent<MyUGUIButton>();
            button.Button.transition = Selectable.Transition.Animation;

            Animator animator = go.AddComponent<Animator>();
            string[] paths = new string[] { "Assets/MyClasses", "Assets/Core/MyClasses", "Assets/Plugin/MyClasses", "Assets/Plugins/MyClasses", "Assets/Framework/MyClasses", "Assets/Frameworks/MyClasses" };
            for (int i = 0; i < paths.Length; i++)
            {
                if (System.IO.File.Exists(paths[i] + "/Sources/Animations/my_animator_button_click_scale.controller"))
                {
                    animator.runtimeAnimatorController = (RuntimeAnimatorController)UnityEditor.AssetDatabase.LoadAssetAtPath(paths[i] + "/Sources/Animations/my_animator_button_click_scale.controller", typeof(RuntimeAnimatorController));
                    break;
                }
            }

            Debug.Log("[MyClasses] Text Button (Scale Animation) was created.");
        }

        /// <summary>
        /// Create a game object with MyUGUIButton & TextMeshPro attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Create/TextMeshPro Button (Color Tint)", false, 3)]
        public static void CreateMyUGUITextMeshProButtonColorTint()
        {
            GameObject go = MyUGUIButton.CreateTextMeshProTemplate();

            MyUGUIButton button = go.GetComponent<MyUGUIButton>();
            button.Button.transition = Selectable.Transition.ColorTint;

            Debug.Log("[MyClasses] TextMeshPro Button (Color Tint) was created.");
        }

        /// <summary>
        /// Create a game object with MyUGUIButton & TextMeshPro attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Create/TextMeshPro Button (Scale Animation)", false, 4)]
        public static void CreateMyUGUITextMeshProButtonScaleAnimation()
        {
            GameObject go = MyUGUIButton.CreateTextMeshProTemplate();

            MyUGUIButton button = go.GetComponent<MyUGUIButton>();
            button.Button.transition = Selectable.Transition.ColorTint;

            Debug.Log("[MyClasses] TextMeshPro Button (Scale Animation) was created.");
        }

        /// <summary>
        /// Create a game object with MyUGUIToggleButton attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Create/Toggle Button", false, 5)]
        public static void CreateMyUGUIToggleButton()
        {
            MyUGUIToggleButton.CreateTemplate();

            Debug.Log("[MyClasses] " + typeof(MyUGUIToggleButton) + " was created.");
        }

        /// <summary>
        /// Create a game object with MyUGUIReusableListView attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Create/Reusable List View", false, 14)]
        public static void CreateMyUGUIReusableListView()
        {
            MyUGUIReusableListView.CreateTemplate();

            Debug.Log("[MyClasses] " + typeof(MyUGUIReusableListView) + " was created.");
        }

        /// <summary>
        /// Create a game object with MyUGUIPieChart attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Create/Pie Chart", false, 25)]
        public static void CreateMyUGUIPieChart()
        {
            MyUGUIPieChart.CreateTemplate();

            Debug.Log("[MyClasses] " + typeof(MyUGUIPieChart) + " was created.");
        }

        /// <summary>
        /// Create a game object with MyUGUIRadarChart attached.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Create/Radar Chart", false, 26)]
        public static void CreateMyUGUIRadarChart()
        {
            MyUGUIRadarChart.CreateTemplate();

            Debug.Log("[MyClasses] " + typeof(MyUGUIRadarChart) + " was created.");
        }

        #endregion

        #region ----- Utilities -----

        /// <summary>
        /// Invoke all "Anchor Now" buttons in current scene.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Utilities/Refresh All MyUGUIAnchors Now", false, 3)]
        public static void OrientationAnchorAllNow()
        {
            MyUGUIAnchor[] scripts = GameObject.FindObjectsOfType<MyUGUIAnchor>();
            foreach (var item in scripts)
            {
                item.Anchor();
            }

            Debug.Log("[MyClasses] All \"Anchor Now\" buttons were invoked.");
        }

        /// <summary>
        /// Invoke all "Layout Now" buttons in current scene.
        /// </summary>
        [MenuItem("MyClasses/UGUI/Utilities/Refresh All MyUGUIGridLayoutGroups Now", false, 2)]
        public static void AspectRatioScaleAllNow()
        {
            MyUGUIGridLayoutGroup[] scripts = GameObject.FindObjectsOfType<MyUGUIGridLayoutGroup>();
            foreach (var item in scripts)
            {
                item.Layout();
            }

            Debug.Log("[MyClasses] All \"Layout Now\" buttons were invoked.");
        }

        #endregion
    }
}
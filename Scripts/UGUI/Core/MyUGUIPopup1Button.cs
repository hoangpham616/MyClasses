/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIPopup1Button (version 2.11)
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

#if USE_MY_UI_TMPRO
using TMPro;
#endif

namespace MyClasses.UI
{
    public class MyUGUIPopup1Button : MyUGUIPopup
    {
        #region ----- Variable -----

#if USE_MY_UI_TMPRO
        private TextMeshProUGUI mTitleTMPro;
#endif

        private Text mTitle;
        private Text mBody;
        private MyUGUIButton mButtonClose;
        private MyUGUIButton mButtonMain;
        private Action<object> mActionClose;
        private Action<object> mActionMain;

        private bool mIsAutoHideWhenClickButton;

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIPopup1Button(EPopupID id, string prefabName, bool isFloat = false, bool isRepeatable = false)
            : base(id, prefabName, isFloat, isRepeatable)
        {
#if UNITY_EDITOR
            if (!_CheckPrefab())
            {
                _CreatePrefab();
            }
#endif
        }

        #endregion

        #region ----- Implement MyUGUIPopup -----

        /// <summary>
        /// OnUGUIInit.
        /// </summary>
        public override void OnUGUIInit()
        {
            base.OnUGUIInit();

            GameObject _container = MyUtilities.FindObjectInAllLayers(GameObject, "Container");
            mBody = MyUtilities.FindObjectInFirstLayer(_container, "Body").GetComponent<Text>();
            mButtonMain = MyUtilities.FindObjectInFirstLayer(_container, "ButtonMain").GetComponent<MyUGUIButton>();

            GameObject _title = MyUtilities.FindObjectInFirstLayer(_container, "Title");
            if (_title != null)
            {
                mTitle = _title.GetComponent<Text>();
#if USE_MY_UI_TMPRO
                if (mTitle == null)
                {
                    mTitleTMPro = _title.GetComponent<TextMeshProUGUI>();
                }
#endif
            }

            GameObject _close = MyUtilities.FindObjectInFirstLayer(_container, "ButtonClose");
            if (_close != null)
            {
                mButtonClose = _close.GetComponent<MyUGUIButton>();
            }
        }

        /// <summary>
        /// OnEnter.
        /// </summary>
        public override void OnUGUIEnter()
        {
            base.OnUGUIEnter();

            if (mButtonClose != null)
            {
                mButtonClose.OnEventPointerClick.AddListener(_OnClickClose);
            }
            mButtonMain.OnEventPointerClick.AddListener(_OnClickMain);
        }

        /// <summary>
        /// OnUGUIVisible.
        /// </summary>
        public override bool OnUGUIVisible()
        {
            return base.OnUGUIVisible();
        }

        /// <summary>
        /// OnUpdateUGUI.
        /// </summary>
        public override void OnUGUIUpdate(float deltaTime)
        {
            base.OnUGUIUpdate(deltaTime);
        }

        /// <summary>
        /// OnUGUIExit.
        /// </summary>
        public override void OnUGUIExit()
        {
            base.OnUGUIExit();

            if (mButtonClose != null)
            {
                mButtonClose.OnEventPointerClick.RemoveAllListeners();
            }
            mButtonMain.OnEventPointerClick.RemoveAllListeners();

            mActionClose = null;
            mActionMain = null;
        }

        /// <summary>
        /// OnUGUIInvisible.
        /// </summary>
        public override bool OnUGUIInvisible()
        {
            return base.OnUGUIInvisible();
        }

        #endregion

        #region ----- Button Event -----

        /// <summary>
        /// Click on close button.
        /// </summary>
        private void _OnClickClose(PointerEventData arg0)
        {
            if (mActionClose != null)
            {
                mActionClose(AttachedData);
            }

            Hide();
        }

        /// <summary>
        /// Click on main button.
        /// </summary>
        private void _OnClickMain(PointerEventData arg0)
        {
            if (mActionMain != null)
            {
                mActionMain(AttachedData);
            }

            if (mIsAutoHideWhenClickButton)
            {
                Hide();
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set data.
        /// </summary>
        public void SetData(string title, string content, string mainButton, Action<object> actionMain, Action<object> actionClose, bool isAutoHideWhenClickButton = true)
        {
            _SetData(title, content, mainButton, actionMain, true, actionClose, isAutoHideWhenClickButton);
        }

        /// <summary>
        /// Set data.
        /// </summary>
        public void SetData(string title, string content, string mainButton, Action<object> actionMain = null, bool isAutoHideWhenClickButton = true)
        {
            _SetData(title, content, mainButton, actionMain, false, null, isAutoHideWhenClickButton);
        }

        /// <summary>
        /// Set data.
        /// </summary>
        public void SetData(string content, string mainButton, Action<object> actionMain, Action<object> actionClose, bool isAutoHideWhenClickButton = true)
        {
            _SetData(string.Empty, content, mainButton, actionMain, true, actionClose, isAutoHideWhenClickButton);
        }

        /// <summary>
        /// Set data.
        /// </summary>
        public void SetData(string content, string buttonMain, Action<object> actionMain, bool isAutoHideWhenClickButton = true)
        {
            _SetData(string.Empty, content, buttonMain, actionMain, false, null, isAutoHideWhenClickButton);
        }

        /// <summary>
        /// Set data.
        /// </summary>
        public void SetData(string content, string buttonMain, bool isShowCloseButton = false, bool isAutoHideWhenClickButton = true)
        {
            _SetData(string.Empty, content, buttonMain, null, isShowCloseButton, null, isAutoHideWhenClickButton);
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Set data.
        /// </summary>
        private void _SetData(string title, string body, string buttonMain, Action<object> actionMain, bool isShowButtonClose, Action<object> actionClose, bool isAutoHideWhenClickButton)
        {
            if (mTitle != null)
            {
                mTitle.text = title;
            }
#if USE_MY_UI_TMPRO
            else if (mTitleTMPro != null)
            {
                mTitleTMPro.text = title;
            }
#endif

            mBody.text = body;

            if (mButtonClose != null)
            {
                mButtonClose.SetActive(isShowButtonClose);
            }
            mActionClose = actionClose;

            mButtonMain.SetText(buttonMain);
            mActionMain = actionMain;

            mIsAutoHideWhenClickButton = isAutoHideWhenClickButton;
        }

#if UNITY_EDITOR

        /// <summary>
        /// Check existence of prefab.
        /// </summary>
        private static bool _CheckPrefab(string subfixName = "")
        {
            string filePath = "Assets/Resources/" + MyUGUIManager.POPUP_DIRECTORY + "Dialog1ButtonPopup" + subfixName + ".prefab";
            return System.IO.File.Exists(filePath);
        }

        /// <summary>
        /// Create template prefab.
        /// </summary>
        private static void _CreatePrefab(string subfixName = "")
        {
            string prefabName = "Dialog1ButtonPopup" + subfixName;

            GameObject prefab = new GameObject(prefabName);

            Animator root_animator = prefab.AddComponent<Animator>();
            string[] paths = new string[] { "Assets/MyClasses", "Assets/Core/MyClasses", "Assets/Plugin/MyClasses", "Assets/Plugins/MyClasses", "Assets/Framework/MyClasses", "Assets/Frameworks/MyClasses" };
            for (int i = 0; i < paths.Length; i++)
            {
                if (System.IO.File.Exists(paths[i] + "/Animations/my_animator_dialog.controller"))
                {
                    root_animator.runtimeAnimatorController = (RuntimeAnimatorController)UnityEditor.AssetDatabase.LoadAssetAtPath(paths[i] + "/Animations/my_animator_dialog.controller", typeof(RuntimeAnimatorController));
                    Debug.LogError("[" + typeof(MyUGUIPopup1Button).Name + "] CreateTemplate(): please setup \"my_animator_dialog\" controller.");
                    Debug.LogError("[" + typeof(MyUGUIPopup1Button).Name + "] CreateTemplate(): mapping \"my_animation_dialog_show\" motion for \"Show\" state.");
                    Debug.LogError("[" + typeof(MyUGUIPopup1Button).Name + "] CreateTemplate(): mapping \"my_animation_dialog_hide\" motion for \"Hide\" state.");
                    break;
                }
            }

            RectTransform root_rect = prefab.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref root_rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

            GameObject container = new GameObject("Container");
            container.transform.SetParent(prefab.transform, false);

            RectTransform container_rect = container.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref container_rect, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 1000, 600, 0, 0);

            Image container_image = container.AddComponent<Image>();
            container_image.color = new Color(0.9f, 0.9f, 0.9f);
            container_image.raycastTarget = true;

            GameObject title = new GameObject("Title");
            title.transform.SetParent(container.transform, false);

            RectTransform title_rect = title.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref title_rect, MyUtilities.EAnchorPreset.TopCenter, MyUtilities.EAnchorPivot.TopCenter, 800, 100, 0, 0);

            Text title_text = title.AddComponent<Text>();
            title_text.text = "TITLE";
            title_text.color = Color.black;
            title_text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            title_text.fontSize = 50;
            title_text.fontStyle = FontStyle.Bold;
            title_text.alignment = TextAnchor.MiddleCenter;
            title_text.horizontalOverflow = HorizontalWrapMode.Wrap;
            title_text.verticalOverflow = VerticalWrapMode.Truncate;
            title_text.raycastTarget = false;

            GameObject body = new GameObject("Body");
            body.transform.SetParent(container.transform, false);

            RectTransform body_rect = body.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref body_rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, new Vector2(100, 120), new Vector2(-100, -120));

            Text body_text = body.AddComponent<Text>();
            body_text.text = "Body";
            body_text.color = Color.black;
            body_text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            body_text.fontSize = 40;
            body_text.alignment = TextAnchor.MiddleCenter;
            body_text.horizontalOverflow = HorizontalWrapMode.Wrap;
            body_text.verticalOverflow = VerticalWrapMode.Truncate;
            body_text.raycastTarget = false;

            GameObject buttonClose = new GameObject("ButtonClose");
            buttonClose.transform.SetParent(container.transform, false);

            RectTransform buttonClose_rect = buttonClose.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref buttonClose_rect, MyUtilities.EAnchorPreset.TopRight, MyUtilities.EAnchorPivot.TopRight, 80, 80, -10, -10);

            Image buttonClose_image = buttonClose.AddComponent<Image>();
            buttonClose_image.color = Color.red;
            buttonClose_image.raycastTarget = true;

            buttonClose.AddComponent<MyUGUIButton>();

            GameObject buttonMain = new GameObject("ButtonMain");
            buttonMain.transform.SetParent(container.transform, false);

            RectTransform buttonMain_rect = buttonMain.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref buttonMain_rect, MyUtilities.EAnchorPreset.BottomCenter, MyUtilities.EAnchorPivot.BottomCenter, 300, 100, 0, 20);

            Image buttonMain_image = buttonMain.AddComponent<Image>();
            buttonMain_image.color = Color.green;
            buttonMain_image.raycastTarget = true;

            buttonMain.AddComponent<MyUGUIButton>();

            GameObject buttonMainText = new GameObject("Text");
            buttonMainText.transform.SetParent(buttonMain.transform, false);

            Text buttonMainText_text = buttonMainText.AddComponent<Text>();
            buttonMainText_text.text = "Main Button";
            buttonMainText_text.color = Color.black;
            buttonMainText_text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            buttonMainText_text.fontSize = 40;
            buttonMainText_text.alignment = TextAnchor.MiddleCenter;
            buttonMainText_text.horizontalOverflow = HorizontalWrapMode.Wrap;
            buttonMainText_text.verticalOverflow = VerticalWrapMode.Overflow;
            buttonMainText_text.raycastTarget = false;

            string folderPath = "Assets/Resources/" + MyUGUIManager.POPUP_DIRECTORY;
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }

            string filePath = folderPath + prefabName + ".prefab";
            UnityEditor.PrefabUtility.CreatePrefab(filePath, prefab, UnityEditor.ReplacePrefabOptions.ReplaceNameBased);

            GameObject.Destroy(prefab);
        }

#endif

        #endregion
    }
}
/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIPopup2Buttons (version 2.9)
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace MyClasses.UI
{
    public class MyUGUIPopup2Buttons : MyUGUIPopup
    {
        #region ----- Variable -----

        private Text mTitle;
        private Text mBody;
        private MyUGUIButton mButtonClose;
        private MyUGUIButton mButtonLeft;
        private MyUGUIButton mButtonRight;
        private Action<object> mActionClose;
        private Action<object> mActionLeft;
        private Action<object> mActionRight;

        private bool mIsAutoHideWhenClickButton;

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIPopup2Buttons(EPopupID id, string prefabName, bool isFloat = false, bool isRepeatable = false)
            : base(id, prefabName, isFloat, isRepeatable)
        {
#if UNITY_EDITOR
            if (!_CheckPrefab())
            {
                _CreatePrefab();
            }
#endif

            mIsAutoHideWhenClickButton = true;
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
            mButtonLeft = MyUtilities.FindObjectInFirstLayer(_container, "ButtonLeft").GetComponent<MyUGUIButton>();
            mButtonRight = MyUtilities.FindObjectInFirstLayer(_container, "ButtonRight").GetComponent<MyUGUIButton>();

            GameObject _title = MyUtilities.FindObjectInAllLayers(GameObject, "Title");
            if (_title != null)
            {
                mTitle = _title.GetComponent<Text>();
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
            mButtonLeft.OnEventPointerClick.AddListener(_OnClickLeft);
            mButtonRight.OnEventPointerClick.AddListener(_OnClickRight);
        }

        /// <summary>
        /// OnUGUIVisible.
        /// </summary>
        public override bool OnUGUIVisible()
        {
            return base.OnUGUIInvisible();
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
                mButtonClose.onClick.RemoveAllListeners();
            }
            mButtonLeft.onClick.RemoveAllListeners();
            mButtonRight.onClick.RemoveAllListeners();

            mActionClose = null;
            mActionLeft = null;
            mActionRight = null;
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
        /// Click on button close.
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
        /// Click on button left.
        /// </summary>
        private void _OnClickLeft(PointerEventData arg0)
        {
            if (mActionLeft != null)
            {
                mActionLeft(AttachedData);
            }

            if (mIsAutoHideWhenClickButton)
            {
                Hide();
            }
        }

        /// <summary>
        /// Click on button right.
        /// </summary>
        private void _OnClickRight(PointerEventData arg0)
        {
            if (mActionRight != null)
            {
                mActionRight(AttachedData);
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
        public void SetData(string title, string content, string buttonLeft, Action<object> actionLeft, string buttonRight, Action<object> actionRight, Action<object> actionClose, bool isAutoHideWhenClickButton = true)
        {
            _SetData(title, content, buttonLeft, actionLeft, buttonRight, actionRight, true, actionClose, isAutoHideWhenClickButton);
        }

        /// <summary>
        /// Set data.
        /// </summary>
        public void SetData(string title, string content, string buttonLeft, Action<object> actionLeft, string buttonRight, Action<object> actionRight, bool isAutoHideWhenClickButton = true)
        {
            _SetData(title, content, buttonLeft, actionLeft, buttonRight, actionRight, false, null, isAutoHideWhenClickButton);
        }

        /// <summary>
        /// Set data.
        /// </summary>
        public void SetData(string content, string buttonLeft, Action<object> actionLeft, string buttonRight, Action<object> actionRight, Action<object> actionClose, bool isAutoHideWhenClickButton = true)
        {
            _SetData(string.Empty, content, buttonLeft, actionLeft, buttonRight, actionRight, true, actionClose, isAutoHideWhenClickButton);
        }

        /// <summary>
        /// Set data.
        /// </summary>
        public void SetData(string content, string buttonLeft, Action<object> actionLeft, string buttonRight, Action<object> actionRight, bool isAutoHideWhenClickButton = true)
        {
            _SetData(string.Empty, content, buttonLeft, actionLeft, buttonRight, actionRight, false, null, isAutoHideWhenClickButton);
        }

        /// <summary>
        /// Set data.
        /// </summary>
        public void SetData(string content, string buttonLeft, string buttonRight, bool isShowCloseButton = false, bool isAutoHideWhenClickButton = true)
        {
            _SetData(string.Empty, content, buttonLeft, null, buttonRight, null, isShowCloseButton, null, isAutoHideWhenClickButton);
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Set data.
        /// </summary>
        private void _SetData(string title, string body, string buttonLeft, Action<object> actionLeft, string buttonRight, Action<object> actionRight, bool isShowButtonClose, Action<object> actionClose, bool isAutoHideWhenClickButton)
        {
            if (mTitle != null)
            {
                mTitle.text = title;
            }

            mBody.text = body;

            if (mButtonClose != null)
            {
                mButtonClose.SetActive(isShowButtonClose);
            }
            mActionClose = actionClose;

            mButtonLeft.SetText(buttonLeft);
            mActionLeft = actionLeft;

            mButtonRight.SetText(buttonRight);
            mActionRight = actionRight;

            mIsAutoHideWhenClickButton = isAutoHideWhenClickButton;
        }

#if UNITY_EDITOR

        /// <summary>
        /// Check existence of prefab.
        /// </summary>
        private static bool _CheckPrefab()
        {
            string filePath = "Assets/Resources/" + MyUGUIManager.POPUP_DIRECTORY + "Dialog2ButtonsPopup.prefab";
            return System.IO.File.Exists(filePath);
        }

        /// <summary>
        /// Create template prefab.
        /// </summary>
        private static void _CreatePrefab(string subfixName = "")
        {
            string prefabName = "Dialog2ButtonsPopup" + subfixName;

            GameObject prefab = new GameObject(prefabName);

            Animator root_animator = prefab.AddComponent<Animator>();
            string[] paths = new string[] { "Assets/MyClasses", "Assets/Core/MyClasses", "Assets/Plugin/MyClasses", "Assets/Plugins/MyClasses", "Assets/Framework/MyClasses", "Assets/Frameworks/MyClasses" };
            for (int i = 0; i < paths.Length; i++)
            {
                if (System.IO.File.Exists(paths[i] + "/Animations/my_animator_dialog.controller"))
                {
                    root_animator.runtimeAnimatorController = (RuntimeAnimatorController)UnityEditor.AssetDatabase.LoadAssetAtPath(paths[i] + "/Animations/my_animator_dialog.controller", typeof(RuntimeAnimatorController));
                    Debug.LogError("[" + typeof(MyUGUIPopup2Buttons).Name + "] CreateTemplate(): please setup \"my_animator_dialog\" controller.");
                    Debug.LogError("[" + typeof(MyUGUIPopup2Buttons).Name + "] CreateTemplate(): mapping \"my_animation_dialog_show\" motion for \"Show\" state.");
                    Debug.LogError("[" + typeof(MyUGUIPopup2Buttons).Name + "] CreateTemplate(): mapping \"my_animation_dialog_hide\" motion for \"Hide\" state.");
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

            GameObject buttonLeft = new GameObject("ButtonLeft");
            buttonLeft.transform.SetParent(container.transform, false);

            RectTransform buttonLeft_rect = buttonLeft.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref buttonLeft_rect, MyUtilities.EAnchorPreset.BottomCenter, MyUtilities.EAnchorPivot.BottomCenter, 300, 100, -200, 20);

            Image buttonLeft_image = buttonLeft.AddComponent<Image>();
            buttonLeft_image.color = Color.green;
            buttonLeft_image.raycastTarget = true;

            buttonLeft.AddComponent<MyUGUIButton>();

            GameObject buttonLeftText = new GameObject("Text");
            buttonLeftText.transform.SetParent(buttonLeft.transform, false);

            Text buttonLeftText_text = buttonLeftText.AddComponent<Text>();
            buttonLeftText_text.text = "Left Button";
            buttonLeftText_text.color = Color.black;
            buttonLeftText_text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            buttonLeftText_text.fontSize = 40;
            buttonLeftText_text.alignment = TextAnchor.MiddleCenter;
            buttonLeftText_text.horizontalOverflow = HorizontalWrapMode.Wrap;
            buttonLeftText_text.verticalOverflow = VerticalWrapMode.Overflow;
            buttonLeftText_text.raycastTarget = false;

            GameObject buttonRight = new GameObject("ButtonRight");
            buttonRight.transform.SetParent(container.transform, false);

            RectTransform buttonRight_rect = buttonRight.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref buttonRight_rect, MyUtilities.EAnchorPreset.BottomCenter, MyUtilities.EAnchorPivot.BottomCenter, 300, 100, 200, 20);

            Image buttonRight_image = buttonRight.AddComponent<Image>();
            buttonRight_image.color = Color.green;
            buttonRight_image.raycastTarget = true;

            buttonRight.AddComponent<MyUGUIButton>();

            GameObject buttonRightText = new GameObject("Text");
            buttonRightText.transform.SetParent(buttonRight.transform, false);

            Text buttonRightText_text = buttonRightText.AddComponent<Text>();
            buttonRightText_text.text = "Right Button";
            buttonRightText_text.color = Color.black;
            buttonRightText_text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            buttonRightText_text.fontSize = 40;
            buttonRightText_text.alignment = TextAnchor.MiddleCenter;
            buttonRightText_text.horizontalOverflow = HorizontalWrapMode.Wrap;
            buttonRightText_text.verticalOverflow = VerticalWrapMode.Overflow;
            buttonRightText_text.raycastTarget = false;

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
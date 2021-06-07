/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIPopupOverlay (version 2.12)
 */

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0649

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MyClasses.UI
{
    public class MyUGUIPopupOverlay
    {
        #region ----- Variable -----

        public const string PREFAB_NAME = "PopupOverlay";

        private GameObject mGameObject;
        private Animator mAnimator;
        private MyUGUIButton mButton;

        #endregion

        #region ----- Property -----

        public GameObject GameObject
        {
            get { return mGameObject; }
            set { mGameObject = value; }
        }

        public Transform Transform
        {
            get { return mGameObject != null ? mGameObject.transform : null; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIPopupOverlay()
        {
#if UNITY_EDITOR
            if (!_CheckPrefab())
            {
                _CreatePrefab();
            }
#endif
        }

        #endregion

        #region ----- Button Event -----

        /// <summary>
        /// Click on close button.
        /// </summary>
        private void _OnClickClose(PointerEventData arg0)
        {
            if (MyUGUIManager.Instance.CurrentLoadingIndicator != null && MyUGUIManager.Instance.CurrentLoadingIndicator.IsActive)
            {
                return;
            }

            if (MyUGUIManager.Instance.IsClosePopupByClickingOutside)
            {
                if (MyUGUIManager.Instance.CurrentPopup != null)
                {
                    if (MyUGUIManager.Instance.CurrentPopup.State == EBaseState.Update)
                    {
                        MyUGUIManager.Instance.CurrentPopup.Hide();
                    }
                }
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Show.
        /// </summary>
        public void Show()
        {
            if (mGameObject != null)
            {
                mGameObject.SetActive(true);

                if (mAnimator == null)
                {
                    mAnimator = mGameObject.GetComponent<Animator>();
                }
                if (mAnimator != null)
                {
                    mAnimator.Play("Show");
                }

                if (mButton == null)
                {
                    mButton = mGameObject.GetComponent<MyUGUIButton>();
                    if (mButton == null)
                    {
                        mButton = mGameObject.AddComponent<MyUGUIButton>();
                    }
                }
                mButton.OnEventPointerClick.RemoveAllListeners();
                mButton.OnEventPointerClick.AddListener(_OnClickClose);
            }
        }

        /// <summary>
        /// Hide.
        /// </summary>
        public void Hide()
        {
            if (mGameObject != null)
            {
                if (mAnimator == null)
                {
                    mAnimator = mGameObject.GetComponent<Animator>();
                }
                if (mAnimator != null)
                {
                    mAnimator.Play("Hide");
                    return;
                }

                mGameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Create a template game object.
        /// </summary>
        public static GameObject CreateTemplate()
        {
            GameObject obj = new GameObject(PREFAB_NAME);

#if UNITY_EDITOR
            Animator root_animator = obj.AddComponent<Animator>();
            string[] paths = new string[] { "Assets/MyClasses", "Assets/Core/MyClasses", "Assets/Plugin/MyClasses", "Assets/Plugins/MyClasses", "Assets/Framework/MyClasses", "Assets/Frameworks/MyClasses" };
            for (int i = 0; i < paths.Length; i++)
            {
                if (System.IO.File.Exists(paths[i] + "/Sources/Animations/my_animator_popup_overlay.controller"))
                {
                    root_animator.runtimeAnimatorController = (RuntimeAnimatorController)UnityEditor.AssetDatabase.LoadAssetAtPath(paths[i] + "/Sources/Animations/my_animator_popup_overlay.controller", typeof(RuntimeAnimatorController));
                    Debug.LogError("[" + typeof(MyUGUIPopupOverlay).Name + "] CreateTemplate(): please setup \"my_animator_popup_overlay\" controller.");
                    Debug.LogError("[" + typeof(MyUGUIPopupOverlay).Name + "] CreateTemplate(): mapping \"my_animation_popup_overlay_show\" motion for \"Show\" state.");
                    Debug.LogError("[" + typeof(MyUGUIPopupOverlay).Name + "] CreateTemplate(): mapping \"my_animation_popup_overlay_hide\" motion for \"Hide\" state.");
                    break;
                }
            }
#endif

            RectTransform rect = obj.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

            obj.AddComponent<CanvasRenderer>();

            Image image = obj.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0.5f);
            image.raycastTarget = true;

            Button button = obj.AddComponent<Button>();
            button.transition = Selectable.Transition.None;

            MyUGUIButton myButton = obj.AddComponent<MyUGUIButton>();
            ColorBlock colorBlock = myButton.Button.colors;
            colorBlock.normalColor = Color.white;
            colorBlock.highlightedColor = Color.white;
            colorBlock.pressedColor = Color.white;
            colorBlock.disabledColor = Color.white;
            myButton.Button.colors = colorBlock;
            myButton.SoundResourcePath = string.Empty;
            myButton.PressAnimationType = MyUGUIButton.EAnimationType.None;
            myButton.ClickAnimationType = MyUGUIButton.EAnimationType.None;

            return obj;
        }

        #endregion

        #region ----- Private Method -----

#if UNITY_EDITOR

        /// <summary>
        /// Check existence of prefab.
        /// </summary>
        private static bool _CheckPrefab()
        {
            string filePath = "Assets/Resources/" + MyUGUIManager.SPECIAL_DIRECTORY + PREFAB_NAME + ".prefab";
            return System.IO.File.Exists(filePath);
        }

        /// <summary>
        /// Create template prefab.
        /// </summary>
        private static void _CreatePrefab()
        {
            Debug.Log("[" + typeof(MyUGUIPopupOverlay).Name + "] CreatePrefab(): a template prefab was created.");

            GameObject prefab = CreateTemplate();

            string folderPath = "Assets/Resources/" + MyUGUIManager.SPECIAL_DIRECTORY;
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }

            string filePath = "Assets/Resources/" + MyUGUIManager.SPECIAL_DIRECTORY + PREFAB_NAME;
            UnityEditor.PrefabUtility.CreatePrefab(filePath + ".prefab", prefab, UnityEditor.ReplacePrefabOptions.ReplaceNameBased);
        }

#endif

        #endregion
    }
}
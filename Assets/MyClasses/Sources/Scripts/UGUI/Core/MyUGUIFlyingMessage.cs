/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIFlyingMessage (version 2.1)
 */

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0649

using UnityEngine;
using UnityEngine.UI;

#if USE_MY_UI_TMPRO
using TMPro;
#endif

namespace MyClasses.UI
{
    public class MyUGUIFlyingMessage
    {
        #region ----- Variable -----

        public const string PREFAB_NAME = "FlyingMessage";

#if USE_MY_UI_TMPRO
        private TextMeshProUGUI mTextTMPro;
#endif

        private Text mText;

        private GameObject mGameObject;
        private Animator mAnimator;
        private CanvasGroup mCanvasGroup;

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

        public bool IsPlaying
        {
            get { return mCanvasGroup != null && mCanvasGroup.alpha > 0; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIFlyingMessage()
        {
#if UNITY_EDITOR
            if (!_CheckPrefab())
            {
                _CreatePrefab();
            }
#endif
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Show.
        /// </summary>
        public void Show(string content, EType type = EType.ShortFlyFromBot)
        {
            if (mGameObject != null)
            {
                if (mCanvasGroup == null)
                {
                    mCanvasGroup = mGameObject.GetComponent<CanvasGroup>();
                    if (mCanvasGroup == null)
                    {
                        Debug.LogError("[" + typeof(MyUGUIFlyingMessage).Name + "] Show(): Could not find Canvas Group component.");
                        return;
                    }
                }

                if (mText == null)
                {
                    mText = MyUtilities.FindObjectInAllLayers(mGameObject, "Text").GetComponent<Text>();
                }
#if USE_MY_UI_TMPRO
                if (mTextTMPro == null)
                {
                    mTextTMPro = MyUtilities.FindObjectInAllLayers(mGameObject, "Text").GetComponent<TextMeshProUGUI>();
                }
#endif

                mGameObject.SetActive(true);

                if (mText != null)
                {
                    mText.text = content;
                }
#if USE_MY_UI_TMPRO
                if (mTextTMPro != null)
                {
                    mTextTMPro.text = content;
                }
#endif

                if (mAnimator == null)
                {
                    mAnimator = mGameObject.GetComponent<Animator>();
                }
                if (mAnimator != null)
                {
                    mAnimator.Play(type.ToString());
                }
            }
        }

        /// <summary>
        /// Hide.
        /// </summary>
        public void Hide()
        {
            if (mGameObject != null)
            {
                mGameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Create a template game object.
        /// </summary>
        public static GameObject CreateTemplate()
        {
            GameObject obj = new GameObject(PREFAB_NAME);

            RectTransform root_rect = obj.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref root_rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

#if UNITY_EDITOR
            Animator root_animator = obj.AddComponent<Animator>();
            string[] paths = new string[] { "Assets/MyClasses", "Assets/Core/MyClasses", "Assets/Plugin/MyClasses", "Assets/Plugins/MyClasses", "Assets/Framework/MyClasses", "Assets/Frameworks/MyClasses" };
            for (int i = 0; i < paths.Length; i++)
            {
                if (System.IO.File.Exists(paths[i] + "/Sources/Animations/my_animator_flying_message.controller"))
                {
                    root_animator.runtimeAnimatorController = (RuntimeAnimatorController)UnityEditor.AssetDatabase.LoadAssetAtPath(paths[i] + "/Sources/Animations/my_animator_flying_message.controller", typeof(RuntimeAnimatorController));
                    Debug.LogError("[" + typeof(MyUGUIFlyingMessage).Name + "] CreateTemplate(): please setup \"my_animator_flying_message\" controller.");
                    Debug.LogError("[" + typeof(MyUGUIFlyingMessage).Name + "] CreateTemplate(): mapping \"my_animation_flying_message_short_fly_from_bot\" motion for \"ShortFlyFromBot\" state.");
                    Debug.LogError("[" + typeof(MyUGUIFlyingMessage).Name + "] CreateTemplate(): mapping \"my_animation_flying_message_long_fly_from_bot\" motion for \"LongFlyFromBot\" state.");
                    Debug.LogError("[" + typeof(MyUGUIFlyingMessage).Name + "] CreateTemplate(): mapping \"my_animation_flying_message_short_fly_from_mid\" motion for \"ShortFlyFromMid\" state.");
                    Debug.LogError("[" + typeof(MyUGUIFlyingMessage).Name + "] CreateTemplate(): mapping \"my_animation_flying_message_long_fly_from_mid\" motion for \"LongFlyFromBot\" state.");
                    break;
                }
            }
#endif

            CanvasGroup root_canvasGroup = obj.AddComponent<CanvasGroup>();
            root_canvasGroup.alpha = 1;
            root_canvasGroup.interactable = false;
            root_canvasGroup.blocksRaycasts = false;

            GameObject text = new GameObject("Text");
            text.transform.SetParent(root_rect.transform, false);

            RectTransform text_rect = text.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref text_rect, MyUtilities.EAnchorPreset.HorizontalStretchMiddle, MyUtilities.EAnchorPivot.MiddleCenter, -400, 50, 0, 0);

            Text text_text = text.AddComponent<Text>();
            text_text.text = "This is flying message";
            text_text.color = Color.white;
            text_text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text_text.fontSize = 50;
            text_text.alignment = TextAnchor.MiddleCenter;
            text_text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text_text.verticalOverflow = VerticalWrapMode.Overflow;
            text_text.raycastTarget = false;

            Outline text_outline = text.AddComponent<Outline>();
            text_outline.effectColor = Color.black;
            text_outline.effectDistance = new Vector2(2, 2);
            text_outline.useGraphicAlpha = false;

            Shadow text_shadow = text.AddComponent<Shadow>();
            text_shadow.effectColor = Color.black;
            text_shadow.effectDistance = new Vector2(1, -2);
            text_shadow.useGraphicAlpha = false;

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
            Debug.Log("[" + typeof(MyUGUIFlyingMessage).Name + "] CreatePrefab(): a template prefab was created.");

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

        #region ----- Enumeration -----

        public enum EType
        {
            ShortFlyFromBot = 0,
            LongFlyFromBot = 1,
            ShortFlyFromMid = 2,
            LongFlyFromMid = 3
        }

        #endregion
    }
}
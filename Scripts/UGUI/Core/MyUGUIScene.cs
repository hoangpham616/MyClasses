/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIScene (version 2.0)
 */

using UnityEngine;

namespace MyClasses.UI
{
    public abstract class MyUGUIScene : MyUGUIBase
    {
        #region ----- Variable -----

        private ESceneID mID;
        private EUnitySceneID mUnitySceneID;
        private bool mIsInitWhenLoadUnityScene;
        private bool mIsHideHUD;
        private float mFadeInDuration;
        private float mFadeOutDuration;

        #endregion

        #region ----- Property -----

        public ESceneID ID
        {
            get { return mID; }
        }

        public EUnitySceneID UnitySceneID
        {
            get { return mUnitySceneID; }
            set { mUnitySceneID = value; }
        }

        public bool IsInitWhenLoadUnityScene
        {
            get { return mIsInitWhenLoadUnityScene; }
        }

        public bool IsHideHUD
        {
            get { return mIsHideHUD; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIScene(ESceneID id, string prefabName, bool isInitWhenLoadUnityScene, bool isHideHUD = false, float fadeInDuration = 0.5f, float fadeOutDuration = 0.5f)
            : base(prefabName)
        {
            mID = id;
            mIsInitWhenLoadUnityScene = isInitWhenLoadUnityScene;
            mIsHideHUD = isHideHUD;
            mFadeInDuration = fadeInDuration;
            mFadeOutDuration = fadeOutDuration;
        }

        #endregion

        #region ----- Implement MyUGUIBase -----

        /// <summary>
        /// OnUGUIInit.
        /// </summary>
        public override void OnUGUIInit()
        {
            base.OnUGUIInit();

            Root = MyUtilities.FindObjectInFirstLayer(MyUGUIManager.Instance.Canvas, PrefabName);
            if (Root == null)
            {
                if (IsUseAssetBundle)
                {
                    if (Bundle == null)
                    {
                        Debug.LogError("[" + typeof(MyUGUIScene).Name + "] OnUGUIInit(): Asset bundle null.");
                    }
                    Root = GameObject.Instantiate(Bundle.LoadAsset(PrefabName), Vector3.zero, Quaternion.identity) as GameObject;
                }
                else
                {
                    string path = MyUGUIManager.SCENE_DIRECTORY + PrefabName;
                    GameObject template = Resources.Load<GameObject>(path);
                    if (template == null)
                    {
                        Debug.LogError("[" + typeof(MyUGUIScene).Name + "] OnUGUIInit(): Could not find file \"" + path + "\".");
                    }
                    Root = GameObject.Instantiate(template, Vector3.zero, Quaternion.identity) as GameObject;
                }
                Root.name = PrefabName;
                Root.transform.SetParent(MyUGUIManager.Instance.Canvas.transform, false);
            }
            Root.SetActive(false);
        }

        /// <summary>
        /// OnUGUIEnter.
        /// </summary>
        public override void OnUGUIEnter()
        {
            base.OnUGUIEnter();

            if (mFadeInDuration > 0)
            {
                MyUGUIManager.Instance.CurrentSceneFading.FadeIn(mFadeInDuration);
            }
        }

        /// <summary>
        /// OnUGUIVisible.
        /// </summary>
        public override bool OnUGUIVisible()
        {
            if (mFadeInDuration > 0 && MyUGUIManager.Instance.CurrentSceneFading != null)
            {
                return !MyUGUIManager.Instance.CurrentSceneFading.IsFading;
            }
            else
            {
                return base.OnUGUIVisible();
            }
        }

        /// <summary>
        /// OnUGUIUpdate.
        /// </summary>
        public override void OnUGUIUpdate(float deltaTime)
        {
        }

        /// <summary>
        /// OnUGUIExit.
        /// </summary>
        public override void OnUGUIExit()
        {
            base.OnUGUIExit();

            if (mFadeOutDuration > 0)
            {
                MyUGUIManager.Instance.CurrentSceneFading.FadeOut(mFadeOutDuration);
            }
        }

        /// <summary>
        /// OnUGUIInvisible.
        /// </summary>
        public override bool OnUGUIInvisible()
        {
            if (mFadeOutDuration > 0)
            {
                return !MyUGUIManager.Instance.CurrentSceneFading.IsFading;
            }
            else
            {
                return base.OnUGUIVisible();
            }
        }

        /// <summary>
        /// OnDestroy.
        /// </summary>
        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        #endregion
    }
}
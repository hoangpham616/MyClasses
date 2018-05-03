/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIBase (version 2.8)
 */

using UnityEngine;

namespace MyClasses.UI
{
    public abstract class MyUGUIBase
    {
        #region ----- Variable -----

        private EBaseState mState;
        private GameObject mGameObject;
        private AssetBundle mAssetBundle;
        private string mAssetBundleURL;
        private string mPrefabName;
        private int mAssetBundleVersion;
        private bool mIsAssetBundleLoading;
        private bool mIsAssetBundleLoaded;
        private bool mIsLoaded;

        #endregion

        #region ----- Property -----

        public EBaseState State
        {
            get { return mState; }
            set { mState = value; }
        }

        public bool IsUseAssetBundle
        {
            get { return !string.IsNullOrEmpty(mAssetBundleURL); }
        }

        public string PrefabName
        {
            get { return mPrefabName; }
        }

        public GameObject GameObject
        {
            get { return mGameObject; }
            protected set { mGameObject = value; }
        }

        public Transform Transform
        {
            get { return mGameObject != null ? mGameObject.transform : null; }
        }

        public AssetBundle Bundle
        {
            get { return mAssetBundle; }
        }

        public bool IsActive
        {
            get { return mGameObject != null && mGameObject.activeSelf; }
        }

        public bool IsLoaded
        {
            get { return mIsLoaded; }
            protected set { mIsLoaded = value; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIBase(string prefabName)
        {
            mState = EBaseState.Idle;
            mPrefabName = prefabName;
            mIsAssetBundleLoaded = false;
        }

        #endregion

        #region ----- UGUI Event -----

        /// <summary>
        /// OnUGUILoadAssetBundle.
        /// </summary>
        public bool OnUGUILoadAssetBundle()
        {
            if (!IsUseAssetBundle || mIsAssetBundleLoaded)
            {
                return true;
            }

            if (!mIsAssetBundleLoading)
            {
                MyAssetBundleManager.Load(mAssetBundleURL, mAssetBundleVersion, _OnLoadAssetBundleComplete, MyAssetBundleManager.ECacheMode.UnremovableCache);
                mIsAssetBundleLoading = true;
            }

            return mIsAssetBundleLoaded;
        }

        /// <summary>
        /// OnUGUIInit.
        /// </summary>
        public virtual void OnUGUIInit()
        {
            mIsLoaded = true;
        }

        /// <summary>
        /// OnUGUIEnter.
        /// </summary>
        public virtual void OnUGUIEnter()
        {
            if (mGameObject != null)
            {
                mGameObject.SetActive(true);
            }
        }

        /// <summary>
        /// OnUGUIVisible.
        /// </summary>
        public virtual bool OnUGUIVisible()
        {
            return true;
        }

        /// <summary>
        /// OnUGUIUpdate.
        /// </summary>
        public virtual void OnUGUIUpdate(float deltaTime)
        {
        }

        /// <summary>
        /// OnUGUIExit.
        /// </summary>
        public virtual void OnUGUIExit()
        {
        }

        /// <summary>
        /// OnUGUIInvisible.
        /// </summary>
        public virtual bool OnUGUIInvisible()
        {
            return true;
        }

        /// <summary>
        /// OnDestroy.
        /// </summary>
        public virtual void OnUGUIDestroy()
        {
            mIsAssetBundleLoading = false;
            mIsAssetBundleLoaded = false;
            mIsLoaded = false;
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Setup asset bundle.
        /// </summary>
        public void SetAssetBundle(string url, int versionCode)
        {
            mAssetBundleURL = url;
            mAssetBundleVersion = versionCode;
        }

        #endregion

        #region ----- Asset Bundle Event -----

        /// <summary>
        /// Loading asset bundle complete.
        /// </summary>
        private void _OnLoadAssetBundleComplete(AssetBundle assetBundle)
        {
            mAssetBundle = assetBundle;
            mIsAssetBundleLoaded = true;
        }

        #endregion
    }
}
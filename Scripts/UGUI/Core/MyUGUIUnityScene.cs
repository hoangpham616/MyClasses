/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIUnityScene (version 2.0)
 */

using System.Collections.Generic;

namespace MyClasses.UI
{
    public class MyUGUIUnityScene
    {
        #region ----- Variable -----

        private EUnitySceneID mID;
        private EUnitySceneState mState;
        private MyUGUIHUD mHUD;
        private List<MyUGUIScene> mListScene;
        private string mName;
        
        #endregion

        #region ----- Property -----

        public EUnitySceneID ID
        {
            get { return mID; }
        }

        public EUnitySceneState State
        {
            get { return mState; }
            set { mState = value; }
        }

        public MyUGUIHUD HUD
        {
            get { return mHUD; }
        }

        public List<MyUGUIScene> ListScene
        {
            get { return mListScene; }
        }

        public string Name
        {
            get { return mName; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIUnityScene(EUnitySceneID sceneID, string unitySceneName)
        {
            mID = sceneID;
            mState = EUnitySceneState.Idle;
            mName = unitySceneName;
            mHUD = null;
            mListScene = new List<MyUGUIScene>();
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set HUD.
        /// </summary>
        public void SetHUD(MyUGUIHUD hud)
        {
            mHUD = hud;
        }

        /// <summary>
        /// Add a scene.
        /// </summary>
        public void AddScene(MyUGUIScene scene)
        {
            if (scene != null)
            {
                mListScene.Add(scene);
            }
        }

        #endregion
    }
}
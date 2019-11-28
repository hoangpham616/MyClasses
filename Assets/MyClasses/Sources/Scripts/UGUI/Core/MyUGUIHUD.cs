/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIHUD (version 2.3)
 */

using UnityEngine;

namespace MyClasses.UI
{
    public abstract class MyUGUIHUD : MyUGUIBase
    {
        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIHUD(string prefabName)
            : base(prefabName)
        {
        }

        #endregion

        #region ----- MyUGUIBase Implementation -----

        /// <summary>
        /// OnUGUIInit.
        /// </summary>
        public override void OnUGUIInit()
        {
            base.OnUGUIInit();

            GameObject = MyUtilities.FindObjectInFirstLayer(MyUGUIManager.Instance.CanvasOnTopHUD, PrefabName);
            if (GameObject == null)
            {
                GameObject = GameObject.Instantiate(Resources.Load(MyUGUIManager.HUD_DIRECTORY + PrefabName), Vector3.zero, Quaternion.identity) as GameObject;
                GameObject.name = PrefabName;
                GameObject.transform.SetParent(MyUGUIManager.Instance.CanvasOnTopHUD.transform, false);
            }
            GameObject.SetActive(false);
        }

        /// <summary>
        /// OnUGUIEnter.
        /// </summary>
        public override void OnUGUIEnter()
        {
            base.OnUGUIEnter();
        }

        /// <summary>
        /// OnUGUIVisible.
        /// </summary>
        public override bool OnUGUIVisible()
        {
            return true;
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
        }

        /// <summary>
        /// OnUGUIInvisible.
        /// </summary>
        public override bool OnUGUIInvisible()
        {
            return true;
        }

        /// <summary>
        /// OnUGUIDestroy.
        /// </summary>
        public override void OnUGUIDestroy()
        {
            base.OnUGUIDestroy();

            IsLoaded = false;
        }

        /// <summary>
        /// OnUGUISceneSwitch.
        /// </summary>
        public virtual void OnUGUISceneSwitch(MyUGUIScene scene)
        {
        }

        #endregion
    }
}
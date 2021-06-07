/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUISampleScene (version 2.23)
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using MyClasses;
using MyClasses.UI;

namespace MyApp
{
    public class MyUGUISampleScene : MyUGUIScene
    {
        #region ----- Variable -----

        private MyUGUIButton _btnTest;

        #endregion

        #region ----- Constructor -----

        public MyUGUISampleScene(ESceneID id, string prefabName, bool isInitWhenLoadScene, bool isHideHUD = false, float fadeInDuration = 0.5f, float fadeOutDuration = 0.5f)
            : base(id, prefabName, isInitWhenLoadScene, isHideHUD, fadeInDuration, fadeOutDuration)
        {
        }

        #endregion

        #region ----- MyUGUIScene Implementation -----

        public override void OnUGUIInit()
        {
            base.OnUGUIInit();

            _btnTest = MyUtilities.FindObject(GameObject, "Something/ButtonTest").GetComponent<MyUGUIButton>();
        }

        public override void OnUGUIEnter()
        {
            base.OnUGUIEnter();

            _btnTest.OnEventPointerClick.AddListener(_OnClickTest);
        }

        public override bool OnUGUIVisible()
        {
            if (base.OnUGUIVisible())
            {
                return true;
            }
            return false;
        }

        public override void OnUGUIUpdate(float deltaTime)
        {
        }

        public override void OnUGUIExit()
        {
            base.OnUGUIExit();

            _btnTest.OnEventPointerClick.RemoveAllListeners();
        }

        public override bool OnUGUIInvisible()
        {
            if (base.OnUGUIInvisible())
            {
                return true;
            }
            return false;
        }

        public override void OnUGUIBackKey()
        {
            MyUGUIManager.Instance.Back();
        }

        #endregion

        #region ----- Button Event -----

        private void _OnClickTest(PointerEventData arg0)
        {
        }

        #endregion

        #region ----- Public Method -----



        #endregion

        #region ----- Private Method -----



        #endregion
    }
}
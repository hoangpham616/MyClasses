/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUISampleHUD (version 2.8)
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MyClasses;
using MyClasses.UI;

public class MyUGUISampleHUD : MyUGUIHUD
{
    #region ----- Variable -----

    //private MyUGUIButton _btnBack;

    #endregion

    #region ----- Constructor -----

    public MyUGUISampleHUD(string prefabName)
        : base(prefabName)
    {
    }

    #endregion

    #region ----- Implement MyUGUIHUD -----

    public override void OnUGUIInit()
    {
        base.OnUGUIInit();

        //_btnBack = MyUtilities.FindObjectInAllLayers(GameObject, "ButtonBack").GetComponent<MyUGUIButton>();
    }

    public override void OnUGUIEnter()
    {
        base.OnUGUIEnter();

        //_btnBack.OnEventPointerClick.AddListener(_OnClickBack());
    }

    public override void OnUGUIUpdate(float deltaTime)
    {
    }

    public override void OnUGUIExit()
    {
        base.OnUGUIExit();

        //_btnBack.OnEventPointerClick.RemoveAllListeners();
    }

    public override void OnUGUISceneSwitch(MyUGUIScene scene)
    {
    }

    #endregion

    #region ----- Button Event -----

    private void _OnClickBack(PointerEventData arg0)
    {
        //MyUGUIManager.Instance.Back();
    }

    #endregion

    #region ----- Public Method -----



    #endregion

    #region ----- Private Method -----



    #endregion
}
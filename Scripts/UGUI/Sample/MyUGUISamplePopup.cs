/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUISamplePopup (version 2.1)
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MyClasses;
using MyClasses.UI;

public class MyUGUISamplePopup : MyUGUIPopup
{
    #region ----- Variable -----

    //private MyUGUIButton _btnClose;

    #endregion

    #region ----- Constructor -----

    public MyUGUISamplePopup(EPopupID id, string prefabName, bool isFloat = false, bool isRepeatable = false)
        : base(id, prefabName, isFloat, isRepeatable)
    {
    }

    #endregion

    #region ----- Implement MyUGUIPopup -----

    public override void OnUGUIInit()
    {
        base.OnUGUIInit();

        //_btnClose = MyUtilities.FindObjectInAllLayers(Root, "ButtonClose").GetComponent<MyUGUIButton>();
    }

    public override void OnUGUIEnter()
    {
        base.OnUGUIEnter();

        //_btnClose.OnEventPointerClick.AddListener(_OnClickClose());
    }

    public override bool OnUGUIVisible()
    {
        return base.OnUGUIInvisible();
    }

    public override void OnUGUIUpdate(float deltaTime)
    {
    }

    public override void OnUGUIExit()
    {
        base.OnUGUIExit();

        //_btnClose.OnEventPointerClick.RemoveAllListeners();
    }

    public override bool OnUGUIInvisible()
    {
        return base.OnUGUIInvisible();
    }

    #endregion

    #region ----- Button Event -----
    
    private void _OnClickClose(PointerEventData arg0)
    {
        Hide();
    }

    #endregion

    #region ----- Public Method -----



    #endregion

    #region ----- Private Method -----



    #endregion
}
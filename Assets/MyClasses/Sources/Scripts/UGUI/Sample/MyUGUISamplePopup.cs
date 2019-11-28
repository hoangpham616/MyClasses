/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUISamplePopup (version 2.22)
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using MyClasses;
using MyClasses.UI;

public class MyUGUISamplePopup : MyUGUIPopup
{
    #region ----- Variable -----

    private MyUGUIButton _btnClose;

    #endregion

    #region ----- Constructor -----

    public MyUGUISamplePopup(EPopupID id, string prefabName, bool isFloat = false, bool isRepeatable = false)
        : base(id, prefabName, isFloat, isRepeatable)
    {
    }

    #endregion

    #region ----- MyUGUIPopup Implementation -----

    public override void OnUGUIInit()
    {
        base.OnUGUIInit();

        _btnClose = MyUtilities.FindObject(GameObject, "Container/ButtonClose").GetComponent<MyUGUIButton>();
    }

    public override void OnUGUIEnter()
    {
        base.OnUGUIEnter();

        _btnClose.OnEventPointerClick.AddListener(_OnClickClose);
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

        _btnClose.OnEventPointerClick.RemoveAllListeners();
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
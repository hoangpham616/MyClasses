﻿/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIReusableListItem (version 2.0)
 */

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0649

using UnityEngine;

namespace MyClasses.UI
{
    public abstract class MyUGUIReusableListItem : MonoBehaviour
    {
        #region ----- Property -----

        public int Index { get; set; }

        #endregion

        #region ----- Event -----

        public abstract void OnReload();

        #endregion
    }
}

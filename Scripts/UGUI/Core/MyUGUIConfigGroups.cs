/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIConfigGroups (version 2.0)
 */

using UnityEngine;
using System;
using System.Collections.Generic;

namespace MyClasses.UI
{
    [Serializable]
    public class MyUGUIConfigGroups : ScriptableObject
    {
        public List<MyUGUIConfigGroup> ListGroup = new List<MyUGUIConfigGroup>();
    }

    [Serializable]
    public class MyUGUIConfigGroup
    {
        public bool IsFoldOut;
        public string Name;
        public int NumDefault;
        public List<string> ListID = new List<string>();
    }
}
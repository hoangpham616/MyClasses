/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyImageStyleConfig (version 1.0)
 */


using UnityEngine;
using System;

namespace MyClasses
{
    [Serializable]
    public class MyImageStyleConfig : ScriptableObject
    {
        public MyImageStyleManager.MyImageStyleInfo[] Infos;
    }
}
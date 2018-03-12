/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIConfigUnityScenes (version 2.0)
 */

using UnityEngine;
using System;
using System.Collections.Generic;

namespace MyClasses.UI
{
    [Serializable]
    public class MyUGUIConfigUnityScenes : ScriptableObject
    {
        public List<MyUGUIConfigUnityScene> ListUnityScene = new List<MyUGUIConfigUnityScene>();
    }

    [Serializable]
    public class MyUGUIConfigUnityScene
    {
        public bool IsFoldOut;
        public EUnitySceneID ID;
        public string SceneName;
        public int SceneNameIndex;
        public int HUDScriptNameIndex;
        public int HUDPrefabNameIndex;
        public string HUDScriptName;
        public string HUDPrefabName;
        public List<MyUGUIConfigScene> ListScene;
    }

    [Serializable]
    public class MyUGUIConfigScene
    {
        public bool IsFoldOut;
        public ESceneID ID;
        public int ScriptNameIndex;
        public int PrefabNameIndex;
        public string ScriptName;
        public string PrefabName;
        public string AssetBundleURL;
        public int AssetBundleVersion;
        public bool IsInitWhenLoadUnityScene;
        public bool IsHideHUD;
        public float FadeInDuration;
        public float FadeOutDuration;
    }
}
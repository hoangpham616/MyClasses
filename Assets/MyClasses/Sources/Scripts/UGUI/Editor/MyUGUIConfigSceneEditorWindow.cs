/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIConfigSceneEditorWindow (version 2.8)
 */

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;

namespace MyClasses.UI.Tool
{
    public class MyUGUIConfigSceneEditorWindow : EditorWindow
    {
        #region ----- Variable -----

        private MyUGUIConfigUnityScenes mUnityScenes;
        private Vector2 mScrollPosition;

        private string[] mScriptNames;
        private string[] mUnitySceneNames;
        private string[] mScenePrefabNames;
        private string[] mHUDPrefabNames;

        #endregion

        #region ----- Implement MonoBehaviour -----

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            if (!Directory.Exists("Assets/Resources/" + MyUGUIManager.CONFIG_DIRECTORY))
            {
                Directory.CreateDirectory("Assets/Resources/" + MyUGUIManager.CONFIG_DIRECTORY);
            }

            if (!Directory.Exists("Assets/Resources/" + MyUGUIManager.SCENE_DIRECTORY))
            {
                Directory.CreateDirectory("Assets/Resources/" + MyUGUIManager.SCENE_DIRECTORY);
            }

            if (!Directory.Exists("Assets/Resources/" + MyUGUIManager.POPUP_DIRECTORY))
            {
                Directory.CreateDirectory("Assets/Resources/" + MyUGUIManager.POPUP_DIRECTORY);
            }

            if (!Directory.Exists("Assets/Resources/" + MyUGUIManager.HUD_DIRECTORY))
            {
                Directory.CreateDirectory("Assets/Resources/" + MyUGUIManager.HUD_DIRECTORY);
            }

            _LoadAssetFile();
            _AddNewUnityScenes();
            _CorrectValues();
        }

        /// <summary>
        /// OnFocus.
        /// </summary>
        void OnFocus()
        {
            _CorrectValues();
        }

        #endregion

        #region ----- GUI Implementation -----

        /// <summary>
        /// OnGUI.
        /// </summary>
        void OnGUI()
        {
            mScrollPosition = EditorGUILayout.BeginScrollView(mScrollPosition, new GUILayoutOption[0]);
            for (int i = 0, countI = mUnityScenes.ListUnityScene.Count; i < countI; i++)
            {
                MyUGUIConfigUnityScene unityScene = mUnityScenes.ListUnityScene[i];

                EditorGUILayout.BeginHorizontal();
                unityScene.IsFoldOut = EditorGUILayout.Foldout(unityScene.IsFoldOut, unityScene.ID.ToString());
                if (unityScene.IsFoldOut)
                {
                    if (GUILayout.Button("+", GUILayout.Width(30)))
                    {
                        unityScene.ListScene.Add(new MyUGUIConfigScene()
                        {
                            IsFoldOut = true,
                            IsInitWhenLoadUnityScene = false,
                            IsHideHUD = false,
                            FadeInDuration = 0.2f,
                            FadeOutDuration = 0.2f,
                        });
                    }
                    EditorGUI.BeginDisabledGroup(unityScene.ListScene.Count == 0);
                    if (GUILayout.Button("-", GUILayout.Width(30)))
                    {
                        unityScene.ListScene.RemoveAt(unityScene.ListScene.Count - 1);
                    }
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUILayout.EndHorizontal();

                if (unityScene.IsFoldOut)
                {
                    EditorGUI.indentLevel++;

                    unityScene.SceneNameIndex = EditorGUILayout.Popup("Unity Scene", unityScene.SceneNameIndex, mUnitySceneNames);
                    unityScene.SceneName = mUnitySceneNames[unityScene.SceneNameIndex];
                    unityScene.SceneName = unityScene.SceneName.Equals("<null>") ? string.Empty : unityScene.SceneName.Substring(0, unityScene.SceneName.Length - 6);

                    unityScene.HUDScriptNameIndex = EditorGUILayout.Popup("HUD Script (Nullable)", unityScene.HUDScriptNameIndex, mScriptNames);
                    unityScene.HUDScriptName = mScriptNames[unityScene.HUDScriptNameIndex];
                    unityScene.HUDScriptName = unityScene.HUDScriptName.Equals("<null>") ? string.Empty : unityScene.HUDScriptName.Substring(0, unityScene.HUDScriptName.Length - 3);

                    unityScene.HUDPrefabNameIndex = EditorGUILayout.Popup("HUD Prefab (Nullable)", unityScene.HUDPrefabNameIndex, mHUDPrefabNames);
                    unityScene.HUDPrefabName = mHUDPrefabNames[unityScene.HUDPrefabNameIndex];
                    unityScene.HUDPrefabName = unityScene.HUDPrefabName.Equals("<null>") ? string.Empty : unityScene.HUDPrefabName.Substring(0, unityScene.HUDPrefabName.Length - 7);

                    for (int j = 0, countJ = unityScene.ListScene.Count; j < countJ; j++)
                    {
                        MyUGUIConfigScene scene = unityScene.ListScene[j];

                        scene.IsFoldOut = EditorGUILayout.Foldout(scene.IsFoldOut, scene.ID.ToString());
                        if (scene.IsFoldOut)
                        {
                            EditorGUI.indentLevel++;

                            scene.ID = (ESceneID)EditorGUILayout.EnumPopup("ID", scene.ID);

                            scene.ScriptNameIndex = EditorGUILayout.Popup("Script", scene.ScriptNameIndex, mScriptNames);
                            scene.ScriptName = mScriptNames[scene.ScriptNameIndex];
                            scene.ScriptName = scene.ScriptName.Equals("<null>") ? string.Empty : scene.ScriptName.Substring(0, scene.ScriptName.Length - 3);

                            scene.PrefabNameIndex = EditorGUILayout.Popup("Prefab", scene.PrefabNameIndex, mScenePrefabNames);
                            scene.PrefabName = mScenePrefabNames[scene.PrefabNameIndex];
                            scene.PrefabName = scene.PrefabName.Equals("<null>") ? string.Empty : scene.PrefabName.Substring(0, scene.PrefabName.Length - 7);

                            scene.IsInitWhenLoadUnityScene = EditorGUILayout.Toggle("Is Init When Load Unity Scene", scene.IsInitWhenLoadUnityScene);
                            scene.IsHideHUD = EditorGUILayout.Toggle("Is Hide HUD", scene.IsHideHUD);
                            scene.FadeInDuration = EditorGUILayout.FloatField("Fade-In Duration", scene.FadeInDuration);
                            scene.FadeOutDuration = EditorGUILayout.FloatField("Fade-Out Duration", scene.FadeOutDuration);

                            EditorGUI.indentLevel--;
                        }
                    }

                    EditorGUI.indentLevel--;
                }

                if (i < countI - 1)
                {
                    EditorGUILayout.LabelField(string.Empty);
                }
            }
            EditorGUILayout.EndScrollView();

            EditorUtility.SetDirty(mUnityScenes);

            EditorGUILayout.LabelField(string.Empty);
            if (GUILayout.Button("Reset", GUILayout.Width(100)))
            {
                _DeleteAssetFile();
                _LoadAssetFile();
                _AddNewUnityScenes();

                Debug.Log("[MyClasses] Data was reset.");
            }
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Delete the asset file.
        /// </summary>
        private void _DeleteAssetFile()
        {
            string filePath = "Assets/Resources/" + MyUGUIManager.CONFIG_DIRECTORY + typeof(MyUGUIConfigUnityScenes).Name + ".asset";
            if (File.Exists(filePath))
            {
                AssetDatabase.DeleteAsset(filePath);
            }

            mUnityScenes = null;
        }

        /// <summary>
        /// Load the asset file.
        /// </summary>
        private void _LoadAssetFile()
        {
            if (mUnityScenes != null)
            {
                return;
            }

            string filePath = "Assets/Resources/" + MyUGUIManager.CONFIG_DIRECTORY + typeof(MyUGUIConfigUnityScenes).Name + ".asset";
            mUnityScenes = AssetDatabase.LoadAssetAtPath(filePath, typeof(MyUGUIConfigUnityScenes)) as MyUGUIConfigUnityScenes;
            if (mUnityScenes == null)
            {
                mUnityScenes = ScriptableObject.CreateInstance<MyUGUIConfigUnityScenes>();
                AssetDatabase.CreateAsset(mUnityScenes, filePath);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Update new popups.
        /// </summary>
        private void _AddNewUnityScenes()
        {
            if (mUnityScenes == null)
            {
                return;
            }

            foreach (EUnitySceneID id in Enum.GetValues(typeof(EUnitySceneID)))
            {
                bool isNewUnityScene = true;
                for (int i = 0, countI = mUnityScenes.ListUnityScene.Count; i < countI; i++)
                {
                    if (mUnityScenes.ListUnityScene[i].ID == id)
                    {
                        isNewUnityScene = false;
                        break;
                    }
                }
                if (isNewUnityScene)
                {
                    mUnityScenes.ListUnityScene.Add(new MyUGUIConfigUnityScene()
                    {
                        IsFoldOut = true,
                        ID = id,
                        ListScene = new List<MyUGUIConfigScene>()
                    });
                }
            }
        }

        /// <summary>
        /// Correct values.
        /// </summary>
        private void _CorrectValues()
        {
            mScriptNames = _GetScriptNames();
            mUnitySceneNames = _GetUnitySceneNames();
            mScenePrefabNames = _GetPrefabNames(MyUGUIManager.SCENE_DIRECTORY);
            mHUDPrefabNames = _GetPrefabNames(MyUGUIManager.HUD_DIRECTORY);

            for (int i = 0, countI = mUnityScenes.ListUnityScene.Count; i < countI; i++)
            {
                MyUGUIConfigUnityScene unityScene = mUnityScenes.ListUnityScene[i];

                if (!string.IsNullOrEmpty(unityScene.SceneName) && (unityScene.SceneNameIndex >= mUnitySceneNames.Length || !unityScene.SceneName.Equals(mUnitySceneNames[unityScene.SceneNameIndex])))
                {
                    string unitySceneName = unityScene.SceneName + ".unity";
                    unityScene.SceneName = string.Empty;
                    unityScene.SceneNameIndex = 0;
                    for (int j = 0; j < mUnitySceneNames.Length; j++)
                    {
                        if (unitySceneName.Equals(mUnitySceneNames[j]))
                        {
                            unityScene.SceneNameIndex = j;
                            break;
                        }
                    }
                    if (unityScene.SceneNameIndex > 0)
                    {
                        unityScene.SceneName = mUnitySceneNames[unityScene.SceneNameIndex];
                        unityScene.SceneName = unityScene.SceneName.Equals("<null>") ? string.Empty : unityScene.SceneName.Substring(0, unityScene.SceneName.Length - 6);
                    }
                }

                if (!string.IsNullOrEmpty(unityScene.HUDScriptName) && (unityScene.HUDScriptNameIndex >= mScriptNames.Length || !unityScene.HUDScriptName.Equals(mScriptNames[unityScene.HUDScriptNameIndex])))
                {
                    string hudScriptName = unityScene.HUDScriptName + ".cs";
                    unityScene.HUDScriptName = string.Empty;
                    unityScene.HUDScriptNameIndex = 0;
                    for (int j = 0; j < mScriptNames.Length; j++)
                    {
                        if (hudScriptName.Equals(mScriptNames[j]))
                        {
                            unityScene.HUDScriptNameIndex = j;
                            break;
                        }
                    }
                    if (unityScene.HUDScriptNameIndex > 0)
                    {
                        unityScene.HUDScriptName = mScriptNames[unityScene.HUDScriptNameIndex];
                        unityScene.HUDScriptName = unityScene.HUDScriptName.Equals("<null>") ? string.Empty : unityScene.HUDScriptName.Substring(0, unityScene.HUDScriptName.Length - 3);
                    }
                }

                if (!string.IsNullOrEmpty(unityScene.HUDPrefabName) && (unityScene.HUDPrefabNameIndex >= mHUDPrefabNames.Length || !unityScene.HUDPrefabName.Equals(mHUDPrefabNames[unityScene.HUDPrefabNameIndex])))
                {
                    string hudPrefabName = unityScene.HUDPrefabName + ".prefab";
                    unityScene.HUDPrefabName = string.Empty;
                    unityScene.HUDPrefabNameIndex = 0;
                    for (int j = 0; j < mHUDPrefabNames.Length; j++)
                    {
                        if (hudPrefabName.Equals(mHUDPrefabNames[j]))
                        {
                            unityScene.HUDPrefabNameIndex = j;
                            break;
                        }
                    }
                    if (unityScene.HUDPrefabNameIndex > 0)
                    {
                        unityScene.HUDPrefabName = mHUDPrefabNames[unityScene.HUDPrefabNameIndex];
                        unityScene.HUDPrefabName = unityScene.HUDPrefabName.Equals("<null>") ? string.Empty : unityScene.HUDPrefabName.Substring(0, unityScene.HUDPrefabName.Length - 7);
                    }
                }

                for (int j = 0, countJ = unityScene.ListScene.Count; j < countJ; j++)
                {
                    MyUGUIConfigScene scene = unityScene.ListScene[j];

                    if (scene.ScriptNameIndex >= mScriptNames.Length || !scene.ScriptName.Equals(mScriptNames[scene.ScriptNameIndex]))
                    {
                        string scriptName = scene.ScriptName + ".cs";
                        scene.ScriptName = string.Empty;
                        scene.ScriptNameIndex = 0;
                        for (int k = 0; k < mScriptNames.Length; k++)
                        {
                            if (scriptName.Equals(mScriptNames[k]))
                            {
                                scene.ScriptNameIndex = k;
                                break;
                            }
                        }
                        if (scene.ScriptNameIndex > 0)
                        {
                            scene.ScriptName = mScriptNames[scene.ScriptNameIndex];
                            scene.ScriptName = scene.ScriptName.Equals("<null>") ? string.Empty : scene.ScriptName.Substring(0, scene.ScriptName.Length - 3);
                        }
                    }

                    if (scene.PrefabNameIndex >= mScenePrefabNames.Length || !scene.PrefabName.Equals(mScenePrefabNames[scene.PrefabNameIndex]))
                    {
                        string prefabName = scene.PrefabName + ".prefab";
                        scene.PrefabName = string.Empty;
                        scene.PrefabNameIndex = 0;
                        for (int k = 0; k < mScenePrefabNames.Length; k++)
                        {
                            if (prefabName.Equals(mScenePrefabNames[k]))
                            {
                                scene.PrefabNameIndex = k;
                                break;
                            }
                        }
                        if (scene.PrefabNameIndex > 0)
                        {
                            scene.PrefabName = mScenePrefabNames[scene.PrefabNameIndex];
                            scene.PrefabName = scene.PrefabName.Equals("<null>") ? string.Empty : scene.PrefabName.Substring(0, scene.PrefabName.Length - 7);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Return list unity scene name.
        /// </summary>
        private string[] _GetUnitySceneNames()
        {
            List<string> listUnitySceneNames = new List<string>() { "<null>" };

            string[] unitySceneNames = Directory.GetFiles("Assets/", "*.unity", SearchOption.AllDirectories);
            if (unitySceneNames != null && unitySceneNames.Length > 0)
            {
                foreach (string unitySceneName in unitySceneNames)
                {
                    if (!(unitySceneName.StartsWith("MyClasses", StringComparison.Ordinal)
                          || unitySceneName.StartsWith("Assets/Core", StringComparison.Ordinal)
                          || unitySceneName.StartsWith("Assets/Framework", StringComparison.Ordinal)
                          || unitySceneName.StartsWith("Assets/Plugin", StringComparison.Ordinal)))
                    {
#if UNITY_EDITOR_WIN
                        listUnitySceneNames.Add(unitySceneName.Substring(unitySceneName.LastIndexOf('\\') + 1));
#else
                        listUnitySceneNames.Add(unitySceneName.Substring(unitySceneName.LastIndexOf('/') + 1));
#endif
                    }
                }
            }

            return listUnitySceneNames.ToArray();
        }

        /// <summary>
        /// Return list script name.
        /// </summary>
        private string[] _GetScriptNames()
        {
            List<string> listScriptNames = new List<string>() { "<null>" };

            string[] scriptNames = Directory.GetFiles("Assets/", "*.cs", SearchOption.AllDirectories);
            if (scriptNames != null && scriptNames.Length > 0)
            {
                foreach (string scriptName in scriptNames)
                {
                    if (!(scriptName.StartsWith("MyClasses", StringComparison.Ordinal)
                          || scriptName.StartsWith("Assets/Core", StringComparison.Ordinal)
                          || scriptName.StartsWith("Assets/Framework", StringComparison.Ordinal)
                          || scriptName.StartsWith("Assets/Plugin", StringComparison.Ordinal)))
                    {
#if UNITY_EDITOR_WIN
                        listScriptNames.Add(scriptName.Substring(scriptName.LastIndexOf('\\') + 1));
#else
                        listScriptNames.Add(scriptName.Substring(scriptName.LastIndexOf('/') + 1));
#endif
                    }
                }
            }

            return listScriptNames.ToArray();
        }

        /// <summary>
        /// Return list prefab name.
        /// </summary>
        private string[] _GetPrefabNames(string directory)
        {
            List<string> listPrefabNames = new List<string>();

            listPrefabNames.Add("<null>");

            string[] prefabNames = Directory.GetFiles("Assets/Resources/" + directory, "*.prefab");
            if (prefabNames != null && prefabNames.Length > 0)
            {
                foreach (string prefabName in prefabNames)
                {
                    listPrefabNames.Add(prefabName.Substring(prefabName.LastIndexOf('/') + 1));
                }
            }

            return listPrefabNames.ToArray();
        }

#endregion
    }
}

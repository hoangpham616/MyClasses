/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIConfigIDEditor (version 2.3)
 */

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace MyClasses.UI.Tool
{
    public class MyUGUIConfigIDEditorWindow : EditorWindow
    {
        #region ----- Variable -----

        private MyUGUIConfigGroups mGroups;
        private Vector2 mScrollPosition;

        #endregion

        #region ----- MonoBehaviour Implemention -----

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            if (!Directory.Exists("Assets/Resources/" + MyUGUIManager.SETTING_DIRECTORY))
            {
                Directory.CreateDirectory("Assets/Resources/" + MyUGUIManager.SETTING_DIRECTORY);
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
        }

        #endregion

        #region ----- Implement GUI -----

        /// <summary>
        /// OnGUI.
        /// </summary>
        void OnGUI()
        {
            mScrollPosition = EditorGUILayout.BeginScrollView(mScrollPosition, new GUILayoutOption[0]);
            for (int i = 0, countI = mGroups.ListGroup.Count; i < countI; i++)
            {
                MyUGUIConfigGroup group = mGroups.ListGroup[i];

                EditorGUILayout.BeginHorizontal();
                group.IsFoldOut = EditorGUILayout.Foldout(group.IsFoldOut, group.Name + " (" + group.ListID.Count + ")");
                if (group.IsFoldOut)
                {
                    if (GUILayout.Button("+", GUILayout.Width(30)))
                    {
                        group.ListID.Add(string.Empty);
                    }
                    EditorGUI.BeginDisabledGroup(group.ListID.Count <= group.NumDefault);
                    if (GUILayout.Button("-", GUILayout.Width(30)))
                    {
                        if (group.ListID.Count > group.NumDefault)
                        {
                            group.ListID.RemoveAt(group.ListID.Count - 1);
                        }
                    }
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUILayout.EndHorizontal();

                if (group.IsFoldOut)
                {
                    EditorGUI.indentLevel++;
                    EditorGUI.BeginDisabledGroup(true);
                    for (int j = 0; j < group.NumDefault; j++)
                    {
                        EditorGUILayout.TextField(group.ListID[j], GUILayout.Width(400));
                    }
                    EditorGUI.EndDisabledGroup();
                    for (int j = group.NumDefault, countJ = group.ListID.Count; j < countJ; j++)
                    {
                        group.ListID[j] = EditorGUILayout.TextField(group.ListID[j], GUILayout.Width(400));
                    }
                    EditorGUI.indentLevel--;
                }

                if (i < countI - 1)
                {
                    EditorGUILayout.LabelField(string.Empty);
                }
            }
            EditorGUILayout.EndScrollView();

            EditorUtility.SetDirty(mGroups);

            EditorGUILayout.LabelField(string.Empty);
            if (GUILayout.Button("Apply", GUILayout.Width(100)))
            {
                _GenerateScript();
            }
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Load the asset file.
        /// </summary>
        private void _LoadAssetFile()
        {
            if (mGroups != null)
            {
                return;
            }

            string filePath = "Assets/Resources/" + MyUGUIManager.SETTING_DIRECTORY + typeof(MyUGUIConfigGroups).Name + ".asset";
            mGroups = AssetDatabase.LoadAssetAtPath(filePath, typeof(MyUGUIConfigGroups)) as MyUGUIConfigGroups;
            if (mGroups == null)
            {
                mGroups = ScriptableObject.CreateInstance<MyUGUIConfigGroups>();
                mGroups.ListGroup = new List<MyUGUIConfigGroup>();
                mGroups.ListGroup.Add(new MyUGUIConfigGroup()
                {
                    IsFoldOut = true,
                    Name = "EUnitySceneID",
                    NumDefault = 0,
                    ListID = new List<string>() { "StartupUnityScene", "MainUnityScene", "GameUnityScene" }
                });
                mGroups.ListGroup.Add(new MyUGUIConfigGroup()
                {
                    IsFoldOut = true,
                    Name = "ESceneID",
                    NumDefault = 0,
                    ListID = new List<string>()
                });
                mGroups.ListGroup.Add(new MyUGUIConfigGroup()
                {
                    IsFoldOut = true,
                    Name = "EPopupID",
                    NumDefault = 3,
                    ListID = new List<string>() { "Dialog0ButtonPopup", "Dialog1ButtonPopup", "Dialog2ButtonsPopup" }
                });
                AssetDatabase.CreateAsset(mGroups, filePath);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Generate the script.
        /// </summary>
        private void _GenerateScript()
        {
            if (mGroups == null)
            {
                return;
            }

            string scriptName = typeof(MyUGUIConfigIDEditorWindow).Name;
            string unityScenes = string.Empty;
            for (int i = 0, countI = mGroups.ListGroup[0].ListID.Count; i < countI; i++)
            {
                unityScenes += "\n\t\t" + mGroups.ListGroup[0].ListID[i] + ",";
            }
            string scenes = string.Empty;
            for (int i = 0, countI = mGroups.ListGroup[1].ListID.Count; i < countI; i++)
            {
                scenes += "\n\t\t" + mGroups.ListGroup[1].ListID[i] + ",";
            }
            string popups = string.Empty;
            for (int i = 0, countI = mGroups.ListGroup[2].ListID.Count; i < countI; i++)
            {
                popups += "\n\t\t" + mGroups.ListGroup[2].ListID[i] + ",";
            }
            string content = "/*\n * Copyright(c) 2016 Phạm Minh Hoàng\n * Framework:\tMyClasses\n * Description:\tThis script is generated by " + scriptName + "\n */\n\nnamespace MyClasses.UI\n{\n\tpublic enum EUnitySceneID\n\t{" + unityScenes + "\n\t}\n\n\tpublic enum ESceneID\n\t{" + scenes + "\n\t}\n\n\tpublic enum EPopupID\n\t{" + popups + "\n\t}\n}";

            string[] paths = new string[] { "Assets/MyClasses", "Assets/Core/MyClasses", "Assets/Plugin/MyClasses", "Assets/Plugins/MyClasses", "Assets/Framework/MyClasses", "Assets/Frameworks/MyClasses" };
            for (int i = 0; i < paths.Length; i++)
            {
                if (Directory.Exists(paths[i]))
                {
                    File.WriteAllText(paths[i] + "/Scripts/UGUI/Core/MyUGUIConfigID.cs", content);
                    AssetDatabase.Refresh();
                    Debug.Log("[MyClasses] MyUGUIConfigID was created.");
                    return;
                }
            }

            Debug.LogError("[MyClasses] Could not find MyClasses location.");
        }

        #endregion
    }
}
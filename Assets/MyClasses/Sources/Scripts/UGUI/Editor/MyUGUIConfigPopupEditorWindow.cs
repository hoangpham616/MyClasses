/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIConfigPopupEditorWindow (version 2.7)
 */

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;

namespace MyClasses.UI.Tool
{
    public class MyUGUIConfigPopupEditorWindow : EditorWindow
    {
        #region ----- Variable -----

        private MyUGUIConfigPopups mPopups;
        private Vector2 mScrollPosition;

        private string[] mScriptNames;
        private string[] mPrefabNames;

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
            _UpdateNewPopups();
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
            for (int i = 0, countI = mPopups.ListPopup.Count; i < countI; i++)
            {
                MyUGUIConfigPopup popup = mPopups.ListPopup[i];

                popup.IsFoldOut = EditorGUILayout.Foldout(popup.IsFoldOut, popup.ID.ToString());
                if (popup.IsFoldOut)
                {
                    EditorGUI.indentLevel++;
                    if (i < mPopups.NumDefault)
                    {
                        EditorGUI.BeginDisabledGroup(i < mPopups.NumDefault);
                        EditorGUILayout.TextField("Script", popup.ScriptName + ".cs", GUILayout.Width(400));
                        EditorGUILayout.TextField("Prefab", popup.PrefabName + ".prefab", GUILayout.Width(400));
                        EditorGUI.EndDisabledGroup();
                    }
                    else
                    {
                        popup.ScriptNameIndex = EditorGUILayout.Popup("Script", popup.ScriptNameIndex, mScriptNames);
                        popup.ScriptName = mScriptNames[popup.ScriptNameIndex];
                        popup.ScriptName = popup.ScriptName.Equals("<empty>") ? string.Empty : popup.ScriptName.Substring(0, popup.ScriptName.Length - 3);

                        popup.PrefabNameIndex = EditorGUILayout.Popup("Prefab", popup.PrefabNameIndex, mPrefabNames);
                        popup.PrefabName = mPrefabNames[popup.PrefabNameIndex];
                        popup.PrefabName = popup.PrefabName.Equals("<empty>") ? string.Empty : popup.PrefabName.Substring(0, popup.PrefabName.Length - 7);
                    }
                    EditorGUI.indentLevel--;
                }

                if (i < countI - 1)
                {
                    EditorGUILayout.LabelField(string.Empty);
                }
            }
            EditorGUILayout.EndScrollView();

            EditorUtility.SetDirty(mPopups);

            EditorGUILayout.LabelField(string.Empty);
            if (GUILayout.Button("Reset", GUILayout.Width(100)))
            {
                _DeleteAssetFile();
                _LoadAssetFile();
                _UpdateNewPopups();

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
            string filePath = "Assets/Resources/" + MyUGUIManager.CONFIG_DIRECTORY + typeof(MyUGUIConfigPopups).Name + ".asset";
            if (File.Exists(filePath))
            {
                AssetDatabase.DeleteAsset(filePath);
            }

            mPopups = null;
        }

        /// <summary>
        /// Load the asset file.
        /// </summary>
        private void _LoadAssetFile()
        {
            if (mPopups != null)
            {
                return;
            }

            string filePath = "Assets/Resources/" + MyUGUIManager.CONFIG_DIRECTORY + typeof(MyUGUIConfigPopups).Name + ".asset";
            mPopups = AssetDatabase.LoadAssetAtPath(filePath, typeof(MyUGUIConfigPopups)) as MyUGUIConfigPopups;
            if (mPopups == null)
            {
                mPopups = ScriptableObject.CreateInstance<MyUGUIConfigPopups>();
                mPopups.ListPopup = new List<MyUGUIConfigPopup>();
                mPopups.ListPopup.Add(new MyUGUIConfigPopup()
                {
                    IsFoldOut = true,
                    ID = EPopupID.Dialog0ButtonPopup,
                    ScriptName = typeof(MyUGUIPopup0Button).ToString(),
                    PrefabName = EPopupID.Dialog0ButtonPopup.ToString()
                });
                mPopups.ListPopup.Add(new MyUGUIConfigPopup()
                {
                    IsFoldOut = true,
                    ID = EPopupID.Dialog1ButtonPopup,
                    ScriptName = typeof(MyUGUIPopup1Button).ToString(),
                    PrefabName = EPopupID.Dialog1ButtonPopup.ToString()
                });
                mPopups.ListPopup.Add(new MyUGUIConfigPopup()
                {
                    IsFoldOut = true,
                    ID = EPopupID.Dialog2ButtonsPopup,
                    ScriptName = typeof(MyUGUIPopup2Buttons).ToString(),
                    PrefabName = EPopupID.Dialog2ButtonsPopup.ToString()
                });
                mPopups.NumDefault = mPopups.ListPopup.Count;
                AssetDatabase.CreateAsset(mPopups, filePath);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Update new popups.
        /// </summary>
        private void _UpdateNewPopups()
        {
            if (mPopups == null)
            {
                return;
            }

            foreach (EPopupID item in Enum.GetValues(typeof(EPopupID)))
            {
                if (item == EPopupID.Dialog0ButtonPopup || item == EPopupID.Dialog1ButtonPopup || item == EPopupID.Dialog2ButtonsPopup)
                {
                    continue;
                }

                bool isNewPopup = true;
                for (int i = 0, countI = mPopups.ListPopup.Count; i < countI; i++)
                {
                    if (mPopups.ListPopup[i].ID == item)
                    {
                        isNewPopup = false;
                        break;
                    }
                }
                if (isNewPopup)
                {
                    mPopups.ListPopup.Add(new MyUGUIConfigPopup()
                    {
                        IsFoldOut = true,
                        ID = item,
                        ScriptName = string.Empty,
                        PrefabName = string.Empty,
                    });
                }
            }
        }

        /// <summary>
        /// Correct values.
        /// </summary>
        private void _CorrectValues()
        {
            mScriptNames = _GetPopupScriptNames();
            mPrefabNames = _GetPopupPrefabNames();

            for (int i = 0, countI = mPopups.ListPopup.Count; i < countI; i++)
            {
                MyUGUIConfigPopup popup = mPopups.ListPopup[i];

                if (popup.ScriptNameIndex >= mScriptNames.Length || !popup.ScriptName.Equals(mScriptNames[popup.ScriptNameIndex]))
                {
                    string scriptName = popup.ScriptName + ".cs";
                    popup.ScriptNameIndex = 0;
                    for (int j = 0; j < mScriptNames.Length; j++)
                    {
                        if (scriptName.Equals(mScriptNames[j]))
                        {
                            popup.ScriptNameIndex = j;
                            break;
                        }
                    }
                }

                if (popup.PrefabNameIndex >= mPrefabNames.Length || !popup.PrefabName.Equals(mPrefabNames[popup.PrefabNameIndex]))
                {
                    string prefabName = popup.PrefabName + ".prefab";
                    popup.PrefabNameIndex = 0;
                    for (int j = 0; j < mPrefabNames.Length; j++)
                    {
                        if (prefabName.Equals(mPrefabNames[j]))
                        {
                            popup.PrefabNameIndex = j;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Return list popup script name.
        /// </summary>
        private string[] _GetPopupScriptNames()
        {
            List<string> listScriptNames = new List<string>();

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

            if (listScriptNames.Count == 0)
            {
                listScriptNames.Add("<empty>");
            }

            return listScriptNames.ToArray();
        }

        /// <summary>
        /// Return list popup prefab name.
        /// </summary>
        private string[] _GetPopupPrefabNames()
        {
            List<string> listPrefabNames = new List<string>();

            string[] prefabNames = Directory.GetFiles("Assets/Resources/" + MyUGUIManager.POPUP_DIRECTORY, "*.prefab");
            if (prefabNames != null && prefabNames.Length > 0)
            {
                foreach (string prefabName in prefabNames)
                {
                    listPrefabNames.Add(prefabName.Substring(prefabName.LastIndexOf('/') + 1));
                }
            }

            listPrefabNames.Remove(EPopupID.Dialog0ButtonPopup.ToString() + ".prefab");
            listPrefabNames.Remove(EPopupID.Dialog1ButtonPopup.ToString() + ".prefab");
            listPrefabNames.Remove(EPopupID.Dialog2ButtonsPopup.ToString() + ".prefab");

            if (listPrefabNames.Count == 0)
            {
                listPrefabNames.Add("<empty>");
            }

            return listPrefabNames.ToArray();
        }

        #endregion
    }
}

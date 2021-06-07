/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyToolEditor (version 1.4)
 */

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace MyClasses.Tool
{
    public class MyToolEditor
    {
        #region ----- Managers -----

        /// <summary>
        /// Create a game object with MyLocalizationManager attached.
        /// </summary>
        [MenuItem("MyClasses/Managers/MyLocalizationManager/Create", false, 1)]
        public static void CreateMyLocalizationManager()
        {
            MyLocalizationManager script = GameObject.FindObjectOfType<MyLocalizationManager>();
            if (script != null)
            {
                EditorGUIUtility.PingObject(script);
                Selection.activeGameObject = script.gameObject;

                Debug.Log("[MyClasses] " + typeof(MyLocalizationManager).Name + " is existed.");
            }
            else
            {
                MyLocalizationManager.CreateTemplate();

                Debug.Log("[MyClasses] " + typeof(MyLocalizationManager).Name + " was created.");
            }
        }

        /// <summary>
        /// Localize all MyLocalization in current scene.
        /// </summary>
        [MenuItem("MyClasses/Managers/MyLocalizationManager/Localize All", false, 2)]
        public static void LocalizeAllMyLocalization()
        {
            MyLocalizationManager script = GameObject.FindObjectOfType<MyLocalizationManager>();
            if (script != null)
            {
                script.LoadLanguage(script.Language, true);
            }

            MyLocalization[] scripts = GameObject.FindObjectsOfType<MyLocalization>();
            foreach (var item in scripts)
            {
                item.Initialize();
                item.Localize();
            }

            Debug.Log("[MyClasses] All " + typeof(MyLocalization).Name + "s in scene were lozalized.");
        }

        /// <summary>
        /// Create a game object with MyTextStyleManager attached.
        /// </summary>
        [MenuItem("MyClasses/Managers/MyTextStyleManager/Create", false, 1)]
        public static void CreateMyTextStyleManager()
        {
            MyTextStyleManager script = GameObject.FindObjectOfType<MyTextStyleManager>();
            if (script != null)
            {
                EditorGUIUtility.PingObject(script);
                Selection.activeGameObject = script.gameObject;

                Debug.Log("[MyClasses] " + typeof(MyTextStyleManager).Name + " is existed.");
            }
            else
            {
                MyTextStyleManager.CreateTemplate();

                Debug.Log("[MyClasses] " + typeof(MyTextStyleManager).Name + " was created.");
            }
        }

        /// <summary>
        /// Refresh all MyTextStyle in current scene.
        /// </summary>
        [MenuItem("MyClasses/Managers/MyTextStyleManager/Refresh All", false, 2)]
        public static void LocalizeAllMyTextStyles()
        {
            MyTextStyle[] scripts = GameObject.FindObjectsOfType<MyTextStyle>();
            foreach (var item in scripts)
            {
                item.Refresh();
            }

            Debug.Log("[MyClasses] All " + typeof(MyTextStyle).Name + "s in scene were refreshed.");
        }

        /// <summary>
        /// Create a game object with MyImageStyleManager attached.
        /// </summary>
        [MenuItem("MyClasses/Managers/MyImageStyleManager/Create", false, 1)]
        public static void CreateMyImageStyleManager()
        {
            MyImageStyleManager script = GameObject.FindObjectOfType<MyImageStyleManager>();
            if (script != null)
            {
                EditorGUIUtility.PingObject(script);
                Selection.activeGameObject = script.gameObject;

                Debug.Log("[MyClasses] " + typeof(MyImageStyleManager).Name + " is existed.");
            }
            else
            {
                MyImageStyleManager.CreateTemplate();

                Debug.Log("[MyClasses] " + typeof(MyImageStyleManager).Name + " was created.");
            }
        }

        /// <summary>
        /// Refresh all MyImageStyle in current scene.
        /// </summary>
        [MenuItem("MyClasses/Managers/MyImageStyleManager/Refresh All", false, 2)]
        public static void RefreshAllMyImageStyles()
        {
            MyImageStyle[] scripts = GameObject.FindObjectsOfType<MyImageStyle>();
            foreach (var item in scripts)
            {
                item.Refresh();
            }

            Debug.Log("[MyClasses] All " + typeof(MyImageStyle).Name + "s in scene were refreshed.");
        }

        #endregion

        #region ----- Utilities -----

        /// <summary>
        /// Delete all PlayerPrefs.
        /// </summary>
        [MenuItem("MyClasses/Utilities/Clear PlayerPrefs", false, 1)]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();

            Debug.Log("[MyClasses] PlayerPrefs was cleared.");
        }

        /// <summary>
        /// Clear cached AssetBundles.
        /// </summary>
        [MenuItem("MyClasses/Utilities/Clear AssetBundles", false, 2)]
        public static void ClearAssetBundles()
        {
            Caching.ClearCache();

            Debug.Log("[MyClasses] AssetBundles was cleared.");
        }

        /// <summary>
        /// Check Packing Tag and Asset Bundle.
        /// </summary>
        [MenuItem("MyClasses/Utilities/Check Sprite (Packing Tag and Asset Bundle)", false, 13)]
        public static void CheckSpriteTagsAndBundles()
        {
            Dictionary<string, string> spriteDict = new Dictionary<string, string>();
            string[] spriteNames = AssetDatabase.FindAssets("t:sprite");
            foreach (string spriteName in spriteNames)
            {
                string spritePath = AssetDatabase.GUIDToAssetPath(spriteName);
                TextureImporter textureImporter = TextureImporter.GetAtPath(spritePath) as TextureImporter;
                if (!spriteDict.ContainsKey(textureImporter.spritePackingTag))
                {
                    spriteDict.Add(textureImporter.spritePackingTag, textureImporter.assetBundleName);
                }
                else if (spriteDict[textureImporter.spritePackingTag] != textureImporter.assetBundleName)
                {
                    Debug.LogError("[MyClasses] Sprite \"" + textureImporter.assetPath + "\" (PackingTag=\"" + textureImporter.spritePackingTag + "\") should be packed in Asset Bundle \"" + spriteDict[textureImporter.spritePackingTag] + "\".");
                }
            }

            Debug.Log("[MyClasses] Sprite checking completed.");
        }

        /// <summary>
        /// Create a noise texture.
        /// </summary>
        [MenuItem("MyClasses/Utilities/Create Noise Texture (32x32)", false, 24)]
        public static void CreateNoiseTexture32()
        {
            Texture2D noiseTexture = new Texture2D(32, 32, TextureFormat.ARGB32, false);
            Color color = Color.gray;
            for (var c = 0; c < noiseTexture.width; c++)
            {
                for (var r = 0; r < noiseTexture.height; r++)
                {
                    if (Random.Range(0, 100) < 50)
                    {
                        float v = Random.Range(0f, 1f);
                        color.r = v;
                        color.g = v;
                        color.b = v;
                    }
                    noiseTexture.SetPixel(r, c, color);
                }
            }
            noiseTexture.Apply();

            System.IO.File.WriteAllBytes("Assets/tex_noise.png", noiseTexture.EncodeToPNG());

            Debug.Log("[MyClasses] Noise Texture (32x32) was created.");
        }

        /// <summary>
        /// Create a noise texture.
        /// </summary>
        [MenuItem("MyClasses/Utilities/Create Noise Texture (128x128)", false, 25)]
        public static void CreateNoiseTexture128()
        {
            Texture2D noiseTexture = new Texture2D(128, 128, TextureFormat.ARGB32, false);
            Color color = Color.gray;
            for (var c = 0; c < noiseTexture.width; c++)
            {
                for (var r = 0; r < noiseTexture.height; r++)
                {
                    if (Random.Range(0, 100) < 50)
                    {
                        float v = Random.Range(0f, 1f);
                        color.r = v;
                        color.g = v;
                        color.b = v;
                    }
                    noiseTexture.SetPixel(r, c, color);
                }
            }
            noiseTexture.Apply();

            System.IO.File.WriteAllBytes("Assets/tex_noise.png", noiseTexture.EncodeToPNG());

            Debug.Log("[MyClasses] Noise Texture (128x128) was created.");
        }

        #endregion
    }
}
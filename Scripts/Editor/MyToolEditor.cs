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
    }
}

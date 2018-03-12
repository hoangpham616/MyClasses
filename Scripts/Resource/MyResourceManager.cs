/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyResourceManager (version 1.4)
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyClasses
{
    public static class MyResourceManager
    {
        #region ----- Variable -----

        private static Dictionary<string, GameObject> mDictPrefab = new Dictionary<string, GameObject>();
        private static Dictionary<string, Material> mDictMaterial = new Dictionary<string, Material>();
        private static Dictionary<string, Sprite> mDictSprite = new Dictionary<string, Sprite>();
        private static Dictionary<string, Texture> mDictTexture = new Dictionary<string, Texture>();

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Return Darkening material.
        /// </summary>
        public static Material GetMaterialDarkening()
        {
#if UNITY_EDITOR
            Material mat = LoadMaterial("Materials/MyDarkening");
            if (mat == null)
            {
                Debug.Log("[" + typeof(MyResourceManager).Name + "] GetMaterialDarkening(): A material was created at \"Assets/Resources/Materials/MyDarkening.mat\".");
                if (!System.IO.Directory.Exists("Assets/Resources/Materials"))
                {
                    System.IO.Directory.CreateDirectory("Assets/Resources/Materials");
                }
                mat = new Material(Shader.Find("MyClasses/Unlit/Darkening"));
                UnityEditor.AssetDatabase.CreateAsset(mat, "Assets/Resources/Materials/MyDarkening.mat");
            }
#endif
            return LoadMaterial("Materials/MyDarkening", true);
        }

        /// <summary>
        /// Return Grayscale material.
        /// </summary>
        public static Material GetMaterialGrayscale()
        {
#if UNITY_EDITOR
            Material mat = LoadMaterial("Materials/MyGrayscale");
            if (mat == null)
            {
                Debug.Log("[" + typeof(MyResourceManager).Name + "] GetMaterialGrayscale(): A material was created at \"Assets/Resources/Materials/MyGrayscale.mat\".");
                if (!System.IO.Directory.Exists("Assets/Resources/Materials"))
                {
                    System.IO.Directory.CreateDirectory("Assets/Resources/Materials");
                }
                mat = new Material(Shader.Find("MyClasses/Unlit/Grayscale"));
                UnityEditor.AssetDatabase.CreateAsset(mat, "Assets/Resources/Materials/MyGrayscale.mat");
            }
#endif
            return LoadMaterial("Materials/MyGrayscale", true);
        }

        /// <summary>
        /// Return Grayscale material.
        /// </summary>
        public static Material GetMaterialBlur()
        {
#if UNITY_EDITOR
            Material mat = LoadMaterial("Materials/MyBlur");
            if (mat == null)
            {
                Debug.Log("[" + typeof(MyResourceManager).Name + "] GetMaterialBlur(): A material was created at \"Assets/Resources/Materials/MyBlur.mat\".");
                if (!System.IO.Directory.Exists("Assets/Resources/Materials"))
                {
                    System.IO.Directory.CreateDirectory("Assets/Resources/Materials");
                }
                mat = new Material(Shader.Find("MyClasses/Unlit/MyBlur"));
                UnityEditor.AssetDatabase.CreateAsset(mat, "Assets/Resources/Materials/MyBlur.mat");
            }
#endif
            return LoadMaterial("Materials/MyBlur", true);
        }

        /// <summary>
        /// Load a prefab.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        public static GameObject LoadPrefab(string path, bool isCache = true)
        {
            if (mDictPrefab.ContainsKey(path))
            {
                return mDictPrefab[path];
            }

            GameObject asset = Resources.Load(path, typeof(GameObject)) as GameObject;
            if (asset == null)
            {
                Debug.LogError("[" + typeof(MyResourceManager).Name + "] LoadPrefab(): Could not find file \"" + path + "\".");
            }
            else if (isCache)
            {
                mDictPrefab[path] = asset;
            }
            return asset;
        }

        /// <summary>
        /// Load some prefabs.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        public static List<GameObject> LoadPrefabs(List<string> paths, bool isCache = true)
        {
            List<GameObject> assets = new List<GameObject>();

            foreach (var path in paths)
            {
                assets.Add(LoadPrefab(path, isCache));
            }

            return assets;
        }

        /// <summary>
        /// Load a prefab asynchronously.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        public static void LoadAsyncPrefab(string path, Action<GameObject> onLoadComplete, bool isCache = true)
        {
            if (mDictPrefab.ContainsKey(path))
            {
                if (onLoadComplete != null)
                {
                    onLoadComplete(mDictPrefab[path]);
                }
                return;
            }

            MyCoroutiner.StartCoroutine(_DoLoadAsyncPrefab(path, onLoadComplete, isCache));
        }

        /// <summary>
        /// Load some prefabs asynchronously.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        public static void LoadAsyncPrefabs(List<string> paths, Action<List<GameObject>> onLoadComplete, bool isCache = true)
        {
            MyCoroutiner.StartCoroutine(_DoLoadAsyncPrefabs(paths, onLoadComplete, isCache));
        }

        /// <summary>
        /// Load a material.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        public static Material LoadMaterial(string path, bool isCache = true)
        {
            if (mDictMaterial.ContainsKey(path))
            {
                return mDictMaterial[path];
            }

            Material asset = Resources.Load(path, typeof(Material)) as Material;
            if (asset == null)
            {
                Debug.LogError("[" + typeof(MyResourceManager).Name + "] LoadMaterial(): Could not find file \"" + path + "\".");
            }
            else if (isCache)
            {
                mDictMaterial[path] = asset;
            }
            return asset;
        }

        /// <summary>
        /// Load some materials.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        public static List<Material> LoadMaterials(List<string> paths, bool isCache = true)
        {
            List<Material> assets = new List<Material>();

            foreach (var path in paths)
            {
                assets.Add(LoadMaterial(path, isCache));
            }

            return assets;
        }

        /// <summary>
        /// Load a material asynchronously.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        public static void LoadAsyncMaterial(string path, Action<Material> onLoadComplete, bool isCache = true)
        {
            if (mDictMaterial.ContainsKey(path))
            {
                if (onLoadComplete != null)
                {
                    onLoadComplete(mDictMaterial[path]);
                }
                return;
            }

            MyCoroutiner.StartCoroutine(_DoLoadAsyncMaterial(path, onLoadComplete, isCache));
        }

        /// <summary>
        /// Load some materials asynchronously.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        public static void LoadAsyncMaterials(List<string> paths, Action<List<Material>> onLoadComplete, bool isCache = true)
        {
            MyCoroutiner.StartCoroutine(_DoLoadAsyncMaterials(paths, onLoadComplete, isCache));
        }

        /// <summary>
        /// Load a sprite.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        public static Sprite LoadSprite(string path, bool isCache = true)
        {
            if (mDictSprite.ContainsKey(path))
            {
                return mDictSprite[path];
            }

            Sprite asset = Resources.Load(path, typeof(Sprite)) as Sprite;
            if (asset == null)
            {
                Debug.LogError("[" + typeof(MyResourceManager).Name + "] LoadSprite(): Could not find file \"" + path + "\".");
            }
            else if (isCache)
            {
                mDictSprite[path] = asset;
            }
            return asset;
        }

        /// <summary>
        /// Load some sprites.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        public static List<Sprite> LoadSprites(List<string> paths, bool isCache = true)
        {
            List<Sprite> assets = new List<Sprite>();

            foreach (var path in paths)
            {
                assets.Add(LoadSprite(path, isCache));
            }

            return assets;
        }

        /// <summary>
        /// Load a sprite asynchronously.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        public static void LoadAsyncSprite(string path, Action<Sprite> onLoadComplete, bool isCache = true)
        {
            if (mDictSprite.ContainsKey(path))
            {
                if (onLoadComplete != null)
                {
                    onLoadComplete(mDictSprite[path]);
                }
                return;
            }

            MyCoroutiner.StartCoroutine(_DoLoadAsyncSprite(path, onLoadComplete, isCache));
        }

        /// <summary>
        /// Load some sprites asynchronously.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        public static void LoadAsyncSprites(List<string> paths, Action<List<Sprite>> onLoadComplete, bool isCache = true)
        {
            MyCoroutiner.StartCoroutine(_DoLoadAsyncSprites(paths, onLoadComplete, isCache));
        }


        /// <summary>
        /// Load a texture.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        public static Texture LoadTexture(string path, bool isCache = true)
        {
            if (mDictTexture.ContainsKey(path))
            {
                return mDictTexture[path];
            }

            Texture asset = Resources.Load(path, typeof(Texture)) as Texture;
            if (asset == null)
            {
                Debug.LogError("[" + typeof(MyResourceManager).Name + "] LoadTexture(): Could not find file \"" + path + "\".");
            }
            else if (isCache)
            {
                mDictTexture[path] = asset;
            }
            return asset;
        }

        /// <summary>
        /// Load some textures.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        public static List<Texture> LoadTextures(List<string> paths, bool isCache = true)
        {
            List<Texture> assets = new List<Texture>();

            foreach (var path in paths)
            {
                assets.Add(LoadTexture(path, isCache));
            }

            return assets;
        }

        /// <summary>
        /// Load a texture asynchronously.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        public static void LoadAsyncTexture(string path, Action<Texture> onLoadComplete, bool isCache = true)
        {
            if (mDictTexture.ContainsKey(path))
            {
                if (onLoadComplete != null)
                {
                    onLoadComplete(mDictTexture[path]);
                }
                return;
            }

            MyCoroutiner.StartCoroutine(_DoLoadAsyncTexture(path, onLoadComplete, isCache));
        }

        /// <summary>
        /// Load some textures asynchronously.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        public static void LoadAsyncTextures(List<string> paths, Action<List<Texture>> onLoadComplete, bool isCache = true)
        {
            MyCoroutiner.StartCoroutine(_DoLoadAsyncTextures(paths, onLoadComplete, isCache));
        }

        /// <summary>
        /// Unload all cached resources.
        /// </summary>
        public static void UnloadAll()
        {
            mDictPrefab.Clear();
            mDictMaterial.Clear();
            mDictSprite.Clear();
            mDictTexture.Clear();
        }

        /// <summary>
        /// Unload all cached prefabs.
        /// </summary>
        public static void UnloadPrefabs()
        {
            mDictPrefab.Clear();
        }

        /// <summary>
        /// Unload all cached materials.
        /// </summary>
        public static void UnloadMaterials()
        {
            mDictMaterial.Clear();
        }

        /// <summary>
        /// Unload all cached sprites.
        /// </summary>
        public static void UnloadSprites()
        {
            mDictSprite.Clear();
        }

        /// <summary>
        /// Unload all cached textures.
        /// </summary>
        public static void UnloadTextures()
        {
            mDictTexture.Clear();
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Load a prefab asynchronously.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        private static IEnumerator _DoLoadAsyncPrefab(string path, Action<GameObject> onLoadComplete, bool isCache)
        {
            ResourceRequest requester = Resources.LoadAsync<GameObject>(path);
            yield return requester;

            if (requester.asset == null)
            {
                Debug.LogError("[" + typeof(MyResourceManager).Name + "] _DoLoadAsyncPrefab(): Could not find file \"" + path + "\".");
                if (onLoadComplete != null)
                {
                    onLoadComplete(null);
                }
                yield break;
            }

            GameObject asset = requester.asset as GameObject;
            if (isCache)
            {
                mDictPrefab[path] = asset;
            }
            if (onLoadComplete != null)
            {
                onLoadComplete(asset);
            }
        }

        /// <summary>
        /// Load some prefabs asynchronously.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        private static IEnumerator _DoLoadAsyncPrefabs(List<string> paths, Action<List<GameObject>> onLoadComplete, bool isCache)
        {
            List<GameObject> assets = new List<GameObject>();

            foreach (var path in paths)
            {
                if (mDictPrefab.ContainsKey(path))
                {
                    assets.Add(mDictPrefab[path]);
                    continue;
                }

                ResourceRequest requester = Resources.LoadAsync<GameObject>(path);
                yield return requester;

                if (requester.asset == null)
                {
                    Debug.LogError("[" + typeof(MyResourceManager).Name + "] _DoLoadAsyncPrefabs(): Could not find file \"" + path + "\".");
                    continue;
                }

                GameObject asset = requester.asset as GameObject;
                if (isCache)
                {
                    mDictPrefab[path] = asset;
                }
                assets.Add(asset);
            }

            if (onLoadComplete != null)
            {
                onLoadComplete(assets);
            }
        }

        /// <summary>
        /// Load a material asynchronously.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        private static IEnumerator _DoLoadAsyncMaterial(string path, Action<Material> onLoadComplete, bool isCache)
        {
            ResourceRequest requester = Resources.LoadAsync<Material>(path);
            yield return requester;

            if (requester.asset == null)
            {
                Debug.LogError("[" + typeof(MyResourceManager).Name + "] _DoLoadAsyncMaterial(): Could not find file \"" + path + "\".");
                if (onLoadComplete != null)
                {
                    onLoadComplete(null);
                }
                yield break;
            }

            Material asset = requester.asset as Material;
            if (isCache)
            {
                mDictMaterial[path] = asset;
            }
            if (onLoadComplete != null)
            {
                onLoadComplete(asset);
            }
        }

        /// <summary>
        /// Load some materials asynchronously.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        private static IEnumerator _DoLoadAsyncMaterials(List<string> paths, Action<List<Material>> onLoadComplete, bool isCache)
        {
            List<Material> assets = new List<Material>();

            foreach (var path in paths)
            {
                if (mDictMaterial.ContainsKey(path))
                {
                    assets.Add(mDictMaterial[path]);
                    continue;
                }

                ResourceRequest requester = Resources.LoadAsync<Material>(path);
                yield return requester;

                if (requester.asset == null)
                {
                    Debug.LogError("[" + typeof(MyResourceManager).Name + "] _DoLoadAsyncMaterials(): Could not find file \"" + path + "\".");
                    continue;
                }

                Material asset = requester.asset as Material;
                if (isCache)
                {
                    mDictMaterial[path] = asset;
                }
                assets.Add(asset);
            }

            if (onLoadComplete != null)
            {
                onLoadComplete(assets);
            }
        }

        /// <summary>
        /// Load a sprite asynchronously.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        private static IEnumerator _DoLoadAsyncSprite(string path, Action<Sprite> onLoadComplete, bool isCache)
        {
            ResourceRequest requester = Resources.LoadAsync<Sprite>(path);
            yield return requester;

            if (requester.asset == null)
            {
                Debug.LogError("[" + typeof(MyResourceManager).Name + "] _DoLoadAsyncSprite(): Could not find file \"" + path + "\".");
                if (onLoadComplete != null)
                {
                    onLoadComplete(null);
                }
                yield break;
            }

            Sprite asset = requester.asset as Sprite;
            if (isCache)
            {
                mDictSprite[path] = asset;
            }
            if (onLoadComplete != null)
            {
                onLoadComplete(asset);
            }
        }

        /// <summary>
        /// Load some sprites asynchronously.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        private static IEnumerator _DoLoadAsyncSprites(List<string> paths, Action<List<Sprite>> onLoadComplete, bool isCache)
        {
            List<Sprite> assets = new List<Sprite>();

            foreach (var path in paths)
            {
                if (mDictSprite.ContainsKey(path))
                {
                    assets.Add(mDictSprite[path]);
                    continue;
                }

                ResourceRequest requester = Resources.LoadAsync<Sprite>(path);
                yield return requester;

                if (requester.asset == null)
                {
                    Debug.LogError("[" + typeof(MyResourceManager).Name + "] _DoLoadAsyncSprites(): Could not find file \"" + path + "\".");
                    continue;
                }

                Sprite asset = requester.asset as Sprite;
                if (isCache)
                {
                    mDictSprite[path] = asset;
                }
                assets.Add(asset);
            }

            if (onLoadComplete != null)
            {
                onLoadComplete(assets);
            }
        }

        /// <summary>
        /// Load a texture asynchronously.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        private static IEnumerator _DoLoadAsyncTexture(string path, Action<Texture> onLoadComplete, bool isCache)
        {
            ResourceRequest requester = Resources.LoadAsync<Texture>(path);
            yield return requester;

            if (requester.asset == null)
            {
                Debug.LogError("[" + typeof(MyResourceManager).Name + "] _DoLoadAsyncTexture(): Could not find file \"" + path + "\".");
                if (onLoadComplete != null)
                {
                    onLoadComplete(null);
                }
                yield break;
            }

            Texture asset = requester.asset as Texture;
            if (isCache)
            {
                mDictTexture[path] = asset;
            }
            if (onLoadComplete != null)
            {
                onLoadComplete(asset);
            }
        }

        /// <summary>
        /// Load some textures asynchronously.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        private static IEnumerator _DoLoadAsyncTextures(List<string> paths, Action<List<Texture>> onLoadComplete, bool isCache)
        {
            List<Texture> assets = new List<Texture>();

            foreach (var path in paths)
            {
                if (mDictTexture.ContainsKey(path))
                {
                    assets.Add(mDictTexture[path]);
                    continue;
                }

                ResourceRequest requester = Resources.LoadAsync<Texture>(path);
                yield return requester;

                if (requester.asset == null)
                {
                    Debug.LogError("[" + typeof(MyResourceManager).Name + "] _DoLoadAsyncTextures(): Could not find file \"" + path + "\".");
                    continue;
                }

                Texture asset = requester.asset as Texture;
                if (isCache)
                {
                    mDictTexture[path] = asset;
                }
                assets.Add(asset);
            }

            if (onLoadComplete != null)
            {
                onLoadComplete(assets);
            }
        }

        #endregion
    }
}
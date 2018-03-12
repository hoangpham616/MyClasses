/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyAssetBundleManager (version 1.0)
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyClasses
{
    public static class MyAssetBundleManager
    {
        #region ----- Internal Class -----

        public class MyAssetBundle
        {
            public AssetBundle Bundle;
            public int Version;
            public ECacheMode CacheMode;
        }

        #endregion

        #region ----- Variable -----

        private static Dictionary<string, MyAssetBundle> mDictBundle = new Dictionary<string, MyAssetBundle>();

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Return a cached asset bundle.
        /// </summary>
        public static AssetBundle Get(string url, int version)
        {
            if (mDictBundle.ContainsKey(url))
            {
                MyAssetBundle bundle = mDictBundle[url];
                if (bundle.Version >= version && bundle.Bundle != null)
                {
                    return bundle.Bundle;
                }
            }

            return null;
        }

        /// <summary>
        /// Load a asset bundle.
        /// </summary>
        public static void Load(string url, int version, Action<AssetBundle> onLoadComplete, ECacheMode cacheMode = ECacheMode.Cache)
        {
            if (mDictBundle.ContainsKey(url))
            {
                MyAssetBundle bundle = mDictBundle[url];
                if (bundle.Version >= version && bundle.Bundle != null)
                {
                    if (onLoadComplete != null)
                    {
                        onLoadComplete(bundle.Bundle);
                        return;
                    }
                }
            }

            MyCoroutiner.StartCoroutine(_DoLoadAssetBundle(url, version, onLoadComplete, cacheMode));
        }

        /// <summary>
        /// Unload a asset bundle.
        /// </summary>
        public static void Unload(string url)
        {
            if (mDictBundle.ContainsKey(url))
            {
                MyAssetBundle bundle = mDictBundle[url];
                if (bundle.Bundle != null)
                {
                    bundle.Bundle.Unload(false);
                }
                mDictBundle.Remove(url);
            }
        }

        /// <summary>
        /// Unload all asset bundles.
        /// </summary>
        public static void UnloadAll(bool isIncludeUnremovableCache = false)
        {
            if (isIncludeUnremovableCache)
            {
                mDictBundle.Clear();
                return;
            }

            foreach (var item in mDictBundle)
            {
                MyAssetBundle bundle = mDictBundle[item.Key];
                if (bundle.CacheMode != ECacheMode.UnremovableCache)
                {
                    mDictBundle.Remove(item.Key);
                }
            }
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Load a asset bundle asynchronously.
        /// </summary>
        private static IEnumerator _DoLoadAssetBundle(string url, int version, Action<AssetBundle> onLoadComplete, ECacheMode cacheMode = ECacheMode.Cache)
        {
            using (WWW www = WWW.LoadFromCacheOrDownload(url, version))
            {
                yield return www;

                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.LogError("[" + typeof(MyAssetBundleManager).Name + "] _DoLoadAssetBundle(): Could not load asset bundle url=\"" + url + "\" version=\"" + version + "\".");
                }
                else if (cacheMode != ECacheMode.None)
                {
                    mDictBundle[url] = new MyAssetBundle()
                    {
                        Bundle = www.assetBundle,
                        Version = version,
                        CacheMode = cacheMode
                    };
                }

                if (onLoadComplete != null)
                {
                    onLoadComplete(www.assetBundle);
                }
            }
        }

        #endregion

        #region ----- Enumeration -----

        public enum ECacheMode
        {
            None,
            Cache,
            UnremovableCache
        }

        #endregion
    }
}
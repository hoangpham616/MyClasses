/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyImageDownloader (version 1.0)
 */

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyClasses
{
    public class MyImageDownloader : MonoBehaviour
    {
        #region ----- Variable -----

        private Dictionary<string, Sprite> mDictionarySprite = new Dictionary<string, Sprite>();
        private Dictionary<string, Texture2D> mDictionaryTexture2D = new Dictionary<string, Texture2D>();

        #endregion

        #region ----- Singleton -----

        private static object mSingletonLock = new object();
        private static MyImageDownloader mInstance;

        public static MyImageDownloader Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mSingletonLock)
                    {
                        mInstance = (MyImageDownloader)FindObjectOfType(typeof(MyImageDownloader));
                        if (mInstance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyImageDownloader).Name);
                            mInstance = obj.AddComponent<MyImageDownloader>();
                            DontDestroyOnLoad(obj);
                        }
                    }
                }
                return mInstance;
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Clear all cached images.
        /// </summary>
        public void Clear()
        {
            mDictionarySprite.Clear();
            mDictionaryTexture2D.Clear();
        }

        /// <summary>
        /// Load an image from a url.
        /// </summary>
        public void LoadImage(Image image, string url, Action onLoadSuccess = null, Action onLoadError = null)
        {
            if (image != null && !string.IsNullOrEmpty(url))
            {
                StartCoroutine(_DoLoadImage(image, url, onLoadSuccess, onLoadError));
            }
            else
            {
                if (onLoadError != null)
                {
                    onLoadError();
                }
            }
        }

        /// <summary>
        /// Load a raw image from a url.
        /// </summary>
        public void LoadImage(RawImage rawImage, string url, Action onLoadSuccess = null, Action onLoadError = null)
        {
            if (rawImage != null && !string.IsNullOrEmpty(url))
            {
                StartCoroutine(_DoLoadRawImage(rawImage, url, onLoadSuccess, onLoadError));
            }
            else
            {
                if (onLoadError != null)
                {
                    onLoadError();
                }
            }
        }

        #endregion

        #region ----- Private Method -----

        private IEnumerator _DoLoadImage(Image image, string url, Action onLoadSuccess = null, Action onLoadError = null)
        {
            if (image.sprite == null)
            {
                image.enabled = false;
            }

            if (mDictionarySprite.ContainsKey(url))
            {
                image.sprite = mDictionarySprite[url];
                image.enabled = true;
            }
            else
            {
                WWW www = new WWW(url);
                yield return www;

                if (string.IsNullOrEmpty(www.error))
                {
                    Sprite sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
                    mDictionarySprite[url] = sprite;
                    if (image != null)
                    {
                        image.sprite = sprite;
                        image.enabled = true;
                    }
                    if (onLoadSuccess != null)
                    {
                        onLoadSuccess();
                    }
                }
                else
                {
                    if (onLoadError != null)
                    {
                        onLoadError();
                    }
                }
            }
        }

        private IEnumerator _DoLoadRawImage(RawImage rawImage, string url, Action onLoadSuccess = null, Action onLoadError = null)
        {
            if (rawImage.texture == null)
            {
                rawImage.enabled = false;
            }

            if (mDictionaryTexture2D.ContainsKey(url))
            {
                Texture2D texture = mDictionaryTexture2D[url];
                mDictionaryTexture2D[url] = texture;

                rawImage.texture = texture;
                rawImage.enabled = true;
            }
            else
            {
                WWW www = new WWW(url);
                yield return www;

                if (string.IsNullOrEmpty(www.error))
                {
                    Texture2D texture = new Texture2D(4, 4, TextureFormat.DXT1, false);
                    www.LoadImageIntoTexture(texture);
                    mDictionaryTexture2D[url] = texture;
                    if (rawImage != null)
                    {
                        rawImage.texture = texture;
                        rawImage.enabled = true;
                    }
                    if (onLoadSuccess != null)
                    {
                        onLoadSuccess();
                    }
                }
                else
                {
                    if (onLoadError != null)
                    {
                        onLoadError();
                    }
                }
            }
        }

        #endregion
    }
}
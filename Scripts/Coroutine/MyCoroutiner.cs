/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyCoroutiner (version 1.2)
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyClasses
{
    public class MyCoroutiner
    {
        #region ----- Variable -----

        private static GameObject mCoroutineObject;
        private static CoroutineInstance mCoroutineInstance;
        private static Dictionary<string, IEnumerator> mDictionaryRoutine;

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Start a coroutine.
        /// </summary>
        public static void StartCoroutine(IEnumerator routine)
        {
            _Initialize();

            mCoroutineInstance.StartCoroutine(routine);
        }

        /// <summary>
        /// Start a coroutine.
        /// </summary>
        public static void StartCoroutine(string key, IEnumerator routine)
        {
            _Initialize();

            if (mDictionaryRoutine.ContainsKey(key))
            {
                if (mDictionaryRoutine[key] != null)
                {
                    mCoroutineInstance.StopCoroutine(mDictionaryRoutine[key]);
                }
                mDictionaryRoutine.Remove(key);
            }

            mCoroutineInstance.StartCoroutine(routine);
            mDictionaryRoutine.Add(key, routine);
        }

        /// <summary>
        /// Stop a coroutine.
        /// </summary>
        public static void StopCoroutine(string key)
        {
            _Initialize();

            if (mDictionaryRoutine.ContainsKey(key))
            {
                if (mDictionaryRoutine[key] != null)
                {
                    mCoroutineInstance.StopCoroutine(mDictionaryRoutine[key]);
                }
                mDictionaryRoutine.Remove(key);
            }
        }

        /// <summary>
        /// Stop all coroutines.
        /// </summary>
        public static void StopAllCoroutine()
        {
            _Initialize();

            mCoroutineInstance.StopAllCoroutines();
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Initialize.
        /// </summary>
        private static void _Initialize()
        {
            if (mCoroutineObject == null)
            {
                string objName = typeof(MyCoroutiner).Name;

                mCoroutineObject = MyUtilities.FindObjectInRoot(objName);

                if (mCoroutineObject == null)
                {
                    mCoroutineObject = new GameObject(objName);
                }

                GameObject.DontDestroyOnLoad(mCoroutineObject);

                mDictionaryRoutine = new Dictionary<string, IEnumerator>();
            }

            if (mCoroutineInstance == null)
            {
                mCoroutineInstance = mCoroutineObject.GetComponent<CoroutineInstance>();

                if (mCoroutineInstance == null)
                {
                    mCoroutineInstance = mCoroutineObject.AddComponent(typeof(CoroutineInstance)) as CoroutineInstance;
                }
            }
        }

        #endregion

        #region ----- Internal Class -----

        public class CoroutineInstance : MonoBehaviour
        {
        }

        #endregion
    }
}

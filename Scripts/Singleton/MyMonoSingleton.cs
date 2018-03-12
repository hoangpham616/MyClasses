/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyMonoSingleton (version 1.0)
 */

using UnityEngine;

namespace MyClasses
{
    public class MyMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static object mSingletonLock = new object();
        protected static T mInstance;

        /// <summary>
        /// Return the only one instance of inherited class.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mSingletonLock)
                    {
                        mInstance = (T)FindObjectOfType(typeof(T));
                        if (mInstance)
                        {
                            if (FindObjectsOfType(typeof(T)).Length > 1)
                            {
                                Debug.LogError("[" + typeof(T).Name + "] There is more than one instance of this class.");
                            }
                        }
                        else
                        {
                            GameObject obj = new GameObject(typeof(T).Name);
                            mInstance = obj.AddComponent<T>();
                            DontDestroyOnLoad(obj);
                        }
                    }
                }
                return mInstance;
            }
        }
    }
}

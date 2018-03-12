/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyInternetChecking (version 2.0)
 */

#pragma warning disable 0414

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;
using System.Collections;

namespace MyClasses
{
    public class MyInternetChecking : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private string[] mGlobalDNSs = new string[] { "8.8.8.8", "208.67.222.222", "8.26.56.26", "4.2.2.1" };   // International DNS
        [SerializeField]
        private string[] mLocalDNSs = new string[] { "203.162.4.191", "203.113.131.1", "210.245.24.20" };       // Vietnam DNS
        [SerializeField]
        private float mCheckTime = 0;

        private int mGlobalIndex = 0;
        private int mLocalIndex = 0;
        private bool mIsGlobalCheck = true;
        private bool mIsConnecting = false;
        private float mLastTimePingAllSuccess = -1;
        private Action<bool> mPingCallback = null;

        #endregion

        #region ----- Property -----

        public float CheckingTime
        {
            get { return mCheckTime; }
        }

        public bool IsConnecting
        {
            get { return mCheckTime > 0 && mIsConnecting && Application.internetReachability != NetworkReachability.NotReachable; }
        }

        #endregion

        #region ----- Singleton -----

        private static object mSingletonLock = new object();
        private static MyInternetChecking mInstance;

        public static MyInternetChecking Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mSingletonLock)
                    {
                        mInstance = (MyInternetChecking)FindObjectOfType(typeof(MyInternetChecking));
                        if (mInstance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyInternetChecking).Name);
                            mInstance = obj.AddComponent<MyInternetChecking>();
                            DontDestroyOnLoad(obj);
                        }
                    }
                }
                return mInstance;
            }
        }

        #endregion

        #region ----- Implement MonoBehaviour -----

        /// <summary>
        /// Start.
        /// </summary>
        void Start()
        {
            StartCoroutine(_Ping());
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Check internet connection immediately.
        /// </summary>
        public void CheckConnection(Action<bool> callback, float timeout = 5f)
        {
            if (callback != null)
            {
                StartCoroutine(_PingAll(callback, timeout));
            }
        }

        /// <summary>
        /// Set ping frequency.
        /// </summary>
        /// <param name="firstResultCallback">callback when the first ping done</param>
        public void SetFrequency(float checkTime, Action<bool> firstResultCallback = null)
        {
            mCheckTime = checkTime >= 0.025f ? checkTime : 0.025f;

            if (firstResultCallback != null)
            {
                mPingCallback = firstResultCallback;
            }
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Ping to check internet connection.
        /// </summary>
        private IEnumerator _Ping()
        {
#if UNITY_WEBGL
            mIsConnecting = true;

            if (mPingCallback != null)
            {
                mPingCallback(mIsConnecting);
                mPingCallback = null;
            }

            yield return null;
#else
            float timeout = 1;

            while (true)
            {
                if (mCheckTime <= 0)
                {
                    yield return null;
                    continue;
                }

                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    mIsConnecting = false;

                    if (mPingCallback != null)
                    {
                        mPingCallback(mIsConnecting);
                        mPingCallback = null;
                    }

                    yield return new WaitForSecondsRealtime(mCheckTime);
                }
                else
                {
                    Ping ping = new Ping(mIsGlobalCheck ? mGlobalDNSs[mGlobalIndex] : mLocalDNSs[mLocalIndex]);
                    float deadline = Time.time + timeout;
                    while (!ping.isDone && Time.time < deadline)
                    {
                        yield return null;
                    }

                    mIsConnecting = ping.isDone && ping.time >= 0;
                    if (!mIsConnecting)
                    {
                        if (mIsGlobalCheck)
                        {
                            mGlobalIndex = (mGlobalIndex + 1) % mGlobalDNSs.Length;
                        }
                        else
                        {
                            mLocalIndex = (mLocalIndex + 1) % mLocalDNSs.Length;
                        }
                        mIsGlobalCheck = !mIsGlobalCheck;

                        timeout = Mathf.Clamp(timeout + 0.25f, 1, 3);
                    }
                    else
                    {
                        timeout = 1;
                    }

                    if (mLastTimePingAllSuccess > 0 && Time.time - mLastTimePingAllSuccess < 3)
                    {
                        mLastTimePingAllSuccess = -1;
                        mIsConnecting = true;
                    }

                    if (mPingCallback != null)
                    {
                        mPingCallback(mIsConnecting);
                        mPingCallback = null;
                    }

                    yield return new WaitForSecondsRealtime(mIsConnecting || mCheckTime < 0.25f ? mCheckTime : mCheckTime / 10f);
                }
            }
#endif
        }

        /// <summary>
        /// Ping to check internet connection.
        /// </summary>
        private IEnumerator _PingAll(Action<bool> callback, float timeout = 5f)
        {
#if UNITY_WEBGL
            yield return null;
            
            mIsConnecting = true;

            callback(mIsConnecting);
#else
            bool isConnect = false;

            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                Ping[] pings = new Ping[mLocalDNSs.Length + mGlobalDNSs.Length];
                for (int i = 0; i < mLocalDNSs.Length; i++)
                {
                    pings[i] = new Ping(mLocalDNSs[i]);
                }
                for (int i = 0; i < mGlobalDNSs.Length; i++)
                {
                    pings[mLocalDNSs.Length + i] = new Ping(mGlobalDNSs[i]);
                }

                float deadline = Time.time + timeout;
                while (!isConnect && Time.time < deadline)
                {
                    for (int i = 0; i < pings.Length; i++)
                    {
                        if (pings[i].isDone && pings[i].time >= 0)
                        {
                            mLastTimePingAllSuccess = Time.time;
                            mIsConnecting = true;
                            isConnect = true;
                            callback(true);

                            break;
                        }
                    }

                    if (!isConnect)
                    {
                        yield return null;
                    }
                }
            }

            if (!isConnect)
            {
                mLastTimePingAllSuccess = -1;
                mIsConnecting = false;
                callback(false);
            }
#endif
        }

        #endregion

        #region ----- Enumeration -----

        public enum EFrequency
        {
            Never = 0,
            Always = 1,
            Regularly = 4,
            Normally = 8,
            Occasionally = 12,
            Rarely = 20
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyInternetChecking))]
    public class MyInternetCheckingEditor : Editor
    {
        private MyInternetChecking mScript;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyInternetChecking)target;
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyInternetChecking), false);

            SerializedProperty globalProperty = serializedObject.FindProperty("mGlobalDNSs");
            SerializedProperty localProperty = serializedObject.FindProperty("mLocalDNSs");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(globalProperty, new GUIContent("Glogal DNS"), true);
            EditorGUILayout.PropertyField(localProperty, new GUIContent("Local DNS"), true);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }

#endif
}
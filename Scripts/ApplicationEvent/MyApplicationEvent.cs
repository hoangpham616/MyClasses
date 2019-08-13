/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyApplicationEvent (version 1.0)
 */

using UnityEngine;
using System;

namespace MyClasses
{
    public class MyApplicationEvent
    {
        #region ----- Variable -----

        private static GameObject mApplicationObject;
        private static ApplicationInstance mApplicationEventInstance;

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Add a OnApplicationFocus event.
        /// </summary>
        public static void AddApplicationFocus(Action<bool> evt)
        {
            _Initialize();
            mApplicationEventInstance.OnFocus += evt;
        }

        /// <summary>
        /// Remove a OnApplicationFocus event.
        /// </summary>
        public static void RemoveApplicationFocus(Action<bool> evt)
        {
            _Initialize();
            mApplicationEventInstance.OnFocus -= evt;
        }

        /// <summary>
        /// Add a OnApplicationPause event.
        /// </summary>
        public static void AddApplicationPause(Action<bool> evt)
        {
            _Initialize();
            mApplicationEventInstance.OnPause += evt;
        }

        /// <summary>
        /// Remove a OnApplicationPause event.
        /// </summary>
        public static void RemoveApplicationPause(Action<bool> evt)
        {
            _Initialize();
            mApplicationEventInstance.OnPause -= evt;
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Initialize.
        /// </summary>
        private static void _Initialize()
        {
            if (mApplicationObject == null)
            {
                string objName = typeof(MyApplicationEvent).Name;

                mApplicationObject = MyUtilities.FindObjectInRoot(objName);

                if (mApplicationObject == null)
                {
                    mApplicationObject = new GameObject(objName);
                }

                GameObject.DontDestroyOnLoad(mApplicationObject);
            }

            if (mApplicationEventInstance == null)
            {
                mApplicationEventInstance = mApplicationObject.GetComponent<ApplicationInstance>();

                if (mApplicationEventInstance == null)
                {
                    mApplicationEventInstance = mApplicationObject.AddComponent(typeof(ApplicationInstance)) as ApplicationInstance;
                }
            }
        }

        #endregion

        #region ----- Internal Class -----

        public class ApplicationInstance : MonoBehaviour
        {
            #region ----- Property -----

            public Action<bool> OnFocus;
            public Action<bool> OnPause;

            #endregion

            #region ----- MonoBehaviour Implemention -----

            /// <summary>
            /// OnApplicationFocus.
            /// </summary>
            void OnApplicationFocus(bool hasFocus)
            {
                if (OnFocus != null)
                {
                    OnFocus(hasFocus);
                }
            }

            /// <summary>
            /// OnApplicationPause.
            /// </summary>
            void OnApplicationPause(bool isPause)
            {
                if (OnPause != null)
                {
                    OnPause(isPause);
                }
            }

            #endregion
        }

        #endregion
    }
}
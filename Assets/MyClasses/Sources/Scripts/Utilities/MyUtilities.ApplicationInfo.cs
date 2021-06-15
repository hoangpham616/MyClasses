/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.ApplicationInfo (version 1.0)
 */

using UnityEngine;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        #region ----- Public Method -----

        /// <summary>
        /// Get application version code.
        /// </summary>
        public static int GetVersionCode()
        {
#if UNITY_EDITOR && UNITY_ANDROID
            return UnityEditor.PlayerSettings.Android.bundleVersionCode;
#elif UNITY_ANDROID
            AndroidJavaClass contextCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = contextCls.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject packageMngr = context.Call<AndroidJavaObject>("getPackageManager");
            string packageName = context.Call<string>("getPackageName");
            AndroidJavaObject packageInfo = packageMngr.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
            return packageInfo.Get<int>("versionCode");
#else
            return 0;
#endif
        }

        /// <summary>
        /// Get application version name.
        /// </summary>
        public static string GetVersionName()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            AndroidJavaClass contextCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = contextCls.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject packageMngr = context.Call<AndroidJavaObject>("getPackageManager");
            string packageName = context.Call<string>("getPackageName");
            AndroidJavaObject packageInfo = packageMngr.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
            return packageInfo.Get<string>("versionName");
#else
            return Application.version;
#endif
        }

#endregion
    }
}

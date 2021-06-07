/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIScaler (version 2.0)
 */

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0649

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;

namespace MyClasses.UI
{
    public class MyUGUIScaler : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private EOrientation mOrientation = EOrientation.Both;
        [SerializeField]
        private ELevel mLevel = ELevel.Three;
        [SerializeField]
        private EFrequency mFrequency = EFrequency.OneTimeOnly;
        [SerializeField]
        private ERoundingRatio mRoundingRatio = ERoundingRatio.TwoDigits;

        [SerializeField]
        private Vector2 mHighestRatio = new Vector2(19, 9);
        [SerializeField]
        private Vector3 mHighestScale = Vector3.one;

        [SerializeField]
        private Vector2 mHighRatio = new Vector2(18, 9);
        [SerializeField]
        private Vector3 mHighScale = Vector3.one;

        [SerializeField]
        private Vector3 mDefaultScale = Vector3.one;

        [SerializeField]
        private Vector2 mLowRatio = new Vector2(16.2f, 10);
        [SerializeField]
        private Vector3 mLowScale = Vector3.one;

        [SerializeField]
        private Vector2 mLowestRatio = new Vector2(4.2f, 3);
        [SerializeField]
        private Vector3 mLowestScale = Vector3.one;

        [SerializeField]
        private ScreenOrientation mDeviceOrientation;

        [SerializeField]
        private bool mIsCurrentAnchorLoaded = false;

        #endregion

        #region ----- Property -----

        public ELevel Level
        {
            get { return mLevel; }
            set { mLevel = value; }
        }

        public EFrequency Frequency
        {
            get { return mFrequency; }
            set { mFrequency = value; }
        }

        public ERoundingRatio RoundingRatio
        {
            get { return mRoundingRatio; }
            set { mRoundingRatio = value; }
        }

#if UNITY_EDITOR

        public bool IsCurrentAnchorLoaded
        {
            get { return mIsCurrentAnchorLoaded; }
            set { mIsCurrentAnchorLoaded = value; }
        }

#endif

        #endregion

        #region ----- Implement MonoBehaviour -----

        /// <summary>
        /// Awake.
        /// </summary>
        void Awake()
        {
#if UNITY_WEBGL
            mScaleFrequency = EScaleFrequency.EverytimeActive;
#endif

            if (mFrequency == EFrequency.OneTimeOnly)
            {
                Scale();
            }
        }

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            if (mFrequency == EFrequency.EverytimeActive)
            {
                Scale();
            }
        }

        /// <summary>
        /// Update.
        /// </summary>
        void Update()
        {
            if (mOrientation == EOrientation.Both)
            {
                return;
            }

#if UNITY_EDITOR
            if ((mDeviceOrientation == ScreenOrientation.Landscape && Screen.width < Screen.height) || (mDeviceOrientation == ScreenOrientation.Portrait && Screen.width > Screen.height))
#else
            if (mDeviceOrientation != Screen.orientation)
#endif
            {
                Scale();
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Scale.
        /// </summary>
        public void Scale()
        {
#if UNITY_EDITOR
            System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo getSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Vector2 resolution = (Vector2)getSizeOfMainGameView.Invoke(null, null);
            mDeviceOrientation = resolution.x > resolution.y ? ScreenOrientation.Landscape : ScreenOrientation.Portrait;
#else
            mDeviceOrientation = Screen.orientation;
#endif

            if (mOrientation == EOrientation.Portrait && (mDeviceOrientation == ScreenOrientation.Landscape || mDeviceOrientation == ScreenOrientation.LandscapeLeft || mDeviceOrientation == ScreenOrientation.LandscapeRight))
            {
                return;
            }

            if (mOrientation == EOrientation.Landscape && (mDeviceOrientation == ScreenOrientation.Portrait || mDeviceOrientation == ScreenOrientation.PortraitUpsideDown))
            {
                return;
            }

            switch (mLevel)
            {
                case ELevel.One:
                    {
                        _Scale1();
                    }
                    break;
                case ELevel.Three:
                    {
                        _Scale3();
                    }
                    break;
                case ELevel.Five:
                    {
                        _Scale5();
                    }
                    break;
            }
        }

        /// <summary>
        /// Return current aspect ratio value.
        /// </summary>
        public double GetCurrentRatio()
        {
#if UNITY_EDITOR
            System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            object Res = GetSizeOfMainGameView.Invoke(null, null);
            return GetRatio((Vector2)Res);
#else
            return GetRatio(new Vector2(Screen.width, Screen.height));
#endif
        }

        /// <summary>
        /// Return aspect ratio value by resolution.
        /// </summary>
        public double GetRatio(Vector2 resolution)
        {
            if (resolution.x > resolution.y)
            {
                return Math.Round(resolution.x / resolution.y, (int)mRoundingRatio);
            }
            else
            {
                return Math.Round(resolution.y / resolution.x, (int)mRoundingRatio);
            }
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Scale for One Level. 
        /// </summary>
        private void _Scale1()
        {
            transform.localScale = mDefaultScale;
        }

        /// <summary>
        /// Scale for Three Levels. 
        /// </summary>
        private void _Scale3()
        {
            double curRatio = GetCurrentRatio();

            if (curRatio > GetRatio(mHighestRatio))
            {
                transform.localScale = mHighestScale;
            }
            else if (curRatio < GetRatio(mLowestRatio))
            {
                transform.localScale = mLowestScale;
            }
            else
            {
                transform.localScale = mDefaultScale;
            }
        }

        /// <summary>
        /// Scale for Five Levels. 
        /// </summary>
        private void _Scale5()
        {
            double curRatio = GetCurrentRatio();

            if (curRatio >= GetRatio(mHighestRatio))
            {
                transform.localScale = mHighestScale;
            }
            else if (curRatio >= GetRatio(mHighRatio))
            {
                transform.localScale = mHighScale;
            }
            else if (curRatio <= GetRatio(mLowestRatio))
            {
                transform.localScale = mLowestScale;
            }
            else if (curRatio <= GetRatio(mLowRatio))
            {
                transform.localScale = mLowScale;
            }
            else
            {
                transform.localScale = mDefaultScale;
            }
        }

        #endregion

        #region ----- Enumeration -----

        public enum EOrientation
        {
            Both,
            Portrait,
            Landscape,
        }

        public enum ELevel
        {
            One,
            Three,
            Five
        }

        public enum EFrequency
        {
            OneTimeOnly,
            EverytimeActive
        }

        public enum ERoundingRatio
        {
            OneDigit = 1,
            TwoDigits = 2
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyUGUIScaler))]
    public class MyUGUIScalerEditor : Editor
    {
        private MyUGUIScaler mScript;
        private RectTransform mRectTransform;

        private SerializedProperty mOrientation;

        private SerializedProperty mHighestRatio;
        private SerializedProperty mHighestScale;

        private SerializedProperty mHighRatio;
        private SerializedProperty mHighScale;

        private SerializedProperty mDefaultHighRatio;
        private SerializedProperty mDefaultLowRatio;
        private SerializedProperty mDefaultScale;

        private SerializedProperty mLowRatio;
        private SerializedProperty mLowScale;

        private SerializedProperty mLowestRatio;
        private SerializedProperty mLowestScale;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUIScaler)target;

            mRectTransform = mScript.gameObject.GetComponent<RectTransform>();
            if (mRectTransform == null)
            {
                Debug.LogError("[" + typeof(MyUGUIScalerEditor).Name + "] OnEnable(): Could not find RectTransform component.");
            }

            mOrientation = serializedObject.FindProperty("mOrientation");

            mHighestRatio = serializedObject.FindProperty("mHighestRatio");
            mHighestScale = serializedObject.FindProperty("mHighestScale");

            mHighRatio = serializedObject.FindProperty("mHighRatio");
            mHighScale = serializedObject.FindProperty("mHighScale");

            mDefaultHighRatio = serializedObject.FindProperty("mDefaultHighRatio");
            mDefaultLowRatio = serializedObject.FindProperty("mDefaultLowRatio");
            mDefaultScale = serializedObject.FindProperty("mDefaultScale");

            mLowRatio = serializedObject.FindProperty("mLowRatio");
            mLowScale = serializedObject.FindProperty("mLowScale");

            mLowestRatio = serializedObject.FindProperty("mLowestRatio");
            mLowestScale = serializedObject.FindProperty("mLowestScale");

            if (!mScript.IsCurrentAnchorLoaded)
            {
                mScript.IsCurrentAnchorLoaded = true;

                mHighestScale.vector3Value = mRectTransform.localScale;
                mHighScale.vector3Value = mRectTransform.localScale;
                mDefaultScale.vector3Value = mRectTransform.localScale;
                mLowScale.vector3Value = mRectTransform.localScale;
                mLowestScale.vector3Value = mRectTransform.localScale;

                serializedObject.ApplyModifiedProperties();
            }
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUGUIScaler), false);

            System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo getSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Vector2 resolution = (Vector2)getSizeOfMainGameView.Invoke(null, null);
            ScreenOrientation deviceOrientation = resolution.x > resolution.y ? ScreenOrientation.Landscape : ScreenOrientation.Portrait;

            serializedObject.Update();

            EditorGUILayout.LabelField("Orientation", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("   Screen Orientation: " + (deviceOrientation == ScreenOrientation.Landscape ? "Landscape" : "Portrait"));
            if ((mOrientation.enumValueIndex == (int)MyUGUIAnchor.EOrientation.Both)
                || (mOrientation.enumValueIndex == (int)MyUGUIAnchor.EOrientation.Portrait && deviceOrientation == ScreenOrientation.Portrait)
                || (mOrientation.enumValueIndex == (int)MyUGUIAnchor.EOrientation.Landscape && deviceOrientation == ScreenOrientation.Landscape))
            {
                EditorGUILayout.LabelField("   Status: Valid");
            }
            else
            {
                EditorGUILayout.LabelField("   Status: Invalid");
            }

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Scale Mode", EditorStyles.boldLabel);
            mOrientation.enumValueIndex = (int)(MyUGUIAnchor.EOrientation)EditorGUILayout.EnumPopup("   Orientation", (MyUGUIAnchor.EOrientation)mOrientation.enumValueIndex);
            mScript.Level = (MyUGUIScaler.ELevel)EditorGUILayout.EnumPopup("   Level", mScript.Level);
            mScript.Frequency = (MyUGUIScaler.EFrequency)EditorGUILayout.EnumPopup("   Frequency", mScript.Frequency);
            if (mOrientation.enumValueIndex != (int)MyUGUIAnchor.EOrientation.Both)
            {
                mScript.Frequency = MyUGUIScaler.EFrequency.EverytimeActive;
            }
            mScript.RoundingRatio = (MyUGUIScaler.ERoundingRatio)EditorGUILayout.EnumPopup("   Rounding Ratio", mScript.RoundingRatio);

            switch (mScript.Level)
            {
                case MyUGUIScaler.ELevel.One:
                    {

                    }
                    break;
                case MyUGUIScaler.ELevel.Three:
                    {
                        double ratio = mScript.GetCurrentRatio();
                        int ratioLevel = 0;
                        if (ratio >= mScript.GetRatio(mHighestRatio.vector2Value))
                        {
                            ratioLevel = 1;
                        }
                        else if (ratio <= mScript.GetRatio(mLowestRatio.vector2Value))
                        {
                            ratioLevel = -1;
                        }

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("High Aspect Ratio" + (ratioLevel == 1 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                        {
                            mHighestScale.vector3Value = mRectTransform.localScale;
                        }
                        GUILayout.EndHorizontal();
                        mHighestRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio >= (" + mScript.GetRatio(mHighestRatio.vector2Value) + ")", mHighestRatio.vector2Value);
                        mHighestScale.vector3Value = EditorGUILayout.Vector3Field("   Scale", mHighestScale.vector3Value);

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Medium Aspect Ratio" + (ratioLevel == 0 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                        {
                            mDefaultScale.vector3Value = mRectTransform.localScale;
                        }
                        GUILayout.EndHorizontal();
                        mDefaultScale.vector3Value = EditorGUILayout.Vector3Field("   Scale", mDefaultScale.vector3Value);

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Low Aspect Ratio" + (ratioLevel == -1 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                        {
                            mLowestScale.vector3Value = mRectTransform.localScale;
                        }
                        GUILayout.EndHorizontal();
                        mLowestRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio <= (" + mScript.GetRatio(mLowestRatio.vector2Value) + ")", mLowestRatio.vector2Value);
                        mLowestScale.vector3Value = EditorGUILayout.Vector3Field("   Scale", mLowestScale.vector3Value);
                    }
                    break;
                case MyUGUIScaler.ELevel.Five:
                    {
                        double ratio = mScript.GetCurrentRatio();
                        int ratioLevel = 0;
                        if (ratio >= mScript.GetRatio(mHighestRatio.vector2Value))
                        {
                            ratioLevel = 2;
                        }
                        else if (ratio >= mScript.GetRatio(mHighRatio.vector2Value))
                        {
                            ratioLevel = 1;
                        }
                        else if (ratio <= mScript.GetRatio(mLowestRatio.vector2Value))
                        {
                            ratioLevel = -2;
                        }
                        else if (ratio <= mScript.GetRatio(mLowRatio.vector2Value))
                        {
                            ratioLevel = -1;
                        }

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Highest Aspect Ratio" + (ratioLevel == 2 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                        {
                            mHighestScale.vector3Value = mRectTransform.localScale;
                        }
                        GUILayout.EndHorizontal();
                        mHighestRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio >= (" + mScript.GetRatio(mHighestRatio.vector2Value) + ")", mHighestRatio.vector2Value);
                        mHighestScale.vector3Value = EditorGUILayout.Vector3Field("   Scale", mHighestScale.vector3Value);

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("High Aspect Ratio" + (ratioLevel == 1 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                        {
                            mHighScale.vector3Value = mRectTransform.localScale;
                        }
                        GUILayout.EndHorizontal();
                        mHighRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio >= (" + mScript.GetRatio(mHighRatio.vector2Value) + ")", mHighRatio.vector2Value);
                        mHighScale.vector3Value = EditorGUILayout.Vector3Field("   Scale", mHighScale.vector3Value);

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Medium Aspect Ratio" + (ratioLevel == 0 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                        {
                            mDefaultScale.vector3Value = mRectTransform.localScale;
                        }
                        GUILayout.EndHorizontal();
                        mDefaultScale.vector3Value = EditorGUILayout.Vector3Field("   Scale", mDefaultScale.vector3Value);

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Low Aspect Ratio" + (ratioLevel == -1 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                        {
                            mLowScale.vector3Value = mRectTransform.localScale;
                        }
                        GUILayout.EndHorizontal();
                        mLowRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio <= (" + mScript.GetRatio(mLowRatio.vector2Value) + ")", mLowRatio.vector2Value);
                        mLowScale.vector3Value = EditorGUILayout.Vector3Field("   Scale", mLowScale.vector3Value);

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Lowest Aspect Ratio" + (ratioLevel == -2 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                        {
                            mLowestScale.vector3Value = mRectTransform.localScale;
                        }
                        GUILayout.EndHorizontal();
                        mLowestRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mLowestRatio.vector2Value) + ")", mLowestRatio.vector2Value);
                        mLowestScale.vector3Value = EditorGUILayout.Vector3Field("   Scale", mLowestScale.vector3Value);
                    }
                    break;
            }

            EditorGUILayout.LabelField(string.Empty);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Aspect Ratio (" + mScript.GetCurrentRatio() + ")", EditorStyles.boldLabel);
            if (GUILayout.Button("Scale Now", GUILayout.MaxWidth(135)))
            {
                mScript.Scale();
            }
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}

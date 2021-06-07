/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIAnchor (version 2.1)
 */

#pragma warning disable 0414
#pragma warning disable 0649

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;

namespace MyClasses.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class MyUGUIAnchor : MonoBehaviour
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
        private Vector2 mHighestPivot;
        [SerializeField]
        private Vector2 mHighestAnchorMin;
        [SerializeField]
        private Vector2 mHighestAnchorMax;
        [SerializeField]
        private Vector2 mHighestOffsetMin;
        [SerializeField]
        private Vector2 mHighestOffsetMax;

        [SerializeField]
        private Vector2 mHighRatio = new Vector2(18, 9);
        [SerializeField]
        private Vector2 mHighPivot;
        [SerializeField]
        private Vector2 mHighAnchorMin;
        [SerializeField]
        private Vector2 mHighAnchorMax;
        [SerializeField]
        private Vector2 mHighOffsetMin;
        [SerializeField]
        private Vector2 mHighOffsetMax;

        [SerializeField]
        private Vector2 mDefaultPivot;
        [SerializeField]
        private Vector2 mDefaultAnchorMin;
        [SerializeField]
        private Vector2 mDefaultAnchorMax;
        [SerializeField]
        private Vector2 mDefaultOffsetMin;
        [SerializeField]
        private Vector2 mDefaultOffsetMax;

        [SerializeField]
        private Vector2 mLowRatio = new Vector2(16, 10);
        [SerializeField]
        private Vector2 mLowPivot;
        [SerializeField]
        private Vector2 mLowAnchorMin;
        [SerializeField]
        private Vector2 mLowAnchorMax;
        [SerializeField]
        private Vector2 mLowOffsetMin;
        [SerializeField]
        private Vector2 mLowOffsetMax;

        [SerializeField]
        private Vector2 mLowestRatio = new Vector2(4.2f, 3);
        [SerializeField]
        private Vector2 mLowestPivot;
        [SerializeField]
        private Vector2 mLowestAnchorMin;
        [SerializeField]
        private Vector2 mLowestAnchorMax;
        [SerializeField]
        private Vector2 mLowestOffsetMin;
        [SerializeField]
        private Vector2 mLowestOffsetMax;

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
            mAnchorFrequency = EAnchorFrequency.EverytimeActive;
#endif

            if (mFrequency == EFrequency.OneTimeOnly)
            {
                Anchor();
            }
        }

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            if (mFrequency == EFrequency.EverytimeActive)
            {
                Anchor();
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
                Anchor();
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Anchor.
        /// </summary>
        public void Anchor()
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
                        _Anchor1();
                    }
                    break;
                case ELevel.Three:
                    {
                        _Anchor3();
                    }
                    break;
                case ELevel.Five:
                    {
                        _Anchor5();
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
        /// Anchor for One Level. 
        /// </summary>
        private void _Anchor1()
        {
            RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
            rectTrans.pivot = mDefaultPivot;
            rectTrans.anchorMin = mDefaultAnchorMin;
            rectTrans.anchorMax = mDefaultAnchorMax;
            rectTrans.offsetMin = mDefaultOffsetMin;
            rectTrans.offsetMax = mDefaultOffsetMax;
        }

        /// <summary>
        /// Anchor for Three Levels. 
        /// </summary>
        private void _Anchor3()
        {
            double curRatio = GetCurrentRatio();

            if (curRatio > GetRatio(mHighestRatio))
            {
                RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
                rectTrans.pivot = mHighestPivot;
                rectTrans.anchorMin = mHighestAnchorMin;
                rectTrans.anchorMax = mHighestAnchorMax;
                rectTrans.offsetMin = mHighestOffsetMin;
                rectTrans.offsetMax = mHighestOffsetMax;
            }
            else if (curRatio < GetRatio(mLowestRatio))
            {
                RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
                rectTrans.pivot = mLowestPivot;
                rectTrans.anchorMin = mLowestAnchorMin;
                rectTrans.anchorMax = mLowestAnchorMax;
                rectTrans.offsetMin = mLowestOffsetMin;
                rectTrans.offsetMax = mLowestOffsetMax;
            }
            else
            {
                RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
                rectTrans.pivot = mDefaultPivot;
                rectTrans.anchorMin = mDefaultAnchorMin;
                rectTrans.anchorMax = mDefaultAnchorMax;
                rectTrans.offsetMin = mDefaultOffsetMin;
                rectTrans.offsetMax = mDefaultOffsetMax;
            }
        }

        /// <summary>
        /// Anchor for Five Levels. 
        /// </summary>
        private void _Anchor5()
        {
            double curRatio = GetCurrentRatio();

            if (curRatio >= GetRatio(mHighestRatio))
            {
                RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
                rectTrans.pivot = mHighestPivot;
                rectTrans.anchorMin = mHighestAnchorMin;
                rectTrans.anchorMax = mHighestAnchorMax;
                rectTrans.offsetMin = mHighestOffsetMin;
                rectTrans.offsetMax = mHighestOffsetMax;
            }
            else if (curRatio >= GetRatio(mHighRatio))
            {
                RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
                rectTrans.pivot = mHighPivot;
                rectTrans.anchorMin = mHighAnchorMin;
                rectTrans.anchorMax = mHighAnchorMax;
                rectTrans.offsetMin = mHighOffsetMin;
                rectTrans.offsetMax = mHighOffsetMax;
            }
            else if (curRatio <= GetRatio(mLowestRatio))
            {
                RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
                rectTrans.pivot = mLowestPivot;
                rectTrans.anchorMin = mLowestAnchorMin;
                rectTrans.anchorMax = mLowestAnchorMax;
                rectTrans.offsetMin = mLowestOffsetMin;
                rectTrans.offsetMax = mLowestOffsetMax;
            }
            else if (curRatio <= GetRatio(mLowRatio))
            {
                RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
                rectTrans.pivot = mLowPivot;
                rectTrans.anchorMin = mLowAnchorMin;
                rectTrans.anchorMax = mLowAnchorMax;
                rectTrans.offsetMin = mLowOffsetMin;
                rectTrans.offsetMax = mLowOffsetMax;
            }
            else
            {
                RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
                rectTrans.pivot = mDefaultPivot;
                rectTrans.anchorMin = mDefaultAnchorMin;
                rectTrans.anchorMax = mDefaultAnchorMax;
                rectTrans.offsetMin = mDefaultOffsetMin;
                rectTrans.offsetMax = mDefaultOffsetMax;
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

    [CustomEditor(typeof(MyUGUIAnchor))]
    public class MyUGUIAnchorEditor : Editor
    {
        private MyUGUIAnchor mScript;
        private RectTransform mRectTransform;

        private SerializedProperty mOrientation;

        private SerializedProperty mHighestRatio;
        private SerializedProperty mHighestPivot;
        private SerializedProperty mHighestAnchorMin;
        private SerializedProperty mHighestAnchorMax;
        private SerializedProperty mHighestOffsetMin;
        private SerializedProperty mHighestOffsetMax;

        private SerializedProperty mHighRatio;
        private SerializedProperty mHighPivot;
        private SerializedProperty mHighAnchorMin;
        private SerializedProperty mHighAnchorMax;
        private SerializedProperty mHighOffsetMin;
        private SerializedProperty mHighOffsetMax;

        private SerializedProperty mDefaultPivot;
        private SerializedProperty mDefaultAnchorMin;
        private SerializedProperty mDefaultAnchorMax;
        private SerializedProperty mDefaultOffsetMin;
        private SerializedProperty mDefaultOffsetMax;

        private SerializedProperty mLowRatio;
        private SerializedProperty mLowPivot;
        private SerializedProperty mLowAnchorMin;
        private SerializedProperty mLowAnchorMax;
        private SerializedProperty mLowOffsetMin;
        private SerializedProperty mLowOffsetMax;

        private SerializedProperty mLowestRatio;
        private SerializedProperty mLowestPivot;
        private SerializedProperty mLowestAnchorMin;
        private SerializedProperty mLowestAnchorMax;
        private SerializedProperty mLowestOffsetMin;
        private SerializedProperty mLowestOffsetMax;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUIAnchor)target;

            mRectTransform = mScript.gameObject.GetComponent<RectTransform>();

            mOrientation = serializedObject.FindProperty("mOrientation");

            mHighestRatio = serializedObject.FindProperty("mHighestRatio");
            mHighestPivot = serializedObject.FindProperty("mHighestPivot");
            mHighestAnchorMin = serializedObject.FindProperty("mHighestAnchorMin");
            mHighestAnchorMax = serializedObject.FindProperty("mHighestAnchorMax");
            mHighestOffsetMin = serializedObject.FindProperty("mHighestOffsetMin");
            mHighestOffsetMax = serializedObject.FindProperty("mHighestOffsetMax");

            mHighRatio = serializedObject.FindProperty("mHighRatio");
            mHighPivot = serializedObject.FindProperty("mHighPivot");
            mHighAnchorMin = serializedObject.FindProperty("mHighAnchorMin");
            mHighAnchorMax = serializedObject.FindProperty("mHighAnchorMax");
            mHighOffsetMin = serializedObject.FindProperty("mHighOffsetMin");
            mHighOffsetMax = serializedObject.FindProperty("mHighOffsetMax");

            mDefaultPivot = serializedObject.FindProperty("mDefaultPivot");
            mDefaultAnchorMin = serializedObject.FindProperty("mDefaultAnchorMin");
            mDefaultAnchorMax = serializedObject.FindProperty("mDefaultAnchorMax");
            mDefaultOffsetMin = serializedObject.FindProperty("mDefaultOffsetMin");
            mDefaultOffsetMax = serializedObject.FindProperty("mDefaultOffsetMax");

            mLowRatio = serializedObject.FindProperty("mLowRatio");
            mLowPivot = serializedObject.FindProperty("mLowPivot");
            mLowAnchorMin = serializedObject.FindProperty("mLowAnchorMin");
            mLowAnchorMax = serializedObject.FindProperty("mLowAnchorMax");
            mLowOffsetMin = serializedObject.FindProperty("mLowOffsetMin");
            mLowOffsetMax = serializedObject.FindProperty("mLowOffsetMax");

            mLowestRatio = serializedObject.FindProperty("mLowestRatio");
            mLowestPivot = serializedObject.FindProperty("mLowestPivot");
            mLowestAnchorMin = serializedObject.FindProperty("mLowestAnchorMin");
            mLowestAnchorMax = serializedObject.FindProperty("mLowestAnchorMax");
            mLowestOffsetMin = serializedObject.FindProperty("mLowestOffsetMin");
            mLowestOffsetMax = serializedObject.FindProperty("mLowestOffsetMax");

            if (!mScript.IsCurrentAnchorLoaded)
            {
                mScript.IsCurrentAnchorLoaded = true;

                mHighestPivot.vector2Value = mRectTransform.pivot;
                mHighestAnchorMin.vector2Value = mRectTransform.anchorMin;
                mHighestAnchorMax.vector2Value = mRectTransform.anchorMax;
                mHighestOffsetMin.vector2Value = mRectTransform.offsetMin;
                mHighestOffsetMax.vector2Value = mRectTransform.offsetMax;

                mHighPivot.vector2Value = mRectTransform.pivot;
                mHighAnchorMin.vector2Value = mRectTransform.anchorMin;
                mHighAnchorMax.vector2Value = mRectTransform.anchorMax;
                mHighOffsetMin.vector2Value = mRectTransform.offsetMin;
                mHighOffsetMax.vector2Value = mRectTransform.offsetMax;

                mDefaultPivot.vector2Value = mRectTransform.pivot;
                mDefaultAnchorMin.vector2Value = mRectTransform.anchorMin;
                mDefaultAnchorMax.vector2Value = mRectTransform.anchorMax;
                mDefaultOffsetMin.vector2Value = mRectTransform.offsetMin;
                mDefaultOffsetMax.vector2Value = mRectTransform.offsetMax;

                mLowPivot.vector2Value = mRectTransform.pivot;
                mLowAnchorMin.vector2Value = mRectTransform.anchorMin;
                mLowAnchorMax.vector2Value = mRectTransform.anchorMax;
                mLowOffsetMin.vector2Value = mRectTransform.offsetMin;
                mLowOffsetMax.vector2Value = mRectTransform.offsetMax;

                mLowestPivot.vector2Value = mRectTransform.pivot;
                mLowestAnchorMin.vector2Value = mRectTransform.anchorMin;
                mLowestAnchorMax.vector2Value = mRectTransform.anchorMax;
                mLowestOffsetMin.vector2Value = mRectTransform.offsetMin;
                mLowestOffsetMax.vector2Value = mRectTransform.offsetMax;

                serializedObject.ApplyModifiedProperties();
            }
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUGUIAnchor), false);

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
            EditorGUILayout.LabelField("Anchor Mode", EditorStyles.boldLabel);
            mOrientation.enumValueIndex = (int)(MyUGUIAnchor.EOrientation)EditorGUILayout.EnumPopup("   Orientation", (MyUGUIAnchor.EOrientation)mOrientation.enumValueIndex);
            mScript.Level = (MyUGUIAnchor.ELevel)EditorGUILayout.EnumPopup("   Level", mScript.Level);
            mScript.Frequency = (MyUGUIAnchor.EFrequency)EditorGUILayout.EnumPopup("   Frequency", mScript.Frequency);
            if (mOrientation.enumValueIndex != (int)MyUGUIAnchor.EOrientation.Both)
            {
                mScript.Frequency = MyUGUIAnchor.EFrequency.EverytimeActive;
            }
            mScript.RoundingRatio = (MyUGUIAnchor.ERoundingRatio)EditorGUILayout.EnumPopup("   Rounding Ratio", mScript.RoundingRatio);

            switch (mScript.Level)
            {
                case MyUGUIAnchor.ELevel.One:
                    {
                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Any Aspect Ratio", EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
                        {
                            mDefaultPivot.vector2Value = mRectTransform.pivot;
                            mDefaultAnchorMin.vector2Value = mRectTransform.anchorMin;
                            mDefaultAnchorMax.vector2Value = mRectTransform.anchorMax;
                            mDefaultOffsetMin.vector2Value = mRectTransform.offsetMin;
                            mDefaultOffsetMax.vector2Value = mRectTransform.offsetMax;
                        }
                        GUILayout.EndHorizontal();
                        mDefaultPivot.vector2Value = EditorGUILayout.Vector2Field("   Pivot", mDefaultPivot.vector2Value);
                        mDefaultAnchorMin.vector2Value = EditorGUILayout.Vector2Field("   Anchor Min", mDefaultAnchorMin.vector2Value);
                        mDefaultAnchorMax.vector2Value = EditorGUILayout.Vector2Field("   Anchor Max", mDefaultAnchorMax.vector2Value);
                        mDefaultOffsetMin.vector2Value = EditorGUILayout.Vector2Field("   Offset Min", mDefaultOffsetMin.vector2Value);
                        mDefaultOffsetMax.vector2Value = EditorGUILayout.Vector2Field("   Offset Max", mDefaultOffsetMax.vector2Value);
                    }
                    break;
                case MyUGUIAnchor.ELevel.Three:
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
                        if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
                        {
                            mHighestPivot.vector2Value = mRectTransform.pivot;
                            mHighestAnchorMin.vector2Value = mRectTransform.anchorMin;
                            mHighestAnchorMax.vector2Value = mRectTransform.anchorMax;
                            mHighestOffsetMin.vector2Value = mRectTransform.offsetMin;
                            mHighestOffsetMax.vector2Value = mRectTransform.offsetMax;
                        }
                        GUILayout.EndHorizontal();
                        mHighestRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio >= (" + mScript.GetRatio(mHighestRatio.vector2Value) + ")", mHighestRatio.vector2Value);
                        EditorGUILayout.LabelField(string.Empty);
                        mHighestPivot.vector2Value = EditorGUILayout.Vector2Field("   Pivot", mHighestPivot.vector2Value);
                        mHighestAnchorMin.vector2Value = EditorGUILayout.Vector2Field("   Anchor Min", mHighestAnchorMin.vector2Value);
                        mHighestAnchorMax.vector2Value = EditorGUILayout.Vector2Field("   Anchor Max", mHighestAnchorMax.vector2Value);
                        mHighestOffsetMin.vector2Value = EditorGUILayout.Vector2Field("   Offset Min", mHighestOffsetMin.vector2Value);
                        mHighestOffsetMax.vector2Value = EditorGUILayout.Vector2Field("   Offset Max", mHighestOffsetMax.vector2Value);

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Medium Aspect Ratio" + (ratioLevel == 0 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
                        {
                            mDefaultPivot.vector2Value = mRectTransform.pivot;
                            mDefaultAnchorMin.vector2Value = mRectTransform.anchorMin;
                            mDefaultAnchorMax.vector2Value = mRectTransform.anchorMax;
                            mDefaultOffsetMin.vector2Value = mRectTransform.offsetMin;
                            mDefaultOffsetMax.vector2Value = mRectTransform.offsetMax;
                        }
                        GUILayout.EndHorizontal();
                        mDefaultPivot.vector2Value = EditorGUILayout.Vector2Field("   Pivot", mDefaultPivot.vector2Value);
                        mDefaultAnchorMin.vector2Value = EditorGUILayout.Vector2Field("   Anchor Min", mDefaultAnchorMin.vector2Value);
                        mDefaultAnchorMax.vector2Value = EditorGUILayout.Vector2Field("   Anchor Max", mDefaultAnchorMax.vector2Value);
                        mDefaultOffsetMin.vector2Value = EditorGUILayout.Vector2Field("   Offset Min", mDefaultOffsetMin.vector2Value);
                        mDefaultOffsetMax.vector2Value = EditorGUILayout.Vector2Field("   Offset Max", mDefaultOffsetMax.vector2Value);

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Low Aspect Ratio" + (ratioLevel == -1 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
                        {
                            mLowestPivot.vector2Value = mRectTransform.pivot;
                            mLowestAnchorMin.vector2Value = mRectTransform.anchorMin;
                            mLowestAnchorMax.vector2Value = mRectTransform.anchorMax;
                            mLowestOffsetMin.vector2Value = mRectTransform.offsetMin;
                            mLowestOffsetMax.vector2Value = mRectTransform.offsetMax;
                        }
                        GUILayout.EndHorizontal();
                        mLowestRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio <= (" + mScript.GetRatio(mLowestRatio.vector2Value) + ")", mLowestRatio.vector2Value);
                        EditorGUILayout.LabelField(string.Empty);
                        mLowestPivot.vector2Value = EditorGUILayout.Vector2Field("   Pivot", mLowestPivot.vector2Value);
                        mLowestAnchorMin.vector2Value = EditorGUILayout.Vector2Field("   Anchor Min", mLowestAnchorMin.vector2Value);
                        mLowestAnchorMax.vector2Value = EditorGUILayout.Vector2Field("   Anchor Max", mLowestAnchorMax.vector2Value);
                        mLowestOffsetMin.vector2Value = EditorGUILayout.Vector2Field("   Offset Min", mLowestOffsetMin.vector2Value);
                        mLowestOffsetMax.vector2Value = EditorGUILayout.Vector2Field("   Offset Max", mLowestOffsetMax.vector2Value);
                    }
                    break;
                case MyUGUIAnchor.ELevel.Five:
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
                        if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
                        {
                            mHighestPivot.vector2Value = mRectTransform.pivot;
                            mHighestAnchorMin.vector2Value = mRectTransform.anchorMin;
                            mHighestAnchorMax.vector2Value = mRectTransform.anchorMax;
                            mHighestOffsetMin.vector2Value = mRectTransform.offsetMin;
                            mHighestOffsetMax.vector2Value = mRectTransform.offsetMax;
                        }
                        GUILayout.EndHorizontal();
                        mHighestRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio >= (" + mScript.GetRatio(mHighestRatio.vector2Value) + ")", mHighestRatio.vector2Value);
                        EditorGUILayout.LabelField(string.Empty);
                        mHighestPivot.vector2Value = EditorGUILayout.Vector2Field("   Pivot", mHighestPivot.vector2Value);
                        mHighestAnchorMin.vector2Value = EditorGUILayout.Vector2Field("   Anchor Min", mHighestAnchorMin.vector2Value);
                        mHighestAnchorMax.vector2Value = EditorGUILayout.Vector2Field("   Anchor Max", mHighestAnchorMax.vector2Value);
                        mHighestOffsetMin.vector2Value = EditorGUILayout.Vector2Field("   Offset Min", mHighestOffsetMin.vector2Value);
                        mHighestOffsetMax.vector2Value = EditorGUILayout.Vector2Field("   Offset Max", mHighestOffsetMax.vector2Value);

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("High Aspect Ratio" + (ratioLevel == 1 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
                        {
                            mHighPivot.vector2Value = mRectTransform.pivot;
                            mHighAnchorMin.vector2Value = mRectTransform.anchorMin;
                            mHighAnchorMax.vector2Value = mRectTransform.anchorMax;
                            mHighOffsetMin.vector2Value = mRectTransform.offsetMin;
                            mHighOffsetMax.vector2Value = mRectTransform.offsetMax;
                        }
                        GUILayout.EndHorizontal();
                        mHighRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio >= (" + mScript.GetRatio(mHighRatio.vector2Value) + ")", mHighRatio.vector2Value);
                        EditorGUILayout.LabelField(string.Empty);
                        mHighPivot.vector2Value = EditorGUILayout.Vector2Field("   Pivot", mHighPivot.vector2Value);
                        mHighAnchorMin.vector2Value = EditorGUILayout.Vector2Field("   Anchor Min", mHighAnchorMin.vector2Value);
                        mHighAnchorMax.vector2Value = EditorGUILayout.Vector2Field("   Anchor Max", mHighAnchorMax.vector2Value);
                        mHighOffsetMin.vector2Value = EditorGUILayout.Vector2Field("   Offset Min", mHighOffsetMin.vector2Value);
                        mHighOffsetMax.vector2Value = EditorGUILayout.Vector2Field("   Offset Max", mHighOffsetMax.vector2Value);

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Medium Aspect Ratio" + (ratioLevel == 0 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
                        {
                            mDefaultPivot.vector2Value = mRectTransform.pivot;
                            mDefaultAnchorMin.vector2Value = mRectTransform.anchorMin;
                            mDefaultAnchorMax.vector2Value = mRectTransform.anchorMax;
                            mDefaultOffsetMin.vector2Value = mRectTransform.offsetMin;
                            mDefaultOffsetMax.vector2Value = mRectTransform.offsetMax;
                        }
                        GUILayout.EndHorizontal();
                        EditorGUILayout.LabelField(string.Empty);
                        mDefaultPivot.vector2Value = EditorGUILayout.Vector2Field("   Pivot", mDefaultPivot.vector2Value);
                        mDefaultAnchorMin.vector2Value = EditorGUILayout.Vector2Field("   Anchor Min", mDefaultAnchorMin.vector2Value);
                        mDefaultAnchorMax.vector2Value = EditorGUILayout.Vector2Field("   Anchor Max", mDefaultAnchorMax.vector2Value);
                        mDefaultOffsetMin.vector2Value = EditorGUILayout.Vector2Field("   Offset Min", mDefaultOffsetMin.vector2Value);
                        mDefaultOffsetMax.vector2Value = EditorGUILayout.Vector2Field("   Offset Max", mDefaultOffsetMax.vector2Value);

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Low Aspect Ratio" + (ratioLevel == -1 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
                        {
                            mLowPivot.vector2Value = mRectTransform.pivot;
                            mLowAnchorMin.vector2Value = mRectTransform.anchorMin;
                            mLowAnchorMax.vector2Value = mRectTransform.anchorMax;
                            mLowOffsetMin.vector2Value = mRectTransform.offsetMin;
                            mLowOffsetMax.vector2Value = mRectTransform.offsetMax;
                        }
                        GUILayout.EndHorizontal();
                        mLowRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio <= (" + mScript.GetRatio(mLowRatio.vector2Value) + ")", mLowRatio.vector2Value);
                        EditorGUILayout.LabelField(string.Empty);
                        mLowPivot.vector2Value = EditorGUILayout.Vector2Field("   Pivot", mLowPivot.vector2Value);
                        mLowAnchorMin.vector2Value = EditorGUILayout.Vector2Field("   Anchor Min", mLowAnchorMin.vector2Value);
                        mLowAnchorMax.vector2Value = EditorGUILayout.Vector2Field("   Anchor Max", mLowAnchorMax.vector2Value);
                        mLowOffsetMin.vector2Value = EditorGUILayout.Vector2Field("   Offset Min", mLowOffsetMin.vector2Value);
                        mLowOffsetMax.vector2Value = EditorGUILayout.Vector2Field("   Offset Max", mLowOffsetMax.vector2Value);

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Lowest Aspect Ratio" + (ratioLevel == -2 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
                        {
                            mLowestPivot.vector2Value = mRectTransform.pivot;
                            mLowestAnchorMin.vector2Value = mRectTransform.anchorMin;
                            mLowestAnchorMax.vector2Value = mRectTransform.anchorMax;
                            mLowestOffsetMin.vector2Value = mRectTransform.offsetMin;
                            mLowestOffsetMax.vector2Value = mRectTransform.offsetMax;
                        }
                        GUILayout.EndHorizontal();
                        mLowestRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio <= (" + mScript.GetRatio(mLowestRatio.vector2Value) + ")", mLowestRatio.vector2Value);
                        EditorGUILayout.LabelField(string.Empty);
                        mLowestPivot.vector2Value = EditorGUILayout.Vector2Field("   Pivot", mLowestPivot.vector2Value);
                        mLowestAnchorMin.vector2Value = EditorGUILayout.Vector2Field("   Anchor Min", mLowestAnchorMin.vector2Value);
                        mLowestAnchorMax.vector2Value = EditorGUILayout.Vector2Field("   Anchor Max", mLowestAnchorMax.vector2Value);
                        mLowestOffsetMin.vector2Value = EditorGUILayout.Vector2Field("   Offset Min", mLowestOffsetMin.vector2Value);
                        mLowestOffsetMax.vector2Value = EditorGUILayout.Vector2Field("   Offset Max", mLowestOffsetMax.vector2Value);
                    }
                    break;
            }

            EditorGUILayout.LabelField(string.Empty);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Aspect Ratio (" + mScript.GetCurrentRatio() + ")", EditorStyles.boldLabel);
            if (GUILayout.Button("Anchor Now", GUILayout.MaxWidth(135)))
            {
                mScript.Anchor();
            }
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}
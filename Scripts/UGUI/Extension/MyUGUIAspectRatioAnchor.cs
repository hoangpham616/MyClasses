/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIAspectRatioAnchor (version 2.0)
 */

#pragma warning disable 0414

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;

namespace MyClasses.UI
{
    public class MyUGUIAspectRatioAnchor : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private EAnchorLevel mAnchorLevel = EAnchorLevel.Three;
        [SerializeField]
        private EAnchorFrequency mAnchorFrequency = EAnchorFrequency.OneTimeOnly;
        [SerializeField]
        private ERoundingRatio mRoundingRatio = ERoundingRatio.OneDigit;

        [SerializeField]
        private Vector2 mHighestRatio = new Vector2(18.5f, 9);
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
        private Vector2 mDefaultHighRatio = new Vector2(16.3f, 9);
        [SerializeField]
        private Vector2 mDefaultLowRatio = new Vector2(15.7f, 9);
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
        private Vector2 mLowestRatio = new Vector2(4, 3);
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
        private bool mIsInit = false;

        #endregion

        #region ----- Property -----

#if UNITY_EDITOR

        public EAnchorLevel AnchorLevel
        {
            get { return mAnchorLevel; }
            set { mAnchorLevel = value; }
        }

        public EAnchorFrequency AnchorFrequency
        {
            get { return mAnchorFrequency; }
            set { mAnchorFrequency = value; }
        }

        public ERoundingRatio RoundingRatio
        {
            get { return mRoundingRatio; }
            set { mRoundingRatio = value; }
        }

        public Vector2 HighestRatio
        {
            get { return mHighestRatio; }
            set { mHighestRatio = value; }
        }

        public Vector2 HighestPivot
        {
            get { return mHighestPivot; }
            set { mHighestPivot = value; }
        }

        public Vector2 HighestAnchorMin
        {
            get { return mHighestAnchorMin; }
            set { mHighestAnchorMin = value; }
        }

        public Vector2 HighestAnchorMax
        {
            get { return mHighestAnchorMax; }
            set { mHighestAnchorMax = value; }
        }

        public Vector2 HighestOffsetMin
        {
            get { return mHighestOffsetMin; }
            set { mHighestOffsetMin = value; }
        }

        public Vector2 HighestOffsetMax
        {
            get { return mHighestOffsetMax; }
            set { mHighestOffsetMax = value; }
        }

        public Vector2 HighRatio
        {
            get { return mHighRatio; }
            set { mHighRatio = value; }
        }

        public Vector2 HighPivot
        {
            get { return mHighPivot; }
            set { mHighPivot = value; }
        }

        public Vector2 HighAnchorMin
        {
            get { return mHighAnchorMin; }
            set { mHighAnchorMin = value; }
        }

        public Vector2 HighAnchorMax
        {
            get { return mHighAnchorMax; }
            set { mHighAnchorMax = value; }
        }

        public Vector2 HighOffsetMin
        {
            get { return mHighOffsetMin; }
            set { mHighOffsetMin = value; }
        }

        public Vector2 HighOffsetMax
        {
            get { return mHighOffsetMax; }
            set { mHighOffsetMax = value; }
        }

        public Vector2 DefaultHighRatio
        {
            get { return mDefaultHighRatio; }
            set { mDefaultHighRatio = value; }
        }

        public Vector2 DefaultLowRatio
        {
            get { return mDefaultLowRatio; }
            set { mDefaultLowRatio = value; }
        }

        public Vector2 DefaultPivot
        {
            get { return mDefaultPivot; }
            set { mDefaultPivot = value; }
        }

        public Vector2 DefaultAnchorMin
        {
            get { return mDefaultAnchorMin; }
            set { mDefaultAnchorMin = value; }
        }

        public Vector2 DefaultAnchorMax
        {
            get { return mDefaultAnchorMax; }
            set { mDefaultAnchorMax = value; }
        }

        public Vector2 DefaultOffsetMin
        {
            get { return mDefaultOffsetMin; }
            set { mDefaultOffsetMin = value; }
        }

        public Vector2 DefaultOffsetMax
        {
            get { return mDefaultOffsetMax; }
            set { mDefaultOffsetMax = value; }
        }

        public Vector2 LowRatio
        {
            get { return mLowRatio; }
            set { mLowRatio = value; }
        }

        public Vector2 LowPivot
        {
            get { return mLowPivot; }
            set { mLowPivot = value; }
        }

        public Vector2 LowAnchorMin
        {
            get { return mLowAnchorMin; }
            set { mLowAnchorMin = value; }
        }

        public Vector2 LowAnchorMax
        {
            get { return mLowAnchorMax; }
            set { mLowAnchorMax = value; }
        }

        public Vector2 LowOffsetMin
        {
            get { return mLowOffsetMin; }
            set { mLowOffsetMin = value; }
        }

        public Vector2 LowOffsetMax
        {
            get { return mLowOffsetMax; }
            set { mLowOffsetMax = value; }
        }

        public Vector2 LowestRatio
        {
            get { return mLowestRatio; }
            set { mLowestRatio = value; }
        }

        public Vector2 LowestPivot
        {
            get { return mLowestPivot; }
            set { mLowestPivot = value; }
        }

        public Vector2 LowestAnchorMin
        {
            get { return mLowestAnchorMin; }
            set { mLowestAnchorMin = value; }
        }

        public Vector2 LowestAnchorMax
        {
            get { return mLowestAnchorMax; }
            set { mLowestAnchorMax = value; }
        }

        public Vector2 LowestOffsetMin
        {
            get { return mLowestOffsetMin; }
            set { mLowestOffsetMin = value; }
        }

        public Vector2 LowestOffsetMax
        {
            get { return mLowestOffsetMax; }
            set { mLowestOffsetMax = value; }
        }

        public bool IsInit
        {
            get { return mIsInit; }
            set { mIsInit = value; }
        }

#endif

        #endregion

        #region ----- Implement MonoBehaviour -----

        /// <summary>
        /// Awake.
        /// </summary>
        void Awake()
        {
            if (mAnchorFrequency == EAnchorFrequency.OneTimeOnly)
            {
                Anchor();
            }
        }

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            if (mAnchorFrequency == EAnchorFrequency.EverytimeActive)
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
            if (mAnchorLevel == EAnchorLevel.Three)
            {
                _Anchor3();
            }
            else
            {
                _Anchor5();
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
            return Math.Round(resolution.x / resolution.y, (int)mRoundingRatio);
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Anchor for Level Three. 
        /// </summary>
        private void _Anchor3()
        {
            double curRatio = GetCurrentRatio();

            if (curRatio > GetRatio(mDefaultHighRatio))
            {
                RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
                rectTrans.pivot = mHighestPivot;
                rectTrans.anchorMin = mHighestAnchorMin;
                rectTrans.anchorMax = mHighestAnchorMax;
                rectTrans.offsetMin = mHighestOffsetMin;
                rectTrans.offsetMax = mHighestOffsetMax;
            }
            else if (curRatio < GetRatio(mDefaultLowRatio))
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
        /// Anchor for Level Five. 
        /// </summary>
        private void _Anchor5()
        {
            double curRatio = GetCurrentRatio();

            if (curRatio > GetRatio(mHighRatio))
            {
                RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
                rectTrans.pivot = mHighestPivot;
                rectTrans.anchorMin = mHighestAnchorMin;
                rectTrans.anchorMax = mHighestAnchorMax;
                rectTrans.offsetMin = mHighestOffsetMin;
                rectTrans.offsetMax = mHighestOffsetMax;
            }
            else if (curRatio > GetRatio(mDefaultHighRatio))
            {
                RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
                rectTrans.pivot = mHighPivot;
                rectTrans.anchorMin = mHighAnchorMin;
                rectTrans.anchorMax = mHighAnchorMax;
                rectTrans.offsetMin = mHighOffsetMin;
                rectTrans.offsetMax = mHighOffsetMax;
            }
            else if (curRatio < GetRatio(mLowRatio))
            {
                RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
                rectTrans.pivot = mLowestPivot;
                rectTrans.anchorMin = mLowestAnchorMin;
                rectTrans.anchorMax = mLowestAnchorMax;
                rectTrans.offsetMin = mLowestOffsetMin;
                rectTrans.offsetMax = mLowestOffsetMax;
            }
            else if (curRatio < GetRatio(mDefaultLowRatio))
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

        public enum EAnchorLevel
        {
            Three,
            Five
        }

        public enum EAnchorFrequency
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

    [CustomEditor(typeof(MyUGUIAspectRatioAnchor))]
    public class MyUGUIAspectRatioAnchorEditor : Editor
    {
        private MyUGUIAspectRatioAnchor mScript;
        private RectTransform mRectTransform;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUIAspectRatioAnchor)target;
            
            mRectTransform = mScript.gameObject.GetComponent<RectTransform>();
            if (mRectTransform == null)
            {
                Debug.LogError("[" + typeof(MyUGUIAspectRatioAnchorEditor).Name + "] OnEnable(): Could not find RectTransform component.");
            }

            if (!mScript.IsInit)
            {
                mScript.IsInit = true;

                mScript.HighestPivot = mRectTransform.pivot;
                mScript.HighestAnchorMin = mRectTransform.anchorMin;
                mScript.HighestAnchorMax = mRectTransform.anchorMax;
                mScript.HighestOffsetMin = mRectTransform.offsetMin;
                mScript.HighestOffsetMax = mRectTransform.offsetMax;

                mScript.HighPivot = mRectTransform.pivot;
                mScript.HighAnchorMin = mRectTransform.anchorMin;
                mScript.HighAnchorMax = mRectTransform.anchorMax;
                mScript.HighOffsetMin = mRectTransform.offsetMin;
                mScript.HighOffsetMax = mRectTransform.offsetMax;

                mScript.DefaultPivot = mRectTransform.pivot;
                mScript.DefaultAnchorMin = mRectTransform.anchorMin;
                mScript.DefaultAnchorMax = mRectTransform.anchorMax;
                mScript.DefaultOffsetMin = mRectTransform.offsetMin;
                mScript.DefaultOffsetMax = mRectTransform.offsetMax;

                mScript.LowPivot = mRectTransform.pivot;
                mScript.LowAnchorMin = mRectTransform.anchorMin;
                mScript.LowAnchorMax = mRectTransform.anchorMax;
                mScript.LowOffsetMin = mRectTransform.offsetMin;
                mScript.LowOffsetMax = mRectTransform.offsetMax;

                mScript.LowestPivot = mRectTransform.pivot;
                mScript.LowestAnchorMin = mRectTransform.anchorMin;
                mScript.LowestAnchorMax = mRectTransform.anchorMax;
                mScript.LowestOffsetMin = mRectTransform.offsetMin;
                mScript.LowestOffsetMax = mRectTransform.offsetMax;
            }
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUGUIAspectRatioAnchor), false);
        
            EditorGUILayout.LabelField("Anchor Mode", EditorStyles.boldLabel);
            mScript.AnchorLevel = (MyUGUIAspectRatioAnchor.EAnchorLevel)EditorGUILayout.EnumPopup("   Anchor Level", mScript.AnchorLevel);
            mScript.AnchorFrequency = (MyUGUIAspectRatioAnchor.EAnchorFrequency)EditorGUILayout.EnumPopup("   Anchor Frequency", mScript.AnchorFrequency);
            mScript.RoundingRatio = (MyUGUIAspectRatioAnchor.ERoundingRatio)EditorGUILayout.EnumPopup("   Rounding Ratio", mScript.RoundingRatio);

            if (mScript.AnchorLevel == MyUGUIAspectRatioAnchor.EAnchorLevel.Three)
            {
                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Highest Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
                {
                    mScript.HighestPivot = mRectTransform.pivot;
                    mScript.HighestAnchorMin = mRectTransform.anchorMin;
                    mScript.HighestAnchorMax = mRectTransform.anchorMax;
                    mScript.HighestOffsetMin = mRectTransform.offsetMin;
                    mScript.HighestOffsetMax = mRectTransform.offsetMax;
                }
                GUILayout.EndHorizontal();
                mScript.HighestRatio = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mScript.HighestRatio) + ")", mScript.HighestRatio);
                EditorGUILayout.LabelField(string.Empty);
                mScript.HighestPivot = EditorGUILayout.Vector2Field("   Pivot", mScript.HighestPivot);
                mScript.HighestAnchorMin = EditorGUILayout.Vector2Field("   Anchor Min", mScript.HighestAnchorMin);
                mScript.HighestAnchorMax = EditorGUILayout.Vector2Field("   Anchor Max", mScript.HighestAnchorMax);
                mScript.HighestOffsetMin = EditorGUILayout.Vector2Field("   Offset Min", mScript.HighestOffsetMin);
                mScript.HighestOffsetMax = EditorGUILayout.Vector2Field("   Offset Max", mScript.HighestOffsetMax);

                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Default Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
                {
                    mScript.DefaultPivot = mRectTransform.pivot;
                    mScript.DefaultAnchorMin = mRectTransform.anchorMin;
                    mScript.DefaultAnchorMax = mRectTransform.anchorMax;
                    mScript.DefaultOffsetMin = mRectTransform.offsetMin;
                    mScript.DefaultOffsetMax = mRectTransform.offsetMax;
                }
                GUILayout.EndHorizontal();
                mScript.DefaultHighRatio = EditorGUILayout.Vector2Field("   High Ratio (" + mScript.GetRatio(mScript.DefaultHighRatio) + ")", mScript.DefaultHighRatio);
                mScript.DefaultLowRatio = EditorGUILayout.Vector2Field("   Low Ratio (" + mScript.GetRatio(mScript.DefaultLowRatio) + ")", mScript.DefaultLowRatio);
                EditorGUILayout.LabelField(string.Empty);
                mScript.DefaultPivot = EditorGUILayout.Vector2Field("   Pivot", mScript.DefaultPivot);
                mScript.DefaultAnchorMin = EditorGUILayout.Vector2Field("   Anchor Min", mScript.DefaultAnchorMin);
                mScript.DefaultAnchorMax = EditorGUILayout.Vector2Field("   Anchor Max", mScript.DefaultAnchorMax);
                mScript.DefaultOffsetMin = EditorGUILayout.Vector2Field("   Offset Min", mScript.DefaultOffsetMin);
                mScript.DefaultOffsetMax = EditorGUILayout.Vector2Field("   Offset Max", mScript.DefaultOffsetMax);

                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Low Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
                {
                    mScript.LowestPivot = mRectTransform.pivot;
                    mScript.LowestAnchorMin = mRectTransform.anchorMin;
                    mScript.LowestAnchorMax = mRectTransform.anchorMax;
                    mScript.LowestOffsetMin = mRectTransform.offsetMin;
                    mScript.LowestOffsetMax = mRectTransform.offsetMax;
                }
                GUILayout.EndHorizontal();
                mScript.LowestRatio = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mScript.LowestRatio) + ")", mScript.LowestRatio);
                EditorGUILayout.LabelField(string.Empty);
                mScript.LowestPivot = EditorGUILayout.Vector2Field("   Pivot", mScript.LowestPivot);
                mScript.LowestAnchorMin = EditorGUILayout.Vector2Field("   Anchor Min", mScript.LowestAnchorMin);
                mScript.LowestAnchorMax = EditorGUILayout.Vector2Field("   Anchor Max", mScript.LowestAnchorMax);
                mScript.LowestOffsetMin = EditorGUILayout.Vector2Field("   Offset Min", mScript.LowestOffsetMin);
                mScript.LowestOffsetMax = EditorGUILayout.Vector2Field("   Offset Max", mScript.LowestOffsetMax);
            }
            else
            {
                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Highest Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
                {
                    mScript.HighestPivot = mRectTransform.pivot;
                    mScript.HighestAnchorMin = mRectTransform.anchorMin;
                    mScript.HighestAnchorMax = mRectTransform.anchorMax;
                    mScript.HighestOffsetMin = mRectTransform.offsetMin;
                    mScript.HighestOffsetMax = mRectTransform.offsetMax;
                }
                GUILayout.EndHorizontal();
                mScript.HighestRatio = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mScript.HighestRatio) + ")", mScript.HighestRatio);
                EditorGUILayout.LabelField(string.Empty);
                mScript.HighestPivot = EditorGUILayout.Vector2Field("   Pivot", mScript.HighestPivot);
                mScript.HighestAnchorMin = EditorGUILayout.Vector2Field("   Anchor Min", mScript.HighestAnchorMin);
                mScript.HighestAnchorMax = EditorGUILayout.Vector2Field("   Anchor Max", mScript.HighestAnchorMax);
                mScript.HighestOffsetMin = EditorGUILayout.Vector2Field("   Offset Min", mScript.HighestOffsetMin);
                mScript.HighestOffsetMax = EditorGUILayout.Vector2Field("   Offset Max", mScript.HighestOffsetMax);

                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("High Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
                {
                    mScript.HighPivot = mRectTransform.pivot;
                    mScript.HighAnchorMin = mRectTransform.anchorMin;
                    mScript.HighAnchorMax = mRectTransform.anchorMax;
                    mScript.HighOffsetMin = mRectTransform.offsetMin;
                    mScript.HighOffsetMax = mRectTransform.offsetMax;
                }
                GUILayout.EndHorizontal();
                mScript.HighRatio = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mScript.HighRatio) + ")", mScript.HighRatio);
                EditorGUILayout.LabelField(string.Empty);
                mScript.HighPivot = EditorGUILayout.Vector2Field("   Pivot", mScript.HighPivot);
                mScript.HighAnchorMin = EditorGUILayout.Vector2Field("   Anchor Min", mScript.HighAnchorMin);
                mScript.HighAnchorMax = EditorGUILayout.Vector2Field("   Anchor Max", mScript.HighAnchorMax);
                mScript.HighOffsetMin = EditorGUILayout.Vector2Field("   Offset Min", mScript.HighOffsetMin);
                mScript.HighOffsetMax = EditorGUILayout.Vector2Field("   Offset Max", mScript.HighOffsetMax);

                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Default Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
                {
                    mScript.DefaultPivot = mRectTransform.pivot;
                    mScript.DefaultAnchorMin = mRectTransform.anchorMin;
                    mScript.DefaultAnchorMax = mRectTransform.anchorMax;
                    mScript.DefaultOffsetMin = mRectTransform.offsetMin;
                    mScript.DefaultOffsetMax = mRectTransform.offsetMax;
                }
                GUILayout.EndHorizontal();
                mScript.DefaultHighRatio = EditorGUILayout.Vector2Field("   High Ratio (" + mScript.GetRatio(mScript.DefaultHighRatio) + ")", mScript.DefaultHighRatio);
                mScript.DefaultLowRatio = EditorGUILayout.Vector2Field("   Low Ratio (" + mScript.GetRatio(mScript.DefaultLowRatio) + ")", mScript.DefaultLowRatio);
                EditorGUILayout.LabelField(string.Empty);
                mScript.DefaultPivot = EditorGUILayout.Vector2Field("   Pivot", mScript.DefaultPivot);
                mScript.DefaultAnchorMin = EditorGUILayout.Vector2Field("   Anchor Min", mScript.DefaultAnchorMin);
                mScript.DefaultAnchorMax = EditorGUILayout.Vector2Field("   Anchor Max", mScript.DefaultAnchorMax);
                mScript.DefaultOffsetMin = EditorGUILayout.Vector2Field("   Offset Min", mScript.DefaultOffsetMin);
                mScript.DefaultOffsetMax = EditorGUILayout.Vector2Field("   Offset Max", mScript.DefaultOffsetMax);

                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Low Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
                {
                    mScript.LowPivot = mRectTransform.pivot;
                    mScript.LowAnchorMin = mRectTransform.anchorMin;
                    mScript.LowAnchorMax = mRectTransform.anchorMax;
                    mScript.LowOffsetMin = mRectTransform.offsetMin;
                    mScript.LowOffsetMax = mRectTransform.offsetMax;
                }
                GUILayout.EndHorizontal();
                mScript.LowRatio = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mScript.LowRatio) + ")", mScript.LowRatio);
                EditorGUILayout.LabelField(string.Empty);
                mScript.LowPivot = EditorGUILayout.Vector2Field("   Pivot", mScript.LowPivot);
                mScript.LowAnchorMin = EditorGUILayout.Vector2Field("   Anchor Min", mScript.LowAnchorMin);
                mScript.LowAnchorMax = EditorGUILayout.Vector2Field("   Anchor Max", mScript.LowAnchorMax);
                mScript.LowOffsetMin = EditorGUILayout.Vector2Field("   Offset Min", mScript.LowOffsetMin);
                mScript.LowOffsetMax = EditorGUILayout.Vector2Field("   Offset Max", mScript.LowOffsetMax);

                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Lowest Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
                {
                    mScript.LowestPivot = mRectTransform.pivot;
                    mScript.LowestAnchorMin = mRectTransform.anchorMin;
                    mScript.LowestAnchorMax = mRectTransform.anchorMax;
                    mScript.LowestOffsetMin = mRectTransform.offsetMin;
                    mScript.LowestOffsetMax = mRectTransform.offsetMax;
                }
                GUILayout.EndHorizontal();
                mScript.LowestRatio = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mScript.LowestRatio) + ")", mScript.LowestRatio);
                EditorGUILayout.LabelField(string.Empty);
                mScript.LowestPivot = EditorGUILayout.Vector2Field("   Pivot", mScript.LowestPivot);
                mScript.LowestAnchorMin = EditorGUILayout.Vector2Field("   Anchor Min", mScript.LowestAnchorMin);
                mScript.LowestAnchorMax = EditorGUILayout.Vector2Field("   Anchor Max", mScript.LowestAnchorMax);
                mScript.LowestOffsetMin = EditorGUILayout.Vector2Field("   Offset Min", mScript.LowestOffsetMin);
                mScript.LowestOffsetMax = EditorGUILayout.Vector2Field("   Offset Max", mScript.LowestOffsetMax);
            }

            EditorGUILayout.LabelField(string.Empty);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Aspect Ratio (" + mScript.GetCurrentRatio() + ")", EditorStyles.boldLabel);
            if (GUILayout.Button("Anchor Now", GUILayout.MaxWidth(135)))
            {
                mScript.Anchor();
            }
            GUILayout.EndHorizontal();
        }
    }

#endif
}
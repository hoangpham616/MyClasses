/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIAspectRatioScaler (version 2.0)
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;

namespace MyClasses.UI
{
    public class MyUGUIAspectRatioScaler : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private EScaleLevel mScaleLevel = EScaleLevel.Three;
        [SerializeField]
        private EScaleFrequency mScaleFrequency = EScaleFrequency.OneTimeOnly;
        [SerializeField]
        private EScaleType mScaleType = EScaleType.Dynamic;
        [SerializeField]
        private ERoundingRatio mRoundingRatio = ERoundingRatio.OneDigit;

        [SerializeField]
        private Vector2 mHighestRatio = new Vector2(18.5f, 9);
        [SerializeField]
        private Vector2 mHighestScale = Vector3.one;

        [SerializeField]
        private Vector2 mHighRatio = new Vector2(18, 9);
        [SerializeField]
        private Vector2 mHighScale = Vector3.one;

        [SerializeField]
        private Vector2 mDefaultHighRatio = new Vector2(16.3f, 9);
        [SerializeField]
        private Vector2 mDefaultLowRatio = new Vector2(15.7f, 9);
        [SerializeField]
        private Vector2 mDefaultScale = Vector3.one;

        [SerializeField]
        private Vector2 mLowRatio = new Vector2(16, 10);
        [SerializeField]
        private Vector2 mLowScale = Vector3.one;

        [SerializeField]
        private Vector2 mLowestRatio = new Vector2(4, 3);
        [SerializeField]
        private Vector2 mLowestScale = Vector3.one;

        #endregion

        #region ----- Property -----

#if UNITY_EDITOR

        public EScaleLevel ScaleLevel
        {
            get { return mScaleLevel; }
            set { mScaleLevel = value; }
        }

        public EScaleFrequency ScaleFrequency
        {
            get { return mScaleFrequency; }
            set { mScaleFrequency = value; }
        }

        public EScaleType ScaleType
        {
            get { return mScaleType; }
            set { mScaleType = value; }
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

        public Vector2 HighestScale
        {
            get { return mHighestScale; }
            set { mHighestScale = value; }
        }

        public Vector2 HighRatio
        {
            get { return mHighRatio; }
            set { mHighRatio = value; }
        }

        public Vector2 HighScale
        {
            get { return mHighScale; }
            set { mHighScale = value; }
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

        public Vector2 DefaultScale
        {
            get { return mDefaultScale; }
            set { mDefaultScale = value; }
        }

        public Vector2 LowRatio
        {
            get { return mLowRatio; }
            set { mLowRatio = value; }
        }

        public Vector2 LowScale
        {
            get { return mLowScale; }
            set { mLowScale = value; }
        }

        public Vector2 LowestRatio
        {
            get { return mLowestRatio; }
            set { mLowestRatio = value; }
        }

        public Vector2 LowestScale
        {
            get { return mLowestScale; }
            set { mLowestScale = value; }
        }

#endif

        #endregion

        #region ----- Implement MonoBehaviour -----

        /// <summary>
        /// Awake.
        /// </summary>
        void Awake()
        {
            if (mScaleFrequency == EScaleFrequency.OneTimeOnly)
            {
                Scale();
            }
        }

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            if (mScaleFrequency == EScaleFrequency.EverytimeActive)
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
            if (mScaleLevel == EScaleLevel.Three)
            {
                _Scale3();
            }
            else
            {
                _Scale5();
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
        /// Scale for Level Three. 
        /// </summary>
        private void _Scale3()
        {
            double curRatio = GetCurrentRatio();

            if (curRatio > GetRatio(mDefaultHighRatio))
            {
                if (mScaleType == EScaleType.Fixed || curRatio >= (mHighestRatio.x / mHighestRatio.y))
                {
                    transform.localScale = mHighestScale;
                }
                else
                {
                    transform.localScale = _CalculateScale(mDefaultHighRatio, mDefaultScale, mHighestRatio, mHighestScale, curRatio);
                }
            }
            else if (curRatio < GetRatio(mDefaultLowRatio))
            {
                if (mScaleType == EScaleType.Fixed || curRatio <= (mLowestRatio.x / mLowestRatio.y))
                {
                    transform.localScale = mLowestScale;
                }
                else
                {
                    transform.localScale = _CalculateScale(mDefaultHighRatio, mDefaultScale, mLowestRatio, mLowestScale, curRatio);
                }
            }
            else
            {
                transform.localScale = mDefaultScale;
            }
        }

        /// <summary>
        /// Scale for Level Five. 
        /// </summary>
        private void _Scale5()
        {
            double curRatio = GetCurrentRatio();

            if (curRatio > GetRatio(mDefaultHighRatio))
            {
                if (curRatio > GetRatio(mHighRatio))
                {
                    if (mScaleType == EScaleType.Fixed || curRatio >= GetRatio(mHighestRatio))
                    {
                        transform.localScale = mHighestScale;
                    }
                    else
                    {
                        transform.localScale = _CalculateScale(mHighRatio, mHighScale, mHighestRatio, mHighestScale, curRatio);
                    }
                }
                else
                {
                    if (mScaleType == EScaleType.Fixed)
                    {
                        transform.localScale = mHighScale;
                    }
                    else
                    {
                        transform.localScale = _CalculateScale(mDefaultHighRatio, mDefaultScale, mHighRatio, mHighScale, curRatio);
                    }
                }
            }
            else if (curRatio < GetRatio(mDefaultLowRatio))
            {
                if (curRatio < GetRatio(mLowRatio))
                {
                    if (mScaleType == EScaleType.Fixed || curRatio <= GetRatio(mLowestRatio))
                    {
                        transform.localScale = mLowestScale;
                    }
                    else
                    {
                        transform.localScale = _CalculateScale(mLowRatio, mLowScale, mLowestRatio, mLowestScale, curRatio);
                    }
                }
                else
                {
                    if (mScaleType == EScaleType.Fixed)
                    {
                        transform.localScale = mLowScale;
                    }
                    else
                    {
                        transform.localScale = _CalculateScale(mDefaultLowRatio, mDefaultScale, mLowRatio, mLowScale, curRatio);
                    }
                }
            }
            else
            {
                transform.localScale = mDefaultScale;
            }
        }

        /// <summary>
        /// Calculate scale value.
        /// </summary>
        private Vector2 _CalculateScale(Vector2 fromRatio, Vector2 fromScale, Vector2 toRatio, Vector2 toScale, double curRatioValue)
        {
            Vector2 scaleRange = toScale - fromScale;
            float fromRatioValue = fromRatio.x / fromRatio.y;
            float toRatioValue = toRatio.x / toRatio.y;
            float ratioRange = toRatioValue - fromRatioValue;

            return fromScale + ((((float)curRatioValue - fromRatioValue) / ratioRange) * scaleRange);
        }

        #endregion

        #region ----- Enumeration -----

        public enum EScaleLevel
        {
            Three,
            Five
        }

        public enum EScaleFrequency
        {
            OneTimeOnly,
            EverytimeActive
        }

        public enum EScaleType
        {
            Dynamic,
            Fixed
        }

        public enum ERoundingRatio
        {
            OneDigit = 1,
            TwoDigits = 2
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyUGUIAspectRatioScaler))]
    public class MyUGUIAspectRatioScalerEditor : Editor
    {
        private MyUGUIAspectRatioScaler mScript;
        private RectTransform mRectTransform;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUIAspectRatioScaler)target;
            
            mRectTransform = mScript.gameObject.GetComponent<RectTransform>();
            if (mRectTransform == null)
            {
                Debug.LogError("[" + typeof(MyUGUIAspectRatioScalerEditor).Name + "] OnEnable(): Could not find RectTransform component.");
            }
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUGUIAspectRatioScaler), false);
            
            EditorGUILayout.LabelField("Scale Mode", EditorStyles.boldLabel);
            mScript.ScaleLevel = (MyUGUIAspectRatioScaler.EScaleLevel)EditorGUILayout.EnumPopup("   Scale Level", mScript.ScaleLevel);
            mScript.ScaleFrequency = (MyUGUIAspectRatioScaler.EScaleFrequency)EditorGUILayout.EnumPopup("   Scale Frequency", mScript.ScaleFrequency);
            mScript.ScaleType = (MyUGUIAspectRatioScaler.EScaleType)EditorGUILayout.EnumPopup("   Scale Type", mScript.ScaleType);
            mScript.RoundingRatio = (MyUGUIAspectRatioScaler.ERoundingRatio)EditorGUILayout.EnumPopup("   Rounding Ratio", mScript.RoundingRatio);

            if (mScript.ScaleLevel == MyUGUIAspectRatioScaler.EScaleLevel.Three)
            {
                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("High Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                {
                    mScript.HighestScale = mRectTransform.localScale;
                }
                GUILayout.EndHorizontal();
                mScript.HighestRatio = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mScript.HighestRatio) + ")", mScript.HighestRatio);
                mScript.HighestScale = EditorGUILayout.Vector2Field("   Scale", mScript.HighestScale);

                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Default Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                {
                    mScript.DefaultScale = mRectTransform.localScale;
                }
                GUILayout.EndHorizontal();
                mScript.DefaultHighRatio = EditorGUILayout.Vector2Field("   High Ratio (" + mScript.GetRatio(mScript.DefaultHighRatio) + ")", mScript.DefaultHighRatio);
                mScript.DefaultLowRatio = EditorGUILayout.Vector2Field("   Low Ratio (" + mScript.GetRatio(mScript.DefaultLowRatio) + ")", mScript.DefaultLowRatio);
                mScript.DefaultScale = EditorGUILayout.Vector2Field("   Scale", mScript.DefaultScale);

                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Low Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                {
                    mScript.LowestScale = mRectTransform.localScale;
                }
                GUILayout.EndHorizontal();
                mScript.LowestRatio = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mScript.LowestRatio) + ")", mScript.LowestRatio);
                mScript.LowestScale = EditorGUILayout.Vector2Field("   Scale", mScript.LowestScale);
            }
            else
            {
                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Highest Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                {
                    mScript.HighestScale = mRectTransform.localScale;
                }
                GUILayout.EndHorizontal();
                mScript.HighestRatio = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mScript.HighestRatio) + ")", mScript.HighestRatio);
                mScript.HighestScale = EditorGUILayout.Vector2Field("   Scale", mScript.HighestScale);

                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("High Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                {
                    mScript.HighScale = mRectTransform.localScale;
                }
                GUILayout.EndHorizontal();
                mScript.HighRatio = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mScript.HighRatio) + ")", mScript.HighRatio);
                mScript.HighScale = EditorGUILayout.Vector2Field("   Scale", mScript.HighScale);

                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Default Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                {
                    mScript.DefaultScale = mRectTransform.localScale;
                }
                GUILayout.EndHorizontal();
                mScript.DefaultHighRatio = EditorGUILayout.Vector2Field("   High Ratio (" + mScript.GetRatio(mScript.DefaultHighRatio) + ")", mScript.DefaultHighRatio);
                mScript.DefaultLowRatio = EditorGUILayout.Vector2Field("   Low Ratio (" + mScript.GetRatio(mScript.DefaultLowRatio) + ")", mScript.DefaultLowRatio);
                mScript.DefaultScale = EditorGUILayout.Vector2Field("   Scale", mScript.DefaultScale);

                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Low Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                {
                    mScript.LowScale = mRectTransform.localScale;
                }
                GUILayout.EndHorizontal();
                mScript.LowRatio = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mScript.LowRatio) + ")", mScript.LowRatio);
                mScript.LowScale = EditorGUILayout.Vector2Field("   Scale", mScript.LowScale);

                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Lowest Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                {
                    mScript.LowestScale = mRectTransform.localScale;
                }
                GUILayout.EndHorizontal();
                mScript.LowestRatio = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mScript.LowestRatio) + ")", mScript.LowestRatio);
                mScript.LowestScale = EditorGUILayout.Vector2Field("   Scale", mScript.LowestScale);
            }

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Current Aspect Ratio", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("   Ratio (" + mScript.GetCurrentRatio().ToString() + ")");
            if (GUILayout.Button("\t\tScale Now\t\t"))
            {
                mScript.Scale();
            }
            EditorGUILayout.LabelField(string.Empty);
            GUILayout.EndHorizontal();
        }
    }

#endif
}

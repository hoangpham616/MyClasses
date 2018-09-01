/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIAspectRatioScaler (version 2.10)
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
        private Vector2 mDefaultHighRatio = new Vector2(16.4f, 9);
        [SerializeField]
        private Vector2 mDefaultLowRatio = new Vector2(15.6f, 9);
        [SerializeField]
        private Vector3 mDefaultScale = Vector3.one;

        [SerializeField]
        private Vector2 mLowRatio = new Vector2(16, 10);
        [SerializeField]
        private Vector3 mLowScale = Vector3.one;

        [SerializeField]
        private Vector2 mLowestRatio = new Vector2(4, 3);
        [SerializeField]
        private Vector3 mLowestScale = Vector3.one;

        [SerializeField]
        private bool mIsCurrentAnchorLoaded = false;

        #endregion

        #region ----- Property -----

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
            mScript = (MyUGUIAspectRatioScaler)target;
            
            mRectTransform = mScript.gameObject.GetComponent<RectTransform>();
            if (mRectTransform == null)
            {
                Debug.LogError("[" + typeof(MyUGUIAspectRatioScalerEditor).Name + "] OnEnable(): Could not find RectTransform component.");
            }
            
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
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUGUIAspectRatioScaler), false);

            serializedObject.Update();

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
                    mHighestScale.vector3Value = mRectTransform.localScale;
                }
                GUILayout.EndHorizontal();
                mHighestRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mHighestRatio.vector2Value) + ")", mHighestRatio.vector2Value);
                mHighestScale.vector3Value = EditorGUILayout.Vector3Field("   Scale", mHighestScale.vector3Value);

                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Default Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                {
                    mDefaultScale.vector3Value = mRectTransform.localScale;
                }
                GUILayout.EndHorizontal();
                mDefaultHighRatio.vector2Value = EditorGUILayout.Vector2Field("   High Ratio (" + mScript.GetRatio(mDefaultHighRatio.vector2Value) + ")", mDefaultHighRatio.vector2Value);
                mDefaultLowRatio.vector2Value = EditorGUILayout.Vector2Field("   Low Ratio (" + mScript.GetRatio(mDefaultLowRatio.vector2Value) + ")", mDefaultLowRatio.vector2Value);
                mDefaultScale.vector3Value = EditorGUILayout.Vector3Field("   Scale", mDefaultScale.vector3Value);

                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Low Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                {
                    mLowestScale.vector3Value = mRectTransform.localScale;
                }
                GUILayout.EndHorizontal();
                mLowestRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mLowestRatio.vector2Value) + ")", mLowestRatio.vector2Value);
                mLowestScale.vector3Value = EditorGUILayout.Vector3Field("   Scale", mLowestScale.vector3Value);
            }
            else
            {
                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Highest Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                {
                    mHighestScale.vector3Value = mRectTransform.localScale;
                }
                GUILayout.EndHorizontal();
                mHighestRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mHighestRatio.vector2Value) + ")", mHighestRatio.vector2Value);
                mHighestScale.vector3Value = EditorGUILayout.Vector3Field("   Scale", mHighestScale.vector3Value);

                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("High Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                {
                    mHighScale.vector3Value = mRectTransform.localScale;
                }
                GUILayout.EndHorizontal();
                mHighRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mHighRatio.vector2Value) + ")", mHighRatio.vector2Value);
                mHighScale.vector3Value = EditorGUILayout.Vector3Field("   Scale", mHighScale.vector3Value);

                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Default Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                {
                    mDefaultScale.vector3Value = mRectTransform.localScale;
                }
                GUILayout.EndHorizontal();
                mDefaultHighRatio.vector2Value = EditorGUILayout.Vector2Field("   High Ratio (" + mScript.GetRatio(mDefaultHighRatio.vector2Value) + ")", mDefaultHighRatio.vector2Value);
                mDefaultLowRatio.vector2Value = EditorGUILayout.Vector2Field("   Low Ratio (" + mScript.GetRatio(mDefaultLowRatio.vector2Value) + ")", mDefaultLowRatio.vector2Value);
                mDefaultScale.vector3Value = EditorGUILayout.Vector3Field("   Scale", mDefaultScale.vector3Value);

                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Low Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                {
                    mLowScale.vector3Value = mRectTransform.localScale;
                }
                GUILayout.EndHorizontal();
                mLowRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mLowRatio.vector2Value) + ")", mLowRatio.vector2Value);
                mLowScale.vector3Value = EditorGUILayout.Vector3Field("   Scale", mLowScale.vector3Value);

                EditorGUILayout.LabelField(string.Empty);
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Lowest Aspect Ratio", EditorStyles.boldLabel);
                if (GUILayout.Button("Use Current Scale", GUILayout.MaxWidth(135)))
                {
                    mLowestScale.vector3Value = mRectTransform.localScale;
                }
                GUILayout.EndHorizontal();
                mLowestRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio (" + mScript.GetRatio(mLowestRatio.vector2Value) + ")", mLowestRatio.vector2Value);
                mLowestScale.vector3Value = EditorGUILayout.Vector3Field("   Scale", mLowestScale.vector3Value);
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

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}

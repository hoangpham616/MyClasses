/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIGridLayoutGroup (version 2.0)
 */

#pragma warning disable 0414
#pragma warning disable 0649

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;
using System;

namespace MyClasses.UI
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class MyUGUIGridLayoutGroup : MonoBehaviour
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
        private Vector2 mHighestCellSize;
        [SerializeField]
        private Vector2 mHighestSpacing;
        [SerializeField]
        private GridLayoutGroup.Constraint mHighestConstraint;
        [SerializeField]
        private int mHighestConstraintCount;

        [SerializeField]
        private Vector2 mHighRatio = new Vector2(18, 9);
        [SerializeField]
        private Vector2 mHighCellSize;
        [SerializeField]
        private Vector2 mHighSpacing;
        [SerializeField]
        private GridLayoutGroup.Constraint mHighConstraint;
        [SerializeField]
        private int mHighConstraintCount;

        [SerializeField]
        private Vector2 mDefaultCellSize;
        [SerializeField]
        private Vector2 mDefaultSpacing;
        [SerializeField]
        private GridLayoutGroup.Constraint mDefaultConstraint;
        [SerializeField]
        private int mDefaultConstraintCount;

        [SerializeField]
        private Vector2 mLowRatio = new Vector2(16, 10);
        [SerializeField]
        private Vector2 mLowCellSize;
        [SerializeField]
        private Vector2 mLowSpacing;
        [SerializeField]
        private GridLayoutGroup.Constraint mLowConstraint;
        [SerializeField]
        private int mLowConstraintCount;

        [SerializeField]
        private Vector2 mLowestRatio = new Vector2(4.2f, 3);
        [SerializeField]
        private Vector2 mLowestCellSize;
        [SerializeField]
        private Vector2 mLowestSpacing;
        [SerializeField]
        private GridLayoutGroup.Constraint mLowestConstraint;
        [SerializeField]
        private int mLowestConstraintCount;

        [SerializeField]
        private ScreenOrientation mDeviceOrientation;

        [SerializeField]
        private bool mIsCurrentRefreshLoaded = false;

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

        public bool IsCurrentRefreshLoaded
        {
            get { return mIsCurrentRefreshLoaded; }
            set { mIsCurrentRefreshLoaded = value; }
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
            mRefreshFrequency = ERefreshFrequency.EverytimeActive;
#endif

            if (mFrequency == EFrequency.OneTimeOnly)
            {
                Layout();
            }
        }

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            if (mFrequency == EFrequency.EverytimeActive)
            {
                Layout();
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
                Layout();
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Layout.
        /// </summary>
        public void Layout()
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
                        _Layout1();
                    }
                    break;
                case ELevel.Three:
                    {
                        _Layout3();
                    }
                    break;
                case ELevel.Five:
                    {
                        _Layout5();
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
        /// Layout for One Level. 
        /// </summary>
        private void _Layout1()
        {
            GridLayoutGroup gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
            gridLayoutGroup.cellSize = mDefaultCellSize;
            gridLayoutGroup.spacing = mDefaultSpacing;
            gridLayoutGroup.constraint = mDefaultConstraint;
            gridLayoutGroup.constraintCount = mDefaultConstraintCount;
        }

        /// <summary>
        /// Layout for Three Levels. 
        /// </summary>
        private void _Layout3()
        {
            double curRatio = GetCurrentRatio();

            if (curRatio > GetRatio(mHighestRatio))
            {
                GridLayoutGroup gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
                gridLayoutGroup.cellSize = mHighestCellSize;
                gridLayoutGroup.spacing = mHighestSpacing;
                gridLayoutGroup.constraint = mHighestConstraint;
                gridLayoutGroup.constraintCount = mHighestConstraintCount;
            }
            else if (curRatio < GetRatio(mLowestRatio))
            {
                GridLayoutGroup gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
                gridLayoutGroup.cellSize = mLowestCellSize;
                gridLayoutGroup.spacing = mLowestSpacing;
                gridLayoutGroup.constraint = mLowestConstraint;
                gridLayoutGroup.constraintCount = mLowestConstraintCount;
            }
            else
            {
                GridLayoutGroup gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
                gridLayoutGroup.cellSize = mDefaultCellSize;
                gridLayoutGroup.spacing = mDefaultSpacing;
                gridLayoutGroup.constraint = mDefaultConstraint;
                gridLayoutGroup.constraintCount = mDefaultConstraintCount;
            }
        }

        /// <summary>
        /// Layout for Five Levels. 
        /// </summary>
        private void _Layout5()
        {
            double curRatio = GetCurrentRatio();

            if (curRatio >= GetRatio(mHighestRatio))
            {
                GridLayoutGroup gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
                gridLayoutGroup.cellSize = mHighestCellSize;
                gridLayoutGroup.spacing = mHighestSpacing;
                gridLayoutGroup.constraint = mHighestConstraint;
                gridLayoutGroup.constraintCount = mHighestConstraintCount;
            }
            else if (curRatio >= GetRatio(mHighRatio))
            {
                GridLayoutGroup gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
                gridLayoutGroup.cellSize = mHighCellSize;
                gridLayoutGroup.spacing = mHighSpacing;
                gridLayoutGroup.constraint = mHighConstraint;
                gridLayoutGroup.constraintCount = mHighConstraintCount;
            }
            else if (curRatio <= GetRatio(mLowestRatio))
            {
                GridLayoutGroup gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
                gridLayoutGroup.cellSize = mLowestCellSize;
                gridLayoutGroup.spacing = mLowestSpacing;
                gridLayoutGroup.constraint = mLowestConstraint;
                gridLayoutGroup.constraintCount = mLowestConstraintCount;
            }
            else if (curRatio <= GetRatio(mLowRatio))
            {
                GridLayoutGroup gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
                gridLayoutGroup.cellSize = mLowCellSize;
                gridLayoutGroup.spacing = mLowSpacing;
                gridLayoutGroup.constraint = mLowConstraint;
                gridLayoutGroup.constraintCount = mLowConstraintCount;
            }
            else
            {
                GridLayoutGroup gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
                gridLayoutGroup.cellSize = mDefaultCellSize;
                gridLayoutGroup.spacing = mDefaultSpacing;
                gridLayoutGroup.constraint = mDefaultConstraint;
                gridLayoutGroup.constraintCount = mDefaultConstraintCount;
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

    [CustomEditor(typeof(MyUGUIGridLayoutGroup))]
    public class MyUGUIGridLayoutGroupEditor : Editor
    {
        private MyUGUIGridLayoutGroup mScript;
        private GridLayoutGroup mGridLayoutGroup;

        private SerializedProperty mOrientation;

        private SerializedProperty mHighestRatio;
        private SerializedProperty mHighestCellSize;
        private SerializedProperty mHighestSpacing;
        private SerializedProperty mHighestConstraint;
        private SerializedProperty mHighestConstraintCount;

        private SerializedProperty mHighRatio;
        private SerializedProperty mHighCellSize;
        private SerializedProperty mHighSpacing;
        private SerializedProperty mHighConstraint;
        private SerializedProperty mHighConstraintCount;

        private SerializedProperty mDefaultCellSize;
        private SerializedProperty mDefaultSpacing;
        private SerializedProperty mDefaultConstraint;
        private SerializedProperty mDefaultConstraintCount;

        private SerializedProperty mLowRatio;
        private SerializedProperty mLowCellSize;
        private SerializedProperty mLowSpacing;
        private SerializedProperty mLowConstraint;
        private SerializedProperty mLowConstraintCount;

        private SerializedProperty mLowestRatio;
        private SerializedProperty mLowestCellSize;
        private SerializedProperty mLowestSpacing;
        private SerializedProperty mLowestConstraint;
        private SerializedProperty mLowestConstraintCount;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUIGridLayoutGroup)target;

            mGridLayoutGroup = mScript.gameObject.GetComponent<GridLayoutGroup>();

            mOrientation = serializedObject.FindProperty("mOrientation");

            mHighestRatio = serializedObject.FindProperty("mHighestRatio");
            mHighestCellSize = serializedObject.FindProperty("mHighestCellSize");
            mHighestSpacing = serializedObject.FindProperty("mHighestSpacing");
            mHighestConstraint = serializedObject.FindProperty("mHighestConstraint");
            mHighestConstraintCount = serializedObject.FindProperty("mHighestConstraintCount");

            mHighRatio = serializedObject.FindProperty("mHighRatio");
            mHighCellSize = serializedObject.FindProperty("mHighCellSize");
            mHighSpacing = serializedObject.FindProperty("mHighSpacing");
            mHighConstraint = serializedObject.FindProperty("mHighConstraint");
            mHighConstraintCount = serializedObject.FindProperty("mHighConstraintCount");

            mDefaultCellSize = serializedObject.FindProperty("mDefaultCellSize");
            mDefaultSpacing = serializedObject.FindProperty("mDefaultSpacing");
            mDefaultConstraint = serializedObject.FindProperty("mDefaultConstraint");
            mDefaultConstraintCount = serializedObject.FindProperty("mDefaultConstraintCount");

            mLowRatio = serializedObject.FindProperty("mLowRatio");
            mLowCellSize = serializedObject.FindProperty("mLowCellSize");
            mLowSpacing = serializedObject.FindProperty("mLowSpacing");
            mLowConstraint = serializedObject.FindProperty("mLowConstraint");
            mLowConstraintCount = serializedObject.FindProperty("mLowConstraintCount");

            mLowestRatio = serializedObject.FindProperty("mLowestRatio");
            mLowestCellSize = serializedObject.FindProperty("mLowestCellSize");
            mLowestSpacing = serializedObject.FindProperty("mLowestSpacing");
            mLowestConstraint = serializedObject.FindProperty("mLowestConstraint");
            mLowestConstraintCount = serializedObject.FindProperty("mLowestConstraintCount");

            if (!mScript.IsCurrentRefreshLoaded)
            {
                mScript.IsCurrentRefreshLoaded = true;

                mHighestCellSize.vector2Value = mGridLayoutGroup.cellSize;
                mHighestSpacing.vector2Value = mGridLayoutGroup.spacing;
                mHighestConstraint.enumValueIndex = (int)mGridLayoutGroup.constraint;
                mHighestConstraintCount.intValue = mGridLayoutGroup.constraintCount;

                mHighCellSize.vector2Value = mGridLayoutGroup.cellSize;
                mHighSpacing.vector2Value = mGridLayoutGroup.spacing;
                mHighConstraint.enumValueIndex = (int)mGridLayoutGroup.constraint;
                mHighConstraintCount.intValue = mGridLayoutGroup.constraintCount;

                mDefaultCellSize.vector2Value = mGridLayoutGroup.cellSize;
                mDefaultSpacing.vector2Value = mGridLayoutGroup.spacing;
                mDefaultConstraint.enumValueIndex = (int)mGridLayoutGroup.constraint;
                mDefaultConstraintCount.intValue = mGridLayoutGroup.constraintCount;

                mLowCellSize.vector2Value = mGridLayoutGroup.cellSize;
                mLowSpacing.vector2Value = mGridLayoutGroup.spacing;
                mLowConstraint.enumValueIndex = (int)mGridLayoutGroup.constraint;
                mLowConstraintCount.intValue = mGridLayoutGroup.constraintCount;

                mLowestCellSize.vector2Value = mGridLayoutGroup.cellSize;
                mLowestSpacing.vector2Value = mGridLayoutGroup.spacing;
                mLowestConstraint.enumValueIndex = (int)mGridLayoutGroup.constraint;
                mLowestConstraintCount.intValue = mGridLayoutGroup.constraintCount;

                serializedObject.ApplyModifiedProperties();
            }
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUGUIGridLayoutGroup), false);

            System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo getSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Vector2 resolution = (Vector2)getSizeOfMainGameView.Invoke(null, null);
            ScreenOrientation deviceOrientation = resolution.x > resolution.y ? ScreenOrientation.Landscape : ScreenOrientation.Portrait;

            serializedObject.Update();

            EditorGUILayout.LabelField("Orientation", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("   Screen Orientation: " + (deviceOrientation == ScreenOrientation.Landscape ? "Landscape" : "Portrait"));
            if ((mOrientation.enumValueIndex == (int)MyUGUIGridLayoutGroup.EOrientation.Both)
                || (mOrientation.enumValueIndex == (int)MyUGUIGridLayoutGroup.EOrientation.Portrait && deviceOrientation == ScreenOrientation.Portrait)
                || (mOrientation.enumValueIndex == (int)MyUGUIGridLayoutGroup.EOrientation.Landscape && deviceOrientation == ScreenOrientation.Landscape))
            {
                EditorGUILayout.LabelField("   Status: Valid");
            }
            else
            {
                EditorGUILayout.LabelField("   Status: Invalid");
            }

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Refresh Mode", EditorStyles.boldLabel);
            mOrientation.enumValueIndex = (int)(MyUGUIGridLayoutGroup.EOrientation)EditorGUILayout.EnumPopup("   Orientation", (MyUGUIGridLayoutGroup.EOrientation)mOrientation.enumValueIndex);
            mScript.Level = (MyUGUIGridLayoutGroup.ELevel)EditorGUILayout.EnumPopup("   Level", mScript.Level);
            mScript.Frequency = (MyUGUIGridLayoutGroup.EFrequency)EditorGUILayout.EnumPopup("   Frequency", mScript.Frequency);
            if (mOrientation.enumValueIndex != (int)MyUGUIGridLayoutGroup.EOrientation.Both)
            {
                mScript.Frequency = MyUGUIGridLayoutGroup.EFrequency.EverytimeActive;
            }
            mScript.RoundingRatio = (MyUGUIGridLayoutGroup.ERoundingRatio)EditorGUILayout.EnumPopup("   Rounding Ratio", mScript.RoundingRatio);

            switch (mScript.Level)
            {
                case MyUGUIGridLayoutGroup.ELevel.One:
                    {
                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Any Aspect Ratio", EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Layout", GUILayout.MaxWidth(135)))
                        {
                            mDefaultCellSize.vector2Value = mGridLayoutGroup.cellSize;
                            mDefaultSpacing.vector2Value = mGridLayoutGroup.spacing;
                            mDefaultConstraint.enumValueIndex = (int)mGridLayoutGroup.constraint;
                            mDefaultConstraintCount.intValue = mGridLayoutGroup.constraintCount;
                        }
                        GUILayout.EndHorizontal();
                        mDefaultCellSize.vector2Value = EditorGUILayout.Vector2Field("   Cell Size", mDefaultCellSize.vector2Value);
                        mDefaultSpacing.vector2Value = EditorGUILayout.Vector2Field("   Spacing", mDefaultSpacing.vector2Value);
                        mDefaultConstraint.enumValueIndex = (int)(GridLayoutGroup.Constraint)EditorGUILayout.EnumPopup("   Constraint", (GridLayoutGroup.Constraint)mDefaultConstraint.enumValueIndex);
                        if (mDefaultConstraint.enumValueIndex != (int)GridLayoutGroup.Constraint.Flexible)
                        {
                            mDefaultConstraintCount.intValue = EditorGUILayout.IntField("   Constraint Count", mDefaultConstraintCount.intValue);
                        }
                    }
                    break;
                case MyUGUIGridLayoutGroup.ELevel.Three:
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
                        if (GUILayout.Button("Use Current Layout", GUILayout.MaxWidth(135)))
                        {
                            mHighestCellSize.vector2Value = mGridLayoutGroup.cellSize;
                            mHighestSpacing.vector2Value = mGridLayoutGroup.spacing;
                            mHighestConstraint.enumValueIndex = (int)mGridLayoutGroup.constraint;
                            mHighestConstraintCount.intValue = mGridLayoutGroup.constraintCount;
                        }
                        GUILayout.EndHorizontal();
                        mHighestRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio >= (" + mScript.GetRatio(mHighestRatio.vector2Value) + ")", mHighestRatio.vector2Value);
                        EditorGUILayout.LabelField(string.Empty);
                        mHighestCellSize.vector2Value = EditorGUILayout.Vector2Field("   Cell Size", mHighestCellSize.vector2Value);
                        mHighestSpacing.vector2Value = EditorGUILayout.Vector2Field("   Spacing", mHighestSpacing.vector2Value);
                        mHighestConstraint.enumValueIndex = (int)(GridLayoutGroup.Constraint)EditorGUILayout.EnumPopup("   Constraint", (GridLayoutGroup.Constraint)mHighestConstraint.enumValueIndex);
                        if (mHighestConstraint.enumValueIndex != (int)GridLayoutGroup.Constraint.Flexible)
                        {
                            mHighestConstraintCount.intValue = EditorGUILayout.IntField("   Constraint Count", mHighestConstraintCount.intValue);
                        }

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Medium Aspect Ratio" + (ratioLevel == 0 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Layout", GUILayout.MaxWidth(135)))
                        {
                            mDefaultCellSize.vector2Value = mGridLayoutGroup.cellSize;
                            mDefaultSpacing.vector2Value = mGridLayoutGroup.spacing;
                            mDefaultConstraint.enumValueIndex = (int)mGridLayoutGroup.constraint;
                            mDefaultConstraintCount.intValue = mGridLayoutGroup.constraintCount;
                        }
                        GUILayout.EndHorizontal();
                        mDefaultCellSize.vector2Value = EditorGUILayout.Vector2Field("   Cell Size", mDefaultCellSize.vector2Value);
                        mDefaultSpacing.vector2Value = EditorGUILayout.Vector2Field("   Spacing", mDefaultSpacing.vector2Value);
                        mDefaultConstraint.enumValueIndex = (int)(GridLayoutGroup.Constraint)EditorGUILayout.EnumPopup("   Constraint", (GridLayoutGroup.Constraint)mDefaultConstraint.enumValueIndex);
                        if (mDefaultConstraint.enumValueIndex != (int)GridLayoutGroup.Constraint.Flexible)
                        {
                            mDefaultConstraintCount.intValue = EditorGUILayout.IntField("   Constraint Count", mDefaultConstraintCount.intValue);
                        }

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Low Aspect Ratio" + (ratioLevel == -1 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Layout", GUILayout.MaxWidth(135)))
                        {
                            mLowestCellSize.vector2Value = mGridLayoutGroup.cellSize;
                            mLowestSpacing.vector2Value = mGridLayoutGroup.spacing;
                            mLowestConstraint.enumValueIndex = (int)mGridLayoutGroup.constraint;
                            mLowestConstraintCount.intValue = mGridLayoutGroup.constraintCount;
                        }
                        GUILayout.EndHorizontal();
                        mLowestRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio <= (" + mScript.GetRatio(mLowestRatio.vector2Value) + ")", mLowestRatio.vector2Value);
                        EditorGUILayout.LabelField(string.Empty);
                        mLowestCellSize.vector2Value = EditorGUILayout.Vector2Field("   Cell Size", mLowestCellSize.vector2Value);
                        mLowestSpacing.vector2Value = EditorGUILayout.Vector2Field("   Spacing", mLowestSpacing.vector2Value);
                        mLowestConstraint.enumValueIndex = (int)(GridLayoutGroup.Constraint)EditorGUILayout.EnumPopup("   Constraint", (GridLayoutGroup.Constraint)mLowestConstraint.enumValueIndex);
                        if (mLowestConstraint.enumValueIndex != (int)GridLayoutGroup.Constraint.Flexible)
                        {
                            mLowestConstraintCount.intValue = EditorGUILayout.IntField("   Constraint Count", mLowestConstraintCount.intValue);
                        }
                    }
                    break;
                case MyUGUIGridLayoutGroup.ELevel.Five:
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
                        if (GUILayout.Button("Use Current Layout", GUILayout.MaxWidth(135)))
                        {
                            mHighestCellSize.vector2Value = mGridLayoutGroup.cellSize;
                            mHighestSpacing.vector2Value = mGridLayoutGroup.spacing;
                            mHighestConstraint.enumValueIndex = (int)mGridLayoutGroup.constraint;
                            mHighestConstraintCount.intValue = mGridLayoutGroup.constraintCount;
                        }
                        GUILayout.EndHorizontal();
                        mHighestRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio >= (" + mScript.GetRatio(mHighestRatio.vector2Value) + ")", mHighestRatio.vector2Value);
                        EditorGUILayout.LabelField(string.Empty);
                        mHighestCellSize.vector2Value = EditorGUILayout.Vector2Field("   Cell Size", mHighestCellSize.vector2Value);
                        mHighestSpacing.vector2Value = EditorGUILayout.Vector2Field("   Spacing", mHighestSpacing.vector2Value);
                        mHighestConstraint.enumValueIndex = (int)(GridLayoutGroup.Constraint)EditorGUILayout.EnumPopup("   Constraint", (GridLayoutGroup.Constraint)mHighestConstraint.enumValueIndex);
                        if (mHighestConstraint.enumValueIndex != (int)GridLayoutGroup.Constraint.Flexible)
                        {
                            mHighestConstraintCount.intValue = EditorGUILayout.IntField("   Constraint Count", mHighestConstraintCount.intValue);
                        }

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("High Aspect Ratio" + (ratioLevel == 1 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Layout", GUILayout.MaxWidth(135)))
                        {
                            mHighCellSize.vector2Value = mGridLayoutGroup.cellSize;
                            mHighSpacing.vector2Value = mGridLayoutGroup.spacing;
                            mHighConstraint.enumValueIndex = (int)mGridLayoutGroup.constraint;
                            mHighConstraintCount.intValue = mGridLayoutGroup.constraintCount;
                        }
                        GUILayout.EndHorizontal();
                        mHighRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio >= (" + mScript.GetRatio(mHighRatio.vector2Value) + ")", mHighRatio.vector2Value);
                        EditorGUILayout.LabelField(string.Empty);
                        mHighCellSize.vector2Value = EditorGUILayout.Vector2Field("   Cell Size", mHighCellSize.vector2Value);
                        mHighSpacing.vector2Value = EditorGUILayout.Vector2Field("   Spacing", mHighSpacing.vector2Value);
                        mHighConstraint.enumValueIndex = (int)(GridLayoutGroup.Constraint)EditorGUILayout.EnumPopup("   Constraint", (GridLayoutGroup.Constraint)mHighConstraint.enumValueIndex);
                        if (mHighConstraint.enumValueIndex != (int)GridLayoutGroup.Constraint.Flexible)
                        {
                            mHighConstraintCount.intValue = EditorGUILayout.IntField("   Constraint Count", mHighConstraintCount.intValue);
                        }

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Medium Aspect Ratio" + (ratioLevel == 0 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Layout", GUILayout.MaxWidth(135)))
                        {
                            mDefaultCellSize.vector2Value = mGridLayoutGroup.cellSize;
                            mDefaultSpacing.vector2Value = mGridLayoutGroup.spacing;
                            mDefaultConstraint.enumValueIndex = (int)mGridLayoutGroup.constraint;
                            mDefaultConstraintCount.intValue = mGridLayoutGroup.constraintCount;
                        }
                        GUILayout.EndHorizontal();
                        EditorGUILayout.LabelField(string.Empty);
                        mDefaultCellSize.vector2Value = EditorGUILayout.Vector2Field("   Cell Size", mDefaultCellSize.vector2Value);
                        mDefaultSpacing.vector2Value = EditorGUILayout.Vector2Field("   Spacing", mDefaultSpacing.vector2Value);
                        mDefaultConstraint.enumValueIndex = (int)(GridLayoutGroup.Constraint)EditorGUILayout.EnumPopup("   Constraint", (GridLayoutGroup.Constraint)mDefaultConstraint.enumValueIndex);
                        if (mDefaultConstraint.enumValueIndex != (int)GridLayoutGroup.Constraint.Flexible)
                        {
                            mDefaultConstraintCount.intValue = EditorGUILayout.IntField("   Constraint Count", mDefaultConstraintCount.intValue);
                        }

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Low Aspect Ratio" + (ratioLevel == -1 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Layout", GUILayout.MaxWidth(135)))
                        {
                            mLowCellSize.vector2Value = mGridLayoutGroup.cellSize;
                            mLowSpacing.vector2Value = mGridLayoutGroup.spacing;
                            mLowConstraint.enumValueIndex = (int)mGridLayoutGroup.constraint;
                            mLowConstraintCount.intValue = mGridLayoutGroup.constraintCount;
                        }
                        GUILayout.EndHorizontal();
                        mLowRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio <= (" + mScript.GetRatio(mLowRatio.vector2Value) + ")", mLowRatio.vector2Value);
                        EditorGUILayout.LabelField(string.Empty);
                        mLowCellSize.vector2Value = EditorGUILayout.Vector2Field("   Cell Size", mLowCellSize.vector2Value);
                        mLowSpacing.vector2Value = EditorGUILayout.Vector2Field("   Spacing", mLowSpacing.vector2Value);
                        mLowConstraint.enumValueIndex = (int)(GridLayoutGroup.Constraint)EditorGUILayout.EnumPopup("   Constraint", (GridLayoutGroup.Constraint)mLowConstraint.enumValueIndex);
                        if (mLowConstraint.enumValueIndex != (int)GridLayoutGroup.Constraint.Flexible)
                        {
                            mLowConstraintCount.intValue = EditorGUILayout.IntField("   Constraint Count", mLowConstraintCount.intValue);
                        }

                        EditorGUILayout.LabelField(string.Empty);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Lowest Aspect Ratio" + (ratioLevel == -2 ? " (Current)" : string.Empty), EditorStyles.boldLabel);
                        if (GUILayout.Button("Use Current Layout", GUILayout.MaxWidth(135)))
                        {
                            mLowestCellSize.vector2Value = mGridLayoutGroup.cellSize;
                            mLowestSpacing.vector2Value = mGridLayoutGroup.spacing;
                            mLowestConstraint.enumValueIndex = (int)mGridLayoutGroup.constraint;
                            mLowestConstraintCount.intValue = mGridLayoutGroup.constraintCount;
                        }
                        GUILayout.EndHorizontal();
                        mLowestRatio.vector2Value = EditorGUILayout.Vector2Field("   Ratio <= (" + mScript.GetRatio(mLowestRatio.vector2Value) + ")", mLowestRatio.vector2Value);
                        EditorGUILayout.LabelField(string.Empty);
                        mLowestCellSize.vector2Value = EditorGUILayout.Vector2Field("   Cell Size", mLowestCellSize.vector2Value);
                        mLowestSpacing.vector2Value = EditorGUILayout.Vector2Field("   Spacing", mLowestSpacing.vector2Value);
                        mLowestConstraint.enumValueIndex = (int)(GridLayoutGroup.Constraint)EditorGUILayout.EnumPopup("   Constraint", (GridLayoutGroup.Constraint)mLowestConstraint.enumValueIndex);
                        if (mLowestConstraint.enumValueIndex != (int)GridLayoutGroup.Constraint.Flexible)
                        {
                            mLowestConstraintCount.intValue = EditorGUILayout.IntField("   Constraint Count", mLowestConstraintCount.intValue);
                        }
                    }
                    break;
            }

            EditorGUILayout.LabelField(string.Empty);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Aspect Ratio (" + mScript.GetCurrentRatio() + ")", EditorStyles.boldLabel);
            if (GUILayout.Button("Layout Now", GUILayout.MaxWidth(135)))
            {
                mScript.Layout();
            }
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}
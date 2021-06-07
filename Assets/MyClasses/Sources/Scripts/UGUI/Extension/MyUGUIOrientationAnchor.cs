/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIOrientationAnchor (version 2.2)
 */

#pragma warning disable 0414
#pragma warning disable 0649

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace MyClasses.UI
{
    public class MyUGUIOrientationAnchor : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private float mDelayAnchorTime;

        [SerializeField]
        private Vector2 mPortraitPivot;
        [SerializeField]
        private Vector2 mPortraitAnchorMin;
        [SerializeField]
        private Vector2 mPortraitAnchorMax;
        [SerializeField]
        private Vector2 mPortraitOffsetMin;
        [SerializeField]
        private Vector2 mPortraitOffsetMax;

        [SerializeField]
        private Vector2 mLandscapePivot;
        [SerializeField]
        private Vector2 mLandscapeAnchorMin;
        [SerializeField]
        private Vector2 mLandscapeAnchorMax;
        [SerializeField]
        private Vector2 mLandscapeOffsetMin;
        [SerializeField]
        private Vector2 mLandscapeOffsetMax;

        [SerializeField]
        private bool mIsCurrentAnchorLoaded = false;

        [SerializeField]
        private ScreenOrientation mDeviceOrientation;

        #endregion

        #region ----- Property -----

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
            Anchor();
        }

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            Anchor();
        }

        /// <summary>
        /// Update.
        /// </summary>
        void Update()
        {
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

        public void Anchor()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
                System.Reflection.MethodInfo getSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                Vector2 resolution = (Vector2)getSizeOfMainGameView.Invoke(null, null);
                mDeviceOrientation = resolution.x > resolution.y ? ScreenOrientation.Landscape : ScreenOrientation.Portrait;
            }
            else
            {
                mDeviceOrientation = Screen.width > Screen.height ? ScreenOrientation.Landscape : ScreenOrientation.Portrait;
            }
#else
            mDeviceOrientation = Screen.orientation;
#endif

            if (mDeviceOrientation == ScreenOrientation.Landscape || mDeviceOrientation == ScreenOrientation.LandscapeLeft || mDeviceOrientation == ScreenOrientation.LandscapeRight)
            {
                RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
                rectTrans.pivot = mLandscapePivot;
                rectTrans.anchorMin = mLandscapeAnchorMin;
                rectTrans.anchorMax = mLandscapeAnchorMax;
                rectTrans.offsetMin = mLandscapeOffsetMin;
                rectTrans.offsetMax = mLandscapeOffsetMax;
            }
            else if (mDeviceOrientation == ScreenOrientation.Portrait || mDeviceOrientation == ScreenOrientation.PortraitUpsideDown)
            {
                RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
                rectTrans.pivot = mPortraitPivot;
                rectTrans.anchorMin = mPortraitAnchorMin;
                rectTrans.anchorMax = mPortraitAnchorMax;
                rectTrans.offsetMin = mPortraitOffsetMin;
                rectTrans.offsetMax = mPortraitOffsetMax;
            }
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyUGUIOrientationAnchor))]
    public class MyUGUIOrientationAnchorEditor : Editor
    {
        private MyUGUIOrientationAnchor mScript;
        private RectTransform mRectTransform;

        private SerializedProperty mDelayAnchorTime;

        private SerializedProperty mPortraitPivot;
        private SerializedProperty mPortraitAnchorMin;
        private SerializedProperty mPortraitAnchorMax;
        private SerializedProperty mPortraitOffsetMin;
        private SerializedProperty mPortraitOffsetMax;

        private SerializedProperty mLandscapePivot;
        private SerializedProperty mLandscapeAnchorMin;
        private SerializedProperty mLandscapeAnchorMax;
        private SerializedProperty mLandscapeOffsetMin;
        private SerializedProperty mLandscapeOffsetMax;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUIOrientationAnchor)target;

            mRectTransform = mScript.gameObject.GetComponent<RectTransform>();
            if (mRectTransform == null)
            {
                Debug.LogError("[" + typeof(MyUGUIOrientationAnchorEditor).Name + "] OnEnable(): Could not find RectTransform component.");
            }

            mDelayAnchorTime = serializedObject.FindProperty("mDelayAnchorTime");

            mPortraitPivot = serializedObject.FindProperty("mPortraitPivot");
            mPortraitAnchorMin = serializedObject.FindProperty("mPortraitAnchorMin");
            mPortraitAnchorMax = serializedObject.FindProperty("mPortraitAnchorMax");
            mPortraitOffsetMin = serializedObject.FindProperty("mPortraitOffsetMin");
            mPortraitOffsetMax = serializedObject.FindProperty("mPortraitOffsetMax");

            mLandscapePivot = serializedObject.FindProperty("mLandscapePivot");
            mLandscapeAnchorMin = serializedObject.FindProperty("mLandscapeAnchorMin");
            mLandscapeAnchorMax = serializedObject.FindProperty("mLandscapeAnchorMax");
            mLandscapeOffsetMin = serializedObject.FindProperty("mLandscapeOffsetMin");
            mLandscapeOffsetMax = serializedObject.FindProperty("mLandscapeOffsetMax");

            if (!mScript.IsCurrentAnchorLoaded)
            {
                mScript.IsCurrentAnchorLoaded = true;

                mPortraitPivot.vector2Value = mRectTransform.pivot;
                mPortraitAnchorMin.vector2Value = mRectTransform.anchorMin;
                mPortraitAnchorMax.vector2Value = mRectTransform.anchorMax;
                mPortraitOffsetMin.vector2Value = mRectTransform.offsetMin;
                mPortraitOffsetMax.vector2Value = mRectTransform.offsetMax;

                mLandscapePivot.vector2Value = mRectTransform.pivot;
                mLandscapeAnchorMin.vector2Value = mRectTransform.anchorMin;
                mLandscapeAnchorMax.vector2Value = mRectTransform.anchorMax;
                mLandscapeOffsetMin.vector2Value = mRectTransform.offsetMin;
                mLandscapeOffsetMax.vector2Value = mRectTransform.offsetMax;

                serializedObject.ApplyModifiedProperties();
            }
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUGUIOrientationAnchor), false);

            bool isPortrait = true;
            if (!Application.isPlaying)
            {
                System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
                System.Reflection.MethodInfo getSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                Vector2 resolution = (Vector2)getSizeOfMainGameView.Invoke(null, null);
                isPortrait = resolution.x < resolution.y;
            }
            else
            {
                isPortrait = Screen.width < Screen.height;
            }

            serializedObject.Update();

            EditorGUILayout.LabelField(string.Empty);
            mDelayAnchorTime.floatValue = EditorGUILayout.FloatField("Delay Anchor Time", mDelayAnchorTime.floatValue);

            EditorGUILayout.LabelField(string.Empty);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Portrait" + (isPortrait ? " (Current)" : string.Empty), EditorStyles.boldLabel);
            if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
            {
                mPortraitPivot.vector2Value = mRectTransform.pivot;
                mPortraitAnchorMin.vector2Value = mRectTransform.anchorMin;
                mPortraitAnchorMax.vector2Value = mRectTransform.anchorMax;
                mPortraitOffsetMin.vector2Value = mRectTransform.offsetMin;
                mPortraitOffsetMax.vector2Value = mRectTransform.offsetMax;
            }
            GUILayout.EndHorizontal();
            mPortraitPivot.vector2Value = EditorGUILayout.Vector2Field("   Pivot", mPortraitPivot.vector2Value);
            mPortraitAnchorMin.vector2Value = EditorGUILayout.Vector2Field("   Anchor Min", mPortraitAnchorMin.vector2Value);
            mPortraitAnchorMax.vector2Value = EditorGUILayout.Vector2Field("   Anchor Max", mPortraitAnchorMax.vector2Value);
            mPortraitOffsetMin.vector2Value = EditorGUILayout.Vector2Field("   Offset Min", mPortraitOffsetMin.vector2Value);
            mPortraitOffsetMax.vector2Value = EditorGUILayout.Vector2Field("   Offset Max", mPortraitOffsetMax.vector2Value);

            EditorGUILayout.LabelField(string.Empty);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Landscape" + (!isPortrait ? " (Current)" : string.Empty), EditorStyles.boldLabel);
            if (GUILayout.Button("Use Current Anchor", GUILayout.MaxWidth(135)))
            {
                mLandscapePivot.vector2Value = mRectTransform.pivot;
                mLandscapeAnchorMin.vector2Value = mRectTransform.anchorMin;
                mLandscapeAnchorMax.vector2Value = mRectTransform.anchorMax;
                mLandscapeOffsetMin.vector2Value = mRectTransform.offsetMin;
                mLandscapeOffsetMax.vector2Value = mRectTransform.offsetMax;
            }
            GUILayout.EndHorizontal();
            mLandscapePivot.vector2Value = EditorGUILayout.Vector2Field("   Pivot", mLandscapePivot.vector2Value);
            mLandscapeAnchorMin.vector2Value = EditorGUILayout.Vector2Field("   Anchor Min", mLandscapeAnchorMin.vector2Value);
            mLandscapeAnchorMax.vector2Value = EditorGUILayout.Vector2Field("   Anchor Max", mLandscapeAnchorMax.vector2Value);
            mLandscapeOffsetMin.vector2Value = EditorGUILayout.Vector2Field("   Offset Min", mLandscapeOffsetMin.vector2Value);
            mLandscapeOffsetMax.vector2Value = EditorGUILayout.Vector2Field("   Offset Max", mLandscapeOffsetMax.vector2Value);

            EditorGUILayout.LabelField(string.Empty);
            GUILayout.BeginHorizontal();
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
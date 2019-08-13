/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIOrientationAnchor (version 2.1)
 */

#pragma warning disable 0414

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
        private DeviceOrientation mDeviceOrientation;

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

        #region ----- MonoBehaviour Implemention -----

        /// <summary>
        /// Awake.
        /// </summary>
        void Awake()
        {
            Anchor(0);
        }

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            Anchor(0);
        }

        /// <summary>
        /// Update.
        /// </summary>
        void Update()
        {
            if (mDeviceOrientation != Input.deviceOrientation)
            {
                bool isNeedUpdate = (mDeviceOrientation == DeviceOrientation.LandscapeLeft || mDeviceOrientation == DeviceOrientation.LandscapeRight) && (mDeviceOrientation == DeviceOrientation.Portrait || mDeviceOrientation == DeviceOrientation.PortraitUpsideDown);
                mDeviceOrientation = Input.deviceOrientation;
                if (isNeedUpdate)
                {
                    Anchor(mDelayAnchorTime);
                }
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Anchor.
        /// </summary>
        public void Anchor(float delayTime)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                _Anchor();
            }
            else
#endif
            {
                MyCoroutiner.ExcuteAfterDelayTime("MyUGUIOrientationAnchor_Anchor", delayTime, () =>
                {
                    _Anchor();
                });
            }
        }

        #endregion


        #region ----- Private Method -----

        private void _Anchor()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
                System.Reflection.MethodInfo getSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                Vector2 resolution = (Vector2)getSizeOfMainGameView.Invoke(null, null);
                mDeviceOrientation = resolution.x > resolution.y ? DeviceOrientation.LandscapeLeft : DeviceOrientation.Portrait;
            }
            else
#endif
            {
                mDeviceOrientation = Input.deviceOrientation;
            }

            if (mDeviceOrientation == DeviceOrientation.LandscapeLeft || mDeviceOrientation == DeviceOrientation.LandscapeRight)
            {
                RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
                rectTrans.pivot = mPortraitPivot;
                rectTrans.anchorMin = mPortraitAnchorMin;
                rectTrans.anchorMax = mPortraitAnchorMax;
                rectTrans.offsetMin = mPortraitOffsetMin;
                rectTrans.offsetMax = mPortraitOffsetMax;
            }
            else
            {
                RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
                rectTrans.pivot = mLandscapePivot;
                rectTrans.anchorMin = mLandscapeAnchorMin;
                rectTrans.anchorMax = mLandscapeAnchorMax;
                rectTrans.offsetMin = mLandscapeOffsetMin;
                rectTrans.offsetMax = mLandscapeOffsetMax;
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

            serializedObject.Update();

            EditorGUILayout.LabelField(string.Empty);
            mDelayAnchorTime.floatValue = EditorGUILayout.FloatField("Delay Anchor Time", mDelayAnchorTime.floatValue);

            EditorGUILayout.LabelField(string.Empty);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Portrait", EditorStyles.boldLabel);
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
            EditorGUILayout.LabelField("Landscape", EditorStyles.boldLabel);
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
                mScript.Anchor(0);
            }
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}
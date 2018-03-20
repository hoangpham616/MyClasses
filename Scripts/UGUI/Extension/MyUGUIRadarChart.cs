/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIRadarChart (version 2.0)
 */

#pragma warning disable 0114

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MyClasses.UI
{
    [ExecuteInEditMode]
    public class MyUGUIRadarChart : Graphic
    {
        #region ----- Variable -----

        [HideInInspector]
        [SerializeField]
        private float mRadius = 100;
        [HideInInspector]
        [SerializeField]
        private float mRotation = 0;
        [HideInInspector]
        [SerializeField]
        private List<float> mListVerticle = new List<float>() { 1, 1, 1, 1, 1, 1 };

        #endregion

        #region ----- Property -----

        public float Radius
        {
            get { return mRadius; }
            set
            {
                mRadius = value;
                SetAllDirty();
            }
        }

        public float Rotation
        {
            get { return mRotation; }
            set
            {
                mRotation = Mathf.Clamp(value, 0, 360);
                SetAllDirty();
            }
        }

        public List<float> Verticles
        {
            get { return mListVerticle; }
        }

        #endregion

        #region ----- Implement MonoBehaviour -----

        /// <summary>
        /// Start.
        /// </summary>
        void Start()
        {
            mRadius = rectTransform.rect.width < rectTransform.rect.height ? rectTransform.rect.width : rectTransform.rect.height;
        }

        #endregion

        #region ----- Implement Graphic -----

        /// <summary>
        /// OnPopulateMesh.
        /// </summary>
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            int countVerticle = mListVerticle.Count;
            float degrees = 360 / countVerticle;
            float outer = -mRadius / 2;
            for (int i = 0; i < countVerticle; i++)
            {
                float rad = Mathf.Deg2Rad * (270 - (i * degrees) - mRotation);
                float cos = Mathf.Cos(rad);
                float sin = Mathf.Sin(rad);
                Vector3 pos = new Vector2(outer * cos, outer * sin) * mListVerticle[i];
                vh.AddVert(pos, color, Vector2.zero);
            }
            vh.AddVert(Vector3.zero, color, Vector2.zero);

            for (int i = 0; i < countVerticle; i++)
            {
                int prevVert = (i + countVerticle - 1) % countVerticle;
                int curVert = i;
                int nextVert = (i + 1) % countVerticle;
                vh.AddTriangle(countVerticle, curVert % countVerticle, prevVert % countVerticle);
                vh.AddTriangle(countVerticle, curVert % countVerticle, nextVert);
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Refresh.
        /// </summary>
        public void Refresh()
        {
            SetAllDirty();
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyUGUIRadarChart))]
    public class MyUGUIRadarChartEditor : Editor
    {
        private MyUGUIRadarChart mScript;
        private bool mIsVisible = true;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUIRadarChart)target;
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SerializedProperty radiusProperty = serializedObject.FindProperty("mRadius");
            radiusProperty.floatValue = EditorGUILayout.FloatField("Radius", radiusProperty.floatValue);

            SerializedProperty rotationProperty = serializedObject.FindProperty("mRotation");
            rotationProperty.floatValue = EditorGUILayout.Slider("Rotation", rotationProperty.floatValue, 0, 360);

            mIsVisible = EditorGUILayout.Foldout(mIsVisible, "Verticles");
            if (mIsVisible)
            {
                SerializedProperty verticlesProperty = serializedObject.FindProperty("mListVerticle");
                EditorGUI.indentLevel++;
                verticlesProperty.arraySize = EditorGUILayout.IntField("Size", verticlesProperty.arraySize);
                for (int i = 0; i < verticlesProperty.arraySize; i++)
                {
                    Rect elementPosition = GUILayoutUtility.GetRect(0f, 16f);
                    SerializedProperty elementProperty = verticlesProperty.GetArrayElementAtIndex(i);
                    EditorGUI.PropertyField(elementPosition, elementProperty);
                }
                EditorGUI.indentLevel--;
                for (int i = mScript.Verticles.Count - 1; i >= 0; i--)
                {
                    if (mScript.Verticles[i] < 0)
                    {
                        mScript.Verticles[i] = 0;
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}
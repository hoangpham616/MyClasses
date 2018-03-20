/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIPieChart (version 2.0)
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
    public class MyUGUIPieChart : Graphic
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
        private float mFill = 100;
        [HideInInspector]
        [SerializeField]
        private int mDensity = 100;
        [HideInInspector]
        [SerializeField]
        private bool mIsTransparency = false;
        [HideInInspector]
        [SerializeField]
        private List<Piece> mListPiece = new List<Piece>(){ new Piece(0.3f, Color.red), new Piece(0.4f, Color.green), new Piece(0.3f, Color.blue) };

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

        public float Fill
        {
            get { return mFill; }
            set
            { 
                mFill = Mathf.Clamp(value, 0, 100);
                SetAllDirty();
            }
        }

        public int Density
        {
            get { return mDensity; }
            set { mDensity = Mathf.Clamp(value, 1, 200); }
        }

        public bool IsTransparency
        {
            get { return mIsTransparency; }
            set { mIsTransparency = value; }
        }

        public List<Piece> Pieces
        {
            get { return mListPiece; }
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

            int countPiece = mListPiece.Count;
            int curPieceIndex = 0;
            int segment = (int)(3.6f * mDensity) + 1;
            float outer = -mRadius / 2;
            float lastPieceDegrees = 0;
            Vector2 prevPos = Vector2.zero;
            for (int i = 0; i <= segment; i++)
            {
                Piece curPiece = mListPiece[curPieceIndex];
                float curPieceDegrees = lastPieceDegrees + (curPiece.Value * 360f);
                float maxDegrees = 3.6f * mFill;
                float degrees = Mathf.Clamp(i * 100f / mDensity, 0, maxDegrees);
                if (degrees <= curPieceDegrees || curPieceIndex == countPiece - 1)
                {
                    vh.AddUIVertexQuad(_GetVBOs(degrees, outer, curPiece.Color, ref prevPos));
                }
                else
                {
                    vh.AddUIVertexQuad(_GetVBOs(curPieceDegrees, outer, curPiece.Color, ref prevPos));
                    curPieceIndex++;
                    lastPieceDegrees = curPieceDegrees;
                    vh.AddUIVertexQuad(_GetVBOs(degrees, outer, mListPiece[curPieceIndex].Color, ref prevPos));
                }
                if (degrees >= maxDegrees)
                {
                    break;
                }
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

        #region ----- Private Method -----

        /// <summary>
        /// Return vertex buffer objects by degrees.
        /// </summary>
        private UIVertex[] _GetVBOs(float degrees, float outer, Color color, ref Vector2 prevPos)
        {
            float rad = Mathf.Deg2Rad * (270 - degrees - mRotation);
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);

            Vector2 pos0 = prevPos;
            Vector2 pos1 = new Vector2(outer * cos, outer * sin);

            prevPos = pos1;

            UIVertex[] VBOs = new UIVertex[4];
            Vector2[] vertices = mIsTransparency ? new Vector2[]{ pos0, pos1 } : new Vector2[]{ pos0, pos1, Vector2.zero };
            for (int i = 0; i < vertices.Length; i++)
            {
                UIVertex vert = UIVertex.simpleVert;
                vert.color = color;
                vert.position = vertices[i];
                VBOs[i] = vert;
            }
            return VBOs;
        }

        #endregion

        #region ----- Internal Struct -----

        [System.Serializable]
        public class Piece
        {
            public float Value;
            public Color Color;

            public Piece(float value, Color color)
            {
                Value = value;
                Color = color;
            }
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyUGUIPieChart))]
    public class MyUGUIPieChartEditor : Editor
    {
        private MyUGUIPieChart mScript;
        private bool mIsVisible = true;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUIPieChart)target;
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUGUIPieChart), false);

            mScript.raycastTarget = EditorGUILayout.Toggle("Raycast Target", mScript.raycastTarget);

            SerializedProperty transparencyProperty = serializedObject.FindProperty("mIsTransparency");
            transparencyProperty.boolValue = EditorGUILayout.Toggle("Is Transparency", transparencyProperty.boolValue);

            SerializedProperty radiusProperty = serializedObject.FindProperty("mRadius");
            radiusProperty.floatValue = EditorGUILayout.FloatField("Radius", radiusProperty.floatValue);

            SerializedProperty rotationProperty = serializedObject.FindProperty("mRotation");
            rotationProperty.floatValue = EditorGUILayout.Slider("Rotation", rotationProperty.floatValue, 0, 360);

            SerializedProperty fillProperty = serializedObject.FindProperty("mFill");
            fillProperty.floatValue = EditorGUILayout.Slider("Fill", fillProperty.floatValue, 0, 100);

            SerializedProperty densityProperty = serializedObject.FindProperty("mDensity");
            float densityValue = (float)densityProperty.intValue;
            densityProperty.intValue = (int)EditorGUILayout.Slider("Density", densityValue, 1, 200);

            mIsVisible = EditorGUILayout.Foldout(mIsVisible, "Pieces");
            if (mIsVisible)
            {
                SerializedProperty piecesProperty = serializedObject.FindProperty("mListPiece");
                EditorGUI.indentLevel++;
                piecesProperty.arraySize = EditorGUILayout.IntField("Size", piecesProperty.arraySize);
                for (int i = 0; i < piecesProperty.arraySize; i++)
                {
                    EditorGUILayout.LabelField("Element " + i);
                    EditorGUI.indentLevel++;
                    SerializedProperty elementProperty = piecesProperty.GetArrayElementAtIndex(i);
                    SerializedProperty elementPropertyValue = elementProperty.FindPropertyRelative("Value");
                    SerializedProperty elementPropertyColor = elementProperty.FindPropertyRelative("Color");
                    elementPropertyValue.floatValue = EditorGUILayout.FloatField("Value", elementPropertyValue.floatValue);
                    elementPropertyColor.colorValue = EditorGUILayout.ColorField("Color", elementPropertyColor.colorValue);
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}
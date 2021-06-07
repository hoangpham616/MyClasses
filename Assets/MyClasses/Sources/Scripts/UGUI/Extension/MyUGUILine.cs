/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUILine (version 2.1)
 */

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0649

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MyClasses.UI
{
    [ExecuteInEditMode]
    public class MyUGUILine : Graphic
    {
        #region ----- Variable -----

        [SerializeField]
        private Gradient mColor = null;
        [SerializeField]
        private EColorType mColorType = EColorType.Horizontal;
        [SerializeField]
        private EJoinType mJoinType = EJoinType.Bevel;
        [SerializeField]
        private float mThickness = 25;
        [SerializeField]
        private int mNumExtraPointForSegment = 0;
        [SerializeField]
        private List<Vector2> mListPoint = new List<Vector2>() { new Vector2(-100, -100), new Vector2(-100, 100), new Vector2(0, 0), new Vector2(100, 100), new Vector2(100, -100) };

        [SerializeField]
        private List<Vector2> mListFullPoint = new List<Vector2>() { new Vector2(-100, -100), new Vector2(-100, 100), new Vector2(0, 0), new Vector2(100, 100), new Vector2(100, -100) };
        [SerializeField]
        private List<float> mListFullLength = new List<float>();
        [SerializeField]
        private float mTotalLength = 0;

        #endregion

        #region ----- Property -----

        public List<Vector2> Points
        {
            get { return mListPoint; }
            set
            {
                mListPoint = value;
                Refresh();
            }
        }

        public float Thickness
        {
            get { return mThickness; }
            set { mThickness = Mathf.Max(value, 0.1f); }
        }

        public int Density
        {
            get { return mNumExtraPointForSegment; }
            set
            {
                mNumExtraPointForSegment = Mathf.Clamp(value, 0, 5);
                Refresh();
            }
        }

        #endregion

        #region ----- Implement MonoBehaviour -----

        /// <summary>
        /// Awake.
        /// </summary>
        private void Awake()
        {
            if (mColor == null)
            {
                GradientColorKey[] colorKeys = new GradientColorKey[6];
                colorKeys[0] = new GradientColorKey(new Color(0, 0.3f, 1f), 0);
                colorKeys[1] = new GradientColorKey(new Color(0, 1f, 1f), 0.2f);
                colorKeys[2] = new GradientColorKey(new Color(0, 1f, 0.2f), 0.4f);
                colorKeys[3] = new GradientColorKey(new Color(1f, 1f, 0), 0.6f);
                colorKeys[4] = new GradientColorKey(new Color(1f, 0, 0), 0.8f);
                colorKeys[5] = new GradientColorKey(new Color(1f, 0.2f, 0.6f), 1f);

                GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
                alphaKeys[0] = new GradientAlphaKey(1f, 1f);
                alphaKeys[1] = new GradientAlphaKey(0, 1f);

                mColor = new Gradient();
                mColor.SetKeys(colorKeys, alphaKeys);
            }
        }

        /// <summary>
        /// Start.
        /// </summary>
        private void Start()
        {
            Refresh();
        }

        #endregion

        #region ----- Graphic Implementation -----

        /// <summary>
        /// OnPopulateMesh.
        /// </summary>
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            Vector2 prevBot = Vector2.zero;
            Vector2 prevTop = Vector2.zero;
            for (int i = 1, length = mListFullPoint.Count - 1; i <= length; ++i)
            {
                Vector2 prevPoint = mListFullPoint[i - 1] * transform.localScale;
                Vector2 curPoint = mListFullPoint[i] * transform.localScale;
                Vector2 offset = new Vector2((curPoint.y - prevPoint.y), prevPoint.x - curPoint.x).normalized * mThickness / 2;

                if (i == 1)
                {
                    prevTop = prevPoint - offset;
                    prevBot = prevPoint + offset;

                    vh.AddVert(prevBot, _GetColor(0, true), Vector2.zero);
                    vh.AddVert(prevTop, _GetColor(0, false), Vector2.zero);
                }
                Vector2 curTop = curPoint - offset;
                Vector2 curBot = curPoint + offset;

                switch (mJoinType)
                {
                    case EJoinType.None:
                        {
                            // None
                            // [i = 1]       [i = 2]
                            // 1 x x 3       1 x x 3
                            // x x x x       x x 4 x 5
                            // 0 x x 2       0 x x 2 x
                            //                   x x x
                            //                   6 x 7

                            if (2 <= i)
                            {
                                prevTop = prevPoint - offset;
                                prevBot = prevPoint + offset;

                                vh.AddVert(prevBot, _GetColor(i - 1, true), Vector2.zero);
                                vh.AddVert(prevTop, _GetColor(i - 1, false), Vector2.zero);
                            }

                            vh.AddVert(curBot, _GetColor(i, true), Vector2.zero);
                            vh.AddVert(curTop, _GetColor(i, false), Vector2.zero);

                            int lastBotIndex = vh.currentVertCount - 4;
                            int lastTopIndex = lastBotIndex + 1;
                            int curBotIndex = lastBotIndex + 2;
                            int curTopIndex = lastBotIndex + 3;

                            vh.AddTriangle(lastBotIndex, lastTopIndex, curTopIndex);
                            vh.AddTriangle(curTopIndex, curBotIndex, lastBotIndex);
                        }
                        break;

                    case EJoinType.Miter:
                        {
                            // Miter
                            // [i = 1]       [i = 2]
                            // 1 x x x 3     1 x x x 3
                            // x x x x       x x x x x
                            // 0 x 2         0 x 2 x x
                            //                   x x x
                            //                   4 x 5

                            if (i < length)
                            {
                                Vector2 nextPoint = mListFullPoint[i + 1] * transform.localScale;
                                Vector2 vectorCurPrev = prevPoint - curPoint;
                                Vector2 vectorCurNext = nextPoint - curPoint;
                                float angle = Vector2.Angle(vectorCurPrev, vectorCurNext) * Mathf.Deg2Rad;
                                float sign = Mathf.Sign(Vector3.Cross(vectorCurPrev.normalized, vectorCurNext.normalized).z);
                                float miterDistance = mThickness / (2 * Mathf.Tan(angle / 2));
                                Vector2 miterVector = vectorCurPrev.normalized * miterDistance * sign;

                                curTop -= miterVector;
                                curBot += miterVector;
                            }

                            vh.AddVert(curBot, _GetColor(i, true), Vector2.zero);
                            vh.AddVert(curTop, _GetColor(i, false), Vector2.zero);

                            int lastBotIndex = vh.currentVertCount - 4;
                            int lastTopIndex = lastBotIndex + 1;
                            int curBotIndex = lastBotIndex + 2;
                            int curTopIndex = lastBotIndex + 3;

                            vh.AddTriangle(lastBotIndex, lastTopIndex, curTopIndex);
                            vh.AddTriangle(curTopIndex, curBotIndex, lastBotIndex);
                        }
                        break;

                    case EJoinType.Bevel:
                        {
                            // Bevel
                            // [i = 1]     [i = 2]
                            // 1 x x 2     1 x x 2
                            // x x x x     x x x x 4
                            // 0 x 3 x     0 x 3 x x
                            //                 x x x
                            //                 6 x 5

                            if (2 <= i)
                            {
                                prevTop = prevPoint - offset;

                                vh.AddVert(prevTop, _GetColor(i - 1, false), Vector2.zero);

                                int newestIndex = vh.currentVertCount - 1;

                                vh.AddTriangle(newestIndex, newestIndex - 1, newestIndex - 2);
                            }

                            if (i < length)
                            {
                                Vector2 nextPoint = mListFullPoint[i + 1] * transform.localScale;
                                Vector2 vectorCurPrev = prevPoint - curPoint;
                                Vector2 vectorCurNext = nextPoint - curPoint;
                                float angle = Vector2.Angle(vectorCurPrev, vectorCurNext) * Mathf.Deg2Rad;
                                float sign = Mathf.Sign(Vector3.Cross(vectorCurPrev.normalized, vectorCurNext.normalized).z);
                                float miterDistance = mThickness / (2 * Mathf.Tan(angle / 2));
                                Vector2 miterVector = vectorCurPrev.normalized * miterDistance * sign;

                                curBot += miterVector;
                            }

                            vh.AddVert(curTop, _GetColor(i, true), Vector2.zero);
                            vh.AddVert(curBot, _GetColor(i, false), Vector2.zero);

                            int lastBotIndex = vh.currentVertCount - 4;
                            int lastTopIndex = lastBotIndex + 1;
                            int curTopIndex = lastBotIndex + 2;
                            int curBotIndex = lastBotIndex + 3;

                            vh.AddTriangle(lastBotIndex, lastTopIndex, curTopIndex);
                            vh.AddTriangle(curTopIndex, curBotIndex, lastBotIndex);
                        }
                        break;
                }

                prevTop = curTop;
                prevBot = curBot;
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Refresh.
        /// </summary>
        public void Refresh()
        {
            _FindAllPoints();
            _CalculateLength();
            SetAllDirty();
        }

#if UNITY_EDITOR

        /// <summary>
        /// Create a template.
        /// </summary>
        public static void CreateTemplate()
        {
            GameObject obj = new GameObject(typeof(MyUGUILine).Name);
            if (Selection.activeTransform != null)
            {
                obj.transform.parent = Selection.activeTransform;
            }

            obj.AddComponent<MyUGUILine>();

            EditorGUIUtility.PingObject(obj);
            Selection.activeGameObject = obj.gameObject;
        }

#endif

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Find all points based on density.
        /// </summary>
        private void _FindAllPoints()
        {
            mListFullPoint.Clear();
            if (mNumExtraPointForSegment > 0)
            {
                for (int i = 0, count = mListPoint.Count; i < count; ++i)
                {
                    Vector2 curPoint = mListPoint[i];
                    if (1 < i)
                    {
                        Vector2 prevPoint = mListPoint[i - 1];
                        Vector2 offset = (curPoint - prevPoint) / (mNumExtraPointForSegment + 1);
                        for (int j = 1; j < mNumExtraPointForSegment; ++j)
                        {
                            mListFullPoint.Add(prevPoint + (offset * j));
                        }
                    }
                    mListFullPoint.Add(mListPoint[i]);
                }
            }
            else
            {
                mListFullPoint.AddRange(mListPoint);
            }
        }

        /// <summary>
        /// Calculate length.
        /// </summary>
        private void _CalculateLength()
        {
            mTotalLength = 0;
            mListFullLength.Clear();
            mListFullLength.Add(0);
            for (int i = 1, length = mListFullPoint.Count - 1; i <= length; ++i)
            {
                mTotalLength += Vector3.Distance(mListFullPoint[i], mListFullPoint[i - 1]);
                mListFullLength.Add(mTotalLength);
            }
        }

        /// <summary>
        /// Return a color by a point.
        /// </summary>
        private Color _GetColor(int pointIndex, bool isBot = true)
        {
            switch (mColorType)
            {
                case EColorType.Horizontal:
                    {
                        return mColor.Evaluate(mListFullLength[pointIndex] / mTotalLength);
                    }

                case EColorType.ReverseHorizontal:
                    {
                        return mColor.Evaluate(1 - (mListFullLength[pointIndex] / mTotalLength));
                    }

                case EColorType.Vertical:
                    {
                        return mColor.Evaluate(isBot ? 0 : 1);
                    }

                case EColorType.ReverseVertical:
                    {
                        return mColor.Evaluate(isBot ? 1 : 0);
                    }
            }

            return Color.white;
        }

        #endregion

        #region ----- Enumeration -----

        public enum EColorType
        {
            Horizontal = 0,
            Vertical = 1,
            ReverseHorizontal = 2,
            ReverseVertical = 3,
        }

        public enum EJoinType
        {
            None = 0,
            Bevel = 1,
            Miter = 2,
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyUGUILine))]
    public class MyUGUILineEditor : Editor
    {
        private MyUGUILine mScript;

        private SerializedProperty mColor;
        private SerializedProperty mColorType;
        private SerializedProperty mJoinType;
        private SerializedProperty mThickness;
        private SerializedProperty mNumExtraPointForSegment;
        private SerializedProperty mListPoint;

        private bool mIsListPointVisible = true;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUILine)target;

            mColor = serializedObject.FindProperty("mColor");
            mColorType = serializedObject.FindProperty("mColorType");
            mJoinType = serializedObject.FindProperty("mJoinType");
            mThickness = serializedObject.FindProperty("mThickness");
            mNumExtraPointForSegment = serializedObject.FindProperty("mNumExtraPointForSegment");
            mListPoint = serializedObject.FindProperty("mListPoint");
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUGUIAnchor), false);

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Color", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(mColor, new GUIContent("   Color Over Length"), true);
            mColorType.enumValueIndex = (int)(MyUGUILine.EColorType)EditorGUILayout.EnumPopup("   Direction", (MyUGUILine.EColorType)mColorType.enumValueIndex);

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Line", EditorStyles.boldLabel);
            mJoinType.enumValueIndex = (int)(MyUGUILine.EJoinType)EditorGUILayout.EnumPopup("   Join", (MyUGUILine.EJoinType)mJoinType.enumValueIndex);
            mThickness.floatValue = EditorGUILayout.Slider("   Thickness", mThickness.floatValue, 0.1f, 512);
            mNumExtraPointForSegment.intValue = (int)EditorGUILayout.Slider("   Extra Points For Each Segment", mNumExtraPointForSegment.intValue, 0, 5);

            mIsListPointVisible = EditorGUILayout.Foldout(mIsListPointVisible, "   Points", true);
            if (mIsListPointVisible)
            {
                EditorGUI.indentLevel++;
                mListPoint.arraySize = EditorGUILayout.IntField("Size", mListPoint.arraySize);
                for (int i = 0; i < mListPoint.arraySize; i++)
                {
                    Rect elementPosition = GUILayoutUtility.GetRect(0f, 16f);
                    SerializedProperty elementProperty = mListPoint.GetArrayElementAtIndex(i);
                    EditorGUI.PropertyField(elementPosition, elementProperty);
                }
                EditorGUI.indentLevel--;
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                mScript.Refresh();
            }
        }
    }

#endif
}

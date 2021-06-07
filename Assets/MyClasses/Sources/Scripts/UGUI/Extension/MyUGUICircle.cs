/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUICircle (version 2.2)
 */

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0649

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;

namespace MyClasses.UI
{
    [ExecuteInEditMode]
    public class MyUGUICircle : Graphic
    {
        #region ----- Variable -----

        [HideInInspector]
        [SerializeField]
        private bool mIsFill = true;
        [HideInInspector]
        [SerializeField]
        private float mRadius = 100;
        [HideInInspector]
        [SerializeField]
        private float mThickness = 3;
        [HideInInspector]
        [SerializeField]
        private float mRotation = 0;
        [HideInInspector]
        [SerializeField]
        private float mDegrees = 360;
        [HideInInspector]
        [SerializeField]
        private int mDensity = 100;

        #endregion

        #region ----- Property -----

        public bool IsFill
        {
            get { return mIsFill; }
            set { mIsFill = value; }
        }

        public float Radius
        {
            get { return mRadius; }
            set
            { 
                mRadius = value;
                SetAllDirty();
            }
        }

        public float Thickness
        {
            get { return mThickness; }
            set
            { 
                mThickness = value;
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

        public float Degrees
        {
            get { return mDegrees; }
            set
            { 
                mDegrees = Mathf.Clamp(value, 0, 360);
                SetAllDirty();
            }
        }

        public int Density
        {
            get { return mDensity; }
            set { mDensity = Mathf.Clamp(value, 1, 200); }
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

        #region ----- Graphic Implementation -----

        /// <summary>
        /// OnPopulateMesh.
        /// </summary>
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            float outer = -mRadius;
            float inner = outer + mThickness;

            Vector2 prevPos1 = Vector2.zero;
            Vector2 prevPos2 = Vector2.zero;

            int segment = (int)(3.6f * mDensity) + 1;
            for (int i = 0; i <= segment; i++)
            {
                float degrees = Mathf.Clamp(i * 100f / mDensity, 0, mDegrees);

                float rad = Mathf.Deg2Rad * (270 - degrees - mRotation);
                float cos = Mathf.Cos(rad);
                float sin = Mathf.Sin(rad);

                Vector2 pos0 = prevPos1;
                Vector2 pos1 = new Vector2(outer * cos, outer * sin);
                Vector2 pos2 = mIsFill ? Vector2.zero : new Vector2(inner * cos, inner * sin);
                Vector2 pos3 = mIsFill ? Vector2.zero : prevPos2;

                prevPos1 = pos1;
                prevPos2 = pos2;

                Vector2 uv0 = new Vector2(0, 1);
                Vector2 uv1 = new Vector2(1, 1);
                Vector2 uv2 = new Vector2(1, 0);
                Vector2 uv3 = new Vector2(0, 0);

                vh.AddUIVertexQuad(_GetVBOs(new Vector2[]{ pos0, pos1, pos2, pos3 }, new Vector2[] { uv0, uv1, uv2, uv3 }));

                if (degrees >= mDegrees)
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

#if UNITY_EDITOR

        /// <summary>
        /// Create a template.
        /// </summary>
        public static void CreateTemplate()
        {
            GameObject obj = new GameObject(typeof(MyUGUICircle).Name);
            if (Selection.activeTransform != null)
            {
                obj.transform.parent = Selection.activeTransform;
            }

            obj.AddComponent<MyUGUIRadarChart>();

            EditorGUIUtility.PingObject(obj);
            Selection.activeGameObject = obj.gameObject;
        }

#endif

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Return vertex buffer objects by degrees.
        /// </summary>
        private UIVertex[] _GetVBOs(Vector2[] vertices, Vector2[] uvs)
        {
            UIVertex[] vbo = new UIVertex[4];
            for (int i = 0; i < vertices.Length; i++)
            {
                var vert = UIVertex.simpleVert;
                vert.color = color;
                vert.position = vertices[i];
                vert.uv0 = uvs[i];
                vbo[i] = vert;
            }
            return vbo;
        }

        #endregion
    }

    #if UNITY_EDITOR
    
    [CustomEditor(typeof(MyUGUICircle))]
    public class MyUGUICircleEditor : Editor
    {
        private MyUGUICircle mScript;
    
        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUICircle)target;
        }
    
        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUGUICircle), false);

            mScript.raycastTarget = EditorGUILayout.Toggle("Raycast Target", mScript.raycastTarget);
        
            SerializedProperty fillProperty = serializedObject.FindProperty("mIsFill");
            fillProperty.boolValue = EditorGUILayout.Toggle("Is Fill", fillProperty.boolValue);
        
            SerializedProperty radiusProperty = serializedObject.FindProperty("mRadius");
            radiusProperty.floatValue = EditorGUILayout.FloatField("Radius", radiusProperty.floatValue);

            SerializedProperty thicknessProperty = serializedObject.FindProperty("mThickness");
            thicknessProperty.floatValue = EditorGUILayout.FloatField("Thickness", thicknessProperty.floatValue);
        
            SerializedProperty rotationProperty = serializedObject.FindProperty("mRotation");
            rotationProperty.floatValue = EditorGUILayout.Slider("Rotation", rotationProperty.floatValue, 0, 360);

            SerializedProperty degreesProperty = serializedObject.FindProperty("mDegrees");
            degreesProperty.floatValue = EditorGUILayout.Slider("Degrees", degreesProperty.floatValue, 0, 360);
        
            SerializedProperty densityProperty = serializedObject.FindProperty("mDensity");
            float densityValue = (float)densityProperty.intValue;
            densityProperty.intValue = (int)EditorGUILayout.Slider("Density", densityValue, 1, 200);
        
            serializedObject.ApplyModifiedProperties();
        }
    }
    
#endif
}

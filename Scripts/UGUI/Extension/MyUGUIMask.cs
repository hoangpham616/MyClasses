/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIMask (version 2.1)
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;

namespace MyClasses.UI
{
    [RequireComponent(typeof(Graphic))]
    [ExecuteInEditMode]
    public class MyUGUIMask : MonoBehaviour
    {
        #region ----- Variable -----

        [HideInInspector]
        [SerializeField]
        private Texture mMaskTexture;
        [HideInInspector]
        [SerializeField]
        private RectTransform mMaskRect;
        [HideInInspector]
        [SerializeField]
        private bool mIsFlipAlpha;

        private RectTransform mRect;
        private Material mMaterial;

        #endregion

        #region ----- MonoBehaviour Event -----

        /// <summary>
        /// Start.
        /// </summary>
        void Start()
        {
            mRect = GetComponent<RectTransform>();
            mMaterial = MyResourceManager.GetMaterialAlphaMask();
            GetComponent<Graphic>().material = mMaterial;
        }

        /// <summary>
        /// Update.
        /// </summary>
        void Update()
        {
            Rect contentRect = mRect.rect;
            Rect maskRect = mMaskRect != null ? mMaskRect.rect : mRect.rect;

            Vector2 maskCentre = mRect.transform.InverseTransformPoint(mMaskRect != null ? mMaskRect.transform.position : mRect.transform.position);
            Vector2 halfMaskRect = new Vector2(maskRect.width, maskRect.height) * 0.5f;
            Vector2 min = maskCentre - halfMaskRect;
            Vector2 max = maskCentre + halfMaskRect;
            Vector2 half = Vector2.one * 0.5f;
            min = new Vector2(min.x / contentRect.width, min.y / contentRect.height) + half;
            max = new Vector2(max.x / contentRect.width, max.y / contentRect.height) + half;
            mMaterial.SetVector("_Min", min);
            mMaterial.SetVector("_Max", max);

            Vector2 alphaUV = new Vector2(maskRect.width / contentRect.width, maskRect.height / contentRect.height);
            mMaterial.SetVector("_AlphaUV", alphaUV);

            mMaterial.SetTexture("_AlphaMask", mMaskTexture);

            mMaterial.SetInt("_FlipAlphaMask", mIsFlipAlpha ? 1 : 0);
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyUGUIMask))]
    public class MyUGUIMaskEditor : Editor
    {
        private MyUGUIMask mScript;
        private SerializedProperty mMaskTextureProperty;
        private SerializedProperty mMaskRectProperty;
        private SerializedProperty mFlipProperty;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUIMask)target;
            mMaskTextureProperty = serializedObject.FindProperty("mMaskTexture");
            mMaskRectProperty = serializedObject.FindProperty("mMaskRect");
            mFlipProperty = serializedObject.FindProperty("mIsFlipAlpha");
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUGUIMask), false);

            mMaskTextureProperty.objectReferenceValue = (Texture)EditorGUILayout.ObjectField("Mask Texture", mMaskTextureProperty.objectReferenceValue, typeof(Texture), true);
            mMaskRectProperty.objectReferenceValue = (RectTransform)EditorGUILayout.ObjectField("Mask Rect", mMaskRectProperty.objectReferenceValue, typeof(RectTransform), true);
            mFlipProperty.boolValue = EditorGUILayout.Toggle("Is Flip Alpha", mFlipProperty.boolValue);

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}

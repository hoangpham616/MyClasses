/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIDropHandler (version 2.0)
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MyClasses.UI
{
    public class MyUGUIDropHandler : MonoBehaviour, IDropHandler
    {
        #region ----- Variable -----

        [SerializeField]
        private bool mIsEnable = true;

        [SerializeField]
        private bool mIsAutoFitSize = true;

        private GameObject mItem;

        private MyPointerEvent mOnEventPointerDrop;

        #endregion

        #region ----- Property -----

        public GameObject Item
        {
            get
            {
                if (mItem != null && mItem.transform.parent != transform)
                {
                    mItem = null;
                }
                return mItem;
            }
        }

        public bool IsEnable
        {
            get { return mIsEnable; }
            set { mIsEnable = value; }
        }

        public bool IsAutoFitSize
        {
            get { return mIsAutoFitSize; }
            set { mIsAutoFitSize = value; }
        }

        public MyPointerEvent OnEventPointerDrop
        {
            get
            {
                if (mOnEventPointerDrop == null)
                {
                    mOnEventPointerDrop = new MyPointerEvent();
                }
                return mOnEventPointerDrop;
            }
        }

        #endregion

        #region ----- Implement IDropHandler -----

        /// <summary>
        /// OnDrop.
        /// </summary>
        public void OnDrop(PointerEventData eventData)
        {
            if (Item == null)
            {
                if (mIsEnable)
                {
                    mItem = MyUGUIDragHandler.Item;
                    mItem.transform.SetParent(transform);
                    mItem.transform.localPosition = Vector3.zero;

                    if (mIsAutoFitSize)
                    {
                        RectTransform myRect = gameObject.GetComponent<RectTransform>();
                        RectTransform itemRect = mItem.GetComponent<RectTransform>();
                        float x = itemRect.sizeDelta.x - (itemRect.rect.width - myRect.rect.width);
                        float y = itemRect.sizeDelta.y - (itemRect.rect.height - myRect.rect.height);
                        itemRect.sizeDelta = new Vector2(x, y);
                    }
                }

                if (mOnEventPointerDrop != null)
                {
                    mOnEventPointerDrop.Invoke(eventData);
                }
            }
        }

        #endregion

        #region ----- Internal Class -----

        public class MyPointerEvent : UnityEvent<PointerEventData>
        {
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyUGUIDropHandler))]
    public class MyUGUIDropHandlerEditor : Editor
    {
        private MyUGUIDropHandler mScript;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUIDropHandler)target;
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUGUIDropHandler), false);
        
            mScript.IsEnable = EditorGUILayout.Toggle("Is Enable", mScript.IsEnable);
            mScript.IsAutoFitSize = EditorGUILayout.Toggle("Is Auto Fit", mScript.IsAutoFitSize);
        }
    }

#endif
}

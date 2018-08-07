/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIDragHandler (version 2.9)
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyClasses.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MyUGUIDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region ----- Variable -----

        public static GameObject Item;

        [SerializeField]
        private RectTransform mBoundary;

        [SerializeField]
        private Transform mParentObjectForDragging;

        [SerializeField]
        private bool mIsApplyTouchOffset = true;

        [SerializeField]
        private bool mIsResetPositionAfterDragging;

        [SerializeField]
        private bool mIsAlwaysRefindCanvas;

        private Canvas mCanvas;
        private CanvasScaler mCanvasScaler;
        private CanvasGroup mCanvasGroup;
        private RectTransform mRectTransform;
        private Transform mOriginalParent;
        private Vector3 mOriginalPosition;
        private Vector3 mOriginalScale;
        private Vector2 mOriginalSizeDelta;
        private Vector3 mTouchOffsetDistance;
        private Vector2 mScreenCanvasRatio;
        private bool mIsDragging;
        private int mOriginalIndex;

        private MyPointerEvent mOnEventPointerBeginDrag;
        private MyPointerEvent mOnEventPointerEndDrag;

        #endregion

        #region ----- Property -----

        public RectTransform RectTransform
        {
            get
            {
                if (mRectTransform == null)
                {
                    mRectTransform = GetComponent<RectTransform>();
                }
                return mRectTransform;
            }
        }

        public RectTransform Boundary
        {
            get { return mBoundary; }
            set { mBoundary = value; }
        }

        public Transform ParentOjectWhenDragging
        {
            get { return mParentObjectForDragging; }
            set { mParentObjectForDragging = value; }
        }

        public bool IsApplyTouchOffset
        {
            get { return mIsApplyTouchOffset; }
            set { mIsApplyTouchOffset = value; }
        }

        public bool IsResetPositionAfterDragging
        {
            get { return mIsResetPositionAfterDragging; }
            set { mIsResetPositionAfterDragging = value; }
        }

        public bool IsAlwaysRefindCanvas
        {
            get { return mIsAlwaysRefindCanvas; }
            set { mIsAlwaysRefindCanvas = value; }
        }

        public bool IsDragging
        {
            get { return mIsDragging; }
            set { mIsDragging = value; }
        }

        public MyPointerEvent OnEventPointerBeginDrag
        {
            get
            {
                if (mOnEventPointerBeginDrag == null)
                {
                    mOnEventPointerBeginDrag = new MyPointerEvent();
                }
                return mOnEventPointerBeginDrag;
            }
        }

        public MyPointerEvent OnEventPointerEndDrag
        {
            get
            {
                if (mOnEventPointerEndDrag == null)
                {
                    mOnEventPointerEndDrag = new MyPointerEvent();
                }
                return mOnEventPointerEndDrag;
            }
        }

        #endregion

        #region ----- Implement IBeginDragHandler, IDragHandler, IEndDragHandler -----

        /// <summary>
        /// OnBeginDrag.
        /// </summary>
        public void OnBeginDrag(PointerEventData eventData)
        {
            Item = gameObject;

            if (mCanvas == null || mCanvasScaler == null || mIsAlwaysRefindCanvas)
            {
                Transform parent = transform.parent;
                while (parent != null && mCanvas == null)
                {
                    mCanvas = parent.GetComponent<Canvas>();
                    mCanvasScaler = parent.GetComponent<CanvasScaler>();
                    parent = parent.parent;
                }
            }
            mScreenCanvasRatio = Vector3.one;
            if (mCanvasScaler != null && mCanvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
            {
                mScreenCanvasRatio.x = Screen.width / (float)mCanvasScaler.referenceResolution.x;
                mScreenCanvasRatio.y = Screen.height / (float)mCanvasScaler.referenceResolution.y;
            }

            if (mCanvasGroup == null)
            {
                mCanvasGroup = gameObject.GetComponent<CanvasGroup>();
            }
            mCanvasGroup.blocksRaycasts = false;

            if (mOnEventPointerBeginDrag != null)
            {
                mOnEventPointerBeginDrag.Invoke(eventData);
            }

            mOriginalParent = transform.parent;
            mOriginalPosition = transform.position;
            mOriginalScale = transform.localScale;
            mOriginalSizeDelta = transform.GetComponent<RectTransform>().sizeDelta;
            mOriginalIndex = transform.GetSiblingIndex();

            Vector3 screenPoint = eventData.position;
            screenPoint.z = -mCanvas.planeDistance;
            Vector3 worldPoint = mCanvas.worldCamera.ScreenToWorldPoint(screenPoint);
            worldPoint.z = 0;
            mTouchOffsetDistance = transform.position - worldPoint;
            mTouchOffsetDistance.z = 0;

            if (mParentObjectForDragging != null)
            {
                transform.SetParent(mParentObjectForDragging);
            }
        }

        /// <summary>
        /// OnDrag.
        /// </summary>
        public void OnDrag(PointerEventData eventData)
        {
            mIsDragging = true;

            Vector3 screenPoint = eventData.position;
            screenPoint.z = -mCanvas.planeDistance;
            Vector3 worldPoint = mCanvas.worldCamera.ScreenToWorldPoint(screenPoint);
            worldPoint.z = 0;
            transform.position = worldPoint;

            if (mIsApplyTouchOffset)
            {
                transform.position += mTouchOffsetDistance;
            }

            if (mBoundary != null)
            {
                float halfWidth = RectTransform.rect.width * mScreenCanvasRatio.x / 2;
                float halfHeight = RectTransform.rect.height * mScreenCanvasRatio.y / 2;
                float halfBoundaryWidth = mBoundary.rect.width * mScreenCanvasRatio.x / 2;
                float halfBoundaryHeight = mBoundary.rect.height * mScreenCanvasRatio.y / 2;

                float xMin = mBoundary.position.x - halfBoundaryWidth + halfWidth;
                float xMax = mBoundary.position.x + halfBoundaryWidth - halfWidth;
                float yMin = mBoundary.position.y - halfBoundaryHeight + halfHeight;
                float yMax = mBoundary.position.y + halfBoundaryHeight - halfHeight;

                Vector3 limitPosition = transform.position;
                limitPosition.x = Mathf.Clamp(limitPosition.x, xMin, xMax);
                limitPosition.y = Mathf.Clamp(limitPosition.y, yMin, yMax);
                transform.position = limitPosition;
            }
        }

        /// <summary>
        /// OnEndDrag.
        /// </summary>
        public void OnEndDrag(PointerEventData eventData)
        {
            Item = null;

            mIsDragging = false;

            mCanvasGroup.blocksRaycasts = true;

            if (transform.parent == mOriginalParent || transform.parent == mParentObjectForDragging)
            {
                transform.SetParent(mOriginalParent, false);
                transform.SetSiblingIndex(mOriginalIndex);
                if (mIsResetPositionAfterDragging)
                {
                    transform.position = mOriginalPosition;
                }
                transform.localScale = mOriginalScale;
                transform.GetComponent<RectTransform>().sizeDelta = mOriginalSizeDelta;
            }

            if (mOnEventPointerEndDrag != null)
            {
                mOnEventPointerEndDrag.Invoke(eventData);
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Reset the current drag operation.
        /// </summary>
        public void Cancel()
        {
            if (Item == null)
            {
                return;
            }

            if (mCanvasGroup == null)
            {
                mCanvasGroup = gameObject.GetComponent<CanvasGroup>();
            }
            mCanvasGroup.blocksRaycasts = true;

            if (transform.parent == mOriginalParent || transform.parent == mParentObjectForDragging)
            {
                transform.SetParent(mOriginalParent, false);
                transform.SetSiblingIndex(mOriginalIndex);
                if (mIsResetPositionAfterDragging)
                {
                    transform.position = mOriginalPosition;
                }
                transform.localScale = mOriginalScale;
                transform.GetComponent<RectTransform>().sizeDelta = mOriginalSizeDelta;
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

    [CustomEditor(typeof(MyUGUIDragHandler))]
    public class MyUGUIDragHandlerEditor : Editor
    {
        private MyUGUIDragHandler mScript;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUIDragHandler)target;
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUGUIDragHandler), false);

            mScript.Boundary = (RectTransform)EditorGUILayout.ObjectField("Boundary", mScript.Boundary, typeof(RectTransform), true);
            mScript.ParentOjectWhenDragging = (Transform)EditorGUILayout.ObjectField("Parent Object For Dragging", mScript.ParentOjectWhenDragging, typeof(Transform), true);
            mScript.IsApplyTouchOffset = EditorGUILayout.Toggle("Is Apply Touch Offset", mScript.IsApplyTouchOffset);
            mScript.IsResetPositionAfterDragging = EditorGUILayout.Toggle("Is Reset Position After Dragging", mScript.IsResetPositionAfterDragging);
            mScript.IsAlwaysRefindCanvas = EditorGUILayout.Toggle("Is Always Re-Find Canvas", mScript.IsAlwaysRefindCanvas);
        }
    }

#endif
}
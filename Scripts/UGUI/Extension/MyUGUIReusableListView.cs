/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIReusableListView (version 2.7)
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;

namespace MyClasses.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class MyUGUIReusableListView : MonoBehaviour
    {
        #region ----- Variable -----

        [HideInInspector]
        [SerializeField]
        private GameObject mItemPrefab;
        [HideInInspector]
        [SerializeField]
        private Vector2 mItemSize = new Vector2(100, 100);
        [HideInInspector]
        [SerializeField]
        private Vector2 mItemSpacing = Vector2.zero;
        [HideInInspector]
        [SerializeField]
        private int mItemFixedLine = 1;

        [HideInInspector]
        [SerializeField]
        private int mContentRealDisplayItemQuantity;
        [HideInInspector]
        [SerializeField]
        private int mContentRealItemQuantity;
        [HideInInspector]
        [SerializeField]
        private int mContentRealHeadIndex;
        [HideInInspector]
        [SerializeField]
        private int mContentRealTailIndex;

        [HideInInspector]
        [SerializeField]
        private int mContentItemQuantity;
        [HideInInspector]
        [SerializeField]
        private int mContentHeadIndex;
        [HideInInspector]
        [SerializeField]
        private int mContentTailIndex;

        private Transform mCanvas;
        private ScrollRect mScrollRect;
        private RectTransform mContentParent;
        private RectTransform mContent;
        private RectTransform mContentHeadItem;
        private RectTransform mContentNextHeadItem;
        private RectTransform mContentPrevTailItem;
        private RectTransform mContentTailItem;
        private Vector2 mContentLastPosition;
        private Vector2 mContentZone;
        private MyUGUIReusableListItem[] mContentItems;
        private bool mIsHorizontalMode;
        private bool mIsInitialized;

        #endregion

        #region ----- Property -----

        public MyUGUIReusableListItem[] Items
        {
            get { return mContentItems; }
        }

        #endregion

        #region ----- Scroll Event -----

        /// <summary>
        /// Scroll in list.
        /// </summary>
        private void _OnScrollList(Vector2 pos)
        {
            if (mIsHorizontalMode)
            {
                _ProcessHorizontalScrolling();
            }
            else
            {
                _ProcessVerticalScrolling();
            }

            mContentLastPosition = mContent.position;
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Initialize.
        /// </summary>
        public void Initialize(Vector2 itemSize, Vector2 itemSpacing, int itemFixedLine)
        {
            mItemSize = itemSize;
            mItemSpacing = itemSpacing;
            mItemFixedLine = itemFixedLine;
            Initialize();
        }

        /// <summary>
        /// Initialize.
        /// </summary>
        public void Initialize()
        {
            if (mIsInitialized)
            {
                return;
            }

            if (mItemPrefab == null)
            {
                Debug.LogError("[" + typeof(MyUGUIReusableListView).Name + "] Initialize(): Please set the value for \"Item Prefab\".");
                return;
            }

            mScrollRect = gameObject.GetComponent<ScrollRect>();
            mScrollRect.onValueChanged.AddListener(_OnScrollList);
            if (mScrollRect.content == null)
            {
                Debug.LogError("[" + typeof(MyUGUIReusableListView).Name + "] Initialize(): Could not find the reference of the content.");
                return;
            }

            if (mScrollRect.horizontal && mScrollRect.vertical)
            {
                Debug.LogError("[" + typeof(MyUGUIReusableListView).Name + "] Initialize(): This version does not support twin scroll mode.");
                mScrollRect.horizontal = false;
            }
            mIsHorizontalMode = mScrollRect.horizontal;

            mCanvas = transform.GetComponentInParent<Canvas>().transform;

            mContent = mScrollRect.content.GetComponent<RectTransform>();
            if (mIsHorizontalMode)
            {
                MyUtilities.Anchor(ref mContent, MyUtilities.EAnchorPreset.VerticalStretchLeft, MyUtilities.EAnchorPivot.MiddleLeft, Vector2.zero, Vector2.zero);
            }
            else
            {
                MyUtilities.Anchor(ref mContent, MyUtilities.EAnchorPreset.HorizontalStretchTop, MyUtilities.EAnchorPivot.TopCenter, Vector2.zero, Vector2.zero);
            }
            mContentParent = mContent.parent.GetComponent<RectTransform>();
            mContentLastPosition = mContent.position;
            mContentZone = Vector2.zero;

            if (mIsHorizontalMode)
            {
                int rowDisplayItemQuantity = (int)((GetComponent<RectTransform>().rect.width / (mItemSize.x + mItemSpacing.x)) + 0.5f);
                int col = mItemFixedLine;
                mContentRealDisplayItemQuantity = rowDisplayItemQuantity * col;
                mContentRealItemQuantity = (rowDisplayItemQuantity + 2) * col;
            }
            else
            {
                int colDisplayItemQuantity = (int)((GetComponent<RectTransform>().rect.height / (mItemSize.y + mItemSpacing.y)) + 0.5f);
                int row = mItemFixedLine;
                mContentRealDisplayItemQuantity = colDisplayItemQuantity * row;
                mContentRealItemQuantity = (colDisplayItemQuantity + 2) * row;
            }
            mContentRealHeadIndex = 0;
            mContentRealTailIndex = mContentRealItemQuantity;

            mContentItemQuantity = mContentItemQuantity <= 0 ? mContentRealItemQuantity : mContentItemQuantity;
            mContentHeadIndex = mContentRealHeadIndex;
            mContentTailIndex = mContentRealTailIndex;

            mContentItems = new MyUGUIReusableListItem[mContentRealItemQuantity];
            int countExistedItem = mContent.childCount;
            for (int i = 0; i < countExistedItem; i++)
            {
                GameObject item = mContent.GetChild(i).gameObject;
                item.SetActive(false);
                item.name = mItemPrefab.name + " (" + i + ")";
                mContentItems[i] = item.GetComponent<MyUGUIReusableListItem>();
            }
            for (int i = countExistedItem; i < mContentItems.Length; i++)
            {
                GameObject item = Instantiate(mItemPrefab);
                item.SetActive(false);
                item.transform.SetParent(mContent.transform, false);
#if UNITY_EDITOR
                item.name = mItemPrefab.name + " (" + i + ")";
#endif
                mContentItems[i] = item.GetComponent<MyUGUIReusableListItem>();
            }
            for (int i = mContentItems.Length; i < countExistedItem; i++)
            {
                Destroy(mContent.GetChild(i).gameObject);
            }

            if (mContentItems[0] == null)
            {
                Debug.LogError("[" + typeof(MyUGUIReusableListView).Name + "] Initialize(): Could not find component \"" + typeof(MyUGUIReusableListItem).Name + "\" in \"Item Prefab\".");
                return;
            }

            mContentNextHeadItem = null;
            mContentPrevTailItem = null;
            if (mContentRealItemQuantity > (mItemFixedLine * 2) + 1)
            {
                mContentNextHeadItem = mContentItems[mItemFixedLine].GetComponent<RectTransform>();
                mContentPrevTailItem = mContentItems[mContentItems.Length - mItemFixedLine].GetComponent<RectTransform>();
            }

            mIsInitialized = true;
        }

        /// <summary>
        /// Reload.
        /// </summary>
        /// <param name="isTryToKeepItemsPosition">try to keep current items at same position</param>
        public void Reload(int itemQuantity, bool isTryToKeepItemsPosition = true)
        {
            if (mContent == null)
            {
                return;
            }

            if (mContentHeadItem == null || mContentTailItem == null)
            {
                isTryToKeepItemsPosition = false;
            }

            if (isTryToKeepItemsPosition && itemQuantity == mContentItemQuantity)
            {
                _KeepPositionAndReload();
            }
            else
            {
                float size = mIsHorizontalMode ? mItemSize.x : mItemSize.y;
                float spacing = mIsHorizontalMode ? mItemSpacing.x : mItemSpacing.y;
                float line = (itemQuantity / mItemFixedLine) + (itemQuantity % mItemFixedLine == 0 ? 0 : 1);
                float contentSize = Mathf.Max(mContentParent.rect.height, (line * (size + spacing)) - spacing);

                if (isTryToKeepItemsPosition && itemQuantity > mContentRealDisplayItemQuantity && mContentItemQuantity > mContentRealDisplayItemQuantity)
                {
                    if (mContentTailIndex < itemQuantity)
                    {
                        _KeepPositionAndReload(itemQuantity, contentSize);
                    }
                    else if (itemQuantity < mContentTailIndex)
                    {
                        _MoveToTopAndReload(itemQuantity, contentSize);
                    }
                    else
                    {
                        _MoveToBotAndReload(itemQuantity, contentSize);
                    }
                }
                else
                {
                    _MoveToTopAndReload(itemQuantity, contentSize);
                }
            }

            mContentItemQuantity = itemQuantity;
        }

#if UNITY_EDITOR

        /// <summary>
        /// Create a template.
        /// </summary>
        public static void CreateTemplate()
        {
            GameObject canvas = MyUtilities.FindObjectInRoot("Canvas");

            GameObject obj = new GameObject("ReusableListView");
            if (canvas != null)
            {
                obj.transform.SetParent(canvas.transform, false);
            }

            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(obj.transform, false);

            GameObject content = new GameObject("Content");
            content.transform.SetParent(viewport.transform, false);

            RectTransform contentRectTransform = content.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref contentRectTransform, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

            RectTransform viewportRectTransform = viewport.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref viewportRectTransform, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);
            viewport.AddComponent<Mask>();
            Image viewportImage = viewport.AddComponent<Image>();
            viewportImage.color = new Color(0, 0, 0, 100f / 255f);

            RectTransform objRectTransform = obj.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref objRectTransform, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 600, 400, 0, 0);
            ScrollRect scrollRect = obj.AddComponent<ScrollRect>();
            scrollRect.content = contentRectTransform;
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.decelerationRate = 0.1f;
            scrollRect.viewport = viewportRectTransform;

            obj.AddComponent<MyUGUIReusableListView>();

            EditorGUIUtility.PingObject(obj);
            Selection.activeGameObject = obj.gameObject;
        }

#endif

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Keep content panel size, keep the current position and reload.
        /// </summary>
        private void _KeepPositionAndReload()
        {
            for (int i = 0; i < mContentRealItemQuantity; i++)
            {
                MyUGUIReusableListItem item = mContentItems[i];
                if (item.gameObject.activeSelf)
                {
                    item.OnReload();
                }
            }
        }

        /// <summary>
        /// Resize content panel, move to top and reload.
        /// </summary>
        private void _MoveToTopAndReload(int itemQuantity, float contentSize)
        {
            mScrollRect.StopMovement();

            mContent.SetInsetAndSizeFromParentEdge(mIsHorizontalMode ? RectTransform.Edge.Left : RectTransform.Edge.Top, 0, contentSize);

            mContentRealHeadIndex = 0;
            mContentRealTailIndex = itemQuantity < mContentRealDisplayItemQuantity ? itemQuantity - 1 : mContentRealItemQuantity - 1;
            mContentHeadIndex = mContentRealHeadIndex;
            mContentTailIndex = mContentRealTailIndex;

            for (int i = 0; i < mContentRealItemQuantity; i++)
            {
                MyUGUIReusableListItem item = mContentItems[i];

                int row = mIsHorizontalMode ? i % mItemFixedLine : i / mItemFixedLine;
                int col = mIsHorizontalMode ? i / mItemFixedLine : i % mItemFixedLine;
                Vector2 pos = Vector2.zero;
                pos.x = (col * mItemSize.x) + (col * mItemSpacing.x);
                pos.y = -((row * mItemSize.y) + (row * mItemSpacing.y));
                RectTransform itemRect = item.GetComponent<RectTransform>();
                MyUtilities.Anchor(ref itemRect, MyUtilities.EAnchorPreset.TopLeft, MyUtilities.EAnchorPivot.TopLeft, mItemSize.x, mItemSize.y, pos.x, pos.y);

                item.Index = i;
                if (i < itemQuantity)
                {
                    item.gameObject.SetActive(true);
                    item.OnReload();
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }

            mContentHeadItem = mContentItems[mContentRealHeadIndex].GetComponent<RectTransform>();
            mContentTailItem = mContentItems[mContentRealTailIndex].GetComponent<RectTransform>();

            if (mIsHorizontalMode)
            {
                mContentZone.x = mCanvas.InverseTransformPoint(mContentHeadItem.position).x - (mItemSize.x + mItemSpacing.x) * 1.5f;
                mContentZone.y = mCanvas.InverseTransformPoint(mContentTailItem.position).x;
            }
            else
            {
                mContentZone.x = mCanvas.InverseTransformPoint(mContentHeadItem.position).y + (mItemSize.y + mItemSpacing.y) * 1.5f;
                mContentZone.y = mCanvas.InverseTransformPoint(mContentTailItem.position).y;
            }
        }

        /// <summary>
        /// Resize content panel, keep the current position and reload.
        /// </summary>
        private void _KeepPositionAndReload(int itemQuantity, float contentSize)
        {
            Vector3[] itemPositions = new Vector3[mContentRealItemQuantity];
            for (int i = 0; i < mContentRealItemQuantity; i++)
            {
                itemPositions[i] = mContentItems[i].transform.position;
            }

            Vector3 contentPosition = mContent.position;
            mContent.SetInsetAndSizeFromParentEdge(mIsHorizontalMode ? RectTransform.Edge.Left : RectTransform.Edge.Top, 0, contentSize);
            mContent.position = contentPosition;

            for (int i = 0; i < mContentRealItemQuantity; i++)
            {
                MyUGUIReusableListItem item = mContentItems[i];
                item.transform.position = itemPositions[i];
                if (item.Index < itemQuantity)
                {
                    item.gameObject.SetActive(true);
                    item.OnReload();
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Resize content panel, move to bot and reload.
        /// </summary>
        private void _MoveToBotAndReload(int itemQuantity, float contentSize)
        {
            Vector3 contentPosition = mContent.position;
            mContent.SetInsetAndSizeFromParentEdge(mIsHorizontalMode ? RectTransform.Edge.Left : RectTransform.Edge.Top, 0, contentSize);
            contentPosition.y = contentSize - mContentParent.rect.height;
            mContent.position = contentPosition;
            mContentLastPosition = contentPosition;

            int realTailIndex = mContentRealTailIndex;
            int index = mContentTailIndex;
            for (int i = 0; i < mContentRealItemQuantity; i++)
            {
                int realIndex = (realTailIndex - i + mContentRealItemQuantity) % mContentRealItemQuantity;

                MyUGUIReusableListItem item = mContentItems[realIndex];

                int row = mIsHorizontalMode ? index % mItemFixedLine : index / mItemFixedLine;
                int col = mIsHorizontalMode ? index / mItemFixedLine : index % mItemFixedLine;
                Vector2 pos = Vector2.zero;
                pos.x = (col * mItemSize.x) + (col * mItemSpacing.x);
                pos.y = -((row * mItemSize.y) + (row * mItemSpacing.y));
                RectTransform itemRect = item.GetComponent<RectTransform>();
                MyUtilities.Anchor(ref itemRect, MyUtilities.EAnchorPreset.TopLeft, MyUtilities.EAnchorPivot.TopLeft, mItemSize.x, mItemSize.y, pos.x, pos.y);

                mContentHeadIndex = index;
                item.Index = index;
                if (index < itemQuantity)
                {
                    item.gameObject.SetActive(true);
                    item.OnReload();
                }
                else
                {
                    item.gameObject.SetActive(false);
                }

                index--;
                if (index < 0)
                {
                    index += mContentRealItemQuantity;
                }
            }

            mContentHeadItem = mContentItems[mContentRealHeadIndex].GetComponent<RectTransform>();
            mContentTailItem = mContentItems[mContentRealTailIndex].GetComponent<RectTransform>();
        }

        /// <summary>
        /// Update content panel for horizontal scroll mode.
        /// </summary>
        private void _ProcessHorizontalScrolling()
        {
            // move to left
            if (mContent.position.x > mContentLastPosition.x)
            {
                if (mContentTailIndex <= mContentRealItemQuantity - 1 || mContentTailItem == null || mCanvas.InverseTransformPoint(mContentTailItem.position).x <= mContentZone.y)
                {
                    return;
                }

                for (int i = 0; i < mItemFixedLine; i++)
                {
                    int index = mContentHeadIndex - 1;
                    if (index < 0)
                    {
                        break;
                    }

                    int row = index % mItemFixedLine;
                    int col = index / mItemFixedLine;
                    Vector2 pos = Vector2.zero;
                    pos.x = (col * mItemSize.x) + (col * mItemSpacing.x);
                    pos.y = -((row * mItemSize.y) + (row * mItemSpacing.y));
                    MyUtilities.Anchor(ref mContentTailItem, MyUtilities.EAnchorPreset.TopLeft, MyUtilities.EAnchorPivot.TopLeft, mItemSize.x, mItemSize.y, pos.x, pos.y);

                    mContentHeadItem = mContentTailItem;
                    mContentTailIndex--;
                    mContentHeadIndex--;
                    mContentRealHeadIndex = mContentRealTailIndex;
                    mContentRealTailIndex--;
                    if (mContentRealTailIndex < 0)
                    {
                        mContentRealTailIndex = mContentRealItemQuantity - 1;
                    }
                    mContentTailItem = mContent.GetChild(mContentRealTailIndex).GetComponent<RectTransform>();

                    _ReloadItem(mContentHeadItem.gameObject, mContentTailIndex - mContentRealItemQuantity + 1);
                }

                if (mContentPrevTailItem != null && !RectTransformUtility.RectangleContainsScreenPoint(mScrollRect.viewport, mContentPrevTailItem.position))
                {
                    _OnScrollList(Vector2.zero);
                }
            }
            // move to right
            else
            {
                if (mContentTailIndex >= mContentItemQuantity - 1 || mContentHeadItem == null || mCanvas.InverseTransformPoint(mContentHeadItem.position).x >= mContentZone.x)
                {
                    return;
                }

                for (int i = 0; i < mItemFixedLine; i++)
                {
                    int index = mContentTailIndex + 1;
                    if (index >= mContentItemQuantity)
                    {
                        break;
                    }

                    int row = index % mItemFixedLine;
                    int col = index / mItemFixedLine;
                    Vector2 pos = Vector2.zero;
                    pos.x = (col * mItemSize.x) + (col * mItemSpacing.x);
                    pos.y = -((row * mItemSize.y) + (row * mItemSpacing.y));
                    MyUtilities.Anchor(ref mContentHeadItem, MyUtilities.EAnchorPreset.TopLeft, MyUtilities.EAnchorPivot.TopLeft, mItemSize.x, mItemSize.y, pos.x, pos.y);

                    mContentTailItem = mContentHeadItem;
                    mContentTailIndex++;
                    mContentHeadIndex++;
                    mContentRealTailIndex = mContentRealHeadIndex;
                    mContentRealHeadIndex++;
                    if (mContentRealHeadIndex >= mContentRealItemQuantity)
                    {
                        mContentRealHeadIndex = 0;
                    }
                    mContentHeadItem = mContent.GetChild(mContentRealHeadIndex).GetComponent<RectTransform>();

                    _ReloadItem(mContentTailItem.gameObject, mContentTailIndex);
                }

                if (mContentNextHeadItem != null && !RectTransformUtility.RectangleContainsScreenPoint(mScrollRect.viewport, mContentNextHeadItem.position))
                {
                    _OnScrollList(Vector2.zero);
                }
            }
        }

        /// <summary>
        /// Update content panel for vertical scroll mode.
        /// </summary>
        private void _ProcessVerticalScrolling()
        {
            // move down
            if (mContent.position.y > mContentLastPosition.y)
            {
                if (mContentTailIndex >= mContentItemQuantity - 1 || mContentHeadItem == null || mCanvas.InverseTransformPoint(mContentHeadItem.position).y <= mContentZone.x)
                {
                    return;
                }

                for (int i = 0; i < mItemFixedLine; i++)
                {
                    int index = mContentTailIndex + 1;
                    if (index >= mContentItemQuantity)
                    {
                        break;
                    }

                    int row = index / mItemFixedLine;
                    int col = index % mItemFixedLine;
                    Vector2 pos = Vector2.zero;
                    pos.x = (col * mItemSize.x) + (col * mItemSpacing.x);
                    pos.y = -((row * mItemSize.y) + (row * mItemSpacing.y));
                    MyUtilities.Anchor(ref mContentHeadItem, MyUtilities.EAnchorPreset.TopLeft, MyUtilities.EAnchorPivot.TopLeft, mItemSize.x, mItemSize.y, pos.x, pos.y);

                    mContentTailItem = mContentHeadItem;
                    mContentTailIndex++;
                    mContentHeadIndex++;
                    mContentRealTailIndex = mContentRealHeadIndex;
                    mContentRealHeadIndex++;
                    if (mContentRealHeadIndex >= mContentRealItemQuantity)
                    {
                        mContentRealHeadIndex = 0;
                    }
                    mContentHeadItem = mContent.GetChild(mContentRealHeadIndex).GetComponent<RectTransform>();

                    _ReloadItem(mContentTailItem.gameObject, mContentTailIndex);
                }

                if (mContentNextHeadItem != null && !RectTransformUtility.RectangleContainsScreenPoint(mScrollRect.viewport, mContentNextHeadItem.position))
                {
                    _OnScrollList(Vector2.zero);
                }
            }
            // move up
            else
            {
                if (mContentTailIndex <= mContentRealItemQuantity - 1 || mContentTailItem == null || mCanvas.InverseTransformPoint(mContentTailItem.position).y >= mContentZone.y)
                {
                    return;
                }

                for (int i = 0; i < mItemFixedLine; i++)
                {
                    int index = mContentHeadIndex - 1;
                    if (index < 0)
                    {
                        break;
                    }

                    int row = index / mItemFixedLine;
                    int col = index % mItemFixedLine;
                    Vector2 pos = Vector2.zero;
                    pos.x = (col * mItemSize.x) + (col * mItemSpacing.x);
                    pos.y = -((row * mItemSize.y) + (row * mItemSpacing.y));
                    MyUtilities.Anchor(ref mContentTailItem, MyUtilities.EAnchorPreset.TopLeft, MyUtilities.EAnchorPivot.TopLeft, mItemSize.x, mItemSize.y, pos.x, pos.y);

                    mContentHeadItem = mContentTailItem;
                    mContentTailIndex--;
                    mContentHeadIndex--;
                    mContentRealHeadIndex = mContentRealTailIndex;
                    mContentRealTailIndex--;
                    if (mContentRealTailIndex < 0)
                    {
                        mContentRealTailIndex = mContentRealItemQuantity - 1;
                    }
                    mContentTailItem = mContent.GetChild(mContentRealTailIndex).GetComponent<RectTransform>();

                    _ReloadItem(mContentHeadItem.gameObject, mContentTailIndex - mContentRealItemQuantity + 1);
                }

                if (mContentPrevTailItem != null && !RectTransformUtility.RectangleContainsScreenPoint(mScrollRect.viewport, mContentPrevTailItem.position))
                {
                    _OnScrollList(Vector2.zero);
                }
            }
        }

        /// <summary>
        /// Reload item.
        /// </summary>
        private void _ReloadItem(GameObject item, int realIndex = -1)
        {
            MyUGUIReusableListItem itemScript = item.GetComponent<MyUGUIReusableListItem>();
            if (realIndex >= 0)
            {
                itemScript.Index = realIndex;
            }
            itemScript.OnReload();
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyUGUIReusableListView))]
    public class MyUGUIReusableListViewEditor : Editor
    {
        private MyUGUIReusableListView mScript;
        private SerializedProperty mItemPrefab;
        private SerializedProperty mItemSize;
        private SerializedProperty mItemSpacing;
        private SerializedProperty mItemFixedLine;
        private SerializedProperty mContentRealDisplayItemQuantity;
        private SerializedProperty mContentRealItemQuantity;
        private SerializedProperty mContentRealHeadIndex;
        private SerializedProperty mContentRealTailIndex;
        private SerializedProperty mContentItemQuantity;
        private SerializedProperty mContentHeadIndex;
        private SerializedProperty mContentTailIndex;

        private ScrollRect mScrollRect;
        private bool mIsScrollRectHorizontal;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUIReusableListView)target;
            mItemPrefab = serializedObject.FindProperty("mItemPrefab");
            mItemSize = serializedObject.FindProperty("mItemSize");
            mItemSpacing = serializedObject.FindProperty("mItemSpacing");
            mItemFixedLine = serializedObject.FindProperty("mItemFixedLine");
            mContentRealDisplayItemQuantity = serializedObject.FindProperty("mContentRealDisplayItemQuantity");
            mContentRealItemQuantity = serializedObject.FindProperty("mContentRealItemQuantity");
            mContentRealHeadIndex = serializedObject.FindProperty("mContentRealHeadIndex");
            mContentRealTailIndex = serializedObject.FindProperty("mContentRealTailIndex");
            mContentItemQuantity = serializedObject.FindProperty("mContentItemQuantity");
            mContentHeadIndex = serializedObject.FindProperty("mContentHeadIndex");
            mContentTailIndex = serializedObject.FindProperty("mContentTailIndex");

            mScrollRect = mScript.gameObject.GetComponent<ScrollRect>();
            mScrollRect.vertical = !mScrollRect.horizontal;
            mIsScrollRectHorizontal = mScrollRect.horizontal;
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUGUIReusableListView), false);

            serializedObject.Update();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Item", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            mItemPrefab.objectReferenceValue = EditorGUILayout.ObjectField("Prefab", mItemPrefab.objectReferenceValue, typeof(GameObject), false);
            mItemSize.vector2Value = EditorGUILayout.Vector2Field("Size", mItemSize.vector2Value);
            mItemSpacing.vector2Value = EditorGUILayout.Vector2Field("Spacing", mItemSpacing.vector2Value);
            mItemFixedLine.intValue = EditorGUILayout.IntField(mIsScrollRectHorizontal ? "Fixed Row" : "Fixed Column", mItemFixedLine.intValue);
            EditorGUI.indentLevel--;
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Real Display Item Quantity", mContentRealDisplayItemQuantity.intValue.ToString());
            EditorGUILayout.LabelField("Real Item Quantity", mContentRealItemQuantity.intValue.ToString());
            EditorGUILayout.LabelField("Real Head Index", mContentRealHeadIndex.intValue.ToString());
            EditorGUILayout.LabelField("Real Tail Index", mContentRealTailIndex.intValue.ToString());
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Item Quantity", mContentItemQuantity.intValue.ToString());
            EditorGUILayout.LabelField("Head Index", mContentHeadIndex.intValue.ToString());
            EditorGUILayout.LabelField("Tail Index", mContentTailIndex.intValue.ToString());
            EditorGUI.indentLevel--;

            if ((mIsScrollRectHorizontal && mScrollRect.vertical) || (!mIsScrollRectHorizontal && mScrollRect.horizontal))
            {
                Debug.LogWarning("[" + typeof(MyUGUIReusableListView).Name + "] Initialize(): This version does not support twin scroll mode.");
                mIsScrollRectHorizontal = !mIsScrollRectHorizontal;
            }
            mScrollRect.horizontal = mIsScrollRectHorizontal;
            mScrollRect.vertical = !mIsScrollRectHorizontal;
        }
    }

#endif
}

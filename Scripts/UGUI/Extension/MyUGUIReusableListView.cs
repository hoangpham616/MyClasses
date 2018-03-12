/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIReusableListView (version 2.0)
 */

using UnityEngine;
using UnityEngine.UI;

namespace MyClasses.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class MyUGUIReusableListView : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private GameObject mItemPrefab;
        [SerializeField]
        private int mItemSize;
        [SerializeField]
        private int mItemSpacing;

        [SerializeField]
        private int mContentRealItemQuantity;
        [SerializeField]
        private int mContentRealHeadIndex;
        [SerializeField]
        private int mContentRealTailIndex;

        [SerializeField]
        private int mContentItemQuantity;
        [SerializeField]
        private int mContentHeadIndex;
        [SerializeField]
        private int mContentTailIndex;

        private Transform mCanvas;
        private ScrollRect mScrollRect;
        private RectTransform mContentParent;
        private RectTransform mContent;
        private RectTransform mContentHeadItem;
        private RectTransform mContentTailItem;
        private Vector2 mContentLastPosition;
        private Vector2 mContentZone;
        private MyUGUIReusableListItem[] mContentItems;
        private bool mIsHorizontalMode;

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
        public void Initialize()
        {
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
            mContentParent = mContent.parent.GetComponent<RectTransform>();
            mContentLastPosition = mContent.position;
            mContentZone = Vector2.zero;

            mContentRealItemQuantity = (int)((mIsHorizontalMode ? GetComponent<RectTransform>().rect.width : GetComponent<RectTransform>().rect.height) / (mItemSize + mItemSpacing)) + 3;
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
                item.name = mItemPrefab.name + " (" + i + ")";
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
        }

        /// <summary>
        /// Reload list view.
        /// </summary>
        /// <param name="isKeepCurrentItems">keep current items at same position</param>
        public void Reload(int itemQuantity, bool isKeepCurrentItems = false)
        {
            if (mContent == null)
            {
                return;
            }

            if (mContentTailIndex >= itemQuantity || mContentHeadItem == null || mContentTailItem == null)
            {
                isKeepCurrentItems = false;
            }

            if (mContentItemQuantity != itemQuantity || !isKeepCurrentItems)
            {
                _ResizeAndReposition(itemQuantity, isKeepCurrentItems);
            }

            if (!isKeepCurrentItems)
            {
                mContentRealHeadIndex = 0;
                mContentRealTailIndex = mContentRealItemQuantity - 1;
                mContentHeadIndex = mContentRealHeadIndex;
                mContentTailIndex = mContentRealTailIndex;
            }

            for (int i = 0; i < mContentRealItemQuantity; i++)
            {
                GameObject item = mContentItems[i].gameObject;
                if (i < itemQuantity)
                {
                    int realIndex = -1;
                    if (!item.activeSelf || !isKeepCurrentItems)
                    {
                        realIndex = mContentTailIndex - mContentRealTailIndex + i;
                    }
                    item.SetActive(true);
                    _ReloadItem(item, realIndex);
                }
                else
                {
                    item.SetActive(false);
                }
            }

            mContentItemQuantity = itemQuantity;
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Resize the content panel and reposition its items.
        /// </summary>
        /// <param name="isKeepCurrentItems">keep current items at same position</param>
        private void _ResizeAndReposition(int realNumItem, bool isKeepCurrentItems = false)
        {
            float contentSize = Mathf.Max(mContentParent.rect.height, (realNumItem * (mItemSize + mItemSpacing)) - mItemSpacing);

            if (isKeepCurrentItems)
            {
                Vector3 contentPosition = mContent.position;
                Vector3[] itemPositions = new Vector3[mContentRealItemQuantity];
                for (int i = 0; i < mContentRealItemQuantity; i++)
                {
                    itemPositions[i] = mContentItems[i].transform.position;
                }

                mContent.SetInsetAndSizeFromParentEdge(mIsHorizontalMode ? RectTransform.Edge.Left : RectTransform.Edge.Top, 0, contentSize);

                if (mContentRealItemQuantity >= 2 && mContentTailIndex == mContentItemQuantity - 1)
                {
                    Vector3 offset = itemPositions[1] - itemPositions[0];
                    mContent.position = contentPosition + offset;
                    for (int i = 0; i < mContentRealItemQuantity; i++)
                    {
                        mContentItems[i].transform.position = itemPositions[i] + offset;
                        mContentItems[i].Index++;
                    }
                }
                else
                {
                    mContent.position = contentPosition;
                    for (int i = 0; i < mContentRealItemQuantity; i++)
                    {
                        mContentItems[i].transform.position = itemPositions[i];
                    }
                }
            }
            else
            {
                mScrollRect.StopMovement();

                mContent.SetInsetAndSizeFromParentEdge(mIsHorizontalMode ? RectTransform.Edge.Left : RectTransform.Edge.Top, 0, contentSize);

                float contentItemStartPos = 0f;
                for (int i = 0; i < mContentRealItemQuantity; i++)
                {
                    RectTransform itemRect = mContentItems[i].GetComponent<RectTransform>();
                    if (mIsHorizontalMode)
                    {
                        itemRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, contentItemStartPos, mItemSize);
                        itemRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mContent.rect.height);
                    }
                    else
                    {

                        itemRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, contentItemStartPos, mItemSize);
                        itemRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, mContent.rect.width);
                    }
                    contentItemStartPos += (mItemSize + mItemSpacing);
                }

                mContentHeadItem = mContentItems[0].GetComponent<RectTransform>();
                mContentTailItem = mContentItems[mContentRealItemQuantity - 1].GetComponent<RectTransform>();
            }

            if (mIsHorizontalMode)
            {
                mContentZone.x = mCanvas.InverseTransformPoint(mContentHeadItem.position).x - (mItemSpacing + mItemSize) * 1.5f;
                mContentZone.y = mCanvas.InverseTransformPoint(mContentTailItem.position).x;
            }
            else
            {
                mContentZone.x = mCanvas.InverseTransformPoint(mContentHeadItem.position).y + (mItemSpacing + mItemSize) * 1.5f;
                mContentZone.y = mCanvas.InverseTransformPoint(mContentTailItem.position).y;
            }
        }

        /// <summary>
        /// Update content panel items for horizontal scroll mode.
        /// </summary>
        private void _ProcessHorizontalScrolling()
        {
            if (mContent.position.x < mContentLastPosition.x)
            {
                if (mContentTailIndex >= mContentItemQuantity - 1 || mCanvas.InverseTransformPoint(mContentHeadItem.position).x >= mContentZone.x)
                {
                    return;
                }

                mContentHeadItem.anchoredPosition = mContentTailItem.anchoredPosition + new Vector2(mItemSize + mItemSpacing, 0);
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
            else
            {
                if (mContentTailIndex <= mContentRealItemQuantity - 1 || mCanvas.InverseTransformPoint(mContentTailItem.position).x <= mContentZone.y)
                {
                    return;
                }

                mContentTailItem.anchoredPosition = mContentHeadItem.anchoredPosition - new Vector2(mItemSize + mItemSpacing, 0);
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
        }

        /// <summary>
        /// Update content panel items for vertical scroll mode.
        /// </summary>
        private void _ProcessVerticalScrolling()
        {
            if (mContent.position.y > mContentLastPosition.y)
            {
                if (mContentTailIndex >= mContentItemQuantity - 1 || mCanvas.InverseTransformPoint(mContentHeadItem.position).y <= mContentZone.x)
                {
                    return;
                }

                mContentHeadItem.anchoredPosition = mContentTailItem.anchoredPosition - new Vector2(0, mItemSize + mItemSpacing);
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
            else
            {
                if (mContentTailIndex <= mContentRealItemQuantity - 1 || mCanvas.InverseTransformPoint(mContentTailItem.position).y >= mContentZone.y)
                {
                    return;
                }

                mContentTailItem.anchoredPosition = mContentHeadItem.anchoredPosition + new Vector2(0, mItemSize + mItemSpacing);
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
        }

        /// <summary>
        /// Update content panel item.
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
}

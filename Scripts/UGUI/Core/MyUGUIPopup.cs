/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIPopup (version 2.3)
 */

using UnityEngine;

namespace MyClasses.UI
{
    public abstract class MyUGUIPopup : MyUGUIBase
    {
        #region ----- Variable -----

        private EPopupID mID;
        private Animator mAnimator;
        private bool mIsFloat;
        private bool mIsRepeatable;
        private object mAttachedData;

        #endregion

        #region ----- Property -----

        public EPopupID ID
        {
            get { return mID; }
        }

        public object AttachedData
        {
            get { return mAttachedData; }
            set { mAttachedData = value; }
        }

        public bool IsFloat
        {
            get { return mIsFloat; }
        }

        public bool IsRepeatable
        {
            get { return mIsRepeatable; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="isRepeatable">show multiple popups at the same time</param>
        public MyUGUIPopup(EPopupID id, string prefabName, bool isFloat = false, bool isRepeatable = false)
            : base(prefabName)
        {
            mID = id;
            mIsFloat = isFloat;
            mIsRepeatable = isRepeatable;
        }

        #endregion

        #region ----- Implement MyUGUIBase -----

        /// <summary>
        /// OnUGUIInit.
        /// </summary>
        public override void OnUGUIInit()
        {
            base.OnUGUIInit();

            GameObject parent = mIsFloat ? MyUGUIManager.Instance.CanvasOnTopFloatPopup : MyUGUIManager.Instance.CanvasOnTopPopup;

            if (mIsRepeatable)
            {
                if (IsUseAssetBundle)
                {
                    if (Bundle == null)
                    {
                        Debug.LogError("[" + typeof(MyUGUIPopup).Name + "] OnUGUIInit(): Asset bundle null.");
                    }
                    else
                    {
                        Root = GameObject.Instantiate(Bundle.LoadAsset(PrefabName) as GameObject);
                    }
                }
                else
                {
                    Root = GameObject.Instantiate(Resources.Load(MyUGUIManager.POPUP_DIRECTORY + PrefabName), Vector3.zero, Quaternion.identity) as GameObject;
                }
                Root.name = PrefabName + "_Reaptable (" + Random.Range(0, int.MaxValue) + ")";
            }
            else
            {
                Root = MyUtilities.FindObjectInFirstLayer(parent, PrefabName);
                if (Root == null)
                {
                    if (IsUseAssetBundle)
                    {
                        if (Bundle == null)
                        {
                            Debug.LogError("[" + typeof(MyUGUIPopup).Name + "] OnUGUIInit(): Asset bundle null.");
                        }
                        else
                        {
                            Root = GameObject.Instantiate(Bundle.LoadAsset(PrefabName) as GameObject);
                        }
                    }
                    else
                    {
                        Root = GameObject.Instantiate(Resources.Load(MyUGUIManager.POPUP_DIRECTORY + PrefabName), Vector3.zero, Quaternion.identity) as GameObject;
                        Root.name = PrefabName;
                    }
                }
            }

            Root.transform.SetParent(parent.transform, false);
        }

        /// <summary>
        /// OnUGUIEnter.
        /// </summary>
        public override void OnUGUIEnter()
        {
            base.OnUGUIEnter();

            Root.transform.SetAsLastSibling();

            mAnimator = Root.GetComponent<Animator>();
            if (mAnimator != null)
            {
                mAnimator.Play("Show");
            }
        }

        /// <summary>
        /// OnUGUIVisible.
        /// </summary>
        public override bool OnUGUIVisible()
        {
            if (mAnimator != null && mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                return false;
            }

            return base.OnUGUIVisible();
        }

        /// <summary>
        /// OnUGUIUpdate.
        /// </summary>
        public override void OnUGUIUpdate(float deltaTime)
        {
            base.OnUGUIUpdate(deltaTime);
        }

        /// <summary>
        /// OnUGUIExit.
        /// </summary>
        public override void OnUGUIExit()
        {
            base.OnUGUIExit();

            if (mAnimator != null)
            {
                mAnimator.Play("Hide");
            }
        }

        /// <summary>
        /// OnUGUIInvisible.
        /// </summary>
        public override bool OnUGUIInvisible()
        {
            if (mAnimator != null && mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                return false;
            }

            return base.OnUGUIInvisible();
        }

        /// <summary>
        /// OnUGUIDestroy.
        /// </summary>
        public override void OnUGUIDestroy()
        {
            base.OnUGUIDestroy();
        }

        /// <summary>
        /// OnUGUIBackKey.
        /// </summary>
        public virtual void OnUGUIBackKey()
        {
            MyUGUIManager.Instance.Back();
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Hide popup.
        /// </summary>
        public virtual void Hide()
        {
            State = EBaseState.Exit;
        }

        #endregion
    }
}
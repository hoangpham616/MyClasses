/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIPopup (version 2.22)
 */

using UnityEngine;
using System;

namespace MyClasses.UI
{
    public abstract class MyUGUIPopup : MyUGUIBase
    {
        #region ----- Variable -----

        private EPopupID mID;
        private Animator mAnimator;
        private bool mIsFloat;
        private bool mIsRepeatable;
        private bool mIsRetainable;
        private object mAttachedData;
        private Action mOnCloseCallback;

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

        public bool IsRetainable
        {
            get { return mIsRetainable; }
        }

        public bool IsShowing
        {
            get { return State >= EBaseState.Enter; }
        }

        public Action OnCloseCallback
        {
            set { mOnCloseCallback = value; }
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
            mIsRetainable = !isRepeatable;
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
                        GameObject = GameObject.Instantiate(Bundle.LoadAsset(PrefabName) as GameObject);
                    }
                }
                else
                {
                    GameObject = GameObject.Instantiate(Resources.Load(MyUGUIManager.POPUP_DIRECTORY + PrefabName), Vector3.zero, Quaternion.identity) as GameObject;
                }
                GameObject.name = PrefabName + "_Reaptable (" + UnityEngine.Random.Range(0, int.MaxValue) + ")";
            }
            else
            {
                GameObject = MyUtilities.FindObjectInFirstLayer(parent, PrefabName);
                if (GameObject == null)
                {
                    if (IsUseAssetBundle)
                    {
                        if (Bundle == null)
                        {
                            Debug.LogError("[" + typeof(MyUGUIPopup).Name + "] OnUGUIInit(): Asset bundle null.");
                        }
                        else
                        {
                            GameObject = GameObject.Instantiate(Bundle.LoadAsset(PrefabName) as GameObject);
                        }
                    }
                    else
                    {
                        GameObject = GameObject.Instantiate(Resources.Load(MyUGUIManager.POPUP_DIRECTORY + PrefabName), Vector3.zero, Quaternion.identity) as GameObject;
                        GameObject.name = PrefabName;
                    }
                }
            }

            GameObject.transform.SetParent(parent.transform, false);
        }

        /// <summary>
        /// OnUGUIEnter.
        /// </summary>
        public override void OnUGUIEnter()
        {
            base.OnUGUIEnter();

            GameObject.transform.SetAsLastSibling();

            mAnimator = GameObject.GetComponent<Animator>();
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
            if (mAnimator != null && mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 && GameObject.activeSelf)
            {
                return false;
            }
            
            if (base.OnUGUIInvisible())
            {
                if (mOnCloseCallback != null)
                {
                    mOnCloseCallback();
                    mOnCloseCallback = null;
                }
                return true;
            }

            return false;
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
        /// Hide and destroy.
        /// </summary>
        public void HideAndDestroy()
        {
            mIsRetainable = false;

            State = EBaseState.Exit;
        }

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
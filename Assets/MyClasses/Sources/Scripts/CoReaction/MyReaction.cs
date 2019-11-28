/*
* Copyright (c) 2016 Phạm Minh Hoàng
* Framework:   MyClasses
* Class:       MyReaction (version 1.0)
*/

using System;
using System.Collections.Generic;

namespace MyClasses.CoReaction
{
    public class MyReaction
    {
        #region ----- Variable -----

        private List<MyReactionCondition> mListCondition = new List<MyReactionCondition>();
        private Action mAction;
        private bool mIsActive;

        #endregion

        #region ----- Property -----

        public List<MyReactionCondition> ListCondition
        {
            get { return mListCondition; }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Add a condition.
        /// </summary>
        public virtual void AddCondition(int parameterID, MyReactionCondition.EComparedType conditionType, object comparedValue = null)
        {
            mListCondition.Add(new MyReactionCondition(parameterID, conditionType, comparedValue));
        }

        /// <summary>
        /// Remove a condition.
        /// </summary>
        public virtual void RemoveCondition(int parameterID)
        {
            foreach (MyReactionCondition condition in mListCondition)
            {
                if (condition.ParameterID == parameterID)
                {
                    mListCondition.Remove(condition);
                }
            }
        }

        /// <summary>
        /// Remove all conditions.
        /// </summary>
        public virtual void RemoveAllConditions()
        {
            mListCondition.Clear();
        }

        /// <summary>
        /// Set the action.
        /// </summary>
        public virtual void SetAction(Action action)
        {
            mAction = action;
        }

        /// <summary>
        /// Active/deactive reaction.
        /// </summary>
        public virtual void SetActive(bool isActive)
        {
            mIsActive = isActive;
        }

        /// <summary>
        /// Action.
        /// </summary>
        public virtual void Run()
        {
            if (mIsActive && mAction != null)
            {
                mAction();
            }
        }

        /// <summary>
        /// Active this reaction if its conditions are satisfied.
        /// </summary>
        public virtual void CheckConditionsAndActive()
        {
            bool isPass = true;

            foreach (MyReactionCondition condition in mListCondition)
            {
                if (!condition.ComparedFunction(condition.ComparedType, condition.ComparedValue))
                {
                    isPass = false;
                    break;
                }
            }

            SetActive(isPass);
        }

        #endregion
    }
}
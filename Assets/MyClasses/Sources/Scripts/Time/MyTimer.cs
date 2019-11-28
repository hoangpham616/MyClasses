/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyTimer (version 1.2)
 */

namespace MyClasses
{
    public class MyTimer
    {
        #region ----- Variable -----

        private float mTotalTime;
        private float mCurTime;
        private bool mIsJustDone;

        #endregion

        #region ----- Property -----

        public float TotalTime
        {
            get { return mTotalTime; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyTimer()
        {
            mTotalTime = 0;
            mCurTime = 0;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyTimer(float second)
        {
            SetTime(second);
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set the number of seconds.
        /// </summary>
        public void SetTime(float second)
        {
            mTotalTime = second;
            mCurTime = second;
            mIsJustDone = false;
        }

        /// <summary>
        /// Record the time has passed.
        /// </summary>
        public void Update(float dt)
        {
            if (mCurTime > 0)
            {
                mCurTime -= dt;
                if (mCurTime < 0)
                {
                    mCurTime = 0;
                }
            }
        }

        /// <summary>
        /// Return the number of seconds.
        /// </summary>
        public float GetTargetTime()
        {
            return mTotalTime;
        }

        /// <summary>
        /// Return value of the percentage.
        /// </summary>
        public float GetPercent()
        {
            if (mTotalTime > 0)
            {
                return 1f - (mCurTime / mTotalTime);
            }

            return 1f;
        }

        /// <summary>
        /// Check the timer finished.
        /// </summary>
        public bool IsDone()
        {
            return mCurTime <= 0;
        }

        /// <summary>
        /// Check the timer have just finished.
        /// </summary>
        public bool IsJustDone()
        {
            if (!mIsJustDone && IsDone())
            {
                mIsJustDone = true;
                return true;
            }

            return false;
        }

        #endregion
    }
}
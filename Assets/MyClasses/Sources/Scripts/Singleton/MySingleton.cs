/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MySingleton (version 1.0)
 */

namespace MyClasses
{
    public abstract class MySingleton<T> where T : MySingleton<T>, new()
    {
        #region ----- Variable -----

        private static T mInstance = null;

        #endregion

        #region ----- Property -----

        /// <summary>
        /// Return the only one instance of inherited class.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new T();
                }
                return mInstance;
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Destroy the instance.
        /// </summary>
        public virtual void DestroyInstance()
        {
            mInstance = null;
        }

        #endregion
    }
}
/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyPooledObject (version 2.0)
 */

using UnityEngine;

namespace MyClasses
{
    public class MyPooledObject : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private string mPool;

        #endregion

        #region ----- Property -----

        public string Pool
        {
            get { return mPool; }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set pool.
        /// </summary>
        public void SetPool(string pool)
        {
            mPool = pool;
        }

        #endregion
    }
}
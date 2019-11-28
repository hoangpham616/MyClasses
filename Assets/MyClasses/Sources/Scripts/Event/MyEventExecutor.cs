/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyEventExecutor (version 1.0)
 */

using System.Collections.Generic;

namespace MyClasses
{
    public class MyEventExecutor
    {
        #region ----- Variable -----

        public delegate void EventFunction<T>(T listener);

        private List<object> mListObject = new List<object>();

        #endregion

        #region ----- Singleton -----

        private static MyEventExecutor mInstance;

        public static MyEventExecutor Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new MyEventExecutor();
                }
                return mInstance;
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Register a new object.
        /// </summary>
        /// <param name="isHighPriority">add new object at the first index</param>
        public void Register(object obj, bool isHighPriority = false)
        {
            if (!mListObject.Contains(obj))
            {
                if (isHighPriority)
                {
                    mListObject.Insert(0, obj);
                }
                else
                {
                    mListObject.Add(obj);
                }
            }
        }

        /// <summary>
        /// Unregister a object.
        /// </summary>
        public void Unregister(object obj)
        {
            mListObject.Remove(obj);
        }

        /// <summary>
        /// Unregister all objects.
        /// </summary>
        public void UnregisterAll()
        {
            mListObject.Clear();
        }

        /// <summary>
        /// Execute the specified functions.
        /// </summary>
        public void Execute<T>(EventFunction<T> functor) where T : class
        {
            foreach (var obj in mListObject)
            {
                var listener = obj as T;
                if (listener != null)
                {
                    functor.Invoke(listener);
                }
            }
        }

        #endregion
    }
}

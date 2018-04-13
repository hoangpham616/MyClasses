/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyExtension.GameObject (version 1.1)
 */

using UnityEngine;

namespace MyClasses
{
    public static partial class MyExtension
    {
        /// <summary>
        /// Add a component into specified game object if the component doesn't exist on this game object.
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            if (obj != null)
            {
                T component = obj.GetComponent<T>();
                if (component == null)
                {
                    component = obj.AddComponent<T>();
                }
                return component;
            }

            return null;
        }

        /// <summary>
        /// Return the first child of the specified object.
        /// </summary>
        public static GameObject GetFirstChild(this GameObject obj)
        {
            if (obj != null)
            {
                int countChild = obj.transform.childCount;
                if (countChild > 0)
                {
                    return obj.transform.GetChild(0).gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// Return the last child of the specified object.
        /// </summary>
        public static GameObject GetLastChild(this GameObject obj)
        {
            if (obj != null)
            {
                int countChild = obj.transform.childCount;
                if (countChild > 0)
                {
                    return obj.transform.GetChild(countChild - 1).gameObject;
                }
            }
            return null;
        }
    }
}

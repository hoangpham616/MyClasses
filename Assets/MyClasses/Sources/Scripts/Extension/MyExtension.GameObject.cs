/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyExtension.GameObject (version 1.2)
 */

using UnityEngine;

namespace MyClasses
{
    public static partial class MyExtension
    {
        /// <summary>
        /// Add a component into specified game object if the component doesn't exist on this game object.
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject != null)
            {
                T component = gameObject.GetComponent<T>();
                if (component == null)
                {
                    component = gameObject.AddComponent<T>();
                }
                return component;
            }

            return null;
        }

        /// <summary>
        /// Return the first child of the specified object.
        /// </summary>
        public static GameObject GetFirstChild(this GameObject gameObject)
        {
            if (gameObject != null)
            {
                int countChild = gameObject.transform.childCount;
                if (countChild > 0)
                {
                    return gameObject.transform.GetChild(0).gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// Return the last child of the specified object.
        /// </summary>
        public static GameObject GetLastChild(this GameObject gameObject)
        {
            if (gameObject != null)
            {
                int countChild = gameObject.transform.childCount;
                if (countChild > 0)
                {
                    return gameObject.transform.GetChild(countChild - 1).gameObject;
                }
            }
            return null;
        }
    }
}

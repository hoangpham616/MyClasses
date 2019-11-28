/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyExtension.Component (version 1.0)
 */

using UnityEngine;

namespace MyClasses
{
    public static partial class MyExtension
    {
        /// <summary>
        /// Add a component into specified game object if the component doesn't exist on this game object.
        /// </summary>
        public static T GetOrAddComponent<T>(this Component obj) where T : Component
        {
            if (obj != null)
            {
                T component = obj.gameObject.GetComponent<T>();
                if (component == null)
                {
                    component = obj.gameObject.AddComponent<T>();
                }
                return component;
            }

            return null;
        }
    }
}
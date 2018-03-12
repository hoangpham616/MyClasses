/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyExtension.Transform (version 1.0)
 */

using UnityEngine;

namespace MyClasses
{
    public static partial class MyExtension
	{
		/// <summary>
		/// Return the first child of the specified object.
		/// </summary>
        public static Transform GetFirstChild(this Transform obj)
		{
			if (obj != null)
			{
				int countChild = obj.childCount;
				if (countChild > 0)
				{
					return obj.GetChild(0);
				}
			}

			return null;
		}

		/// <summary>
		/// Return the last child of the specified object.
		/// </summary>
        public static Transform GetLastChild(this Transform obj)
        {
            if (obj != null)
            {
                int countChild = obj.childCount;
                if (countChild > 0)
                {
                    return obj.GetChild(countChild - 1);
                }
            }

            return null;
        }
    }
}
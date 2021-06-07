/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.Count (version 1.0)
 */

using System.Collections.Generic;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        #region ----- Number -----

        /// <summary>
        /// Count elements in list.
        /// </summary>
        public static int Count(List<int> list, int element)
        {
            int count = 0;
            for (int i = 0, length = list.Count - 1; i <= length; ++i)
            {
                if (list[i] == element)
                {
                    count += 1;
                }
            }
            return count;
        }

        /// <summary>
        /// Count elements in array.
        /// </summary>
        public static int CountElement(int[] array, int element)
        {
            int count = 0;
            for (int i = 0, length = array.Length - 1; i <= length; ++i)
            {
                if (array[i] == element)
                {
                    count += 1;
                }
            }
            return count;
        }

        #endregion
    }
}
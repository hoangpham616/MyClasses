/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.Sort (version 1.0)
 */

namespace MyClasses
{
    public static partial class MyUtilities
    {
        /// <summary>
        /// Quicksort.
        /// </summary>
        public static void Quicksort(int[] numbers)
        {
            Quicksort(numbers, 0, numbers.Length - 1);
        }

        /// <summary>
        /// Quicksort.
        /// </summary>
        public static void Quicksort(int[] numbers, int start, int end)
        {
            if (start > end)
            {
                return;
            }

            int num = numbers[start];
            int i = start, j = end;
            while (i < j)
            {
                while (i < j && numbers[j] >= num)
                {
                    j--;
                }
                numbers[i] = numbers[j];
                while (i < j && numbers[i] <= num)
                {
                    i++;
                }
                numbers[j] = numbers[i];
            }
            numbers[i] = num;

            Quicksort(numbers, start, i - 1);
            Quicksort(numbers, i + 1, end);
        }
    }
}
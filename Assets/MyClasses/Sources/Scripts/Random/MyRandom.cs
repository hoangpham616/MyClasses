/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyRandom (version 1.1)
 */

using UnityEngine;
using System.Collections.Generic;

namespace MyClasses
{
    public static class MyRandom
    {
        #region ----- Variable -----

        private static long mEvenFactor;
        private static long mOddFactor;
        private static long mFirstValue;
        private static long mCurValue;
        private static long mCount;

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set the seed.
        /// </summary>
        public static void SetSeed(int seed)
        {
            if (seed < 0)
            {
                seed *= -1;
            }
            seed += 1;

            mEvenFactor = (seed * 2) + 2;
            mOddFactor = (seed * 3 / 2) + 1;

            _NextValue();

            mFirstValue = mCurValue;
        }

        /// <summary>
        /// Return a integer in range.
        /// </summary>
        public static int Range(int begin, int end)
        {
            long range = end - begin;
            if (range == 0)
            {
                range = 1;
            }

            _NextValue();

            return (int)(mCurValue % range) + begin;
        }

        /// <summary>
        /// Return a decimal in range.
        /// </summary>
        public static float Range(float begin, float end)
        {
            float range = end - begin;
            if (range <= 1)
            {
                return begin + (Value01() % range);
            }
            else
            {
                return Range((int)begin, (int)(end - 1)) + Value01();
            }
        }

        /// <summary>
        /// Return a decimal between 0 and 1.
        /// </summary>
        public static float Value01()
        {
            _NextValue();

            return (mCurValue % 10000) / 10000f;
        }

        /// <summary>
        /// Randomly choose an element from a list.
        /// </summary>
        public static T Choose<T>(List<T> list)
        {
            if (list != null)
            {
                int count = list.Count;
                if (count > 0)
                {
                    return list[Random.Range(0, count)];
                }
            }

            return default(T);
        }

        /// <summary>
        /// Shuffle a list.
        /// </summary>
        public static void Shuffle<T>(List<T> list)
        {
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                MyUtilities.Swap(list, i, Random.Range(i, count));
            }
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Change value.
        /// </summary>
        private static void _NextValue()
        {
            mCurValue = ((mCurValue * mOddFactor) + mEvenFactor + mCount++) % int.MaxValue;

            if (mFirstValue == mCurValue)
            {
                mCurValue = (mCurValue + mCount) % int.MaxValue;
                mCount = 0;
            }
        }

        #endregion
    }
}

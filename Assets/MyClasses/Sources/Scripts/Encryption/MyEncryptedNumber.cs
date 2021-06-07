/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyEncryptedNumber (version 1.2)
 */

using System;
using UnityEngine;

namespace MyClasses
{
    public class MyEncryptedNumber
    {
        #region ----- Variable -----

        private static int mSeed = 616;

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set encryption seed.
        /// </summary>
        public static void SetSeed(int seed)
        {
            mSeed = seed;
        }

        /// <summary>
        /// Encrypt an integer.
        /// </summary>
        public static int EncryptInt(int number)
        {
            return ~(number ^ mSeed);
        }

        /// <summary>
        /// Decrypt an integer.
        /// </summary>
        public static int DecryptInt(int number)
        {
            return ~number ^ mSeed;
        }

        /// <summary>
        /// Encrypt a float.
        /// </summary>
        public static float EncryptFloat(float number)
        {
            float decimalPart = Mathf.Abs(Convert.ToSingle(Convert.ToDecimal(number) % 1));
            int wholePart = (int)number;
            int encryptedWholePart = EncryptInt(wholePart);
            return encryptedWholePart > 0 ? encryptedWholePart + decimalPart : encryptedWholePart - decimalPart;
        }

        /// <summary>
        /// Decrypt a float.
        /// </summary>
        public static float DecryptFloat(float number)
        {
            float decimalPart = Mathf.Abs(Convert.ToSingle(Convert.ToDecimal(number) % 1));
            int wholePart = (int)number;
            int decryptedWholePart = DecryptInt(wholePart);
            return decryptedWholePart > 0 ? decryptedWholePart + decimalPart : decryptedWholePart - decimalPart;
        }

        #endregion
    }
}
/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.Hash (version 1.1)
 */

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        /// <summary>
        /// Compute the MD5 hash.
        /// </summary>
        public static string HashMD5(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream));
                }
            }
        }

        /// <summary>
        /// Compute the MD5 hash & return hex string.
        /// </summary>
        public static string HashMD5Hex(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    StringBuilder builder = new StringBuilder();

                    byte[] hash = md5.ComputeHash(stream);
                    for (int i = 0; i < hash.Length; i++)
                    {
                        string tmp = hash[i].ToString("X");
                        if (tmp.Length == 1)
                        {
                            builder.Append("0");
                        }
                        builder.Append(tmp);
                    }

                    return builder.ToString();
                }
            }
        }

        /// <summary>
        /// Compute the SHA1 hash.
        /// </summary>
        public static string HashSHA1(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            using (SHA1Managed sha1 = new SHA1Managed())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    return BitConverter.ToString(sha1.ComputeHash(stream));
                }
            }
        }

        /// <summary>
        /// Compute the SHA1 hash & return hex string.
        /// </summary>
        public static string HashSHA1Hex(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            using (SHA1Managed sha1 = new SHA1Managed())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    StringBuilder builder = new StringBuilder();

                    byte[] hash = sha1.ComputeHash(stream);
                    for (int i = 0; i < hash.Length; i++)
                    {
                        string tmp = hash[i].ToString("X");
                        if (tmp.Length == 1)
                        {
                            builder.Append("0");
                        }
                        builder.Append(tmp);
                    }

                    return builder.ToString();
                }
            }
        }
    }
}
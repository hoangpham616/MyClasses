/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.String (version 1.10)
 */

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        #region ----- Check -----

        /// <summary>
        /// Check if string is username.
        /// </summary>
        public static bool CheckUsername(string username, int minLength = 3, int maxLength = 16)
        {
            if (username != null)
            {
                return (new Regex(@"^[a-z0-9_-]{" + minLength + "," + maxLength + "}$").Match(username)).Length > 0;
            }

            return false;
        }

        /// <summary>
        /// Check if string is password.
        /// </summary>
        public static bool CheckPassword(string password, int minLength = 6, int maxLength = 20)
        {
            if (password != null)
            {
                return (new Regex(@"^[\w-!@#$%^&*]{" + minLength + "," + maxLength + "}$").Match(password)).Length > 0;
            }

            return false;
        }

        /// <summary>
        /// Check if string is strong password (the password contains at least 1 uppercase, 1 lowercase, 1 numberic and 1 special character).
        /// </summary>
        public static bool CheckStrongPassword(string password, int minLength = 6, int maxLength = 20)
        {
            if (password != null)
            {
                return (new Regex(@"((?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[-!@#$%^&*]).{" + minLength + "," + maxLength + "})$").Match(password)).Length > 0;
            }

            return false;
        }

        /// <summary>
        /// Check if string is email.
        /// </summary>
        public static bool CheckEmail(string email)
        {
            if (email != null)
            {
                return (new Regex(@"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$").Match(email)).Length > 0;
            }

            return false;
        }

        /// <summary>
        /// Check if string is IP.
        /// </summary>
        public static bool CheckIP(string ip)
        {
            if (ip != null)
            {
                return (new Regex(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$").Match(ip)).Length > 0;
            }

            return false;
        }

        /// <summary>
        /// Check if string is URL.
        /// </summary>
        public static bool CheckURL(string url)
        {
            if (url != null)
            {
                return (new Regex(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$").Match(url)).Length > 0;
            }

            return false;
        }

        /// <summary>
        /// Check if string is hexadecimal color.
        /// </summary>
        public static bool CheckHexColor(string hex)
        {
            if (hex != null)
            {
                return (new Regex(@"^#?([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$").Match(hex)).Length > 0;
            }

            return false;
        }

        #endregion

        #region ----- Edit -----

        /// <summary>
        /// Converts the accented string to ASCII string.
        /// </summary>
        public static string ConvertAccentedStringToAsciiString(string accentedString, bool isVietnameseOnly = false)
        {
            string accentedCharacters = "áàảãạâấầẩẫậăắằẳẵặ ÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶ đĐ éèẻẽẹêếềểễệ ÉÈẺẼẸÊẾỀỂỄỆ íìỉĩị ÍÌỈĨỊ óòỏõọôốồổỗộớờởỡợôốồổỗộ ÓÒỎÕỌƠỚỜỞỠỢÔỐỒỔỖỘ úùủũụưứừửữự ÚÙỦŨỤƯỨỪỬỮỰ ýỳỷỹỵ ÝỲỶỸỴ";
            string asciiCharacters = "aaaaaaaaaaaaaaaaa AAAAAAAAAAAAAAAAAA dD eeeeeeeeeee EEEEEEEEEEE iiiii IIIII oooooooooooooooooooooo OOOOOOOOOOOOOOOOO uuuuuuuuuuu UUUUUUUUUUU yyyyy YYYYY";

            if (!isVietnameseOnly)
            {
                accentedCharacters += "ÄÅĀÇËĒÎÏĪÑÖØÛÜŪÞß äåāçëēîïīðñöøûüūþÿ";
                asciiCharacters += "AAACEEIIINOOUUUPB aaaceeiiiđnoouuupy";
            }

            string asciiString = string.Empty;
            foreach (char character in accentedString)
            {
                int code = (int)character;
                if (48 <= code && code <= 57)
                {
                    asciiString += code;
                }
                else if ((65 <= code && code <= 90) || (97 <= code && code <= 122))
                {
                    asciiString += character;
                }
                else
                {
                    int charIndex = accentedCharacters.IndexOf(character);
                    if (charIndex >= 0)
                    {
                        asciiString += asciiCharacters[charIndex];
                    }
                }
            }
            return asciiString;
        }

        /// <summary>
        /// Limit length of string.
        /// </summary>
        /// <param name="length">the length of the kept string includes length of suffix</param>
        public static string LimitStringLength(string text, int length, string suffix = "...")
        {
            if (text != null)
            {
                if (text.Length <= length)
                {
                    return text;
                }
                return text.Substring(0, length - suffix.Length).TrimEnd() + suffix;
            }

            return string.Empty;
        }

        /// <summary>
        /// Replace multiple whitespace with single whitespace.
        /// </summary>
        public static string NormalizeWhitespaces(string text)
        {
            return Regex.Replace(text, @"\s{2,}", " ");
        }

        /// <summary>
        /// Remove quotation marks of string.
        /// </summary>
        public static string RemoveQuotationMarks(string text)
        {
            if (text != null)
            {
                if (text.Length >= 2 && text[0] == '"' && text[text.Length - 1] == '"')
                {
                    text = text.Substring(1, text.Length - 2);
                }
            }

            return text;
        }

        /// <summary>
        /// Remove all whitespaces of string.
        /// </summary>
        public static string RemoveWhitespaces(string text)
        {
            return text.Replace(" ", string.Empty);
        }

        #endregion

        #region ----- To String ----- 

        /// <summary>
        /// Print items in list byte.
        /// </summary>
        public static string ToString(List<byte> list)
        {
            return ToString(list != null ? list.ToArray() : null);
        }

        /// <summary>
        /// Print items in byte array.
        /// </summary>
        public static string ToString(byte[] array)
        {
            if (array == null)
            {
                return "null";
            }

            if (array.Length == 0)
            {
                return "[]";
            }

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("[");
            stringBuilder.Append(array[0]);
            for (int i = 1; i < array.Length; i++)
            {
                stringBuilder.Append(",");
                stringBuilder.Append(array[i]);
            }
            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Print items in list bool.
        /// </summary>
        public static string ToString(List<bool> list)
        {
            return ToString(list != null ? list.ToArray() : null);
        }

        /// <summary>
        /// Print items in bool array.
        /// </summary>
        public static string ToString(bool[] array)
        {
            if (array == null)
            {
                return "null";
            }

            if (array.Length == 0)
            {
                return "[]";
            }

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("[");
            stringBuilder.Append(array[0]);
            for (int i = 1; i < array.Length; i++)
            {
                stringBuilder.Append(",");
                stringBuilder.Append(array[i]);
            }
            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Print items in list int.
        /// </summary>
        public static string ToString(List<int> list, string splitChar = ",")
        {
            return ToString(list != null ? list.ToArray() : null, splitChar);
        }

        /// <summary>
        /// Print items in int array.
        /// </summary>
        public static string ToString(int[] array, string splitChar = ",")
        {
            if (array == null)
            {
                return "null";
            }

            if (array.Length == 0)
            {
                return "[]";
            }

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("[");
            stringBuilder.Append(array[0]);
            for (int i = 1; i < array.Length; i++)
            {
                stringBuilder.Append(splitChar);
                stringBuilder.Append(array[i]);
            }
            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Print items in list int array.
        /// </summary>
        public static string ToString(List<int[]> list, string splitChar = ",")
        {
            return ToString(list != null ? list.ToArray() : null, splitChar);
        }

        /// <summary>
        /// Print items in int arrays.
        /// </summary>
        public static string ToString(int[][] arrays, string splitChar = ",")
        {
            if (arrays == null)
            {
                return "null";
            }

            if (arrays.Length == 0)
            {
                return "[]";
            }

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("[");
            stringBuilder.Append(ToString(arrays[0]));
            for (int i = 1; i < arrays.Length; i++)
            {
                stringBuilder.Append(splitChar);
                stringBuilder.Append(ToString(arrays[i]));
            }
            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Print items in list float.
        /// </summary>
        public static string ToString(List<float> list, string splitChar = ",")
        {
            return ToString(list != null ? list.ToArray() : null, splitChar);
        }

        /// <summary>
        /// Print items in float array.
        /// </summary>
        public static string ToString(float[] array, string splitChar = ",")
        {
            if (array == null)
            {
                return "null";
            }

            if (array.Length == 0)
            {
                return "[]";
            }

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("[");
            stringBuilder.Append(array[0]);
            for (int i = 1; i < array.Length; i++)
            {
                stringBuilder.Append(splitChar);
                stringBuilder.Append(array[i]);
            }
            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Print items in list float array.
        /// </summary>
        public static string ToString(List<float[]> list, string splitChar = ",")
        {
            return ToString(list != null ? list.ToArray() : null, splitChar);
        }

        /// <summary>
        /// Print items in float arrays.
        /// </summary>
        public static string ToString(float[][] arrays, string splitChar = ",")
        {
            if (arrays == null)
            {
                return "null";
            }

            if (arrays.Length == 0)
            {
                return "[]";
            }

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("[");
            stringBuilder.Append(ToString(arrays[0]));
            for (int i = 1; i < arrays.Length; i++)
            {
                stringBuilder.Append(splitChar);
                stringBuilder.Append(ToString(arrays[i]));
            }
            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }

        #endregion
    }
}

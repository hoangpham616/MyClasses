/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyForbbidenTextManager (version 1.1)
 */

using UnityEngine;
using System;

namespace MyClasses
{
    public class MyForbbidenTextManager
    {
        #region ----- Variable -----

        private const EFormat FORMAT = EFormat.CSV;

        private static string[] mForbiddenTexts;

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Load forbbiden texts.
        /// </summary>
        public static void LoadData()
        {
            if (FORMAT == EFormat.CSV)
            {
                string path = "Configs/fobbiden_word";
                TextAsset textAsset = Resources.Load(path) as TextAsset;
                if (textAsset == null)
                {
                    Debug.LogError("[" + typeof(MyLocalizationManager).Name + "] LoadLanguage(): Could not find file \"" + path + "\"");
                }
                else
                {
                    mForbiddenTexts = MyCSV.DeserializeByCell(textAsset.text).ToArray();
                }
            }
        }

        /// <summary>
        /// Check text contains forbidden text.
        /// </summary>
        /// <param name="text">a text which needs to check</param>
        /// <param name="isNormalizeWhitespaces">normalize whitespaces before handling</param>
        /// <param name="isIgnoreCase">ignore case while finding forbidden words</param>
        public static bool ExistForbiddenWord(string text, bool isNormalizeWhitespaces = false, bool isIgnoreCase = true)
        {
            if (mForbiddenTexts == null)
            {
                LoadData();
            }

            if (text[0] == ' ' && text.Trim().Length == 0)
            {
                return true;
            }

            if (isNormalizeWhitespaces)
            {
                text = MyUtilities.NormalizeWhitespaces(text);
            }

            StringComparison comparison = isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            string textForComparison = isIgnoreCase ? text.ToLower() : text;

            int count = mForbiddenTexts.Length;
            for (int i = 0; i < count; i++)
            {
                if (textForComparison.IndexOf(mForbiddenTexts[i], comparison) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Find and replace forbidden words in a text.
        /// </summary>
        /// <param name="text">a text which needs to handle</param>
        /// <param name="isTrim">trim text before handling</param>
        /// <param name="isNormalizeWhitespaces">normalize whitespaces before handling</param>
        /// <param name="isIgnoreCase">ignore case while finding forbidden words</param>
        /// <param name="charReplace">a string which replaces every illegal character in forbidden word</param>
        public static string ReplaceForbiddenWords(string text, bool isTrim = true, bool isNormalizeWhitespaces = false, bool isIgnoreCase = true, string charReplace = "*")
        {
            if (mForbiddenTexts == null)
            {
                LoadData();
            }

            if (isTrim)
            {
                text = text.Trim();
            }

            if (isNormalizeWhitespaces)
            {
                text = MyUtilities.NormalizeWhitespaces(text);
            }

            return _ReplaceForbiddenWords(text, charReplace, isIgnoreCase);
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Find and replace forbidden words in a text.
        /// </summary>
        private static string _ReplaceForbiddenWords(string text, string charReplace, bool isIgnoreCase)
        {
            StringComparison comparison = isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            string textForComparison = isIgnoreCase ? text.ToLower() : text;

            int count = mForbiddenTexts.Length;
            for (int i = 0; i < count; i++)
            {
                string forbiddenText = mForbiddenTexts[i];
                int leftIndex = textForComparison.IndexOf(forbiddenText, comparison);
                if (leftIndex >= 0)
                {
                    string hiddenText = string.Empty;
                    for (int j = 0; j < forbiddenText.Length; j++)
                    {
                        hiddenText += forbiddenText[j] != ' ' ? charReplace : " ";
                    }

                    string leftText = text.Substring(0, leftIndex);
                    if (leftText.Length > 0)
                    {
                        leftText = _ReplaceForbiddenWords(leftText, charReplace, isIgnoreCase);
                    }

                    int rightIndex = leftIndex + forbiddenText.Length;
                    string rightText = rightIndex < text.Length ? text.Substring(rightIndex) : string.Empty;
                    if (rightText.Length > 0)
                    {
                        rightText = _ReplaceForbiddenWords(rightText, charReplace, isIgnoreCase);
                    }

                    return leftText + hiddenText + rightText;
                }
            }

            return text;
        }

        #endregion

        #region ----- Enumeration -----

        public enum EFormat
        {
            CSV
        }

        #endregion
    }
}
/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.Macro (version 1.1)
 */

using System.Text;
using System.Collections.Generic;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        private static StringBuilder mStringBuilderInput = new StringBuilder();
        private static StringBuilder mStringBuilderMacroKey = new StringBuilder();

        /// <summary>
        /// A macro example.
        /// </summary>
        public static void TestMacro()
        {
#if UNITY_EDITOR
            Dictionary<string, object> macro = new Dictionary<string, object>();
            macro["name"] = "Hoàng";
            macro["birth_year"] = "1991";
            macro["localization_years_old"] = "[loc]LOCALIZATION_KEY_YEARS_OLD";

            string input = "My name is {name}. I was born in {birth_year}. Now I'm {age} <color=#999900FF>{localization_years_old}.</color>";
            string output = ReplaceTextMacros(input, macro, '{', '}');
            string localizedOutput = MyLocalizationManager.Instance.LocalizeKeys(output, "[loc]");

            UnityEngine.Debug.Log("[" + typeof(MyUtilities).Name + "] TestMacro(): ouput_1=\"" + output + "\"");
            UnityEngine.Debug.Log("[" + typeof(MyUtilities).Name + "] TestMacro(): output_2=\"" + localizedOutput + "\"");
#endif
        }

        /// <summary>
        /// Replace all macros in string.
        /// </summary>
        /// <param name="macro">data macro</param>
        /// <param name="charBeginMacro">a char used for detecting the beginning of a macro</param>
        /// <param name="charEndMacro">a char used for detecting the ending of a macro</param>
        public static string ReplaceTextMacros(string input, Dictionary<string, object> macro, char charBeginMacro = '{', char charEndMacro = '}')
        {
            mStringBuilderInput.Length = 0;
            mStringBuilderMacroKey.Length = 0;

            mStringBuilderInput.Append(input);

            int state = 0;

            for (int i = 0; i < mStringBuilderInput.Length; i++)
            {
                if (state == 0)
                {
                    if (mStringBuilderInput[i] == charBeginMacro)
                    {
                        state = 1;
                        continue;
                    }
                }
                else if (state == 1)
                {
                    if (mStringBuilderInput[i] == charEndMacro)
                    {
                        state = 2;
                        continue;
                    }
                    mStringBuilderMacroKey.Append(mStringBuilderInput[i]);
                }
                else if (state == 2)
                {
                    string macro_key = mStringBuilderMacroKey.ToString();
                    if (macro.ContainsKey(macro_key))
                    {
                        mStringBuilderInput.Replace('{' + macro_key + '}', macro[macro_key].ToString());
                    }
                    else
                    {
                        mStringBuilderInput.Replace('{' + macro_key + '}', "[" + macro_key + "_missing]");
                    }

                    mStringBuilderMacroKey.Length = 0;
                    i = 0;
                    state = 0;
                }
            }
            
            if (mStringBuilderMacroKey.Length != 0)
            {
                string macro_key = mStringBuilderMacroKey.ToString();
                if (macro.ContainsKey(macro_key))
                {
                    mStringBuilderInput.Replace('{' + macro_key + '}', macro[macro_key].ToString());
                }
                else
                {
                    mStringBuilderInput.Replace('{' + macro_key + '}', "[" + macro_key + "_missing]");
                }
            }

            return mStringBuilderInput.ToString();
        }
    }
}
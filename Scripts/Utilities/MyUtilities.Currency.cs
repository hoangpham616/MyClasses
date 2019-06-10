/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.Currency (version 1.5)
 */

using System.Globalization;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        private static CultureInfo CULTURE_DOT = CultureInfo.GetCultureInfo("es-ES");
        private static CultureInfo CULTURE_COMMA = CultureInfo.GetCultureInfo("en-US");

        #region ----- Public Method -----

        /// <summary>
        /// Add commas into thousands places.
        /// </summary>
        public static string AddThousandSeparator(long number, ESeparator separator = ESeparator.Comma)
        {
            return number.ToString("#,0", separator == ESeparator.Dot ? CULTURE_DOT : CULTURE_COMMA);
        }

        /// <summary>
        /// Add commas into thousands places.
        /// </summary>
        /// <param name="decimalDigit">-1: show full decimal</param>
        /// <returns></returns>
        public static string AddThousandSeparator(double number, int decimalDigit = 2, ESeparator separator = ESeparator.Comma)
        {
            if (decimalDigit < 0)
            {
                long integerPart = (long)number;
                double decimalPart = System.Math.Abs(number - integerPart);
                if (separator == ESeparator.Dot)
                {
                    return integerPart.ToString("#,0", CULTURE_DOT) + (decimalPart > 0 ? "," + decimalPart.ToString().Substring(2) : string.Empty);
                }
                else
                {
                    return integerPart.ToString("#,0", CULTURE_COMMA) + (decimalPart > 0 ? "." + decimalPart.ToString().Substring(2) : string.Empty);
                }
            }
            else
            {
                return number.ToString("N" + decimalDigit, separator == ESeparator.Dot ? CULTURE_DOT : CULTURE_COMMA);
            }
        }

        /// <summary>
        /// Add commas into thousands places.
        /// </summary>
        /// <param name="decimalDigit">-1: show full decimal</param>
        /// <returns></returns>
        public static string AddThousandSeparator(decimal number, int decimalDigit = 2, ESeparator separator = ESeparator.Comma)
        {
            if (decimalDigit < 0)
            {
                long integerPart = (long)number;
                decimal decimalPart = System.Math.Abs(number - integerPart);
                if (separator == ESeparator.Dot)
                {
                    return integerPart.ToString("#,0", CULTURE_DOT) + (decimalPart > 0 ? "," + decimalPart.ToString().Substring(2) : string.Empty);
                }
                else
                {
                    return integerPart.ToString("#,0", CULTURE_COMMA) + (decimalPart > 0 ? "." + decimalPart.ToString().Substring(2) : string.Empty);
                }
            }
            else
            {
                return number.ToString("N" + decimalDigit, separator == ESeparator.Dot ? CULTURE_DOT : CULTURE_COMMA);
            }
        }

        /// <summary>
        /// Convert currency string to number.
        /// </summary>
        public static long ConvertCurrencyStringToNumber(string currencyString)
        {
            string newCurrencyString = string.Empty;
            for (int i = 0; i < currencyString.Length; i++)
            {
                if (48 <= (char)currencyString[i] && (char)currencyString[i] <= 57)
                {
                    newCurrencyString += currencyString[i];
                }
            }
            long money = 0;
            long.TryParse(newCurrencyString, out money);

            return money;
        }

        /// <summary>
        /// Convert number to currency string.
        /// </summary>
        /// <param name="highestUnitCurrency">highest unit of currency can be shown</param>
        public static string ConvertNumberToFullCurrencyString(ulong number, EUnitCurrency highestUnitCurrency = EUnitCurrency.Billions, string decimalMark = ".",
            string keyThousand = "_TEXT_THOUSAND", string keyMillion = "_TEXT_MILLION", string keyBillion = "_TEXT_BILLION",
            string keyTrillion = "_TEXT_TRILLION", string keyQuarallion = "_TEXT_QUARALLION")
        {
            return _ConvertNumberToCurrencyString(number, highestUnitCurrency, decimalMark, keyThousand, keyMillion, keyBillion, keyTrillion, keyQuarallion);
        }

        /// <summary>
        /// Convert number to currency string.
        /// </summary>
        /// <param name="highestUnitCurrency">highest unit of currency can be shown</param>
        public static string ConvertNumberToShortCurrencyString(ulong number, EUnitCurrency highestUnitCurrency = EUnitCurrency.Billions, string decimalMark = ".",
            string keyThousand = "_TEXT_SHORT_THOUSAND", string keyMillion = "_TEXT_SHORT_MILLION", string keyBillion = "_TEXT_SHORT_BILLION",
            string keyTrillion = "_TEXT_SHORT_TRILLION", string keyQuarallion = "_TEXT_SHORT_QUARALLION")
        {
            return _ConvertNumberToCurrencyString(number, highestUnitCurrency, decimalMark, keyThousand, keyMillion, keyBillion, keyTrillion, keyQuarallion);
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Convert number to currency string.
        /// </summary>
        private static string _ConvertNumberToCurrencyString(ulong number, EUnitCurrency highestUnitCurrency, string decimalMark,
            string keyThousand, string keyMillion, string keyBillion, string keyTrillion, string keyQuarallion)
        {
            if (highestUnitCurrency >= EUnitCurrency.Quadrillions && number >= 1000000000000000)
            {
                ulong v1 = number / 1000000000000000;
                ulong v2 = (number % 1000000000000000) / 1000000000000;
                if (v2 > 0)
                {
                    if (v2 < 10)
                    {
                        return v1 + decimalMark + "00" + v2 + MyLocalizationManager.Instance.LoadKey(keyTrillion);
                    }
                    else if (v2 < 100)
                    {
                        return v1 + decimalMark + "0" + v2.ToString().TrimEnd('0') + MyLocalizationManager.Instance.LoadKey(keyTrillion);
                    }
                    else
                    {
                        return v1 + decimalMark + v2.ToString().TrimEnd('0') + MyLocalizationManager.Instance.LoadKey(keyTrillion);
                    }
                }
                else
                {
                    return v1 + MyLocalizationManager.Instance.LoadKey(keyTrillion);
                }
            }

            if (highestUnitCurrency >= EUnitCurrency.Trillions && number >= 1000000000000)
            {
                ulong v1 = number / 1000000000000;
                ulong v2 = (number % 1000000000000) / 1000000000;
                if (v2 > 0)
                {
                    if (v2 < 10)
                    {
                        return v1 + decimalMark + "00" + v2 + MyLocalizationManager.Instance.LoadKey(keyTrillion);
                    }
                    else if (v2 < 100)
                    {
                        return v1 + decimalMark + "0" + v2.ToString().TrimEnd('0') + MyLocalizationManager.Instance.LoadKey(keyTrillion);
                    }
                    else
                    {
                        return v1 + decimalMark + v2.ToString().TrimEnd('0') + MyLocalizationManager.Instance.LoadKey(keyTrillion);
                    }
                }
                else
                {
                    return v1 + MyLocalizationManager.Instance.LoadKey(keyTrillion);
                }
            }

            if (highestUnitCurrency >= EUnitCurrency.Billions && number >= 1000000000)
            {
                ulong v1 = number / 1000000000;
                ulong v2 = (number % 1000000000) / 1000000;
                if (v2 > 0)
                {
                    if (v2 < 10)
                    {
                        return v1 + decimalMark + "00" + v2 + MyLocalizationManager.Instance.LoadKey(keyBillion);
                    }
                    else if (v2 < 100)
                    {
                        return v1 + decimalMark + "0" + v2.ToString().TrimEnd('0') + MyLocalizationManager.Instance.LoadKey(keyBillion);
                    }
                    else
                    {
                        return v1 + decimalMark + v2.ToString().TrimEnd('0') + MyLocalizationManager.Instance.LoadKey(keyBillion);
                    }
                }
                else
                {
                    return v1 + MyLocalizationManager.Instance.LoadKey(keyBillion);
                }
            }

            if (highestUnitCurrency >= EUnitCurrency.Millions && number >= 1000000)
            {
                ulong v1 = number / 1000000;
                ulong v2 = (number % 1000000) / 1000;
                if (v2 > 0)
                {
                    if (v2 < 10)
                    {
                        return v1 + decimalMark + "00" + v2 + MyLocalizationManager.Instance.LoadKey(keyMillion);
                    }
                    else if (v2 < 100)
                    {
                        return v1 + decimalMark + "0" + v2.ToString().TrimEnd('0') + MyLocalizationManager.Instance.LoadKey(keyMillion);
                    }
                    else
                    {
                        return v1 + decimalMark + v2.ToString().TrimEnd('0') + MyLocalizationManager.Instance.LoadKey(keyMillion);
                    }
                }
                else
                {
                    return v1 + MyLocalizationManager.Instance.LoadKey(keyMillion);
                }
            }

            if (highestUnitCurrency >= EUnitCurrency.Thousands && number >= 1000)
            {
                ulong v1 = number / 1000;
                ulong v2 = number % 1000;
                if (v2 > 0)
                {
                    if (v2 < 10)
                    {
                        return v1 + decimalMark + "00" + v2 + MyLocalizationManager.Instance.LoadKey(keyThousand);
                    }
                    else if (v2 < 100)
                    {
                        return v1 + decimalMark + "0" + v2.ToString().TrimEnd('0') + MyLocalizationManager.Instance.LoadKey(keyThousand);
                    }
                    else
                    {
                        return v1 + decimalMark + v2.ToString().TrimEnd('0') + MyLocalizationManager.Instance.LoadKey(keyThousand);
                    }
                }
                else
                {
                    return v1 + MyLocalizationManager.Instance.LoadKey(keyThousand);
                }
            }

            return number.ToString();
        }

        #endregion

        #region ----- Enumeration -----

        public enum ESeparator
        {
            Dot,
            Comma
        }

        public enum EUnitCurrency
        {
            Thousands,
            Millions,
            Billions,
            Trillions,
            Quadrillions
        }

        #endregion
    }
}

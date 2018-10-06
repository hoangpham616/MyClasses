/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.Currency (version 1.3)
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
        public static string AddThousandSeparator(long number, ESeparator separator = ESeparator.Dot)
        {
            return number.ToString("0,0", separator == ESeparator.Dot ? CULTURE_DOT : CULTURE_COMMA);
        }

        /// <summary>
        /// Add commas into thousands places.
        /// </summary>
        /// <param name="decimalDigit">-1: show full decimal</param>
        /// <returns></returns>
        public static string AddThousandSeparator(float number, int decimalDigit = 2, ESeparator separator = ESeparator.Dot)
        {
            if(decimalDigit == 0)
            {
                return number.ToString("0,0", separator == ESeparator.Dot ? CULTURE_DOT : CULTURE_COMMA);
            }
            else if (decimalDigit == 1)
            {
                return number.ToString("0,0.0", separator == ESeparator.Dot ? CULTURE_DOT : CULTURE_COMMA);
            }
            else if (decimalDigit >= 2)
            {
                return number.ToString("N", separator == ESeparator.Dot ? CULTURE_DOT : CULTURE_COMMA);
            }
            else
            {
                float decimalPart = System.Math.Abs(number - (int)number);
                if (separator == ESeparator.Dot)
                {
                    return number.ToString("0,0", CULTURE_DOT) + (decimalPart > 0 ? "," + decimalPart.ToString().Substring(2) : string.Empty);
                }
                else
                {
                    return number.ToString("0,0", CULTURE_COMMA) + (decimalPart > 0 ? "." + decimalPart.ToString().Substring(2) : string.Empty);
                }
            }
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

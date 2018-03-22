/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUtilities.Currency (version 1.2)
 */

namespace MyClasses
{
    public static partial class MyUtilities
    {
        #region ----- Public Method -----

        /// <summary>
        /// Add commas into thousands places.
        /// </summary>
        public static string AddThousandSeparator(long number)
        {
            return string.Format("{0:n0}", number);
        }

        /// <summary>
        /// Add commas into thousands places.
        /// </summary>
        public static string AddThousandSeparator(float number, bool isDecimalPoint = false)
        {
            if (isDecimalPoint)
            {
                return string.Format("{0:n}", number);
            }
            else
            {
                return string.Format("{0:n0}", number);
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

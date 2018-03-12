/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyBitSet16 (version 1.2)
 */

using System;

namespace MyClasses
{
    public class MyBitSet16
    {
        #region ----- Variable -----

        private const byte NUM_BIT = 16;

        private ushort mValue = 0;

        #endregion

        #region ----- Property -----

        public ushort Value
        {
            get { return mValue; }
            set { mValue = value; }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Reset value.
        /// </summary>
        public void Reset()
        {
            mValue = 0;
        }

        /// <summary>
        /// Set value of a bit by index.
        /// </summary>
        public void SetValueOfBit(byte index, bool value)
        {
            if (index < 0 || index >= NUM_BIT)
            {
                throw new Exception("[" + typeof(MyBitSet16).Name + "] SetValueOfBit(): Index out of range");
            }

            ushort mask = 0x01;
            mask = (ushort)(mask << index);
            mValue = value ? (ushort)(mValue | mask) : (ushort)(mValue & (~mask));
        }

        /// <summary>
        /// Return value of a bit by index.
        /// </summary>
        public bool GetValueOfBit(byte index)
        {
            if (index < 0 || index >= NUM_BIT)
            {
                throw new Exception("[" + typeof(MyBitSet16).Name + "] GetValueOfBit(): Index out of range");
            }

            ushort mask = 0x01;
            mask = (ushort)(mask << index);
            return (mValue & mask) != 0;
        }

        /// <summary>
        /// Set value of some bits by index.
        /// </summary>
        public void SetValueOfBits(byte fromIndex, byte toIndex, ushort value)
        {
            byte numBits = (byte)(toIndex - fromIndex + 1);
            if (numBits <= 1)
            {
                throw new Exception("[" + typeof(MyBitSet16).Name + "] SetValueOfBits(): Number of bits less than or equals 1");
            }
            else if (numBits >= NUM_BIT)
            {
                throw new Exception("[" + typeof(MyBitSet16).Name + "] SetValueOfBits(): Number of bits greater than or equals " + NUM_BIT);
            }
            else if (value > _GetMaxValue(numBits))
            {
                throw new Exception("[" + typeof(MyBitSet16).Name + "] SetValueOfBits(): Value out of range");
            }

            for (byte i = fromIndex; i <= toIndex; i++)
            {
                SetValueOfBit(i, false);
            }
            mValue = (ushort)(mValue | (value << fromIndex));
        }

        /// <summary>
        /// Return value of some bits by index.
        /// </summary>
        public ushort GetValueOfBits(byte fromIndex, byte toIndex)
        {
            if (fromIndex >= toIndex)
            {
                throw new Exception("[" + typeof(MyBitSet16).Name + "] GetValueOfBits(): fromIndex greater than or equals toIndex");
            }

            byte length = (byte)(toIndex - fromIndex + 1);
            ushort result = (ushort)(mValue << (NUM_BIT - length - fromIndex));
            return result = (ushort)(result >> (NUM_BIT - length));
        }

        #endregion

        #region ----- Private Method ------

        /// <summary>
        /// Return maximum value of variable by number of bits.
        /// </summary>
        private ushort _GetMaxValue(byte numBits)
        {
            if (numBits <= 8)
            {
                switch (numBits)
                {
                    case 1:
                        return 1;
                    case 2:
                        return 3;
                    case 3:
                        return 7;
                    case 4:
                        return 15;
                    case 5:
                        return 31;
                    case 6:
                        return 63;
                    case 7:
                        return 127;
                    case 8:
                        return 255;
                }
            }
            else
            {
                switch (numBits)
                {
                    case 9:
                        return 511;
                    case 10:
                        return 1023;
                    case 11:
                        return 2047;
                    case 12:
                        return 4095;
                    case 13:
                        return 8191;
                    case 14:
                        return 16383;
                    case 15:
                        return 32767;
                    case 16:
                        return 65535;
                }
            }

            return 0;
        }

        #endregion
    }
}
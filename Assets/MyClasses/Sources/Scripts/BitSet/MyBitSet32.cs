/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyBitSet32 (version 1.2)
 */

using System;

namespace MyClasses
{
    public class MyBitSet32
    {
        #region ----- Variable -----

        private const byte NUM_BIT = 32;

        private uint mValue = 0;

        #endregion

        #region ----- Property -----

        public uint Value
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
                throw new Exception("[" + typeof(MyBitSet32).Name + "] SetValueOfBit(): Index out of range");
            }

            uint mask = 0x01;
            mask = (uint)(mask << index);
            mValue = value ? (uint)(mValue | mask) : (uint)(mValue & (~mask));
        }

        /// <summary>
        /// Return value of a bit by index.
        /// </summary>
        public bool GetValueOfBit(byte index)
        {
            if (index < 0 || index >= NUM_BIT)
            {
                throw new Exception("[" + typeof(MyBitSet32).Name + "] GetValueOfBit(): Index out of range");
            }

            uint mask = 0x01;
            mask = (uint)(mask << index);
            return (mValue & mask) != 0;
        }

        /// <summary>
        /// Set value of some bits by index.
        /// </summary>
        public void SetValueOfBits(byte fromIndex, byte toIndex, uint value)
        {
            byte numBits = (byte)(toIndex - fromIndex + 1);
            if (numBits <= 1)
            {
                throw new Exception("[" + typeof(MyBitSet32).Name + "] SetValueOfBits(): Number of bits less than or equals 1");
            }
            else if (numBits >= NUM_BIT)
            {
                throw new Exception("[" + typeof(MyBitSet32).Name + "] SetValueOfBits(): Number of bits greater than or equals " + NUM_BIT);
            }
            else if (value > _GetMaxValue(numBits))
            {
                throw new Exception("[" + typeof(MyBitSet32).Name + "] SetValueOfBits(): Value out of range");
            }

            for (byte i = fromIndex; i <= toIndex; i++)
            {
                SetValueOfBit(i, false);
            }
            mValue = (uint)(mValue | (value << fromIndex));
        }

        /// <summary>
        /// Return value of some bits by index.
        /// </summary>
        public uint GetValueOfBits(byte fromIndex, byte toIndex)
        {
            if (fromIndex >= toIndex)
            {
                throw new Exception("[" + typeof(MyBitSet32).Name + "] GetValueOfBits(): fromIndex greater than or equals toIndex");
            }

            byte length = (byte)(toIndex - fromIndex + 1);
            uint result = (uint)(mValue << (NUM_BIT - length - fromIndex));
            return result = (uint)(result >> (NUM_BIT - length));
        }

        #endregion

        #region ----- Private Method ------

        /// <summary>
        /// Return maximum value of variable by number of bits.
        /// </summary>
        private uint _GetMaxValue(byte numBits)
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
            else if (numBits <= 16)
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
            else if (numBits <= 24)
            {
                switch (numBits)
                {
                    case 17:
                        return 131071;
                    case 18:
                        return 262143;
                    case 19:
                        return 524287;
                    case 20:
                        return 1048575;
                    case 21:
                        return 2097151;
                    case 22:
                        return 4194303;
                    case 23:
                        return 8388607;
                    case 24:
                        return 16777215;
                }
            }
            else
            {
                switch (numBits)
                {
                    case 25:
                        return 33554431;
                    case 26:
                        return 67108863;
                    case 27:
                        return 134217727;
                    case 28:
                        return 268435456;
                    case 29:
                        return 536870911;
                    case 30:
                        return 1073741823;
                    case 31:
                        return 2147483647;
                    case 32:
                        return 4294967295;
                }
            }

            return 0;
        }

        #endregion
    }
}
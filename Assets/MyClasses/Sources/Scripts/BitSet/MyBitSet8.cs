/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyBitSet8 (version 1.2)
 */

using System;

namespace MyClasses
{
    public class MyBitSet8
    {
        #region ----- Variable -----

        private const int NUM_BIT = 8;

        private byte mValue = 0;

        #endregion

        #region ----- Property -----

        public byte Value
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
                throw new Exception("[" + typeof(MyBitSet8).Name + "] SetValueOfBit(): Index out of range");
            }

            byte mask = 0x01;
            mask = (byte)(mask << index);
            mValue = value ? (byte)(mValue | mask) : (byte)(mValue & (~mask));
        }

        /// <summary>
        /// Return value of a bit by index.
        /// </summary>
        public bool GetValueOfBit(byte index)
        {
            if (index < 0 || index >= NUM_BIT)
            {
                throw new Exception("[" + typeof(MyBitSet8).Name + "] GetValueOfBit(): Index out of range");
            }

            byte mask = 0x01;
            mask = (byte)(mask << index);
            return (mValue & mask) != 0;
        }

        /// <summary>
        /// Set value of some bits by index.
        /// </summary>
        public void SetValueOfBits(byte fromIndex, byte toIndex, byte value)
        {
            byte numBits = (byte)(toIndex - fromIndex + 1);
            if (numBits <= 1)
            {
                throw new Exception("[" + typeof(MyBitSet8).Name + "] SetValueOfBits(): Number of bits less than or equals 1");
            }
            else if (numBits >= NUM_BIT)
            {
                throw new Exception("[" + typeof(MyBitSet8).Name + "] SetValueOfBits(): Number of bits greater than or equals " + NUM_BIT);
            }
            else if (value > _GetMaxValue(numBits))
            {
                throw new Exception("[" + typeof(MyBitSet8).Name + "] SetValueOfBits(): Value out of range");
            }

            for (byte i = fromIndex; i <= toIndex; i++)
            {
                SetValueOfBit(i, false);
            }
            mValue = (byte)(mValue | (value << fromIndex));
        }

        /// <summary>
        /// Return value of some bits by index.
        /// </summary>
        public byte GetValueOfBits(byte fromIndex, byte toIndex)
        {
            if (fromIndex >= toIndex)
            {
                throw new Exception("[" + typeof(MyBitSet8).Name + "] GetValueOfBits(): fromIndex greater than or equals toIndex");
            }

            byte length = (byte)(toIndex - fromIndex + 1);
            byte result = (byte)(mValue << (NUM_BIT - length - fromIndex));
            return result = (byte)(result >> (NUM_BIT - length));
        }

        #endregion

        #region ----- Private Method ------

        /// <summary>
        /// Return maximum value of variable by number of bits.
        /// </summary>
        private byte _GetMaxValue(byte numBits)
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
                default:
                    return 0;
            }
        }

        #endregion
    }
}
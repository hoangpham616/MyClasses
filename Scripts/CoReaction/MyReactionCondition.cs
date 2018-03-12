/*
* Copyright (c) 2016 Phạm Minh Hoàng
* Framework:   MyClasses
* Class:       MyReactionCondition (version 1.0)
*/

using System;
using System.Collections.Generic;

namespace MyClasses.CoReaction
{
    public class MyReactionCondition
    {
        #region ----- Variable -----

        private int mParameterID;
        private EComparedType mComparedType;
        private object mComparedValue;
        private Func<MyReactionCondition.EComparedType, object, bool> mComparedFunction;

        #endregion

        #region ----- Property -----

        public int ParameterID
        {
            get { return mParameterID; }
        }

        public EComparedType ComparedType
        {
            get { return mComparedType; }
        }

        public object ComparedValue
        {
            get { return mComparedValue; }
        }

        public Func<MyReactionCondition.EComparedType, object, bool> ComparedFunction
        {
            get { return mComparedFunction; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyReactionCondition(int parameterID, EComparedType comparedType, object comparedValue)
        {
            mParameterID = parameterID;
            mComparedType = comparedType;
            mComparedValue = comparedValue;
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set the compared function.
        /// </summary>
        public void SetComparedFunction(Func<MyReactionCondition.EComparedType, object, bool> comparedFunction)
        {
            mComparedFunction = comparedFunction;
        }

        #endregion

        #region ----- Enumeration -----

        public enum EComparedType
        {
            FloatGreater,
            FloatLess,
            IntGreater,
            IntLess,
            IntEquals,
            IntNotEqual,
            BoolTrue,
            BoolFalse,
            Trigger,
        }

        #endregion
    }
}
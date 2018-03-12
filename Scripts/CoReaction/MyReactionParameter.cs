/*
* Copyright (c) 2016 Phạm Minh Hoàng
* Framework:   MyClasses
* Class:       MyReactionParameter (version 1.0)
*/

namespace MyClasses.CoReaction
{
    #region ----- Base Class -----

    public abstract class MyReactionParameter
    {
        #region ----- Variable -----

        protected EType mType;
        protected string mName;
        protected object mValue;

        #endregion

        #region ----- Property -----

        public EType Type
        {
            get { return mType; }
        }

        public string Name
        {
            get { return mName; }
        }

        public object Value
        {
            get { return mValue; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyReactionParameter(string name, object defaultValue)
        {
            SetName(name);
            SetValue(defaultValue);
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set the name.
        /// </summary>
        public void SetName(string name)
        {
            mName = name != null ? name : string.Empty;
        }

        /// <summary>
        /// Set the value.
        /// </summary>
        public virtual void SetValue(object value)
        {
        }

        /// <summary>
        /// Check if the value does not change.
        /// </summary>
        public virtual bool IsEqual(object comparedValue)
        {
            return false;
        }

        /// <summary>
        /// Check if the condition is satisfied.
        /// </summary>
        public virtual bool IsPass(MyReactionCondition.EComparedType comparedType, object comparedValue)
        {
            return false;
        }

        #endregion

        #region ----- Enumeration -----

        public enum EType
        {
            Float,
            Int,
            Bool,
            Trigger
        }

        #endregion
    }

    #endregion

    #region ----- Subclass -----

    public sealed class MyReactionFloatParameter : MyReactionParameter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MyReactionFloatParameter(string name, object defaultValue) : base(name, defaultValue)
        {
            mType = EType.Float;
        }

        /// <summary>
        /// Set the value.
        /// </summary>
        public override void SetValue(object value)
        {
            mValue = value != null && value is float ? (float)value : 0;
        }

        /// <summary>
        /// Check if the value does not change.
        /// </summary>
        public override bool IsEqual(object value)
        {
            if (value is float)
            {
                return UnityEngine.Mathf.Abs((float)mValue - (float)value) <= float.Epsilon;
            }
            return false;
        }

        /// <summary>
        /// Check if the condition is satisfied.
        /// </summary>
        public override bool IsPass(MyReactionCondition.EComparedType comparedType, object comparedValue)
        {
            if (comparedValue is float)
            {
                switch (comparedType)
                {
                    case MyReactionCondition.EComparedType.FloatGreater:
                        {
                            return (float)mValue > (float)comparedValue;
                        }
                    case MyReactionCondition.EComparedType.FloatLess:
                        {
                            return (float)mValue < (float)comparedValue;
                        }
                }
            }
            return false;
        }
    }

    public sealed class MyReactionIntParameter : MyReactionParameter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MyReactionIntParameter(string name, object defaultValue) : base(name, defaultValue)
        {
            mType = EType.Int;
        }

        /// <summary>
        /// Set the value.
        /// </summary>
        public override void SetValue(object value)
        {
            mValue = value != null && value is int ? (int)value : 0;
        }

        /// <summary>
        /// Check if the value does not change.
        /// </summary>
        public override bool IsEqual(object comparedValue)
        {
            if (comparedValue is int)
            {
                return (int)mValue == (int)comparedValue;
            }
            return false;
        }

        /// <summary>
        /// Check if the condition is satisfied.
        /// </summary>
        public override bool IsPass(MyReactionCondition.EComparedType comparedType, object comparedValue)
        {
            if (comparedValue is int)
            {
                switch (comparedType)
                {
                    case MyReactionCondition.EComparedType.IntEquals:
                        {
                            return (int)mValue == (int)comparedValue;
                        }
                    case MyReactionCondition.EComparedType.IntNotEqual:
                        {
                            return (int)mValue != (int)comparedValue;
                        }
                    case MyReactionCondition.EComparedType.IntGreater:
                        {
                            return (int)mValue > (int)comparedValue;
                        }
                    case MyReactionCondition.EComparedType.IntLess:
                        {
                            return (int)mValue < (int)comparedValue;
                        }
                }
            }

            return false;
        }
    }

    public sealed class MyReactionBoolParameter : MyReactionParameter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MyReactionBoolParameter(string name, object defaultValue) : base(name, defaultValue)
        {
            mType = EType.Bool;
        }

        /// <summary>
        /// Set the value.
        /// </summary>
        public override void SetValue(object value)
        {
            mValue = value != null && value is bool ? (bool)value : false;
        }

        /// <summary>
        /// Check if the value does not change.
        /// </summary>
        public override bool IsEqual(object value)
        {
            if (value is bool)
            {
                return (bool)mValue == (bool)value;
            }
            return false;
        }

        /// <summary>
        /// Check if the condition is satisfied.
        /// </summary>
        public override bool IsPass(MyReactionCondition.EComparedType comparedType, object comparedValue)
        {
            switch (comparedType)
            {
                case MyReactionCondition.EComparedType.BoolTrue:
                    {
                        return (bool)mValue == true;
                    }
                case MyReactionCondition.EComparedType.BoolFalse:
                    {
                        return (bool)mValue == false;
                    }
            }
            return false;
        }
    }

    public sealed class MyReactionTriggerParameter : MyReactionParameter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MyReactionTriggerParameter(string name, object defaultValue) : base(name, defaultValue)
        {
            mType = EType.Trigger;
        }

        /// <summary>
        /// Set the value.
        /// </summary>
        public override void SetValue(object value)
        {
            mValue = value != null && value is bool ? (bool)value : false;
        }

        /// <summary>
        /// Check if the value does not change.
        /// </summary>
        public override bool IsEqual(object comparedValue)
        {
            if (comparedValue is bool)
            {
                return (bool)mValue == (bool)comparedValue;
            }
            return false;
        }

        /// <summary>
        /// Check if the condition is satisfied.
        /// </summary>
        public override bool IsPass(MyReactionCondition.EComparedType comparedType, object comparedValue)
        {
            switch (comparedType)
            {
                case MyReactionCondition.EComparedType.Trigger:
                    {
                        return (bool)mValue;
                    }
            }
            return false;
        }
    }

    #endregion
}
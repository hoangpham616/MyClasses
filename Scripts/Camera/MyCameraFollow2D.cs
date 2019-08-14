/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyCameraFollow2D (version 1.1)
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace MyClasses
{
    [RequireComponent(typeof(Camera))]
    public class MyCameraFollow2D : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private Transform mTarget;

        [SerializeField]
        private float mSmoothTimeX = 0.05f;
        [SerializeField]
        private float mSmoothTimeY = 0.05f;

        [SerializeField]
        private bool mIsBound = false;
        [SerializeField]
        private Vector2 mMinPosition = new Vector2(-10, -5);
        [SerializeField]
        private Vector2 mMaxPosition = new Vector2(10, 5);

        private Vector2 mVelocity;

        #endregion

        #region ----- Property -----

        public Vector2 Velocity
        {
            get { return mVelocity; }
        }

#if UNITY_EDITOR

        public Transform Target
        {
            get { return mTarget; }
            set { mTarget = value; }
        }

        public float SmoothTimeX
        {
            get { return mSmoothTimeX; }
            set { mSmoothTimeX = value; }
        }

        public float SmoothTimeY
        {
            get { return mSmoothTimeY; }
            set { mSmoothTimeY = value; }
        }

        public bool IsBound
        {
            get { return mIsBound; }
            set { mIsBound = value; }
        }

        public Vector2 MinPosition
        {
            get { return mMinPosition; }
            set { mMinPosition = value; }
        }

        public Vector2 MaxPosition
        {
            get { return mMaxPosition; }
            set { mMaxPosition = value; }
        }

#endif

        #endregion

        #region ----- MonoBehaviour Event -----

        /// <summary>
        /// LateUpdate.
        /// </summary>
        void LateUpdate()
        {
            if (mTarget != null)
            {
                Vector3 newPos = transform.position;

                newPos.x = Mathf.SmoothDamp(transform.position.x, mTarget.transform.position.x, ref mVelocity.x, mSmoothTimeX);
                newPos.y = Mathf.SmoothDamp(transform.position.y, mTarget.transform.position.y, ref mVelocity.y, mSmoothTimeY);

                if (mIsBound)
                {
                    newPos.x = Mathf.Clamp(newPos.x, mMinPosition.x, mMaxPosition.x);
                    newPos.y = Mathf.Clamp(newPos.y, mMinPosition.y, mMaxPosition.y);
                }

                transform.position = newPos;
            }
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyCameraFollow2D))]
    public class MyCameraFollow2DEditor : Editor
    {
        private MyCameraFollow2D mScript;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyCameraFollow2D)target;
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyCameraFollow2D), false);

            mScript.Target = (Transform)EditorGUILayout.ObjectField("Target", mScript.Target, typeof(Transform), true);

            mScript.SmoothTimeX = EditorGUILayout.FloatField("Smooth Time X", mScript.SmoothTimeX);
            mScript.SmoothTimeY = EditorGUILayout.FloatField("Smooth Time Y", mScript.SmoothTimeY);

            mScript.IsBound = EditorGUILayout.Toggle("Is Bound", mScript.IsBound);
            if (mScript.IsBound)
            {
                EditorGUILayout.BeginHorizontal();
                mScript.MinPosition = EditorGUILayout.Vector2Field("Min Position", mScript.MinPosition);
                if (GUILayout.Button("Set Min Camera Position"))
                {
                    mScript.MinPosition = new Vector2(mScript.transform.position.x, mScript.transform.position.y);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                mScript.MaxPosition = EditorGUILayout.Vector2Field("Max Position", mScript.MaxPosition);
                if (GUILayout.Button("Set Max Camera Position"))
                {
                    mScript.MaxPosition = new Vector2(mScript.transform.position.x, mScript.transform.position.y);
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Velocity: (" + mScript.Velocity.x + ", " + mScript.Velocity.y + ")");
        }
    }

#endif
}

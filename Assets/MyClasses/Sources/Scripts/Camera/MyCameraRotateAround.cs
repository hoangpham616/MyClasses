/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyCameraRotateAround (version 1.0)
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace MyClasses
{
    public class MyCameraRotateAround : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private Transform mCamera;
        [SerializeField]
        private Transform mTarget;

        [SerializeField]
        private Vector2 mLimitXAxis = new Vector2(int.MinValue, int.MaxValue);
        [SerializeField]
        private Vector2 mLimitYAxis = new Vector2(int.MinValue, int.MaxValue);

        [SerializeField]
        private Vector2 mCameraOriginalEulerAngles;
        [SerializeField]
        private Vector2 mCameraOriginalOffset;

        #endregion

        #region ----- Property -----

        public Transform Camera
        {
            get { return mCamera; }
        }

        public Transform Target
        {
            get { return mTarget; }
        }

        public Vector2 LimitXAxis
        {
            get { return mLimitXAxis; }
        }

        public Vector2 LimitYAxis
        {
            get { return mLimitYAxis; }
        }

        public Vector2 CameraOriginalEulerAngles
        {
            get { return mCameraOriginalEulerAngles; }
        }

        public Vector2 CameraOriginalOffset
        {
            get { return mCameraOriginalOffset; }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Setup camera & target.
        /// </summary>
        public void Setup(Transform camera, Transform target, bool isKeepCameraOffset = true)
        {
            mCamera = camera;
            mTarget = target;
            mCameraOriginalEulerAngles = mCamera.eulerAngles;
            mCameraOriginalOffset = isKeepCameraOffset ? mCamera.position - mTarget.position : Vector3.zero;
        }

        /// <summary>
        /// Set limit of vertical axis.
        /// </summary>
        public void SetVerticalAxisLimit(float min, float max)
        {
            mLimitXAxis.x = min;
            mLimitXAxis.y = max;
        }

        /// <summary>
        /// Set limit of horizontal axis.
        /// </summary>
        public void SetHorizontalAxisLimit(float min, float max)
        {
            mLimitYAxis.x = min;
            mLimitYAxis.y = max;
        }

        /// <summary>
        /// Rotate the camera around the target.
        /// You should call this in Update method.
        /// </summary>
        public void Rotate(float deltaX, float deltaY)
        {
            Vector3 cameraPosition = mCamera.position;
            cameraPosition.x -= mCameraOriginalOffset.x;
            cameraPosition.y -= mCameraOriginalOffset.y;
            float distance = Vector3.Magnitude(cameraPosition - mTarget.position);

            mCameraOriginalEulerAngles.x += deltaY;
            mCameraOriginalEulerAngles.x = _ClampAngle(mCameraOriginalEulerAngles.x, mLimitXAxis.x, mLimitXAxis.y);

            mCameraOriginalEulerAngles.y += deltaX;
            mCameraOriginalEulerAngles.y = _ClampAngle(mCameraOriginalEulerAngles.y, mLimitYAxis.x, mLimitYAxis.y);

            Quaternion rotation = Quaternion.Euler(mCameraOriginalEulerAngles.x, mCameraOriginalEulerAngles.y, 0);
            Vector3 negativeDistance = Vector3.zero;
            negativeDistance.z = -distance;

            Vector3 position = rotation * negativeDistance;
            position.x += mCameraOriginalOffset.x;
            position.y += mCameraOriginalOffset.y;

            mCamera.rotation = rotation;
            mCamera.position = position;
        }

        #endregion

        private float _ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            else if (angle > 360F)
            {
                angle -= 360F;
            }
            return Mathf.Clamp(angle, min, max);
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyCameraRotateAround))]
    public class MyCameraRotateAroundEditor : Editor
    {
        private MyCameraRotateAround mScript;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyCameraRotateAround)target;
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyCameraRotateAround), false);

            EditorGUILayout.ObjectField("Camera", mScript.Camera, typeof(Transform), false);
            EditorGUILayout.ObjectField("Target", mScript.Target, typeof(Transform), false);

            EditorGUILayout.Vector2Field("X Axis Limit", mScript.LimitXAxis);
            EditorGUILayout.Vector2Field("Y Axis Limit", mScript.LimitYAxis);

            EditorGUILayout.Vector2Field("Original Euler Angels of Camera", mScript.CameraOriginalEulerAngles);
            EditorGUILayout.Vector2Field("Original Offset of Camera", mScript.CameraOriginalOffset);
        }
    }

#endif
}

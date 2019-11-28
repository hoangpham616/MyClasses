/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyFieldOfView (version 1.0)
 */

using UnityEngine;
using System.Collections.Generic;

namespace MyClasses
{
    public class MyFieldOfView : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        [Range(0, 360)]
        private float _angle = 120;
        [SerializeField]
        [Range(0.1f, 3)]
        private float _anglePerDraw = 2;
        [SerializeField]
        private float _radius = 5;
        [SerializeField]
        private float _edgeDistanceThreshold = 1;
        [SerializeField]
        private float _cutwayDistance = 0.1f;
        [SerializeField]
        private LayerMask _obstacleMask;
        [SerializeField]
        private MeshFilter _meshFilter;

        private Mesh _mesh;

        #endregion

        #region ----- Property -----

        public float Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        public float AnglePerDraw
        {
            get { return _anglePerDraw; }
            set { _anglePerDraw = Mathf.Clamp(value, 0.1f, 3); }
        }

        public float Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        public float EdgeDistanceThreshold
        {
            get { return _edgeDistanceThreshold; }
            set { _edgeDistanceThreshold = value; }
        }

        public float CutwayDistance
        {
            get { return _cutwayDistance; }
            set { _cutwayDistance = value; }
        }

        public LayerMask ObstacleMask
        {
            get { return _obstacleMask; }
            set { _obstacleMask = value; }
        }

        #endregion

        #region ----- Implement MonoBehaviour -----

        /// <summary>
        /// Start.
        /// </summary>
        private void Start()
        {
            _mesh = new Mesh();
            _meshFilter.mesh = _mesh;
        }

        /// <summary>
        /// LateUpdate.
        /// </summary>
        private void LateUpdate()
        {
            _DrawFieldOfView();
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Draw field of view.
        /// </summary>
        private void _DrawFieldOfView()
        {
            List<Vector3> viewPoints = new List<Vector3>();
            ViewInfo oldViewInfo = new ViewInfo();
            int numLine = (int)(_angle / _anglePerDraw) + 1;
            float realAnglePerDraw = _angle / numLine;
            float startAngle = transform.eulerAngles.y - (_angle / 2);
            for (int i = 0; i < numLine; ++i)
            {
                float angle = startAngle + (realAnglePerDraw * i);
                ViewInfo newViewCast = _FindView(angle);
                if (i > 0)
                {
                    bool isEdgeDistanceExceeded = Mathf.Abs(oldViewInfo.Distance - newViewCast.Distance) > _edgeDistanceThreshold;
                    if ((oldViewInfo.IsHit != newViewCast.IsHit) || (isEdgeDistanceExceeded && oldViewInfo.IsHit && newViewCast.IsHit))
                    {
                        EdgeInfo edge = _FindEdge(oldViewInfo, newViewCast);
                        if (edge.PointA != Vector3.zero)
                        {
                            viewPoints.Add(edge.PointA);
                        }
                        if (edge.PointB != Vector3.zero)
                        {
                            viewPoints.Add(edge.PointB);
                        }
                    }
                }
                viewPoints.Add(newViewCast.Point);
                oldViewInfo = newViewCast;
            }

            int numVertex = viewPoints.Count + 1;
            int[] triangles = new int[(numVertex - 2) * 3];
            Vector3[] vertices = new Vector3[numVertex];
            vertices[0] = Vector3.zero;
            Vector3 cutwayDistance = Vector3.forward * _cutwayDistance;
            for (int i = 0; i < numVertex - 1; ++i)
            {
                int iPlusOne = i + 1;
                vertices[iPlusOne] = transform.InverseTransformPoint(viewPoints[i]) + cutwayDistance;
                if (i < numVertex - 2)
                {
                    int iPlusTwo = i + 2;
                    int triangleFirstIndex = i * 3;
                    triangles[triangleFirstIndex] = 0;
                    triangles[triangleFirstIndex + 1] = iPlusOne;
                    triangles[triangleFirstIndex + 2] = iPlusTwo;
                }
            }

            _mesh.Clear();
            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.RecalculateNormals();
        }

        /// <summary>
        /// Find view info.
        /// </summary>
        private ViewInfo _FindView(float angle)
        {
            Vector3 dir = Vector3.zero;
            dir.x = Mathf.Sin(angle * Mathf.Deg2Rad);
            dir.z = Mathf.Cos(angle * Mathf.Deg2Rad);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, _radius, _obstacleMask))
            {
                return new ViewInfo(hit.point, hit.distance, angle, true);
            }

            return new ViewInfo(transform.position + dir * _radius, _radius, angle, false);
        }

        /// <summary>
        /// Find edge info.
        /// </summary>
        private EdgeInfo _FindEdge(ViewInfo minViewInfo, ViewInfo maxViewInfo)
        {
            float minAngle = minViewInfo.Angle;
            float maxAngle = maxViewInfo.Angle;
            Vector3 minPoint = Vector3.zero;
            Vector3 maxPoint = Vector3.zero;

            for (int i = 0; i < 4; i++)
            {
                float angle = (minAngle + maxAngle) / 2;
                ViewInfo newViewCast = _FindView(angle);

                bool isEdgeDistanceExceeded = Mathf.Abs(minViewInfo.Distance - newViewCast.Distance) > _edgeDistanceThreshold;
                if (newViewCast.IsHit == minViewInfo.IsHit && !isEdgeDistanceExceeded)
                {
                    minAngle = angle;
                    minPoint = newViewCast.Point;
                }
                else
                {
                    maxAngle = angle;
                    maxPoint = newViewCast.Point;
                }
            }

            return new EdgeInfo(minPoint, maxPoint);
        }

        #endregion

        #region ----- Internal Class -----

        public struct ViewInfo
        {
            public Vector3 Point;
            public float Distance;
            public float Angle;
            public bool IsHit;

            public ViewInfo(Vector3 point, float distance, float angle, bool isHit)
            {
                Point = point;
                Distance = distance;
                Angle = angle;
                IsHit = isHit;
            }
        }

        public struct EdgeInfo
        {
            public Vector3 PointA;
            public Vector3 PointB;

            public EdgeInfo(Vector3 pointA, Vector3 pointB)
            {
                PointA = pointA;
                PointB = pointB;
            }
        }

        #endregion
    }
}
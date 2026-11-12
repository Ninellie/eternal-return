using System;
using UnityEngine;

namespace EternalReturn.Stress.Collision_System
{
    [Serializable]
    public class Mesh2D
    {
        [SerializeField] private Polygon[] polygons;

        [NonSerialized] private float _boundingRadius;
        [NonSerialized] private bool _isBoundingRadiusCalculated;

        public Polygon[] Polygons => polygons;

        public float BoundingRadius
        {
            get
            {
                if (_isBoundingRadiusCalculated) return _boundingRadius;

                _boundingRadius = CalculateBoundingRadius();
                _isBoundingRadiusCalculated = true;
                return _boundingRadius;
            }
        }

        public Mesh2D(Polygon[] polygons)
        {
            this.polygons = polygons;
        }

        private float CalculateBoundingRadius()
        {
            if (polygons == null) return 0f;

            var maxSqrRadius = 0f;
            foreach (var polygon in polygons)
            {
                maxSqrRadius = Mathf.Max(maxSqrRadius, polygon.A.sqrMagnitude);
                maxSqrRadius = Mathf.Max(maxSqrRadius, polygon.B.sqrMagnitude);
                maxSqrRadius = Mathf.Max(maxSqrRadius, polygon.C.sqrMagnitude);
            }

            return Mathf.Sqrt(maxSqrRadius);
        }
    }
}

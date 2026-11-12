using System;
using UnityEngine;

namespace EternalReturn.Stress.Collision_System
{
    [Serializable]
    public class Mesh2D
    {
        [SerializeField] private Polygon[] polygons;

        public Polygon[] Polygons => polygons;

        public Mesh2D(Polygon[] polygons)
        {
            this.polygons = polygons;
        }
    }
}

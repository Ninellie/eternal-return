using EternalReturn.Stress.Collision_System;
using UnityEngine;

namespace EternalReturn.Stress.Mini_Game
{
    [CreateAssetMenu(menuName = "Eternal Return/Shape 2D")]
    public class Shape2D : ScriptableObject
    {
        [SerializeField] private Mesh2D mesh = CollisionPrimitives.Triangle();

        public Mesh2D Mesh => mesh;

        public void SetMesh(Mesh2D newMesh)
        {
            mesh = newMesh;
        }
    }
}

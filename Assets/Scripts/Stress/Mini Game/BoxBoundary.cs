using EternalReturn.Stress.Collision_System;
using UnityEngine;

namespace EternalReturn.Stress.Mini_Game
{
    public static class BoxBoundary
    {
        public static Vector2 GetPushInsideOffset(BoxFormation box, Transform2D objectTransform, Mesh2D mesh)
        {
            var polygons = mesh?.Polygons;
            if (polygons == null || polygons.Length == 0) return Vector2.zero;
            if (!box.Transform.HasValidScale) return Vector2.zero;

            var min = new Vector2(float.MaxValue, float.MaxValue);
            var max = new Vector2(float.MinValue, float.MinValue);
            foreach (var polygon in polygons)
            {
                Expand(ref min, ref max, box.Transform, objectTransform, polygon.A);
                Expand(ref min, ref max, box.Transform, objectTransform, polygon.B);
                Expand(ref min, ref max, box.Transform, objectTransform, polygon.C);
            }

            var half = box.Size * 0.5f;
            var localShift = new Vector2(
                GetAxisShift(min.x, max.x, half.x),
                GetAxisShift(min.y, max.y, half.y));

            return box.Transform.TransformVector(localShift);
        }

        private static void Expand(
            ref Vector2 min,
            ref Vector2 max,
            Transform2D boxTransform,
            Transform2D objectTransform,
            Vector2 vertex)
        {
            var local = boxTransform.InverseTransformPoint(objectTransform.TransformPoint(vertex));
            min = Vector2.Min(min, local);
            max = Vector2.Max(max, local);
        }

        private static float GetAxisShift(float min, float max, float half)
        {
            if (max - min > half * 2f) return -(min + max) * 0.5f;
            if (max > half) return half - max;
            if (min < -half) return -half - min;

            return 0f;
        }
    }
}

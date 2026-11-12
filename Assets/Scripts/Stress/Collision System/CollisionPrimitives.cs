using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EternalReturn.Stress.Collision_System
{
    public static class CollisionPrimitives
    {
        private const float MinPolygonArea = 0.000001f;

        private static readonly Func<Mesh2D>[] Factories =
        {
            Triangle,
            Heart,
            Bullet,
            Square,
            Rhombus,
            Hexagon,
            FourPointStar,
            FivePointStar,
            SixPointStar,
            Crescent,
        };

        public static Mesh2D CreateRandom()
        {
            return Factories[Random.Range(0, Factories.Length)]();
        }

        public static Mesh2D Triangle()
        {
            return new Mesh2D(new[]
            {
                Create(new Vector2(-0.5f, -0.33333f), new Vector2(0.5f, -0.33333f), new Vector2(0f, 0.66667f)),
            });
        }

        public static Mesh2D Heart()
        {
            return new Mesh2D(new[]
            {
                Create(new Vector2(-0.5f, 0.12724f), new Vector2(0f, -0.57276f), new Vector2(0f, 0.02724f)),
                Create(new Vector2(-0.5f, 0.12724f), new Vector2(0f, 0.02724f), new Vector2(0f, 0.17724f)),
                Create(new Vector2(-0.5f, 0.12724f), new Vector2(-0.25f, 0.42724f), new Vector2(0f, 0.17724f)),
                Create(new Vector2(0.5f, 0.12724f), new Vector2(0f, -0.57276f), new Vector2(0f, 0.02724f)),
                Create(new Vector2(0.5f, 0.12724f), new Vector2(0f, 0.02724f), new Vector2(0f, 0.17724f)),
                Create(new Vector2(0.5f, 0.12724f), new Vector2(0.25f, 0.42724f), new Vector2(0f, 0.17724f)),
            });
        }

        public static Mesh2D Bullet()
        {
            return new Mesh2D(new[]
            {
                Create(new Vector2(-0.25f, -0.40833f), new Vector2(0.25f, -0.40833f), new Vector2(0.25f, 0.19167f)),
                Create(new Vector2(-0.25f, -0.40833f), new Vector2(0.25f, 0.19167f), new Vector2(-0.25f, 0.19167f)),
                Create(new Vector2(-0.25f, 0.19167f), new Vector2(0.25f, 0.19167f), new Vector2(0f, 0.59167f)),
            });
        }

        public static Mesh2D Square()
        {
            return FromRing(
                new Vector2(-0.5f, -0.5f),
                new Vector2(-0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, -0.5f));
        }

        public static Mesh2D Rhombus()
        {
            return FromRing(
                new Vector2(0f, 0.5f),
                new Vector2(0.3f, 0f),
                new Vector2(0f, -0.5f),
                new Vector2(-0.3f, 0f));
        }

        public static Mesh2D Hexagon()
        {
            return FromRing(CreateRegularRing(6, 0.5f, 0f));
        }

        public static Mesh2D FourPointStar()
        {
            return FromRing(
                new Vector2(0f, 0.5f),
                new Vector2(0.08f, 0.08f),
                new Vector2(0.25f, 0f),
                new Vector2(0.08f, -0.08f),
                new Vector2(0f, -0.5f),
                new Vector2(-0.08f, -0.08f),
                new Vector2(-0.25f, 0f),
                new Vector2(-0.08f, 0.08f));
        }

        public static Mesh2D FivePointStar()
        {
            return FromRing(CreateStarRing(5, 0.5f, 0.19098f));
        }

        public static Mesh2D SixPointStar()
        {
            return FromRing(CreateStarRing(6, 0.5f, 0.28868f));
        }

        public static Mesh2D Crescent()
        {
            const float outerRadius = 0.5f;
            const float innerRadius = 0.4f;
            const float innerOffset = 0.2f;
            const int segments = 8;

            var tipX = (outerRadius * outerRadius - innerRadius * innerRadius + innerOffset * innerOffset)
                       / (2f * innerOffset);
            var tipY = Mathf.Sqrt(outerRadius * outerRadius - tipX * tipX);
            var outerTipAngle = Mathf.Atan2(tipY, tipX);
            var innerTipAngle = Mathf.Atan2(tipY, tipX - innerOffset);

            var outer = new Vector2[segments + 1];
            var inner = new Vector2[segments + 1];
            for (var i = 0; i <= segments; i++)
            {
                var t = i / (float)segments;
                var outerAngle = Mathf.Lerp(outerTipAngle, Mathf.PI * 2f - outerTipAngle, t);
                var innerAngle = Mathf.Lerp(innerTipAngle, Mathf.PI * 2f - innerTipAngle, t);
                outer[i] = new Vector2(Mathf.Cos(outerAngle), Mathf.Sin(outerAngle)) * outerRadius;
                inner[i] = new Vector2(innerOffset, 0f)
                           + new Vector2(Mathf.Cos(innerAngle), Mathf.Sin(innerAngle)) * innerRadius;
            }

            var polygons = new List<Polygon>();
            for (var i = 0; i < segments; i++)
            {
                AddIfValid(polygons, outer[i], outer[i + 1], inner[i]);
                AddIfValid(polygons, outer[i + 1], inner[i + 1], inner[i]);
            }

            return Centered(polygons);
        }

        private static Mesh2D FromRing(params Vector2[] ring)
        {
            var polygons = new Polygon[ring.Length];
            for (var i = 0; i < ring.Length; i++)
            {
                polygons[i] = Create(Vector2.zero, ring[i], ring[(i + 1) % ring.Length]);
            }

            return new Mesh2D(polygons);
        }

        private static Vector2[] CreateRegularRing(int sides, float radius, float startAngle)
        {
            var ring = new Vector2[sides];
            for (var i = 0; i < sides; i++)
            {
                var angle = startAngle + i * Mathf.PI * 2f / sides;
                ring[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            }

            return ring;
        }

        private static Vector2[] CreateStarRing(int points, float outerRadius, float innerRadius)
        {
            var ring = new Vector2[points * 2];
            for (var i = 0; i < ring.Length; i++)
            {
                var angle = Mathf.PI * 0.5f + i * Mathf.PI / points;
                var radius = i % 2 == 0 ? outerRadius : innerRadius;
                ring[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            }

            return ring;
        }

        private static void AddIfValid(List<Polygon> polygons, Vector2 a, Vector2 b, Vector2 c)
        {
            if (GetArea(a, b, c) < MinPolygonArea) return;

            polygons.Add(Create(a, b, c));
        }

        private static Mesh2D Centered(List<Polygon> polygons)
        {
            var weightedSum = Vector2.zero;
            var totalArea = 0f;
            foreach (var polygon in polygons)
            {
                var area = GetArea(polygon.A, polygon.B, polygon.C);
                weightedSum += area * (polygon.A + polygon.B + polygon.C) / 3f;
                totalArea += area;
            }

            var centroid = weightedSum / totalArea;
            var result = new Polygon[polygons.Count];
            for (var i = 0; i < result.Length; i++)
            {
                var polygon = polygons[i];
                result[i] = Create(polygon.A - centroid, polygon.B - centroid, polygon.C - centroid);
            }

            return new Mesh2D(result);
        }

        private static float GetArea(Vector2 a, Vector2 b, Vector2 c)
        {
            var ab = b - a;
            var ac = c - a;
            return Mathf.Abs(ab.x * ac.y - ab.y * ac.x) * 0.5f;
        }

        private static Polygon Create(Vector2 a, Vector2 b, Vector2 c)
        {
            return new Polygon { A = a, B = b, C = c };
        }
    }
}

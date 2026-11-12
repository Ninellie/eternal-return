using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Stress.Collision_System
{
    public class CollisionSystem : IFixedTickable
    {
        private readonly List<Entry> _entries = new();
        private readonly List<Entry> _snapshot = new();
        private readonly Dictionary<IColliding, Entry> _entriesByCollider = new();

        public void Register(IColliding colliding)
        {
            if (_entriesByCollider.ContainsKey(colliding)) return;

            var entry = new Entry(colliding);
            _entries.Add(entry);
            _entriesByCollider.Add(colliding, entry);
        }

        public void Unregister(IColliding colliding)
        {
            if (!_entriesByCollider.TryGetValue(colliding, out var entry)) return;

            entry.IsRegistered = false;
            _entries.Remove(entry);
            _entriesByCollider.Remove(colliding);

            foreach (var other in entry.Contacts)
            {
                if (!_entriesByCollider.TryGetValue(other, out var otherEntry)) continue;

                otherEntry.Contacts.Remove(colliding);
                other.OnCollisionExit(colliding);
            }
        }

        public void FixedTick()
        {
            _snapshot.Clear();
            _snapshot.AddRange(_entries);

            foreach (var entry in _snapshot)
            {
                entry.PrepareForTick();
            }

            for (var i = 0; i < _snapshot.Count; i++)
            {
                for (var j = i + 1; j < _snapshot.Count; j++)
                {
                    ProcessPair(_snapshot[i], _snapshot[j]);
                }
            }
        }

        private static bool CanCollide(Entry first, Entry second)
        {
            var firstCollider = first.Collider;
            var secondCollider = second.Collider;

            return (firstCollider.CollidesWith & secondCollider.Layer) != 0
                && (secondCollider.CollidesWith & firstCollider.Layer) != 0;
        }

        private static void ProcessPair(Entry first, Entry second)
        {
            if (!first.IsRegistered || !second.IsRegistered) return;
            if (!CanCollide(first, second)) return;

            var wasColliding = first.Contacts.Contains(second.Collider);
            var isColliding = Overlaps(first, second);

            if (isColliding && !wasColliding)
            {
                first.Contacts.Add(second.Collider);
                second.Contacts.Add(first.Collider);
                first.Collider.OnCollisionEnter(second.Collider);
                second.Collider.OnCollisionEnter(first.Collider);
                return;
            }

            if (isColliding) return;
            if (!wasColliding) return;

            first.Contacts.Remove(second.Collider);
            second.Contacts.Remove(first.Collider);
            first.Collider.OnCollisionExit(second.Collider);
            second.Collider.OnCollisionExit(first.Collider);
        }

        private static bool Overlaps(Entry first, Entry second)
        {
            if (first.PolygonCount == 0 || second.PolygonCount == 0) return false;
            if (!CirclesOverlap(first, second)) return false;

            first.EnsureWorldPolygons();
            second.EnsureWorldPolygons();

            for (var i = 0; i < first.PolygonCount; i++)
            {
                for (var j = 0; j < second.PolygonCount; j++)
                {
                    if (HasSeparatingAxis(in first.WorldPolygons[i], in second.WorldPolygons[j])) continue;
                    if (HasSeparatingAxis(in second.WorldPolygons[j], in first.WorldPolygons[i])) continue;

                    return true;
                }
            }

            return false;
        }

        private static bool CirclesOverlap(Entry first, Entry second)
        {
            var radius = first.Radius + second.Radius;
            return (first.Center - second.Center).sqrMagnitude <= radius * radius;
        }

        private static Vector2 GetVertex(in Polygon polygon, int index)
        {
            return index switch
            {
                0 => polygon.A,
                1 => polygon.B,
                _ => polygon.C,
            };
        }

        private static bool HasSeparatingAxis(in Polygon axisSource, in Polygon other)
        {
            for (var i = 0; i < 3; i++)
            {
                var edge = GetVertex(in axisSource, (i + 1) % 3) - GetVertex(in axisSource, i);
                var axis = new Vector2(-edge.y, edge.x);

                Project(in axisSource, axis, out var minA, out var maxA);
                Project(in other, axis, out var minB, out var maxB);

                if (maxA < minB || maxB < minA) return true;
            }

            return false;
        }

        private static void Project(in Polygon polygon, Vector2 axis, out float min, out float max)
        {
            var a = Vector2.Dot(polygon.A, axis);
            var b = Vector2.Dot(polygon.B, axis);
            var c = Vector2.Dot(polygon.C, axis);

            min = Mathf.Min(a, Mathf.Min(b, c));
            max = Mathf.Max(a, Mathf.Max(b, c));
        }

        private class Entry
        {
            public readonly IColliding Collider;
            public readonly HashSet<IColliding> Contacts = new();

            public bool IsRegistered = true;
            public Vector2 Center;
            public float Radius;
            public int PolygonCount;
            public Polygon[] WorldPolygons;

            private bool _isWorldReady;

            public Entry(IColliding collider)
            {
                Collider = collider;
            }

            public void PrepareForTick()
            {
                var mesh = Collider.Mesh;
                var polygons = mesh?.Polygons;
                var scale = Collider.Transform.Scale;

                PolygonCount = polygons?.Length ?? 0;
                Center = Collider.Transform.Position;
                Radius = mesh == null ? 0f : mesh.BoundingRadius * Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.y));
                _isWorldReady = false;
            }

            public void EnsureWorldPolygons()
            {
                if (_isWorldReady) return;

                if (WorldPolygons == null || WorldPolygons.Length < PolygonCount)
                {
                    WorldPolygons = new Polygon[PolygonCount];
                }

                var transform = Collider.Transform;
                var radians = transform.Rotation * Mathf.Deg2Rad;
                var cos = Mathf.Cos(radians);
                var sin = Mathf.Sin(radians);
                var polygons = Collider.Mesh.Polygons;

                for (var i = 0; i < PolygonCount; i++)
                {
                    WorldPolygons[i] = new Polygon
                    {
                        A = ToWorld(polygons[i].A, transform, cos, sin),
                        B = ToWorld(polygons[i].B, transform, cos, sin),
                        C = ToWorld(polygons[i].C, transform, cos, sin),
                    };
                }

                _isWorldReady = true;
            }

            private static Vector2 ToWorld(Vector2 local, Transform2D transform, float cos, float sin)
            {
                var scaled = Vector2.Scale(local, transform.Scale);
                var rotated = new Vector2(scaled.x * cos - scaled.y * sin, scaled.x * sin + scaled.y * cos);
                return transform.Position + rotated;
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Stress
{
    public class CollisionSystem : IFixedTickable
    {
        private readonly List<IColliding> _colliders = new();
        private readonly List<IColliding> _snapshot = new();
        private readonly Dictionary<IColliding, HashSet<IColliding>> _contacts = new();

        public void Register(IColliding colliding)
        {
            if (_contacts.ContainsKey(colliding)) return;

            _colliders.Add(colliding);
            _contacts.Add(colliding, new HashSet<IColliding>());
        }

        public void Unregister(IColliding colliding)
        {
            if (!_contacts.TryGetValue(colliding, out var contacts)) return;

            _colliders.Remove(colliding);
            _contacts.Remove(colliding);

            foreach (var other in contacts)
            {
                if (!_contacts.TryGetValue(other, out var otherContacts)) continue;

                otherContacts.Remove(colliding);
                other.OnCollisionExit(colliding);
            }
        }

        public void FixedTick()
        {
            _snapshot.Clear();
            _snapshot.AddRange(_colliders);

            for (var i = 0; i < _snapshot.Count; i++)
            {
                for (var j = i + 1; j < _snapshot.Count; j++)
                {
                    ProcessPair(_snapshot[i], _snapshot[j]);
                }
            }
        }

        private void ProcessPair(IColliding first, IColliding second)
        {
            if (!_contacts.TryGetValue(first, out var firstContacts)) return;
            if (!_contacts.TryGetValue(second, out var secondContacts)) return;

            var wasColliding = firstContacts.Contains(second);
            var isColliding = Overlaps(first, second);

            if (isColliding && !wasColliding)
            {
                firstContacts.Add(second);
                secondContacts.Add(first);
                first.OnCollisionEnter(second);
                second.OnCollisionEnter(first);
                return;
            }

            if (isColliding)
            {
                first.OnCollisionStay(second);
                second.OnCollisionStay(first);
                return;
            }

            if (!wasColliding) return;

            firstContacts.Remove(second);
            secondContacts.Remove(first);
            first.OnCollisionExit(second);
            second.OnCollisionExit(first);
        }

        private static bool Overlaps(IColliding first, IColliding second)
        {
            var firstPolygons = first.Polygons;
            var secondPolygons = second.Polygons;
            if (firstPolygons == null || secondPolygons == null) return false;

            foreach (var firstPolygon in firstPolygons)
            {
                foreach (var secondPolygon in secondPolygons)
                {
                    var firstVertices = ToWorld(firstPolygon, first.Position, first.Scale, first.Rotation);
                    var secondVertices = ToWorld(secondPolygon, second.Position, second.Scale, second.Rotation);

                    if (!HasSeparatingAxis(firstVertices, secondVertices)
                        && !HasSeparatingAxis(secondVertices, firstVertices))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static Vector2[] ToWorld(Polygon polygon, Vector2 position, Vector2 scale, float rotation)
        {
            var radians = rotation * Mathf.Deg2Rad;
            var cos = Mathf.Cos(radians);
            var sin = Mathf.Sin(radians);

            return new[]
            {
                ToWorld(polygon.A, position, scale, cos, sin),
                ToWorld(polygon.B, position, scale, cos, sin),
                ToWorld(polygon.C, position, scale, cos, sin),
            };
        }

        private static Vector2 ToWorld(Vector2 local, Vector2 position, Vector2 scale, float cos, float sin)
        {
            var scaled = Vector2.Scale(local, scale);
            var rotated = new Vector2(scaled.x * cos - scaled.y * sin, scaled.x * sin + scaled.y * cos);
            return position + rotated;
        }

        private static bool HasSeparatingAxis(Vector2[] axisSource, Vector2[] other)
        {
            for (var i = 0; i < axisSource.Length; i++)
            {
                var edge = axisSource[(i + 1) % axisSource.Length] - axisSource[i];
                var axis = new Vector2(-edge.y, edge.x);

                Project(axisSource, axis, out var minA, out var maxA);
                Project(other, axis, out var minB, out var maxB);

                if (maxA < minB || maxB < minA) return true;
            }

            return false;
        }

        private static void Project(Vector2[] vertices, Vector2 axis, out float min, out float max)
        {
            min = float.MaxValue;
            max = float.MinValue;

            foreach (var vertex in vertices)
            {
                var projection = Vector2.Dot(vertex, axis);
                min = Mathf.Min(min, projection);
                max = Mathf.Max(max, projection);
            }
        }
    }
}

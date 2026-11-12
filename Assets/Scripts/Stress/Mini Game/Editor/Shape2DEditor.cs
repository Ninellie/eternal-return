using System;
using EternalReturn.Stress.Collision_System;
using UnityEditor;
using UnityEngine;

namespace EternalReturn.Stress.Mini_Game.Editor
{
    [CustomEditor(typeof(Shape2D))]
    public class Shape2DEditor : UnityEditor.Editor
    {
        private const float PreviewSize = 200f;
        private const float PreviewPadding = 16f;
        private const int ButtonsInRow = 3;

        private static readonly Template[] Templates =
        {
            new("Треугольник", CollisionPrimitives.Triangle),
            new("Сердце", CollisionPrimitives.Heart),
            new("Пуля", CollisionPrimitives.Bullet),
            new("Квадрат", CollisionPrimitives.Square),
            new("Ромб", CollisionPrimitives.Rhombus),
            new("Шестигранник", CollisionPrimitives.Hexagon),
            new("Четырёхконечная звезда", CollisionPrimitives.FourPointStar),
            new("Пятиконечная звезда", CollisionPrimitives.FivePointStar),
            new("Шестиконечная звезда", CollisionPrimitives.SixPointStar),
            new("Полумесяц", CollisionPrimitives.Crescent),
        };

        private static readonly Color BackgroundColor = new(0.15f, 0.15f, 0.15f);
        private static readonly Color AxisColor = new(1f, 1f, 1f, 0.2f);
        private static readonly Color FillColor = new(0.3f, 0.8f, 1f, 0.35f);
        private static readonly Color OutlineColor = new(0.3f, 0.8f, 1f, 1f);

        public override void OnInspectorGUI()
        {
            var shape = (Shape2D)target;

            DrawPreview(shape.Mesh);
            DrawPrimitiveButtons(shape);
            DrawDefaultInspector();
        }

        private static void DrawPrimitiveButtons(Shape2D shape)
        {
            EditorGUILayout.LabelField("Применить шаблон");

            for (var row = 0; row < Templates.Length; row += ButtonsInRow)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    var rowEnd = Mathf.Min(row + ButtonsInRow, Templates.Length);
                    for (var i = row; i < rowEnd; i++)
                    {
                        DrawButton(shape, Templates[i]);
                    }
                }
            }
        }

        private static void DrawButton(Shape2D shape, Template template)
        {
            if (!GUILayout.Button(template.Label)) return;

            Undo.RecordObject(shape, template.Label);
            shape.SetMesh(template.Create());
            EditorUtility.SetDirty(shape);
        }

        private static void DrawPreview(Mesh2D mesh)
        {
            var area = GUILayoutUtility.GetRect(0f, PreviewSize, GUILayout.ExpandWidth(true));
            if (Event.current.type != EventType.Repaint) return;

            var square = new Rect(area.center.x - PreviewSize * 0.5f, area.y, PreviewSize, PreviewSize);
            EditorGUI.DrawRect(square, BackgroundColor);
            DrawAxes(square);

            var polygons = mesh?.Polygons;
            if (polygons == null) return;

            var extent = GetExtent(polygons);
            if (extent <= 0f) return;

            var scale = (PreviewSize * 0.5f - PreviewPadding) / extent;
            foreach (var polygon in polygons)
            {
                DrawPolygon(polygon, square.center, scale);
            }
        }

        private static void DrawAxes(Rect square)
        {
            Handles.color = AxisColor;
            Handles.DrawLine(new Vector3(square.xMin, square.center.y), new Vector3(square.xMax, square.center.y));
            Handles.DrawLine(new Vector3(square.center.x, square.yMin), new Vector3(square.center.x, square.yMax));
        }

        private static void DrawPolygon(Polygon polygon, Vector2 center, float scale)
        {
            var points = new[]
            {
                ToScreen(polygon.A, center, scale),
                ToScreen(polygon.B, center, scale),
                ToScreen(polygon.C, center, scale),
            };

            Handles.color = FillColor;
            Handles.DrawAAConvexPolygon(points);

            Handles.color = OutlineColor;
            Handles.DrawAAPolyLine(1.5f, points[0], points[1], points[2], points[0]);
        }

        private static Vector3 ToScreen(Vector2 point, Vector2 center, float scale)
        {
            return center + new Vector2(point.x, -point.y) * scale;
        }

        private static float GetExtent(Polygon[] polygons)
        {
            var extent = 0f;
            foreach (var polygon in polygons)
            {
                extent = Mathf.Max(extent, GetExtent(polygon.A), GetExtent(polygon.B), GetExtent(polygon.C));
            }

            return extent;
        }

        private static float GetExtent(Vector2 point)
        {
            return Mathf.Max(Mathf.Abs(point.x), Mathf.Abs(point.y));
        }

        private class Template
        {
            public string Label { get; }
            public Func<Mesh2D> Create { get; }

            public Template(string label, Func<Mesh2D> create)
            {
                Label = label;
                Create = create;
            }
        }
    }
}

using EternalReturn.Stress.Collision_System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EternalReturn.Stress.Mini_Game
{
    public class StressGameGraphic : MaskableGraphic
    {
        private const float WaitingEnemyAlpha = 0.4f;

        [SerializeField] private Color enemyColor = new(0.95f, 0.35f, 0.3f, 1f);
        [SerializeField] private Color playerColor = new(0.3f, 0.85f, 1f, 1f);
        [SerializeField] private Color playerFieldColor = new(1f, 1f, 1f, 0.6f);
        [SerializeField] private float playerFieldThickness = 3f;
        [SerializeField] private Color damageFlashColor = new(1f, 0.1f, 0.1f, 1f);
        [SerializeField] private float damageFlashDuration = 0.25f;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private float healthTextOffset = 20f;

        [Inject] private StressGameController _controller;
        [Inject] private StressGameConfig _config;

        private Player _trackedPlayer;
        private int _trackedHealth;
        private float _flashTimeLeft;
        private bool _isRendering;

        public bool IsRendering
        {
            get => _isRendering;
            set
            {
                if (_isRendering == value) return;

                _isRendering = value;
                SetVerticesDirty();
            }
        }

        public RectTransform RectTransform => rectTransform;

        private void Update()
        {
            if (!_isRendering) return;

            TrackPlayer();
            _flashTimeLeft = Mathf.Max(_flashTimeLeft - Time.deltaTime, 0f);
            SetVerticesDirty();
        }

        private void TrackPlayer()
        {
            var player = _controller.Player;
            if (player == null) return;

            if (player != _trackedPlayer)
            {
                _trackedPlayer = player;
                _trackedHealth = player.Health;
                _flashTimeLeft = 0f;
                ShowHealth(player.Health);
                return;
            }

            if (player.Health == _trackedHealth) return;

            if (player.Health < _trackedHealth)
            {
                _flashTimeLeft = damageFlashDuration;
            }

            _trackedHealth = player.Health;
            ShowHealth(player.Health);
        }

        private void ShowHealth(int health)
        {
            if (healthText == null) return;

            healthText.SetText("{0}", health);
        }
        
        private Color GetPlayerColor()
        {
            if (_flashTimeLeft <= 0f) return playerColor;

            var progress = 1f - _flashTimeLeft / damageFlashDuration;
            return Color.Lerp(playerColor, damageFlashColor, Mathf.Sin(progress * Mathf.PI));
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (!_isRendering) return;
            if (_controller.Player == null) return;

            DrawPlayerField(vh);
            DrawEnemies(vh);
            DrawMesh(vh, _controller.Player.Transform, _controller.Player.Mesh, GetPlayerColor());
        }

        private void DrawPlayerField(VertexHelper vh)
        {
            var field = _config.PlayerField;
            var half = field.Size * 0.5f;
            var corners = new[]
            {
                ToLocal(field.Transform.TransformPoint(new Vector2(-half.x, -half.y))),
                ToLocal(field.Transform.TransformPoint(new Vector2(-half.x, half.y))),
                ToLocal(field.Transform.TransformPoint(new Vector2(half.x, half.y))),
                ToLocal(field.Transform.TransformPoint(new Vector2(half.x, -half.y))),
            };

            for (var i = 0; i < corners.Length; i++)
            {
                DrawLine(vh, corners[i], corners[(i + 1) % corners.Length], playerFieldThickness, playerFieldColor);
            }
        }

        private void DrawEnemies(VertexHelper vh)
        {
            foreach (var enemy in _controller.Enemies)
            {
                var color = enemyColor;
                if (enemy.WaitTime > 0f)
                {
                    color.a *= WaitingEnemyAlpha;
                }

                DrawMesh(vh, enemy.Transform, enemy.Mesh, color);
            }
        }

        private void DrawMesh(VertexHelper vh, Transform2D transform, Mesh2D mesh, Color color)
        {
            var polygons = mesh?.Polygons;
            if (polygons == null) return;

            foreach (var polygon in polygons)
            {
                DrawTriangle(
                    vh,
                    ToLocal(transform.TransformPoint(polygon.A)),
                    ToLocal(transform.TransformPoint(polygon.B)),
                    ToLocal(transform.TransformPoint(polygon.C)),
                    color);
            }
        }

        private Vector2 ToLocal(Vector2 worldPoint)
        {
            var fieldTransform = _config.GameField.Transform;
            var offset = worldPoint - fieldTransform.Position;
            return (Vector2)(Quaternion.Euler(0f, 0f, -fieldTransform.Rotation) * offset);
        }

        private static void DrawLine(VertexHelper vh, Vector2 from, Vector2 to, float thickness, Color color)
        {
            var direction = to - from;
            if (direction.sqrMagnitude <= 0f) return;

            direction.Normalize();
            var halfThickness = thickness * 0.5f;
            var start = from - direction * halfThickness;
            var end = to + direction * halfThickness;
            var normal = new Vector2(-direction.y, direction.x) * halfThickness;

            var index = vh.currentVertCount;
            vh.AddVert(start + normal, color, Vector2.zero);
            vh.AddVert(end + normal, color, Vector2.zero);
            vh.AddVert(end - normal, color, Vector2.zero);
            vh.AddVert(start - normal, color, Vector2.zero);
            vh.AddTriangle(index, index + 1, index + 2);
            vh.AddTriangle(index, index + 2, index + 3);
        }

        private static void DrawTriangle(VertexHelper vh, Vector2 a, Vector2 b, Vector2 c, Color color)
        {
            var index = vh.currentVertCount;
            vh.AddVert(a, color, Vector2.zero);
            vh.AddVert(b, color, Vector2.zero);
            vh.AddVert(c, color, Vector2.zero);
            vh.AddTriangle(index, index + 1, index + 2);
        }
    }
}

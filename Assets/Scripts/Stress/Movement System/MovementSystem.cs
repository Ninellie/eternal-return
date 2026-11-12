using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Stress.Movement_System
{
    public class MovementSystem : IFixedTickable
    {
        private readonly HashSet<IMovable> _movables = new();
        private readonly List<IMovable> _snapshot = new();

        public void Register(IMovable movable)
        {
            _movables.Add(movable);
        }

        public void Unregister(IMovable movable)
        {
            _movables.Remove(movable);
        }

        public void FixedTick()
        {
            _snapshot.Clear();
            _snapshot.AddRange(_movables);

            foreach (var movable in _snapshot)
            {
                if (!_movables.Contains(movable)) continue;

                movable.Transform.Position += movable.Direction * (movable.Speed * Time.fixedDeltaTime);
            }
        }
    }
}

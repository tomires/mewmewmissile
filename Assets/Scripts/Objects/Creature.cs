using UnityEngine;
using System.Collections;
using Mew.Models;
using Mew.Managers;

namespace Mew.Objects
{
    public class Creature : MonoBehaviour
    {
        private Direction _direction;
        private Vector2 _coordinates;

        public void Initialize(Vector2 coordinates, Direction direction)
        {
            _direction = direction;
            _coordinates = coordinates;
            StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            while(true)
            {
                transform.position = new Vector3(_coordinates.x, 0, _coordinates.y);
                _direction = Game.Instance.GetNextMove(_coordinates, _direction);
                Rotate();
                var animation = StartCoroutine(AnimateMove());
                yield return new WaitForSeconds(Game.Instance.Speed);
                StopCoroutine(animation);
                _coordinates += GetOffset();
            }
        }

        private void Rotate()
        {
            var angle = _direction switch
            {
                Direction.Up => 90,
                Direction.Right => 180,
                Direction.Down => 270,
                _ => 0
            };
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        private IEnumerator AnimateMove()
        {
            var offset = GetOffset();
            var step = 0f;
            while(true)
            {
                step += Time.deltaTime / Game.Instance.Speed;
                transform.position = new Vector3(_coordinates.x + offset.x * step, 0, _coordinates.y + offset.y * step);
                yield return null;
            }
        }

        private Vector2 GetOffset()
        {
            return _direction switch
            {
                Direction.Up => Vector2.up,
                Direction.Down => Vector2.down,
                Direction.Left => Vector2.left,
                Direction.Right => Vector2.right,
                _ => Vector2.zero
            };
        }
    }
}

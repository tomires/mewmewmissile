using UnityEngine;
using System.Collections;
using Mew.Models;
using Mew.Managers;

namespace Mew.Objects
{
    public abstract class Creature : MonoBehaviour
    {
        protected abstract float SpeedMultiplier { get; }
        private Direction _direction;
        private Vector2 _coordinates;
        private bool _isMouse => GetType() == typeof(Mouse);
        private bool _isCat => GetType() == typeof(Cat);

        public virtual void Initialize(Vector2 coordinates, Direction direction)
        {
            _direction = direction;
            _coordinates = coordinates;
            StartCoroutine(Move());

            if (_isMouse) Game.Instance.UpdateMouseCount(true);
            else if (_isCat) Game.Instance.UpdateCatCount(true);
        }

        private IEnumerator Move()
        {
            while (true)
            {
                transform.position = new Vector3(_coordinates.x, 0, _coordinates.y);
                _direction = Game.Instance.GetNextMove(_coordinates, _direction);
                Rotate();
                var animation = StartCoroutine(AnimateMove());
                yield return new WaitForSeconds(Game.Instance.Speed * 1 / SpeedMultiplier);
                StopCoroutine(animation);
                _coordinates += Utils.GetOffset(_direction);
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
            var offset = Utils.GetOffset(_direction);
            var step = 0f;
            while (true)
            {
                step += Time.deltaTime / (Game.Instance.Speed * 1 / SpeedMultiplier);
                transform.position = new Vector3(_coordinates.x + offset.x * step, 0, _coordinates.y + offset.y * step);
                yield return null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.Tags.Rocket))
            {
                var rocket = other.GetComponent<Rocket>();
                if (_isMouse)
                {
                    var mouseType = ((Mouse)this).Type;
                    rocket.PropagateMouseGain(mouseType == Mouse.MouseType.Bonus);
                    if (mouseType == Mouse.MouseType.Mode)
                        Game.Instance.ChangeMode();
                }
                else if (_isCat)
                    rocket.PropagateCatHit();
                Destroy();
            }

            if (_isMouse && !other.CompareTag(Constants.Tags.Mouse))
                Destroy();
            else if (_isCat && other.CompareTag(Constants.Tags.Death))
                Destroy();
        }

        private void Destroy()
        {
            if (_isMouse)
                Game.Instance.UpdateMouseCount(false);
            else if (_isCat)
                Game.Instance.UpdateCatCount(false);
            Destroy(gameObject);
        }
    }
}

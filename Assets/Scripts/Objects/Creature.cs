using UnityEngine;
using System.Collections;
using Mew.Models;
using Mew.Managers;

namespace Mew.Objects
{
    public class Creature : MonoBehaviour
    {
        [SerializeField] float speedMultiplier = 1f;
        private Direction _direction;
        private Vector2 _coordinates;
        private bool _isMouse => gameObject.CompareTag(Constants.Tags.Mouse);
        private bool _isCat => gameObject.CompareTag(Constants.Tags.Cat);

        public void Initialize(Vector2 coordinates, Direction direction)
        {
            _direction = direction;
            _coordinates = coordinates;
            StartCoroutine(Move());

            if (_isMouse) Game.Instance.UpdateMouseCount(true);
            else if (_isCat) Game.Instance.UpdateCatCount(true);
        }

        private IEnumerator Move()
        {
            while(true)
            {
                transform.position = new Vector3(_coordinates.x, 0, _coordinates.y);
                _direction = Game.Instance.GetNextMove(_coordinates, _direction);
                Rotate();
                var animation = StartCoroutine(AnimateMove());
                yield return new WaitForSeconds(Game.Instance.Speed * 1 / speedMultiplier);
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
            while(true)
            {
                step += Time.deltaTime / (Game.Instance.Speed * 1 / speedMultiplier);
                transform.position = new Vector3(_coordinates.x + offset.x * step, 0, _coordinates.y + offset.y * step);
                yield return null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isMouse && !other.CompareTag(Constants.Tags.Mouse))
            {
                Game.Instance.UpdateMouseCount(false);
                Destroy(gameObject);
            }
            else if(_isCat && other.CompareTag(Constants.Tags.Death))
            {
                Game.Instance.UpdateCatCount(false);
                Destroy(gameObject);
            }
        }
    }
}

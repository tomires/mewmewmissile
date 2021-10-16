using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mew.Models;
using Mew.Managers;

namespace Mew.Objects
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private GameObject block;
        [SerializeField] private GameObject blockerDown;
        [SerializeField] private GameObject blockerLeft;
        [SerializeField] private GameObject blockerRight;
        [SerializeField] private GameObject blockerUp;
        [SerializeField] private GameObject arrow;
        [SerializeField] private SpriteRenderer color;

        [Header("Materials")]
        [SerializeField] private Material materialEven;
        [SerializeField] private Material materialOdd;

        private Direction _directionOverride = Direction.Default;
        private BlockerSetting _blockers;
        private bool _arrowPlaceable;
        private int _player = -1;
        private Coroutine _removeArrowCoroutine;
        private int _arrowHits;

        public void Initialize(Vector2 coordinates, BlockerSetting blockers, bool arrowPlaceable)
        {
            _blockers = blockers;
            _arrowPlaceable = arrowPlaceable;

            block.GetComponent<Renderer>().material =
                Mathf.FloorToInt(coordinates.x + coordinates.y) % 2 == 0
                ? materialOdd : materialEven;
            blockerDown.SetActive(blockers.down);
            blockerLeft.SetActive(blockers.left);
            blockerRight.SetActive(blockers.right);
            blockerUp.SetActive(blockers.up);
            ToggleArrow(false);
        }

        public Direction GetNextDirection(Direction initialDirection)
        {
            if (_directionOverride != Direction.Default)
                initialDirection = _directionOverride;

            var preferredDirections = initialDirection switch {
                Direction.Up => new List<Direction>
                { Direction.Up, Direction.Right, Direction.Left, Direction.Down },
                Direction.Right => new List<Direction>
                { Direction.Right, Direction.Down, Direction.Up, Direction.Left },
                Direction.Down => new List<Direction>
                { Direction.Down, Direction.Left, Direction.Right, Direction.Up },
                _ => new List<Direction>
                { Direction.Left, Direction.Up, Direction.Down, Direction.Right },
            };

            if (_blockers.up)
                preferredDirections.Remove(Direction.Up);
            if (_blockers.right)
                preferredDirections.Remove(Direction.Right);
            if (_blockers.down)
                preferredDirections.Remove(Direction.Down);
            if (_blockers.left)
                preferredDirections.Remove(Direction.Left);

            return preferredDirections[0];
        }

        public void PlaceArrow(int player, Direction direction)
        {
            if (!_arrowPlaceable || _player != -1) return;

            _directionOverride = direction;
            _player = player;
            _arrowHits = 0;

            var angle = direction switch
            {
                Direction.Up => 0,
                Direction.Right => 90,
                Direction.Down => 180,
                _ => 270
            };
            arrow.transform.rotation = Quaternion.Euler(90, angle, 0);
            color.color = Constants.Colors.PlayerColor[player];
            ToggleArrow(true);
            PlayerRoster.Instance.PropagateArrowPlaced(_player, this);
            _removeArrowCoroutine = StartCoroutine(RemoveArrowHelper());
        }

        public void RemoveArrow()
        {
            if (_removeArrowCoroutine != null)
                StopCoroutine(_removeArrowCoroutine);

            PlayerRoster.Instance.PropagateArrowRemoved(_player, this);
            _directionOverride = Direction.Default;
            _player = -1;
            ToggleArrow(false);
        }

        private IEnumerator RemoveArrowHelper()
        {
            yield return new WaitForSecondsRealtime(Constants.Settings.ArrowLifetime);
            RemoveArrow();
        }

        private void ToggleArrow(bool enabled)
        {
            arrow.SetActive(enabled);
            color.gameObject.SetActive(enabled);
        }

        public void DamageTile()
        {
            if (_directionOverride == Direction.Default) return;
            if (++_arrowHits == Constants.Settings.ArrowHitpoints)
                RemoveArrow();
        }
    }
}

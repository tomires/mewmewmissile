using UnityEngine;
using Mew.Models;

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

        private Vector2 _coordinates;
        private Direction _directionOverride = Direction.Default;
        private BlockerSetting _blockers;
        private int _player = -1;

        public void Initialize(Vector2 coordinates, BlockerSetting blockers)
        {
            _coordinates = coordinates;
            _blockers = blockers;

            block.GetComponent<Renderer>().material =
                Mathf.FloorToInt(coordinates.x + coordinates.y) % 2 == 0
                ? materialOdd : materialEven;
            blockerDown.SetActive(blockers.down);
            blockerLeft.SetActive(blockers.left);
            blockerRight.SetActive(blockers.right);
            blockerUp.SetActive(blockers.up);
            ToggleArrow(false);
        }

        public Direction GetNextDirection(Direction direction)
        {
            if (_directionOverride != Direction.Default)
                return _directionOverride;

            var index = (int)direction;
            while (true)
            {
                if (index == 1 && !_blockers.up)
                    return Direction.Up;
                else if (index == 2 && !_blockers.right)
                    return Direction.Right;
                else if (index == 3 && !_blockers.down)
                    return Direction.Down;
                else if (index == 4 && !_blockers.left)
                    return Direction.Left;

                index++;
                if (index == 5)
                    index = 1;
            }
        }

        public void PlaceArrow(int player, Direction direction)
        {
            if (_player != -1) return;

            _directionOverride = direction;
            _player = player;

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
        }

        public void RemoveArrow()
        {
            _directionOverride = Direction.Default;
            _player = -1;
            ToggleArrow(false);
        }

        private void ToggleArrow(bool enabled)
        {
            arrow.SetActive(enabled);
            color.gameObject.SetActive(enabled);
        }
    }
}

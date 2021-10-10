using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using System.Collections.Generic;
using System.Linq;
using Mew.Managers;
using Mew.Models;
using Mew.Objects;

namespace Mew
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer background;

        private Vector2 _coordinates;
        private int _playerNumber;
        private bool _initialized = false;
        private Queue<Block> _placedArrows = new Queue<Block>();

        private int _score = 0;
        private int Score
        {
            set
            {
                _score = value;
            }
            get => _score;
        }

        public void PropagateMouseGain() => Score++;
        public void PropagateCatHit() => Score = Mathf.FloorToInt(Constants.Settings.CatHitMultiplier * Score);

        public void PropagateArrowPlaced(Block block)
        {
            if (_placedArrows.Count >= Constants.Settings.MaxArrowsPerPlayer)
                _placedArrows.Dequeue().RemoveArrow();
            _placedArrows.Enqueue(block);
        }

        public void PropagateArrowRemoved(Block block)
        {
            if (_placedArrows.Contains(block))
                _placedArrows = new Queue<Block>(_placedArrows.Where(b => b != block));
        }

        public void Initialize(Color color, Vector2 coordinates, int playerNumber)
        {
            background.color = color;
            _coordinates = coordinates;
            transform.position = Utils.CoordinatesTo3D(coordinates);
            _playerNumber = playerNumber;
            _initialized = true;
        }

        public void OnUp(CallbackContext c)
        { if (c.performed) OnMove(Direction.Up); }
        public void OnDown(CallbackContext c)
        { if (c.performed) OnMove(Direction.Down); }
        public void OnLeft(CallbackContext c)
        { if (c.performed) OnMove(Direction.Left); }
        public void OnRight(CallbackContext c)
        { if (c.performed) OnMove(Direction.Right); }
        public void OnMove(Direction direction)
        {
            if (!Game.Instance || !_initialized) return;
            _coordinates = Utils.PropagateOffset(_coordinates, direction);
            transform.position = Utils.CoordinatesTo3D(_coordinates);
        }

        public void OnNorth(CallbackContext c)
        { if (c.performed) OnPlace(Direction.Up); }
        public void OnSouth(CallbackContext c)
        { if (c.performed) OnPlace(Direction.Down); }
        public void OnWest(CallbackContext c)
        { if (c.performed) OnPlace(Direction.Left); }
        public void OnEast(CallbackContext c)
        { if (c.performed) OnPlace(Direction.Right); }
        public void OnPlace(Direction direction)
        {
            if (!Game.Instance || !_initialized) return;
            Game.Instance.PlaceArrow(_coordinates, direction, _playerNumber);
        }

        public void OnStart(CallbackContext c)
        {
            if (!Menu.Instance || !_initialized || !c.performed) return;
            Menu.Instance.StartGame();
        }

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

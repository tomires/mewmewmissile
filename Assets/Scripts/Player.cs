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
        [SerializeField] private SpriteRenderer foreground;

        private Vector2 _coordinates;
        private int _playerNumber;
        private bool _enabled = false;
        private Queue<Block> _placedArrows = new Queue<Block>();

        private int _score = 0;
        public int Score
        {
            private set
            {
                _score = value;
                Game.Instance.PropagatePlayerScore(_playerNumber, value);
            }
            get => _score;
        }

        private int _wins;
        public int Wins
        {
            private set
            {
                _wins = value;
                Game.Instance.PropagatePlayerWin(_playerNumber);
            }
            get => _wins;
        }

        public void PropagateMouseGain(bool bonus) => Score += bonus ? Constants.Settings.MouseBonusValue : 1;
        public void PropagateCatHit() => Score = Mathf.FloorToInt(Constants.Settings.CatHitMultiplier * Score);
        public void PropagateWin() => Wins++;

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

        public void PrepareForNextMatch()
        {
            _placedArrows.Clear();
            Score = 0;
        }

        public void Initialize(Color color, Vector2 coordinates, int playerNumber)
        {
            Menu.Instance?.ShowController(playerNumber);
            background.color = color;
            _coordinates = coordinates;
            transform.position = Utils.CoordinatesTo3D(coordinates);
            _playerNumber = playerNumber;
        }

        public void ToggleInteractivity(bool enabled)
        {
            background.enabled = enabled;
            foreground.enabled = enabled;
            _enabled = enabled;
        }

        #region Input

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
            Menu.Instance?.BumpController(_playerNumber);
            Game.Instance?.ProceedToNextMatch();

            if (!Game.Instance || !_enabled) return;
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
            Menu.Instance?.BumpController(_playerNumber);
            Game.Instance?.ProceedToNextMatch();

            if (!Game.Instance || !_enabled) return;
            Game.Instance.PlaceArrow(_coordinates, direction, _playerNumber);
        }

        public void OnStart(CallbackContext c)
        {
            if (!Menu.Instance || !c.performed) return;
            Menu.Instance.StartGame();
        }

        #endregion Input

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

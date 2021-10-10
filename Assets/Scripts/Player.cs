using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using Mew.Managers;
using Mew.Models;

namespace Mew
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer background;

        private Color _color;
        private Vector2 _coordinates;
        private int _playerNumber;
        private int _arrowsLeft;

        public bool ArrowsSpawnable => _arrowsLeft > 0;

        public void UpdateArrowCount(bool increase)
        {
            _arrowsLeft += increase ? 1 : -1;
        }

        public void Initialize(Color color, Vector2 coordinates, int playerNumber)
        {
            _color = color;
            background.color = color;
            _coordinates = coordinates;
            transform.position = new Vector3(coordinates.x, 0, coordinates.y);
            _arrowsLeft = Constants.Settings.MaxArrowsPerPlayer;
            _playerNumber = playerNumber;
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
            if (!Game.Instance) return;
            _coordinates = Utils.PropagateOffset(_coordinates, direction);
            transform.position = new Vector3(_coordinates.x, 0, _coordinates.y);
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
            if (!Game.Instance) return;
            Game.Instance.PlaceArrow(_coordinates, direction, _playerNumber);
        }

        public void OnStart(CallbackContext c)
        {
            Menu.Instance?.StartGame();
        }

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

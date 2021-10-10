using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mew.Managers
{
    [RequireComponent(typeof(PlayerInputManager))]
    public class PlayerRoster : MonoSingleton<PlayerRoster>
    {
        private List<Player> _players = new List<Player>();
        private PlayerInputManager _playerManager;

        public bool JoiningEnabled
        {
            set
            {
                if (value)
                    _playerManager.EnableJoining();
                else
                    _playerManager.DisableJoining();
            }
        }

        public void OnPlayerJoined(PlayerInput obj)
        {
            var player = obj.GetComponent<Player>();
            var playerNumber = _players.Count;
            player.Initialize(
                Constants.Colors.PlayerColor[playerNumber],
                Constants.Settings.SelectorPositions[playerNumber],
                playerNumber);
            _players.Add(player);
        }

        public void UpdateArrowCount(int player, bool increase)
        {
            _players[player].UpdateArrowCount(increase);
        }

        public bool GetArrowsSpawnable(int player)
        {
            return _players[player].ArrowsSpawnable;
        }

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            _playerManager = GetComponent<PlayerInputManager>();
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Mew.Objects;

namespace Mew.Managers
{
    [RequireComponent(typeof(PlayerInputManager))]
    public class PlayerRoster : MonoSingleton<PlayerRoster>
    {
        private List<Player> _players = new List<Player>();
        private PlayerInputManager _playerManager;

        public int PlayerCount => _players.Count;

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

        public void ToggleInteractivity(bool enabled)
        {
            foreach (var player in _players)
                player.ToggleInteractivity(enabled);
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

        public void PropagateArrowPlaced(int player, Block block)
        {
            _players[player].PropagateArrowPlaced(block);
        }

        public void PropagateArrowRemoved(int player, Block block)
        {
            _players[player].PropagateArrowRemoved(block);
        }

        public void PrepareForNextMatch()
        {
            foreach (var player in _players)
                player.PrepareForNextMatch();
        }

        public void PropagateMouseGain(int player, bool bonus)
        {
            if (player >= _players.Count) return;
            _players[player].PropagateMouseGain(bonus);
        }

        public void PropagateCatHit(int player)
        {
            if (player >= _players.Count) return;
            _players[player].PropagateCatHit();
        }

        public List<int> GetLeaders()
        {
            var scores = _players.Select(p => p.Score).ToList();
            var leaders = new List<int>();
            var maxScore = -1;
            for (int p = 0; p < scores.Count; p++)
            {
                if (scores[p] > maxScore)
                {
                    maxScore = scores[p];
                    leaders.Clear();
                }
                if (scores[p] >= maxScore)
                    leaders.Add(p);
            }
            return leaders;
        }

        public int GetWinner()
        {
            for (int p = 0; p < _players.Count; p++)
                if (_players[p].Wins == Constants.Settings.WinCount)
                    return p;
            return -1;
        }

        public void PropagateWins(List<int> players)
        {
            foreach (var p in players)
                _players[p].PropagateWin();
        }

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            _playerManager = GetComponent<PlayerInputManager>();
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
using Mew.Objects;
using Mew.Models;

namespace Mew.Managers
{
    public class Game : MonoSingleton<Game>
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject blockPrefab;
        [SerializeField] private GameObject catPrefab;
        [SerializeField] private GameObject mousePrefab;
        [SerializeField] private GameObject rocketPrefab;
        [SerializeField] private GameObject spawnerPrefab;

        private List<Block> _blocks = new List<Block>();

        void Start()
        {
            InitializeBoard();
            Audio.Instance.PlayMusic(GameState.Match);
        }

        private void InitializeBoard()
        {
            _blocks = new List<Block>();

            for(int x = 0; x < Constants.Settings.BoardSize.x; x++)
            {
                for (int y = 0; y < Constants.Settings.BoardSize.y; y++)
                {
                    var block = Instantiate(blockPrefab).GetComponent<Block>();
                    var blockerSettings = new BlockerSetting(false, false, false, false);
                    block.Initialize(new Vector2(x, y), blockerSettings);
                    block.transform.position = new Vector3(x, 0, y);
                    _blocks.Add(block);
                }
            }
        }
    }
}
using UnityEngine;
using System.Collections.Generic;
using System.IO;
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
        [SerializeField] private GameObject holePrefab;

        public GameObject MousePrefab => mousePrefab;
        public GameObject CatPrefab => catPrefab;
        public float Speed => 1f / _currentSpeed;
        public float SpawnRate => 1f / _currentSpawnRate;
        public bool MouseSpawnable => _mouseCount < Constants.Settings.MaxMouseCount;
        public bool CatSpawnable => _catCount < Constants.Settings.MaxCatCount;

        private List<Block> _blocks = new List<Block>();
        private List<Spawner> _spawners = new List<Spawner>();
        private List<Rocket> _rockets = new List<Rocket>();
        private List<GameObject> _holes = new List<GameObject>();

        private float _currentSpeed = Constants.Settings.DefaultSpeed;
        private float _currentSpawnRate = Constants.Settings.DefaultSpawnRate;
        private int _mouseCount = 0;
        private int _catCount = 0;

        public void UpdateMouseCount(bool increase)
        {
            _mouseCount += increase ? 1 : -1;
        }

        public void UpdateCatCount(bool increase)
        {
            _catCount += increase ? 1 : -1;
        }

        public Direction GetNextMove(Vector2 coordinates, Direction startDirection)
        {
            return GetBlock(coordinates).GetNextDirection(startDirection);
        }

        void Start()
        {
            InitializeBoard("stage1.mew");
            Audio.Instance.PlayMusic(GameState.Match);
        }

        private void InitializeBoard(string file)
        {
            List<List<string>> board = new List<List<string>>();
            using (var reader = new StreamReader($"{Constants.Paths.StagesFolder}/{file}"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Split(',');
                    board.Add(new List<string>(line));
                }
            }

            _blocks = new List<Block>();
            _rockets = new List<Rocket>();
            _spawners = new List<Spawner>();
            _holes = new List<GameObject>();
            int player = 0;

            for (int x = 0; x < Constants.Settings.BoardSize.x; x++)
            {
                for (int y = 0; y < Constants.Settings.BoardSize.y; y++)
                {
                    BlockerSetting blockerSetting;
                    bool rocket, hole;
                    Direction spawnerDirection;
                    ParseField(board[y][x], out blockerSetting, out rocket, out hole, out spawnerDirection);

                    var coordinates = new Vector2(x, Constants.Settings.BoardSize.y - y - 1);
                    SpawnBlock(coordinates, blockerSetting);

                    if (rocket)
                        SpawnRocket(coordinates, player++);

                    if (spawnerDirection != Direction.Default)
                        SpawnSpawner(coordinates, spawnerDirection);

                    if (hole)
                        SpawnHole(coordinates);
                }
            }
        }

        private void SpawnBlock(Vector2 coordinates, BlockerSetting blockerSetting)
        {
            var block = Instantiate(blockPrefab).GetComponent<Block>();
            block.Initialize(coordinates, blockerSetting);
            block.transform.position = new Vector3(coordinates.x, 0, coordinates.y);
            _blocks.Add(block);
        }

        private void SpawnRocket(Vector2 coordinates, int player)
        {
            var rocket = Instantiate(rocketPrefab).GetComponent<Rocket>();
            rocket.Initialize(player);
            rocket.transform.position = new Vector3(coordinates.x, 0, coordinates.y);
            _rockets.Add(rocket);
        }

        private void SpawnSpawner(Vector2 coordinates, Direction direction)
        {
            var spawner = Instantiate(spawnerPrefab).GetComponent<Spawner>();
            spawner.Initialize(coordinates, direction);
            spawner.transform.position = new Vector3(coordinates.x, 0, coordinates.y);
            _spawners.Add(spawner);
        }

        private void SpawnHole(Vector2 coordinates)
        {
            var hole = Instantiate(holePrefab);
            hole.transform.position = new Vector3(coordinates.x, 0, coordinates.y);
            _holes.Add(hole);
        }

        private void ParseField(string field, out BlockerSetting blockerSetting, out bool rocket, out bool hole, out Direction spawnerDirection)
        {
            var up = field[0] == 'X';
            var right = field[1] == 'X';
            var down = field[2] == 'X';
            var left = field[3] == 'X';

            blockerSetting = new BlockerSetting(up, right, down, left);
            rocket = field[4] == 'R';
            hole = field[4] == 'H';
            spawnerDirection = field[4] switch
            {
                'u' => Direction.Up,
                'r' => Direction.Right,
                'd' => Direction.Down,
                'l' => Direction.Left,
                _ => Direction.Default
            };
        }

        private Block GetBlock(Vector2 coordinates)
        {
            var ySize = (int)Constants.Settings.BoardSize.y;
            return _blocks[ySize - (int)coordinates.y - 1 + ySize * (int)coordinates.x];
        }
    }
}

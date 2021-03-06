using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using Mew.Objects;
using Mew.Models;
using Mew.UI;

namespace Mew.Managers
{
    public class Game : MonoSingleton<Game>
    {
        [SerializeField] private Text timeLeftText;
        [SerializeField] private Transform scoreCardParent;
        [SerializeField] private Animator cameraAnimator;

        [Header("Prefabs")]
        [SerializeField] private GameObject blockPrefab;
        [SerializeField] private GameObject catPrefab;
        [SerializeField] private GameObject mousePrefab;
        [SerializeField] private GameObject rocketPrefab;
        [SerializeField] private GameObject spawnerPrefab;
        [SerializeField] private GameObject holePrefab;
        [SerializeField] private GameObject scoreCardPrefab;

        public GameObject MousePrefab => mousePrefab;
        public GameObject CatPrefab => catPrefab;
        public float Speed => 1f / _currentSpeed;
        public float SpawnRate => 1f / _currentSpawnRate;
        public bool MouseSpawnable => _mouseCount < Constants.Settings.MaxMouseCount;
        public bool CatSpawnable => _catCount < Constants.Settings.MaxCatCount;

        public GameState CurrentMode
        {
            private set
            {
                _currentMode = value;
                Audio.Instance.PlayMusic(value);
            }
            get => _currentMode;
        }

        private List<Block> _blocks = new List<Block>();
        private List<Spawner> _spawners = new List<Spawner>();
        private List<Rocket> _rockets = new List<Rocket>();
        private List<GameObject> _holes = new List<GameObject>();
        private List<ScoreCard> _scoreCards = new List<ScoreCard>();

        private GameState _currentMode = GameState.Match;
        private float _currentSpeed = Constants.Settings.DefaultSpeed;
        private float _currentSpawnRate = Constants.Settings.DefaultSpawnRate;
        private int _mouseCount = 0;
        private int _catCount = 0;
        private int _timeLeft;
        private Coroutine _changeModeBackCoroutine;
        private bool _readyForNextMatch = false;
        private List<string> _unpickedStages = new List<string>();

        public void PropagatePlayerScore(int player, int score)
        {
            _scoreCards[player].SetScore(score);
        }

        public void PropagatePlayerWin(int player)
        {
            _scoreCards[player].AddWin();
        }

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

        public void DamageTile(Vector2 coordinates)
        {
            GetBlock(coordinates).DamageTile();
        }

        public void PlaceArrow(Vector2 coordinates, Direction direction, int player)
        {
            GetBlock(coordinates).PlaceArrow(player, direction);
        }

        public void ChangeMode()
        {
            GameState[] modes = new GameState[]
            {
                GameState.SpeedUp,
                GameState.SlowDown,
                GameState.MouseMania,
                GameState.CatMania/*,
                GameState.PlaceArrowsAgain,
                GameState.EverybodyMove*/
            };

            CurrentMode = modes[Random.Range(0, modes.Length)];
            if (_changeModeBackCoroutine != null)
                StopCoroutine(_changeModeBackCoroutine);
            _changeModeBackCoroutine = StartCoroutine(ChangeModeBack());

            if (CurrentMode == GameState.MouseMania
                || CurrentMode == GameState.CatMania)
                foreach (var creature in FindObjectsOfType<Creature>())
                    Destroy(creature.gameObject);

            var speedMultiplier = CurrentMode switch
            {
                GameState.SpeedUp => Constants.Settings.SpeedUpMultiplier,
                GameState.SlowDown => Constants.Settings.SlowDownMultiplier,
                _ => 1f
            };
            _currentSpeed = speedMultiplier * Constants.Settings.DefaultSpeed;

            var _spawnRateMultiplier = CurrentMode switch
            {
                GameState.CatMania => Constants.Settings.CatManiaSpawnRateMultiplier,
                GameState.MouseMania => Constants.Settings.MouseManiaSpawnRateMultiplier,
                _ => 1f
            };
            _currentSpawnRate = _spawnRateMultiplier * Constants.Settings.DefaultSpawnRate;

            foreach (var mouse in FindObjectsOfType<Mouse>())
                mouse.Type = Mouse.MouseType.Default;
        }

        public IEnumerator ChangeModeBack()
        {
            yield return new WaitForSecondsRealtime(Constants.Settings.ModeTime);
            CurrentMode = _timeLeft <= Constants.Settings.TimeRunningOutTime
                ? GameState.TimeRunningOut
                : GameState.Match;
            _currentSpeed = Constants.Settings.DefaultSpeed;
            _currentSpawnRate = Constants.Settings.DefaultSpawnRate;
            _changeModeBackCoroutine = null;
        }

        public void ProceedToNextMatch()
        {
            if (!_readyForNextMatch) return;
            _readyForNextMatch = false;

            _blocks.ForEach(x => Destroy(x.gameObject));
            _spawners.ForEach(x => Destroy(x.gameObject));
            _rockets.ForEach(x => Destroy(x.gameObject));
            _holes.ForEach(x => Destroy(x.gameObject));
            FindObjectsOfType<Creature>().ToList().ForEach(x => Destroy(x.gameObject));

            PlayerRoster.Instance.PrepareForNextMatch();

            if (_changeModeBackCoroutine != null)
            {
                StopCoroutine(_changeModeBackCoroutine);
                _currentSpeed = Constants.Settings.DefaultSpeed;
                _currentSpawnRate = Constants.Settings.DefaultSpawnRate;
                _changeModeBackCoroutine = null;
            }

            var winner = PlayerRoster.Instance.GetWinner();
            if (winner == -1)
                InitializeBoard();
            else
                SceneManager.LoadScene(Constants.Scenes.Results);
        }

        void Start()
        {
            InitializeBoard();
            InitializeScoreCards();
        }

        private string GetRandomStagePath()
        {
            if (_unpickedStages.Count == 0)
                _unpickedStages = Utils.GetAllStagePaths();

            var random = Random.Range(0, _unpickedStages.Count);
            var randomLevel = _unpickedStages[random];
            _unpickedStages.RemoveAt(random);
            return randomLevel;
        }

        private void InitializeScoreCards()
        {
            for (int p = 0; p < PlayerRoster.Instance.PlayerCount; p++)
            {
                var scoreCard = Instantiate(scoreCardPrefab, scoreCardParent).GetComponent<ScoreCard>();
                scoreCard.Initialize(p);
                _scoreCards.Add(scoreCard);
            }
        }

        private void InitializeBoard()
        {
            var stagePath = GetRandomStagePath();
            Audio.Instance.PlayMusic(GameState.Match);
            List<List<string>> board = new List<List<string>>();
            using (var reader = new StreamReader(stagePath))
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
                    var arrowPlaceable = !hole && !rocket && spawnerDirection == Direction.Default;
                    SpawnBlock(coordinates, blockerSetting, arrowPlaceable);

                    if (rocket)
                        SpawnRocket(coordinates, player++);

                    if (spawnerDirection != Direction.Default)
                        SpawnSpawner(coordinates, spawnerDirection);

                    if (hole)
                        SpawnHole(coordinates);
                }
            }

            _currentMode = GameState.Match;
            cameraAnimator.Play(Constants.Animations.CameraMatchBegin);
            PlayerRoster.Instance.ResetSelectorPositions();
            StartCoroutine(CountDownTime());
        }

        private IEnumerator CountDownTime()
        {
            _timeLeft = Constants.Settings.MatchTime;
            timeLeftText.text = Utils.TimeToHumanReadable(_timeLeft);
            yield return new WaitForSeconds(Constants.Settings.StartCooldown);
            PlayerRoster.Instance.ToggleInteractivity(true);

            while (_timeLeft > 0)
            {
                yield return new WaitForSecondsRealtime(1f);
                _timeLeft--;
                timeLeftText.text = Utils.TimeToHumanReadable(_timeLeft);


                if (_timeLeft == Constants.Settings.TimeRunningOutTime
                    && CurrentMode == GameState.Match)
                    CurrentMode = GameState.TimeRunningOut;
            }

            EndMatch();
        }

        private void EndMatch()
        {
            CurrentMode = GameState.RoundOver;
            var leaders = PlayerRoster.Instance.GetLeaders();

            foreach(var leader in leaders)
                _rockets[leader].BlastRocket();

            PlayerRoster.Instance.PropagateWins(leaders);
            PlayerRoster.Instance.ToggleInteractivity(false);
            Invoke(Constants.Animations.MoveCameraOnMatchEnd, 2.0f);
        }

        private void MoveCameraOnMatchEnd()
        {
            cameraAnimator.Play(Constants.Animations.CameraMatchEnd);
            _readyForNextMatch = true;
        }

        private void SpawnBlock(Vector2 coordinates, BlockerSetting blockerSetting, bool arrowPlaceable)
        {
            var block = Instantiate(blockPrefab).GetComponent<Block>();
            block.Initialize(coordinates, blockerSetting, arrowPlaceable);
            block.transform.position = Utils.CoordinatesTo3D(coordinates);
            _blocks.Add(block);
        }

        private void SpawnRocket(Vector2 coordinates, int player)
        {
            var rocket = Instantiate(rocketPrefab).GetComponent<Rocket>();
            rocket.Initialize(player);
            rocket.transform.position = Utils.CoordinatesTo3D(coordinates);
            _rockets.Add(rocket);
        }

        private void SpawnSpawner(Vector2 coordinates, Direction direction)
        {
            var spawner = Instantiate(spawnerPrefab).GetComponent<Spawner>();
            spawner.Initialize(coordinates, direction);
            spawner.transform.position = Utils.CoordinatesTo3D(coordinates);
            _spawners.Add(spawner);
        }

        private void SpawnHole(Vector2 coordinates)
        {
            var hole = Instantiate(holePrefab);
            hole.transform.position = Utils.CoordinatesTo3D(coordinates);
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

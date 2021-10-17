using System.Collections.Generic;
using UnityEngine;

namespace Mew.Constants
{
    public static class Colors
    {
        public static readonly Color[] PlayerColor = new Color[]
        {
            new Color(0.353f, 0.373f, 0.839f),
            new Color(0.651f, 0.18f, 0.18f),
            new Color(0f, 0.533f, 0.306f),
            new Color(0.686f, 0.667f, 0.153f)
        };

        public static readonly Color[] PlayerColorLight = new Color[]
        {
            new Color(0.651f, 0.875f, 1f),
            new Color(1f, 0.773f, 0.773f),
            new Color(0.761f, 1f, 0.8f),
            new Color(1f, 0.976f, 0.761f)
        };
    }

    public static class Scenes
    {
        public static readonly string Menu = "Menu";
        public static readonly string Game = "Game";
        public static readonly string Results = "Results";
    }

    public static class Tags
    {
        public static readonly string Mouse = "Mouse";
        public static readonly string Cat = "Cat";
        public static readonly string Death = "Death";
        public static readonly string Rocket = "Rocket";
    }

    public static class Animations
    {
        public static readonly string CameraMatchBegin = "CameraMatchBegin";
        public static readonly string MoveCameraOnMatchEnd = "MoveCameraOnMatchEnd";
        public static readonly string CameraMatchEnd = "CameraMatchEnd";
        public static readonly string CameraResults = "CameraResults";
        public static readonly string RocketRotate = "RocketRotate";
        public static readonly string RocketBlastOff = "RocketBlastOff";
        public static readonly string RocketBump = "RocketBump";
        public static readonly string ControllerBump = "ControllerBump";
        public static readonly string ScoreTextIncrement = "ScoreTextIncrement";
        public static readonly string WinIconFadeIn = "WinIconFadeIn";
    }

    public static class Settings
    {
        public static readonly Vector2 BoardSize = new Vector2(12, 9);
        public static readonly int MatchTime = 180;
        public static readonly int TimeRunningOutTime = 30;
        public static readonly int WinCount = 3;
        public static readonly float StartCooldown = 2f;
        public static readonly float ModeTime = 15f;
        public static readonly float DefaultSpeed = 4f;
        public static readonly float SpeedUpMultiplier = 2f;
        public static readonly float SlowDownMultiplier = 0.5f;
        public static readonly float MouseSpeedMultiplier = 1f;
        public static readonly float CatSpeedMultiplier = 0.7f;
        public static readonly float DefaultSpawnRate = 1.25f;
        public static readonly float MouseManiaSpawnRateMultiplier = 4f;
        public static readonly float CatManiaSpawnRateMultiplier = 8f;
        public static readonly float ArrowLifetime = 20f;
        public static readonly float CatHitMultiplier = 0.66f;
        public static readonly float ModeMouseChance = 0.05f;
        public static readonly float BonusMouseChance = 0.02f;
        public static readonly int MouseBonusValue = 50;
        public static readonly int MaxMouseCount = 50;
        public static readonly int MaxCatCount = 4;
        public static readonly int MaxPlayerCount = 4;
        public static readonly int MaxArrowsPerPlayer = 3;
        public static readonly int ArrowHitpoints = 2;
        public static readonly Vector2[] SelectorPositions = new Vector2[]
        {
            new Vector2(5, 5),
            new Vector2(6, 5),
            new Vector2(5, 6),
            new Vector2(6, 6)
        };
        public static readonly int DicePositions = 24;
        public static readonly List<int> CatDiceThrows = new List<int> { 0 };
        public static readonly List<int> MouseDiceThrows = new List<int> { 2, 4, 6, 8, 10, 12, 14, 16, 18 };
    }   

    public static class Paths
    {
        public static readonly string StagesFolder = $"{Application.streamingAssetsPath}/";
        public static readonly string StageExtension = ".mew";
    }
}

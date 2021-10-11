using UnityEngine;

namespace Mew.Constants
{
    public static class Colors
    {
        public static readonly Color[] PlayerColor = new Color[]
        {
            Color.blue,
            Color.red,
            Color.green,
            Color.yellow
        };
    }

    public static class Scenes
    {
        public static readonly string Menu = "Menu";
        public static readonly string Game = "Game";
    }

    public static class Tags
    {
        public static readonly string Mouse = "Mouse";
        public static readonly string Cat = "Cat";
        public static readonly string Death = "Death";
        public static readonly string Rocket = "Rocket";
    }

    public static class Settings
    {
        public static readonly Vector2 BoardSize = new Vector2(12, 9);
        public static readonly int MatchTime = 180;
        public static readonly float DefaultSpeed = 4f;
        public static readonly float MouseSpeedMultiplier = 1f;
        public static readonly float CatSpeedMultiplier = 0.7f;
        public static readonly float DefaultSpawnRate = 1.25f;
        public static readonly float ArrowLifetime = 20f;
        public static readonly float CatHitMultiplier = 0.66f;
        public static readonly float ModeMouseChance = 0.05f;
        public static readonly float BonusMouseChance = 0.02f;
        public static readonly int MouseBonusValue = 50;
        public static readonly int MaxMouseCount = 50;
        public static readonly int MaxCatCount = 4;
        public static readonly int MaxArrowsPerPlayer = 3;
        public static readonly Vector2[] SelectorPositions = new Vector2[]
        {
            new Vector2(5, 5),
            new Vector2(6, 5),
            new Vector2(5, 6),
            new Vector2(6, 6)
        };
    }   

    public static class Paths
    {
        public static readonly string StagesFolder = $"{Application.streamingAssetsPath}/";
    }
}

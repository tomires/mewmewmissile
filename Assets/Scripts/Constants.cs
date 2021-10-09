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

    public static class Settings
    {
        public static readonly Vector2 BoardSize = new Vector2(12, 9);
    }   

    public static class Paths
    {
        public static readonly string StagesFolder = $"{Application.streamingAssetsPath}/";
    }
}

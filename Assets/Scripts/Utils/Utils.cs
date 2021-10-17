using System.IO;
using UnityEngine;
using Mew.Models;
using System.Collections.Generic;

namespace Mew
{
    public static class Utils
    {
        public static Vector2 GetOffset(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Vector2.up,
                Direction.Down => Vector2.down,
                Direction.Left => Vector2.left,
                Direction.Right => Vector2.right,
                _ => Vector2.zero
            };
        }

        public static Vector2 PropagateOffset(Vector2 coordinates, Direction direction)
        {
            var proposedCoordinates = coordinates + GetOffset(direction);
            var boardSize = Constants.Settings.BoardSize;

            return new Vector2(
                Clamp(proposedCoordinates.x, 0, boardSize.x - 1),
                Clamp(proposedCoordinates.y, 0, boardSize.y - 1));
        }

        public static Vector3 CoordinatesTo3D(Vector2 coordinates)
        {
            return new Vector3(coordinates.x, 0, coordinates.y);
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        public static string TimeToHumanReadable(int time)
        {
            return $"{ Mathf.FloorToInt(time / 60) }:{ string.Format("{0:00}", time % 60) }";
        }

        public static List<string> GetAllStagePaths()
        {
            var files = Directory.GetFiles(Constants.Paths.StagesFolder);
            var stageFiles = new List<string>();
            foreach (var file in files)
                if (file.Contains(Constants.Paths.StageExtension) && !file.Contains(".meta"))
                    stageFiles.Add(file);
            return stageFiles;
        }
    }
}

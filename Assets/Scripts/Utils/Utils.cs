using UnityEngine;
using Mew.Models;

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
    }
}

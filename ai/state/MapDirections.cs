using System;
using System.Collections.Generic;

namespace ai
{
    public class MapDirections
    {

        public enum Direction
        {
            None, // Default value, don't reorder.
            North,
            East,
            South,
            West
        }

        public static Direction[] Directions = new Direction[] { Direction.North, Direction.East, Direction.South, Direction.West };

        public static Dictionary<Direction, Direction> TurnMap = new Dictionary<Direction, Direction> {
            { Direction.North, Direction.East },
            { Direction.East, Direction.South },
            { Direction.South, Direction.West },
            { Direction.West, Direction.North }
        };

        public static Dictionary<Direction, (int, int)> OffsetMap = new Dictionary<Direction, (int, int)> {
            { Direction.North, (0, -1) },
            { Direction.East, (1, 0) },
            { Direction.South, (0, 1) },
            { Direction.West, (-1, 0) },
            { Direction.None, (0, 0) }
        };

        private static Random Random = new Random();

        public static Direction CardinalDirection((int X, int Y) location, (int X, int Y) destination)
        {
            if (destination.Y < location.Y) return Direction.North;
            if (destination.Y > location.Y) return Direction.South;
            if (destination.X > location.X) return Direction.East;
            return Direction.West;
        }

        public static Direction RandomDirection()
        {
            return Directions[Random.Next(4)];
        }

        public static Direction Turn(Direction direction)
        {
            return TurnMap[direction];
        }

        public static (int X, int Y) OffsetForDirection(Direction direction)
        {
            return OffsetMap[direction];
        }

    }
}
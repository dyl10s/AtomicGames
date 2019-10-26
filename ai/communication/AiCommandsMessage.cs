using System;
using System.Collections.Generic;

namespace ai
{
    public class AICommandsMessage
    {
        public IEnumerable<AICommand> Commands { get; set; }
    }

    public class AICommand
    {
        public const string Move = "MOVE";
        public const string Gather = "GATHER";
        public const string Create = "CREATE";
        public const string Shoot = "SHOOT";
        public const string Melee = "MELEE";
        public const string Identify = "IDENTIFY";

        public const string North = "N";
        public const string East = "E";
        public const string South = "S";
        public const string West = "W";

        private static Dictionary<MapDirections.Direction, string> DirectionLookup = new Dictionary<MapDirections.Direction, string>() {
            {MapDirections.Direction.North, North},
            {MapDirections.Direction.South, South},
            {MapDirections.Direction.East, East},
            {MapDirections.Direction.West, West}
        };

        public string Command { get; set; }
        public int Unit { get; set; }
        public string Dir { get; set; }
        public string Type { get; set; }
        public int Dx { get; set; }
        public int Dy { get; set; }
        public int Target { get; set; }

        public static AICommand BuildMoveCommand(Unit unit, MapDirections.Direction direction)
        {
            return new AICommand { Command = AICommand.Move, Unit = unit.Id, Dir = SerializeDirection(direction) };
        }

        public static AICommand BuildGatherCommand(Unit unit, MapDirections.Direction direction)
        {
            return new AICommand { Command = AICommand.Gather, Unit = unit.Id, Dir = SerializeDirection(direction) };
        }

        public static AICommand BuildUnitCommand(string type)
        {
            return new AICommand { Command = AICommand.Create, Type = type };
        }

        public static AICommand BuildShootCommand(Unit unit, (int X, int Y) location)
        {
            return new AICommand { Command = AICommand.Shoot, 
                                   Unit = unit.Id, 
                                   Dx = (location.X - unit.Location.X), 
                                   Dy = (location.Y - unit.Location.Y)};
        }

        public static string SerializeDirection(MapDirections.Direction direction)
        {
            return DirectionLookup.GetValueOrDefault(direction);
        }
    }
}
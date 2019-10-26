using System;
using System.Collections.Generic;
using System.Text;

namespace ai.unitStrategies
{
    public class TankStrategy
    {
        public static Dictionary<int, string> navDirection = new Dictionary<int, string>();
        public static bool inDefenseMode = true;

        public static AICommand GetStrategy(IMap map, Unit unit)
        {
            var command = new AICommand();

            command.Command = AICommand.Move;
            command.Unit = unit.Id;

            if (inDefenseMode)
            {
                command.Dir = Defend(map, unit);

                if(command.Dir == "None"){ //Turret Mode

                }
            }
            else
            {
                command.Dir = Explore(map, unit);
            }

            return command;
        }

        public static string Defend(IMap map, Unit unit)
        {
            PathFinder finder = new PathFinder(map);
            var path = finder.FindPath(unit.Location, map.HomeBaseLocation);

            if(path.Count == 0) //Already on base
            {
                return "None";
            }
            else
            {
                return Globals.directionToAdjactentPoint(unit.Location, path[0]);
            }
        }

        //public static AICommand Turret(IMap map, Unit unit)
        //{
        //    var atkCommand = new AICommand();
        //    atkCommand.Command = AICommand.Shoot;

        //    var locations = map.EnemyLocationsInRange(unit.Location, 2);
        //    if(locations.Count > 0)
        //    {

        //    }
        //}

        public static string Explore(IMap map, Unit unit)
        {
            PathFinder finder = new PathFinder(map);

            if (!navDirection.ContainsKey(unit.Id))
            {
                navDirection.Add(unit.Id, AICommand.SerializeDirection(MapDirections.RandomDirection()));
            }

            var results = finder.FindPath(unit.Location, GetPointFromDir(unit.Location, navDirection[unit.Id]));

            if (results == null)
            {
                var rnd = new Random();
                if (navDirection[unit.Id] == "N" || navDirection[unit.Id] == "S")
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        navDirection[unit.Id] = "E";
                    }
                    else
                    {
                        navDirection[unit.Id] = "W";
                    }
                }
                else if (navDirection[unit.Id] == "E" || navDirection[unit.Id] == "W")
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        navDirection[unit.Id] = "N";
                    }
                    else
                    {
                        navDirection[unit.Id] = "S";
                    }
                }
            }
            else
            {
                return Globals.directionToAdjactentPoint(unit.Location, results[0]);
            }

            return "None";
        }

        public static (int x, int y) GetPointFromDir((int x, int y) start, string dir)
        {
            if (dir == "N")
            {
                start.x -= 1;
                return start;
            }

            if (dir == "S")
            {
                start.x += 1;
                return start;
            }

            if (dir == "E")
            {
                start.y += 1;
                return start;
            }

            if (dir == "W")
            {
                start.y -= 1;
                return start;
            }

            return start;
        }
    }
}

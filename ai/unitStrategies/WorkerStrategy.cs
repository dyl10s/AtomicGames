using System;
using System.Collections.Generic;
using System.Text;

namespace ai.unitStrategies
{
    public class WorkerStrategy
    {
        public static Dictionary<int, string> navDirection = new Dictionary<int, string>();

        public static AICommand GetStrategy(IMap map, Unit unit)
        {
            var command = new AICommand();
            command.Command = AICommand.Move;
            command.Unit = unit.Id;
            
            if (unit.CarryingResource)
            {
                command.Dir = MoveToBase(map, unit);
            }
            else if (map.HasResources)
            {
                command.Dir = GetDirectionToGem(map, unit);
                if (command.Dir == "None")
                {
                    command.Command = AICommand.Gather;
                    command.Dir = AICommand.SerializeDirection(map.DirectionToAdjacentResource(unit.Location));
                }

                if(command.Dir == "no path")
                {
                    command.Dir = Explore(map, unit);
                }
            }
            else
            {
                command.Dir = Explore(map, unit);
            }

            return command;
        }

        public static string GetDirectionToGem(IMap map, Unit unit)
        {
            PathFinder finder = new PathFinder(map);

            if (map.HasResources)
            {
                var closeList = map.ResourceLocationsNearest(unit.Location);
                (int x, int y) closePoint = (0, 0);
                int lowestDistance = -1;

                foreach (var x in closeList)
                {
                    var dist = map.CalculateEstimatedDistance(x, unit.Location);
                    if (lowestDistance == -1)
                    {
                        lowestDistance = dist;
                        closePoint = x;
                    }
                    else if (dist < lowestDistance)
                    {
                        lowestDistance = dist;
                        closePoint = x;
                    }
                }

                var steps = finder.FindPath(unit.Location, closePoint, 1);
                if(steps == null)
                {
                    return "no path";
                }

                if(steps.Count > 0)
                {
                    return Globals.directionToAdjactentPoint(unit.Location, steps[0]);
                }
                else
                {
                    return "None";
                }
            }

            return "None";
        }
    
        public static string MoveToBase(IMap map, Unit unit)
        {
            PathFinder finder = new PathFinder(map);
            var steps = finder.FindPath(unit.Location, map.HomeBaseLocation, 0);
            if (steps.Count > 0)
            {
                return Globals.directionToAdjactentPoint(unit.Location, steps[0]);
            }
            else
            {
                return "None";
            }
        }

        public static string Explore(IMap map, Unit unit)
        {
            PathFinder finder = new PathFinder(map);

            if (!navDirection.ContainsKey(unit.Id))
            {
                navDirection.Add(unit.Id, AICommand.SerializeDirection(MapDirections.RandomDirection()));
            }

            var results = finder.FindPath(unit.Location, GetPointFromDir(unit.Location, navDirection[unit.Id]));

            if(results == null)
            {
                var rnd = new Random();
                if (navDirection[unit.Id] == "N" || navDirection[unit.Id] == "S")
                {
                    if(rnd.Next(0, 2) == 0)
                    {
                        navDirection[unit.Id] = "E";
                    }
                    else
                    {
                        navDirection[unit.Id] = "W";
                    }
                }
                else if(navDirection[unit.Id] == "E" || navDirection[unit.Id] == "W")
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

        public static (int x, int y) GetPointFromDir((int x, int y)start, string dir)
        {
            if(dir == "N")
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

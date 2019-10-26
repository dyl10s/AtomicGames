using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ai.unitStrategies
{
    public class ScoutStrategy
    { 
       
        public static List<(int x, int y)> hasResources = new List<(int x, int y)>();

        public static Dictionary<int, string> navDirection = new Dictionary<int, string>();

        public static List<(int unit, string unitId)> trackScountUnit = new List<(int unit, string unitId)>();

        public static AICommand GetStrategy(IMap map, Unit unit)
        {

            var rand = new Random();
            var command = new AICommand();
            command.Command = AICommand.Move;
            command.Unit = unit.Id;

            var enemies = map.EnemyLocationsInRange(unit.Location, 100);
            
            var closestEnemyLocation = (0, 0);
            var closestEmenyDistance = -1;

            if (enemies.Count > 0)
            {
                var enemiesAttack = map.EnemyLocationsInRange(unit.Location, 1);

                if (enemiesAttack.Count > 0)
                {
                    if (unit.CanAttack)
                    {
                        return new AICommand()
                        {
                            Command = AICommand.Melee,
                            Target = map[(enemiesAttack[0].X, enemiesAttack[0].Y)].TileUpdate.Units[0].Id,
                            Unit = unit.Id
                        };
                    }
                }
               
                foreach (var e in enemies)
                {
                    var enemyDist = map.CalculateEstimatedDistance(unit.Location, e);

                    if (closestEmenyDistance == -1)
                    {
                        closestEnemyLocation = e;
                        closestEmenyDistance = enemyDist;
                    }
                    else if(enemyDist < closestEmenyDistance)
                    {
                        closestEmenyDistance = enemyDist;
                        closestEnemyLocation = e;
                    }
                }

                command.Dir = MoveToPoint(map, unit, closestEnemyLocation);
                if(command.Dir == "None")
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

        public static string MoveToPoint(IMap map, Unit unit, (int x, int y) point)
        {
            PathFinder finder = new PathFinder(map);
            var steps = finder.FindPath(unit.Location, point, 0);
            if (steps != null && steps.Count > 0)
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

            if (results == null)
            {
                var rnd = new Random();

                if (navDirection[unit.Id] == "N" || navDirection[unit.Id] == "S")
                {
                    if (rnd.Next(0, 8) == 0)
                    {
                        if(!map[(unit.Location.X - 1, unit.Location.Y)].TileUpdate.Blocked)
                        {
                            navDirection[unit.Id] = "W";
                        }
                        else
                        {
                            if (!map[(unit.Location.X + 1, unit.Location.Y)].TileUpdate.Blocked)
                            {
                                navDirection[unit.Id] = "E";
                            }
                            else
                            {
                                if(navDirection[unit.Id] == "N")
                                {
                                    navDirection[unit.Id] = "S";
                                }
                                else
                                {
                                    navDirection[unit.Id] = "N";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!map[(unit.Location.X + 1, unit.Location.Y)].TileUpdate.Blocked)
                        {
                            navDirection[unit.Id] = "E";
                        }
                        else
                        {
                            if (!map[(unit.Location.X - 1, unit.Location.Y)].TileUpdate.Blocked)
                            {
                                navDirection[unit.Id] = "W";
                            }
                            else
                            {
                                if (navDirection[unit.Id] == "N")
                                {
                                    navDirection[unit.Id] = "S";
                                }
                                else
                                {
                                    navDirection[unit.Id] = "N";
                                }
                            }
                        }
                    }
                }
                else if (navDirection[unit.Id] == "E" || navDirection[unit.Id] == "W")
                {
                    if (rnd.Next(0, 8) == 0)
                    {
                        if (!map[(unit.Location.X, unit.Location.Y - 1)].TileUpdate.Blocked)
                        {
                            navDirection[unit.Id] = "N";
                        }
                        else
                        {
                            if (!map[(unit.Location.X, unit.Location.Y + 1)].TileUpdate.Blocked)
                            {
                                navDirection[unit.Id] = "S";
                            }
                            else
                            {
                                if (navDirection[unit.Id] == "E")
                                {
                                    navDirection[unit.Id] = "W";
                                }
                                else
                                {
                                    navDirection[unit.Id] = "E";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!map[(unit.Location.X, unit.Location.Y + 1)].TileUpdate.Blocked)
                        {
                            navDirection[unit.Id] = "S";
                        }
                        else
                        {
                            if (!map[(unit.Location.X, unit.Location.Y - 1)].TileUpdate.Blocked)
                            {
                                navDirection[unit.Id] = "N";
                            }
                            else
                            {
                                if (navDirection[unit.Id] == "E")
                                {
                                    navDirection[unit.Id] = "W";
                                }
                                else
                                {
                                    navDirection[unit.Id] = "E";
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                return Globals.directionToAdjactentPoint(unit.Location, results[0]);
            }

            return "None";
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
                if (steps == null)
                {
                    return "no path";
                }

                if (steps.Count > 0)
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


        public static (int x, int y) GetPointFromDir((int x, int y) start, string dir)
        {
            if (dir == "N")
            {
                start.y -= 1;
                return start;
            }

            if (dir == "S")
            {
                start.y += 1;
                return start;
            }

            if (dir == "E")
            {
                start.x += 1;
                return start;
            }

            if (dir == "W")
            {
                start.x -= 1;
                return start;
            }

            return start;
        }
    }

}


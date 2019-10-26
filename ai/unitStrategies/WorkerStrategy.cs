using System;
using System.Collections.Generic;
using System.Text;

namespace ai.unitStrategies
{
    public class WorkerStrategy
    {
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
            }
            else
            {
                command.Dir = AICommand.SerializeDirection(MapDirections.RandomDirection());
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
                if(steps != null && steps.Count > 0)
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
    }
}

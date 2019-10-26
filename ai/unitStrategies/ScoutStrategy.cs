using System;
using System.Collections.Generic;
using System.Text;


namespace ai.unitStrategies
{
    public class ScoutStrategy
    {
        public static AICommand GetStrategy(IMap map, Unit unit)
        {
            PathFinder pfinder = new PathFinder(map);

            var command = new AICommand();


            command.Command = AICommand.Move;  
            command.Unit = unit.Id;
            command.Dir = Globals.North;

            /*
            if (hitsWall)
            {
                command.Dir = Globals.East;
            }
            //Still need to implement this bad boy
            else if (enemyIsNear)
            {
                command.Dir = Globals.North;
                command.Dir = Globals.South;

            }

            else if (map.EnemyBaseFound == true)
            {
                command.Dir = Globals.South;

            }

            else if(map.HasEnemies )

            */
            return command;
        }
    }
}

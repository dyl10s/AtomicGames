using System;
using System.Collections.Generic;
using System.Text;

namespace ai.unitStrategies
{
    class BaseStrategy
    {
        public static int startupCommand = -1;

        public static AICommand GetStrategy(IMap map, Unit unit)
        {
            if (unit.IsIdle)
            {
                startupCommand++;
                return StartupFunctions(map, unit);
            }
            else
            {
                return new AICommand();
            }
        }

        public static AICommand StartupFunctions(IMap map, Unit unit)
        {
            if (startupCommand == 0)
            {
                return new AICommand()
                {
                    Command = AICommand.Create,
                    Type = "scout"
                };
            }
            else if (startupCommand == 2)
            {
                return new AICommand()
                {
                    Command = AICommand.Create,
                    Type = "worker"
                };
            }
            else if (startupCommand == 4)
            {
                return new AICommand()
                {
                    Command = AICommand.Create,
                    Type = "worker"
                };
            }
            else if (startupCommand == 6)
            {
                return new AICommand()
                {
                    Command = AICommand.Create,
                    Type = "worker"
                };
            }
            else if (startupCommand == 8)
            {
                return new AICommand()
                {
                    Command = AICommand.Create,
                    Type = "scout"
                };
            }
            else if (startupCommand == 10)
            {
                return new AICommand()
                {
                    Command = AICommand.Create,
                    Type = "scout"
                };
            }

            if (Globals.um.WorkerCount < 8)
            {
                return new AICommand()
                {
                    Command = AICommand.Create,
                    Type = "worker"
                };
            }

            if (map.EnemyBaseFound)
            {
                if(unit.ResourcesAvailable > 200)
                {
                    return new AICommand()
                    {
                        Command = AICommand.Create,
                        Type = "tank"
                    };
                }
            }

            return new AICommand();
        }
    }
}

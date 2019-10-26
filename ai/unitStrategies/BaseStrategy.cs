﻿using System;
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
                return StartupFunctions();
            }
            else
            {
                return new AICommand();
            }
        }

        public static AICommand StartupFunctions()
        {
            if (startupCommand == 0)
            {
                return new AICommand()
                {
                    Command = AICommand.Create,
                    Type = "scout"
                };
            }

            if (startupCommand == 2)
            {
                return new AICommand()
                {
                    Command = AICommand.Create,
                    Type = "worker"
                };
            }

            if (startupCommand == 4)
            {
                return new AICommand()
                {
                    Command = AICommand.Create,
                    Type = "worker"
                };
            }

            if (startupCommand == 6)
            {
                return new AICommand()
                {
                    Command = AICommand.Create,
                    Type = "worker"
                };
            }

            if (startupCommand == 8)
            {
                return new AICommand()
                {
                    Command = AICommand.Create,
                    Type = "worker"
                };
            }

            if (startupCommand == 10)
            {
                return new AICommand()
                {
                    Command = AICommand.Create,
                    Type = "worker"
                };
            }

            if (startupCommand == 12)
            {
                return new AICommand()
                {
                    Command = AICommand.Create,
                    Type = "worker"
                };
            }

            return new AICommand();
        }
    }
}

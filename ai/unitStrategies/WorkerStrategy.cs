﻿using System;
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
            command.Dir = Globals.East;
            return command;
        }
    }
}

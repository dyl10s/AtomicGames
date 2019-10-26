using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ai
{

    public class AIStrategy : IAIStrategy
    {
        private readonly IUnitManager unitManager;
        private readonly IMap map;

        public AIStrategy(IUnitManager unitManager, IMap map)
        {
            this.unitManager = unitManager;
            this.map = map;
        }

        public IList<AICommand> BuildCommandList()
        {
            return unitManager.Units.Values.Select(unit => unit.BuildCommand())
                                           .Where(command => command != null)
                                           .ToList();
        }
    }

}
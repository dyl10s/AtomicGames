using System;
using System.Collections.Generic;
using System.Linq;
using ai.unitStrategies;

namespace ai
{
    public class ExploreStrategy : IUnitStrategy
    {
        private readonly IMap Map;
        private readonly IUnitManager UnitManager;

        public ExploreStrategy(IMap map, Unit unit, UnitManager unitManager)
        {
            Map = map;
            UnitManager = unitManager;
        }

        public AICommand BuildCommand(Unit unit)
        {
            var direction = AICommand.SerializeDirection(MapDirections.RandomDirection());
            var returnAction = new AICommand { Command = direction, Unit = unit.Id, Dir = direction };

            if (unit.IsScout)
            {
                returnAction = ScoutStrategy.GetStrategy(Map, unit);
            }

            if (unit.IsWorker)
            {
                returnAction = WorkerStrategy.GetStrategy(Map, unit);
            }

            if (unit.IsTank)
            {
                returnAction = TankStrategy.GetStrategy(Map, unit);
            }

            if (unit.IsBase)
            {
                returnAction = BaseStrategy.GetStrategy(Map, unit);
            }

            if(returnAction != null)
            {
                if (returnAction.Dir == "None")
                {
                    return null;
                }
            }
            

            return returnAction;
        }
    }
}
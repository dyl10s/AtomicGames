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


            if (unit.IsScout)
            {
                return ScoutStrategy.GetStrategy(Map, unit);
            }

            if (unit.IsWorker)
            {
                return WorkerStrategy.GetStrategy(Map, unit);
            }

            if (unit.IsTank)
            {
                return TankStrategy.GetStrategy(Map, unit);
            }

            var direction = AICommand.SerializeDirection(MapDirections.RandomDirection());
            return new AICommand { Command = direction, Unit = unit.Id, Dir = direction };
        }
    }
}
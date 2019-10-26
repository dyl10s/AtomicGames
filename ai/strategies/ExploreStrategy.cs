using System;
using System.Collections.Generic;
using System.Linq;

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

            }

            if (unit.IsWorker)
            {

            }

            if (unit.IsTank)
            {

            }

            var direction = AICommand.SerializeDirection(MapDirections.RandomDirection());
            return new AICommand { Command = AICommand.Move, Unit = unit.Id, Dir = direction };
        }
    }
}
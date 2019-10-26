using System;

namespace ai
{
    public class UnitStrategyFactory
    {
        public void AssignStrategy(IMap map, Unit unit, UnitManager unitManager)
        {
            if (unit.Strategy == null)
            {
                unit.Strategy = BuildStrategy(map, unit, unitManager);
            }
        }

        private IUnitStrategy BuildStrategy(IMap map, Unit unit, UnitManager unitManager)
        {
            if (unit.IsMobile)
            {
                return BuildExploreStrategy(map, unit, unitManager);
            }
            return null;
        }

        private ExploreStrategy BuildExploreStrategy(IMap map, Unit unit, UnitManager unitManager)
        {
            return new ExploreStrategy(map, unit, unitManager);
        }
    }
}
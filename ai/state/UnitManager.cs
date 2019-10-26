using System;
using System.Collections.Generic;
using System.Linq;

namespace ai
{
    public class UnitManager : IUnitManager
    {
        public IDictionary<int, Unit> Units { get; } = new Dictionary<int, Unit>();
        public UnitInfoUpdate TankInfo { get; set; }
        public UnitInfoUpdate WorkerInfo { get; set; }
        public UnitInfoUpdate ScoutInfo { get; set; }
        private readonly IMap Map;
        private readonly UnitStrategyFactory StrategyFactory;

        public UnitManager(IMap map, UnitStrategyFactory factory)
        {
            Map = map;
            StrategyFactory = factory;
        }

        public void UpdateUnits(IEnumerable<UnitUpdate> unitUpdates)
        {
            unitUpdates.ToList().ForEach(u =>
            {
                UpdateUnit(u);
            });
        }

        private void UpdateUnit(UnitUpdate u)
        {
            var unit = GetUnitForUpdate(u);
            unit.UnitUpdate = u;
            UpdateUnitStrategy(unit);
        }

        private void UpdateUnitStrategy(Unit unit)
        {
            StrategyFactory.AssignStrategy(Map, unit, this);
        }

        private Unit GetUnitForUpdate(UnitUpdate u)
        {
            if (!Units.ContainsKey(u.Id))
            {
                Units[u.Id] = new Unit();
            }
            return Units[u.Id];
        }

        public int TankCount
        {
            get { return Units.Values.Count(u => u.IsTank && u.IsAlive); }
        }

        public int WorkerCount
        {
            get { return Units.Values.Count(u => u.IsWorker && u.IsAlive); }
        }

        public int ScoutCount
        {
            get { return Units.Values.Count(u => u.IsScout && u.IsAlive); }
        }

        public int Range(Unit unit)
        {
            if (unit.IsScout)
            {
                return ScoutInfo.Range;
            }
            if (unit.IsTank)
            {
                return TankInfo.Range;
            }
            return WorkerInfo.Range;
        }

        public void UpdateGameInfo(GameInfoUpdate gameInfo)
        {
            TankInfo = gameInfo.UnitInfo[UnitUpdate.TypeTank];
            ScoutInfo = gameInfo.UnitInfo[UnitUpdate.TypeScout];
            WorkerInfo = gameInfo.UnitInfo[UnitUpdate.TypeWorker];
        }
    }
}
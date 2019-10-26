using System;
using System.Collections.Generic;
using System.Linq;

namespace ai
{
    public class Unit
    {
        public UnitUpdate UnitUpdate { get; set; }
        public int Id { get { return UnitUpdate.Id; } }
        public IUnitStrategy Strategy { get; set; }
        public List<(int X, int Y)> Path { get; set; }
        public (int X, int Y) Location { get => (UnitUpdate.X, UnitUpdate.Y); }
        public bool IsIdle { get => UnitUpdate.IsIdle; }
        public bool IsWorker { get => UnitUpdate.IsWorker; }
        public bool IsScout { get => UnitUpdate.IsScout; }
        public bool IsTank { get => UnitUpdate.IsTank; }
        public bool IsBase { get => UnitUpdate.IsBase; }
        public bool IsMobile { get => IsWorker || IsScout || IsTank; }
        public bool HasStrategy { get => Strategy != null; }
        public bool HasPath { get => Path != null && Path.Count > 0; }
        public bool CarryingResource { get => UnitUpdate.Resource > 0; }
        public int ResourcesAvailable { get => UnitUpdate.Resource; }
        public bool CanAttack { get => UnitUpdate.CanAttack; }
        public bool IsAlive { get => UnitUpdate.IsAlive; }

        public virtual AICommand BuildCommand()
        {
            if (IsIdle && HasStrategy)
            {
                return Strategy.BuildCommand(this);
            }
            return null;
        }

        public MapDirections.Direction NextMove()
        {
            if (HasPath)
            {
                var direction = Path.First();
                Path.RemoveAt(0);
                return MapDirections.CardinalDirection(Location, direction);
            }
            else
            {
                return MapDirections.RandomDirection();
            }
        }

        public bool AdjacentToResource(IMap map)
        {
            return map.IsResourceAdjacentTo(Location);
        }
    }
}
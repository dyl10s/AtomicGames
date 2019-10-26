using System;
using System.Collections.Generic;
using System.Linq;

namespace ai
{
    public class UnitUpdate
    {
        public const string StatusIdle = "idle";
        public const string StatusMoving = "moving";
        public const string StatusDead = "dead";
        public const string StatusUnknown = "unknown";

        public const string TypeWorker = "worker";
        public const string TypeScout = "scout";
        public const string TypeTank = "tank";
        public const string TypeBase = "base";

        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Resource { get; set; }
        public int Health { get; set; }
        public int PlayerId { get; set; }
        public string Type { get; set; }
        public bool CanAttack { get; set; }
        public string Status { get; set; }
        public int Range { get; set; }
        public int Speed { get; set; }
        public bool IsAlive { get => Status != StatusDead; }
        public bool IsBase { get => Type == TypeBase; }
        public bool IsWorker { get => Type == TypeWorker; }
        public bool IsScout { get => Type == TypeScout; }
        public bool IsTank { get => Type == TypeTank; }
        public bool IsIdle { get => Status == StatusIdle; }
    }

    public class ResourceUpdate
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Total { get; set; }
        public int Value { get; set; }
    }

    public class TileUpdate
    {
        public bool Visible { get; set; }
        public bool Blocked { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public ResourceUpdate Resources { get; set; }
        public ResourceUpdate Resource { get => Resources; }
        public IList<UnitUpdate> Units { get; set; }
    }

    public class UnitInfoUpdate
    {
        public int HP { get; set; }
        public int Range { get; set; }
        public int Cost { get; set; }
        public int Speed { get; set; }
        public int AttackDamage { get; set; }
        public string AttackType { get; set; }
        public int AttackCooldownDuration { get; set; }
        public bool CanCarry { get; set; }
        public int CreateTime { get; set; }
    }

    public class GameInfoUpdate
    {
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
        public int GameDuration { get; set; }
        public int TurnDuration { get; set; }
        public Dictionary<string, UnitInfoUpdate> UnitInfo { get; set; }

    }

    public class GameUpdate
    {
        public int Player { get; set; }
        public int Turn { get; set; }
        public int Time { get; set; }
        public IList<UnitUpdate> UnitUpdates { get; set; }
        public IList<TileUpdate> TileUpdates { get; set; }
        public GameInfoUpdate GameInfo { get; set; }
    }
}
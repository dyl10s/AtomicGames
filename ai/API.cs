using System;
using System.Collections.Generic;

namespace ai
{
    public interface IServerConnection
    {
        void AcceptConnection();
        GameUpdate ReadUpdate();
        void SendCommands(IEnumerable<AICommand> commands);
        void Close();
    }

    public interface IAILoop
    {
        void RunLoop();
    }

    public interface IGameUpdateListener
    {
        void HandleGameUpdate(GameUpdate update);
    }

    public interface IGameStateManager : IGameUpdateListener
    {
        IUnitManager UnitManager { get; }
        IMap Map { get; }
    }

    public interface IUnitManager
    {
        IDictionary<int, Unit> Units { get; }
        int TankCount { get; }
        int WorkerCount { get; }
        int ScoutCount { get; }
        int Range(Unit unit);
        UnitInfoUpdate TankInfo { get; set; }
        UnitInfoUpdate WorkerInfo { get; set; }
        UnitInfoUpdate ScoutInfo { get; set; }
        void UpdateGameInfo(GameInfoUpdate gameInfo);
        void UpdateUnits(IEnumerable<UnitUpdate> unitUpdates);
    }

    public interface IMap
    {
        (int Width, int Height) Size { get; set; }
        Tile this[(int X, int Y) location] { get; set; }
        bool HasResources { get; }
        (int X, int Y) HomeBaseLocation { get; }
        (int X, int Y) EnemyBaseLocation { get; }
        bool HasEnemies { get; }
        bool EnemyBaseFound { get; }
        void UpdateTiles(IEnumerable<TileUpdate> tileUpdates);
        bool CanMove((int X, int Y) location, MapDirections.Direction heading);
        int CalculateEstimatedDistance((int X, int Y) from, (int X, int Y) to);
        bool LocationWithinRange((int X, int Y) from, (int X, int Y) to, int range);
        bool HasUnknownNeighbors((int X, int Y) location, int range = 1);
        List<(int X, int Y)> ResourceLocationsNearest((int X, int Y) location);
        bool IsResourceAdjacentTo((int X, int Y) location);
        MapDirections.Direction DirectionToAdjacentResource((int X, int Y) location);
        List<(int X, int Y)> EnemyLocationsInRange((int X, int Y) start, int range = 1);
    }

    public interface IUnitStrategy
    {
        AICommand BuildCommand(Unit unit);
    }

    public interface IAIStrategy
    {
        IList<AICommand> BuildCommandList();
    }

}
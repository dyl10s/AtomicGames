using System;
using System.Collections.Generic;

namespace ai
{
    public class GameStateManager : IGameStateManager
    {
        public IUnitManager UnitManager { get; }
        public IMap Map { get; }

        public GameStateManager(IUnitManager unitManager, IMap map)
        {
            UnitManager = unitManager;
            Map = map;
        }

        public void HandleGameUpdate(GameUpdate update)
        {
            if (update.GameInfo != null) UpdateGameInfo(update.GameInfo);
            if (update.UnitUpdates != null) UpdateUnits(update.UnitUpdates);
            if (update.TileUpdates != null) UpdateTiles(update.TileUpdates);

        }

        private void UpdateGameInfo(GameInfoUpdate gameInfo)
        {
            Map.Size = (gameInfo.MapWidth, gameInfo.MapHeight);
            UnitManager.UpdateGameInfo(gameInfo);
        }


        private void UpdateUnits(IList<UnitUpdate> unitUpdates)
        {
            UnitManager.UpdateUnits(unitUpdates);
        }


        private void UpdateTiles(IList<TileUpdate> tileUpdates)
        {
            Map.UpdateTiles(tileUpdates);
        }
    }
}
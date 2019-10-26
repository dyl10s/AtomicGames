using Xunit;
using Moq;
using System.Collections.Generic;

namespace ai.test
{
    public class GameStateManagerTest
    {
        [Fact]
        public void Test_HandleGameUpdate()
        {
            var unitManager = new Mock<IUnitManager>();
            var map = new Mock<IMap>();

            var units = new List<UnitUpdate>();
            var tiles = new List<TileUpdate>();
            var update = new GameUpdate();

            update.UnitUpdates = units;
            update.TileUpdates = tiles;

            new GameStateManager(unitManager.Object, map.Object).HandleGameUpdate(update);

            unitManager.Verify(u => u.UpdateUnits(units));
            map.Verify(m => m.UpdateTiles(tiles));
        }
    }
}

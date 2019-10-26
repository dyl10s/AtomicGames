using Xunit;
using FluentAssertions;
using System;
using FluentAssertions.Execution;
using System.Collections.Generic;
using System.Linq;

namespace ai.test
{
    public class MapTest
    {
        [Fact]
        public void Test_Can_Retrieve_Tiles()
        {
            var map = new Map();
            map[(1, 1)].TileUpdate.Visible.Should().BeFalse();
            map[(1, 1)].TileUpdate.Blocked.Should().BeTrue();
            map[(1, 1)].X.Should().Be(1);
            map[(1, 1)].Y.Should().Be(1);

            var tileUpdates = new List<TileUpdate>() {
                new TileUpdate() { Visible = true, Blocked = false, X = 1, Y = 1, Resources = null, Units = null }
            };
            map.UpdateTiles(tileUpdates);
            map[(1, 1)].TileUpdate.Visible.Should().BeTrue();
            map[(1, 1)].TileUpdate.Blocked.Should().BeFalse();
            map[(1, 1)].TileUpdate.Units.Should().BeNull();

            tileUpdates = new List<TileUpdate>() {
                new TileUpdate() { Visible = true, Blocked = false, X = 1, Y = 1, Resources = new ResourceUpdate { Id = 5, Type = "small", Total = 10, Value = 5}, Units = new List<UnitUpdate> {new UnitUpdate() {Id = 12, Type = "tank"}} },
                new TileUpdate() { Visible = true, Blocked = true, X = 2, Y = 2, Resources = null, Units = null }

            };
            map.UpdateTiles(tileUpdates);
            map[(1, 1)].TileUpdate.Visible.Should().BeTrue();
            map[(1, 1)].TileUpdate.Blocked.Should().BeFalse();

            map[(1, 1)].TileUpdate.Resource.Id.Should().Be(5);
            map[(1, 1)].TileUpdate.Resource.Type.Should().Be("small");
            map[(1, 1)].TileUpdate.Resource.Total.Should().Be(10);
            map[(1, 1)].TileUpdate.Resource.Value.Should().Be(5);

            map[(1, 1)].TileUpdate.Units.First().Id.Should().Be(12);
            map[(1, 1)].TileUpdate.Units.First().Type.Should().Be("tank");

            map[(2, 2)].TileUpdate.Visible.Should().BeTrue();
            map[(2, 2)].TileUpdate.Blocked.Should().BeTrue();
        }

        private void CheckNeighbors(List<Tile> neighbors, List<(int, int)> expectedLocations)
        {
            neighbors.Count.Should().Be(expectedLocations.Count);
            var neighborLocations = neighbors.Select(t => t.Location);
            expectedLocations.ForEach(location =>
            {
                neighborLocations.Contains(location).Should().BeTrue();
            });
        }

        [Fact]
        public void Test_Neighbor_Locations()
        {
            var map = new Map();

            CheckNeighbors(
                map.Neighbors((0, 0)),
                new List<(int, int)> { (-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1) });

            CheckNeighbors(
                map.Neighbors((5, 5)),
                new List<(int, int)> { (4, 4), (5, 4), (6, 4), (4, 5), (6, 5), (4, 6), (5, 6), (6, 6) });
        }

        [Fact]
        public void Test_Neighbor_Tiles()
        {
            var map = new Map();
            map[(0, 0)].TileUpdate = new TileUpdate { Visible = true, Blocked = false, Resources = new ResourceUpdate { Value = 10 } };

            var tile = map.Neighbors((1, 1)).First(t => t.Location == (0, 0));
            tile.TileUpdate.Visible.Should().BeTrue();
            tile.TileUpdate.Blocked.Should().BeFalse();
            tile.TileUpdate.Resource.Value.Should().Be(10);
        }

        [Fact]
        public void Test_EverBeenVisible()
        {
            var map = new Map();
            map[(0, 0)].TileUpdate.Visible.Should().BeFalse();
            map[(0, 0)].Unknown.Should().BeTrue();

            map.UpdateTiles(new List<TileUpdate> { new TileUpdate { X = 0, Y = 0, Visible = true } });

            map[(0, 0)].TileUpdate.Visible.Should().BeTrue();
            map[(0, 0)].Unknown.Should().BeFalse();

            map.UpdateTiles(new List<TileUpdate> { new TileUpdate { X = 0, Y = 0, Visible = false } });
            map[(0, 0)].TileUpdate.Visible.Should().BeFalse();
            map[(0, 0)].Unknown.Should().BeFalse();
        }

        [Fact]
        public void Test_BuildNeighborLocationList()
        {
            var map = new Map();
            var neighborsWithinOne = new List<(int X, int Y)> { (-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1) };
            map.BuildNeighborLocationList(1).Should().BeEquivalentTo(neighborsWithinOne);

            var neighborsWithinTwo = new List<(int X, int Y)> {
                (-2, -2), (-1, -2), (0, -2), (1, -2), (2, -2),
                (-2, -1), (-1, -1), (0, -1), (1, -1), (2, -1),
                (-2, 0), (-1, 0), (1, 0), (2, 0),
                (-2, 1), (-1, 1), (0, 1), (1, 1), (2, 1),
                (-2, 2), (-1, 2), (0, 2), (1, 2), (2, 2)
            };
            map.BuildNeighborLocationList(2).Should().BeEquivalentTo(neighborsWithinTwo);
        }

        [Fact]
        public void Test_EnemyLocationsInRange()
        {
            var data = new char[,] {
                {'-', '_', '-', '-', '-'},
                {'-', '_', '-', '-', '-'},
                {'-', '_', '-', '_', '-'},
                {'-', '_', 'B', '_', '-'},
                {'-', '_', '-', '_', '-'},
                {'-', '_', '_', '_', '-'},
                {'-', '-', '-', '-', '-'},
            };
            var map = MapUtilities.CreateMap(data);
            map[(-2, 0)].TileUpdate.Units = new List<UnitUpdate> {
                new UnitUpdate { Id = 2}
            };

            map.EnemyLocationsInRange(map.HomeBaseLocation).Count.Should().Be(0);
            map.EnemyLocationsInRange(map.HomeBaseLocation, 2).Count.Should().Be(1);
            map.EnemyLocationsInRange(map.HomeBaseLocation, 2).Should().BeEquivalentTo(new List<(int, int)> { (-2, 0) });

            map[(-1, 0)].TileUpdate.Units = new List<UnitUpdate> { new UnitUpdate { Id = 2} };
            map.EnemyLocationsInRange(map.HomeBaseLocation).Count.Should().Be(1);
            map.EnemyLocationsInRange(map.HomeBaseLocation, 2).Count.Should().Be(2);
            map.EnemyLocationsInRange(map.HomeBaseLocation, 2).Should().BeEquivalentTo(new List<(int, int)> { (-2, 0), (-1, 0) });

        }
    }
}
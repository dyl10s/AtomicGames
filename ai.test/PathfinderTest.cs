using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace ai.test
{
    public class PathfinderTest
    {
        [Fact]
        public void Test_Straight_Line()
        {
            Map map = new Map();
            map[(1, 1)].TileUpdate = new TileUpdate { Visible = true, Blocked = true };
            map[(1, 2)].TileUpdate = new TileUpdate { Visible = true, Blocked = false };
            map[(1, 3)].TileUpdate = new TileUpdate { Visible = true, Blocked = false };

            var start = map[(1, 1)].Location;
            var end = map[(1, 3)].Location;


            List<(int, int)> path = new PathFinder(map).FindPath(start, end);
            path.Count.Should().Be(2);
            path[0].Should().Be((1, 2));
            path[1].Should().Be((1, 3));
        }


        [Fact]
        public void Test_Straight_Line_Blocked_Destination()
        {
            Map map = new Map();
            map[(1, 1)].TileUpdate = new TileUpdate { Visible = true, Blocked = true };
            map[(1, 2)].TileUpdate = new TileUpdate { Visible = true, Blocked = false };
            map[(1, 3)].TileUpdate = new TileUpdate { Visible = true, Blocked = true };

            var start = map[(1, 1)].Location;
            var end = map[(1, 3)].Location;


            List<(int, int)> path = new PathFinder(map).FindPath(start, end, 1);
            path.Count.Should().Be(1);
            path[0].Should().Be((1, 2));
        }

        [Fact]
        public void Test_Path_Around_Obstacle()
        {
            Map map = new Map();
            map[(0, 0)].TileUpdate = new TileUpdate { Visible = true, Blocked = false };
            map[(1, 0)].TileUpdate = new TileUpdate { Visible = true, Blocked = true };
            map[(0, 1)].TileUpdate = new TileUpdate { Visible = true, Blocked = false };
            map[(1, 1)].TileUpdate = new TileUpdate { Visible = true, Blocked = true };
            map[(0, 2)].TileUpdate = new TileUpdate { Visible = true, Blocked = false };
            map[(1, 2)].TileUpdate = new TileUpdate { Visible = true, Blocked = false };

            var start = map[(1, 0)].Location;
            var end = map[(1, 2)].Location;


            List<(int, int)> path = new PathFinder(map).FindPath(start, end);
            path.Count.Should().Be(4);
            path[0].Should().Be((0, 0));
            path[1].Should().Be((0, 1));
            path[2].Should().Be((0, 2));
            path[3].Should().Be((1, 2));
        }

    }
}
using Xunit;
using System.Collections.Generic;
using FluentAssertions;

namespace ai.test
{
    public class UnitManagerTest
    {
        [Fact]
        public void Test_UpdateUnits()
        {
            var units1 = new List<UnitUpdate>() {
                new UnitUpdate() { Id = 1, X = 1, Y = 1, Resource = 5, Health = 10, PlayerId = 2, Type = "tank", CanAttack = true, Status = "idle", Range = 2, Speed = 1 },
                new UnitUpdate() { Id = 3, X = 3, Y = 3, Resource = 0, Health = 1, PlayerId = 1, Type = "worker", CanAttack = false, Status = "moving", Range = 1, Speed = 2 }
            };

            var unitManager = new UnitManager(new Map(), new UnitStrategyFactory());
            unitManager.UpdateUnits(units1);

            unitManager.Units.Count.Should().Be(2);

            unitManager.Units[1].UnitUpdate.Id.Should().Be(1);
            unitManager.Units[1].UnitUpdate.X.Should().Be(1);
            unitManager.Units[1].UnitUpdate.Y.Should().Be(1);
            unitManager.Units[1].UnitUpdate.Resource.Should().Be(5);
            unitManager.Units[1].UnitUpdate.Health.Should().Be(10);
            unitManager.Units[1].UnitUpdate.PlayerId.Should().Be(2);
            unitManager.Units[1].UnitUpdate.Type.Should().Be("tank");
            unitManager.Units[1].UnitUpdate.CanAttack.Should().Be(true);
            unitManager.Units[1].UnitUpdate.Status.Should().Be("idle");
            unitManager.Units[1].UnitUpdate.Range.Should().Be(2);
            unitManager.Units[1].UnitUpdate.Speed.Should().Be(1);

            unitManager.Units[3].UnitUpdate.Id.Should().Be(3);
            unitManager.Units[3].UnitUpdate.X.Should().Be(3);
            unitManager.Units[3].UnitUpdate.Y.Should().Be(3);
            unitManager.Units[3].UnitUpdate.Resource.Should().Be(0);
            unitManager.Units[3].UnitUpdate.Health.Should().Be(1);
            unitManager.Units[3].UnitUpdate.PlayerId.Should().Be(1);
            unitManager.Units[3].UnitUpdate.Type.Should().Be("worker");
            unitManager.Units[3].UnitUpdate.CanAttack.Should().Be(false);
            unitManager.Units[3].UnitUpdate.Status.Should().Be("moving");
            unitManager.Units[3].UnitUpdate.Range.Should().Be(1);
            unitManager.Units[3].UnitUpdate.Speed.Should().Be(2);
        }
    }
}
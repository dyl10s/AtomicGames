using Xunit;
using Moq;
using System.Collections.Generic;
using FluentAssertions;
using System.Linq;

namespace ai.test
{
    public class AIStrategyTest
    {
        [Fact]
        public void Test_BuildCommandList()
        {
            var unit1 = new Mock<Unit>();
            var command1 = new AICommand() { Command = AICommand.Move, Unit = 1, Dir = AICommand.North };
            unit1.Setup(u => u.BuildCommand()).Returns(command1);

            var unit2 = new Mock<Unit>();
            var command2 = new AICommand() { Command = AICommand.Move, Unit = 2, Dir = AICommand.South };
            unit2.Setup(u => u.BuildCommand()).Returns(command2);

            var unit3 = new Mock<Unit>();
            unit3.Setup(u => u.BuildCommand()).Returns((AICommand) null);

            var unitManager = new Mock<IUnitManager>();
            var units = new Dictionary<int, Unit>() {
                {1, unit1.Object},
                {2, unit2.Object},
                {3, unit3.Object}
            };
            unitManager.Setup(g => g.Units).Returns(units);

            var map = new Map();
            var commands = new AIStrategy(unitManager.Object, map).BuildCommandList();

            commands.Count.Should().Be(2);
            commands[0].Should().Be(command1);
            commands[1].Should().Be(command2);
        }
    }
}
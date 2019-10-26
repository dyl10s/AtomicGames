using Xunit;
using Moq;
using System.Collections.Generic;

namespace ai.test
{
    public class AILoopTest
    {
        [Fact]
        public void TestRunLoop()
        {
            var connection = new Mock<IServerConnection>();
            var stateManager = new Mock<IGameStateManager>();
            var aiStrategy = new Mock<IAIStrategy>();

            connection.SetupSequence(c => c.ReadUpdate())
                .Returns(new GameUpdate())
                .Returns(new GameUpdate())
                .Returns((GameUpdate) null);

            var commandListOne = new List<AICommand>() { new AICommand() { Command = "MOVE"}};
            var commandListTwo = new List<AICommand>() { new AICommand() { Command = "BUILD"}};

            aiStrategy.SetupSequence(ai => ai.BuildCommandList())
              .Returns(commandListOne)
              .Returns(commandListTwo);

            new AILoop(connection.Object, stateManager.Object, aiStrategy.Object).RunLoop();

            stateManager.Verify(s => s.HandleGameUpdate(It.IsAny<GameUpdate>()), Times.Exactly(2));
            connection.Verify(c => c.SendCommands(commandListOne));
            connection.Verify(c => c.SendCommands(commandListTwo));
        }
    }
}

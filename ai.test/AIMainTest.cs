using Xunit;
using Moq;

namespace ai.test
{
    public class AIMainTest
    {
        [Fact]
        public void TestRun()
        {
            var connection = new Mock<IServerConnection>();
            var loop = new Mock<IAILoop>();

            new AIMain(connection.Object, loop.Object).Run();

            connection.Verify(c => c.AcceptConnection());
            loop.Verify(l => l.RunLoop());
            connection.Verify(c => c.Close());
        }
    }
}

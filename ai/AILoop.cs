using System.Collections.Generic;

namespace ai
{
    public class AILoop : IAILoop
    {
        private readonly IServerConnection ServerConnection;
        private readonly IGameStateManager StateManager;
        private readonly IAIStrategy AIStrategy;

        public static int startupCommand = 0;

        public AILoop(IServerConnection serverConnection, IGameStateManager stateManager, IAIStrategy aiStrategy)
        {
            ServerConnection = serverConnection;
            StateManager = stateManager;
            AIStrategy = aiStrategy;
        }

        public void RunLoop()
        {
            GameUpdate update = null;
            while ((update = ServerConnection.ReadUpdate()) != null)
            {
                StateManager.HandleGameUpdate(update);
                ServerConnection.SendCommands(AIStrategy.BuildCommandList());
            }
        }
    }
}
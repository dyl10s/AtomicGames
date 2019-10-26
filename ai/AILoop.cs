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
                if(startupCommand < 4)
                {
                    startupCommand++;
                    var commands = AIStrategy.BuildCommandList();
                    commands.Add(StartupFunctions());
                    
                    ServerConnection.SendCommands(commands);
                }
                else
                {
                    ServerConnection.SendCommands(AIStrategy.BuildCommandList());
                }
            }
        }

        public AICommand StartupFunctions()
        {
            if(startupCommand == 0)
            {
                return new AICommand()
                {
                    Command = AICommand.Create,
                    Type = "scout"
                };
            }

            if (startupCommand == 1)
            {
                return new AICommand()
                {
                    Command = AICommand.Create,
                    Type = "scout"
                };
            }

            if (startupCommand == 2)
            {
                return new AICommand()
                {
                    Command = AICommand.Create,
                    Type = "tank"
                };
            }

            if (startupCommand == 3)
            {
                return new AICommand()
                {
                    Command = AICommand.Create,
                    Type = "tank"
                };
            }

            return new AICommand();
        }
    }
}
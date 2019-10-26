using System.Collections.Generic;

namespace ai
{
    public class AILoop : IAILoop
    {
        private readonly IServerConnection ServerConnection;
        private readonly IGameStateManager StateManager;
        private readonly IAIStrategy AIStrategy;

        public static bool hasStarted = false;

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
                if(hasStarted == false)
                {
                    hasStarted = true;
                    var commands = AIStrategy.BuildCommandList();
                    foreach (var x in StartupFunctions())
                    {
                        commands.Add(x);
                    }
                    ServerConnection.SendCommands(commands);
                }
                else
                {
                    ServerConnection.SendCommands(AIStrategy.BuildCommandList());
                }
            }
        }

        public List<AICommand> StartupFunctions()
        {
            var commands = new List<AICommand>();
            commands.Add(new AICommand()
            {
                Command = AICommand.Create,
                Type = "scout"
            });

            commands.Add(new AICommand()
            {
                Command = AICommand.Create,
                Type = "scout"
            });

            commands.Add(new AICommand()
            {
                Command = AICommand.Create,
                Type = "tank"
            });

            return commands;
        }
    }
}
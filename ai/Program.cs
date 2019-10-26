using System;
using System.Collections.Generic;

namespace ai
{
    public class PortSelector
    {
        public int Select(string[] args)
        {
            var port = 8098;
            if (args.Length > 0)
            {
                int n;
                if (int.TryParse(args[0], out n))
                {
                    port = n;
                }
            }
            return port;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            
            int port = new PortSelector().Select(args);
            var serverConnection = new ServerConnection(port, new MessageSerializer());
            var map = new Map();
            Globals.um = new UnitManager(map, new UnitStrategyFactory());
            var unitManager = Globals.um;
            var stateManager = new GameStateManager(unitManager, map);
            var gameStrategy = new AIStrategy(unitManager, map);
            var loop = new AILoop(serverConnection, stateManager, gameStrategy);
            new AIMain(serverConnection, loop).Run();
        }
    }
}
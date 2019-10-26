namespace ai
{
    public class AIMain
    {
        private readonly IServerConnection ServerConnection;
        private readonly IAILoop Loop;

        public AIMain(IServerConnection serverConnection, IAILoop aiLoop)
        {
            ServerConnection = serverConnection;
            Loop = aiLoop;
        }
        
        public void Run()
        {
            ServerConnection.AcceptConnection();
            try
            {
                Loop.RunLoop();
            }
            finally
            {
                ServerConnection.Close();
            }
        }
    }
}
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ai
{
    public class MessageSerializer
    {

        private readonly DefaultContractResolver contractResolver;

        private readonly JsonSerializerSettings settings;

        public MessageSerializer()
        {
            contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            settings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver
            };
        }

        public GameUpdate ParseUpdate(string updateJSON)
        {
            return JsonConvert.DeserializeObject<GameUpdate>(updateJSON, settings);
        }

        public string SerializeAICommandsMessage(AICommandsMessage message)
        {
            return JsonConvert.SerializeObject(message, settings) + "\n";
        }
    }
}
using Newtonsoft.Json;

namespace facebook_messages_analyser.Models {
    public class Participant {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
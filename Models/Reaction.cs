using Newtonsoft.Json;

namespace facebook_messages_analyser.Models {

    public class Reactions {
        [JsonProperty("reaction")]
        public string Reaction { get; set; }

        [JsonProperty("actor")]
        public string Person { get; set; }
    }
}
using Newtonsoft.Json;

namespace facebook_messages_analyser.Models {

    public class Share {
        [JsonProperty("link")]
        public string Link { get; set; }

    }
}
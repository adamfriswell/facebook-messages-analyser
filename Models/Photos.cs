using Newtonsoft.Json;

namespace facebook_messages_analyser.Models {

    public class Photos {
        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("creation_timestamp")]
        public long Timestamp { get; set; }
    }
}
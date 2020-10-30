using Newtonsoft.Json;
using System.Collections.Generic;

namespace facebook_messages_analyser.Models {
    public class Message {
        [JsonProperty("sender_name")]
        public string SenderName { get; set; }

        [JsonProperty("timestamp_ms")]
        public long Timestamp { get; set; }

        [JsonProperty("photos")]
        public List<Photos> Photos { get; set; }

        [JsonProperty("call_duration")]
        public long CallDuration { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("reactions")]
        public List<Reactions> Reactions { get; set; }

        [JsonProperty("share")]
        public Share Shares { get; set; }

        [JsonProperty("type")]
        public string Type {get; set;}
    }
}
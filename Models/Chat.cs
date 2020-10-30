using System.Collections.Generic;
using Newtonsoft.Json;

namespace facebook_messages_analyser.Models {
    public class Chat {
        [JsonProperty("participants")]
        public List<Participant> Participants {get; set;}

        [JsonProperty("messages")]
        public List<Message> Messages { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("is_still_participant")]
        public bool IsStillParticipant { get; set; }

        [JsonProperty("thread_type")]
        public string ThreadType { get; set; }

        [JsonProperty("thread_path")]
        public string ThreadPath { get; set; }

    }
}
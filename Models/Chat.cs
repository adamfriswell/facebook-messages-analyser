using System.Collections.Generic;

namespace facebook_messages_analyser.Models {

    public class Chat {
        public List<Participant> Participants {get; set;}
        public List<Message> Messages { get; set; }
        public string Title { get; set; }
        public bool Is_still_participant { get; set; }
        public string Thread_type { get; set; }
        public string Thread_path { get; set; }

    }
}
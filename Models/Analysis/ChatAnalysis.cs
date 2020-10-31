using System.Collections.Generic;

namespace facebook_messages_analyser.Models {

    public class ChatAnalysis {
        public string Title { get; set; }
        public long TotalMessages { get; set; }
        public int ParticipantCount { get; set; }
        public List<Person> People { get; set; }
    }
}
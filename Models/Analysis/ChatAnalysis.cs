using System;
using System.Collections.Generic;

namespace facebook_messages_analyser.Models {

    public class ChatAnalysis {
        public string Title { get; set; }
        public List<AnalysedMessage> AllMessages {get; set;}
        public long TotalMessages { get; set; }
        public long UnaccountedMessages { get; set; }
        public AnalysedMessage FirstMessageSent { get; set; }
        public AnalysedMessage LastMessageSent { get; set; }
        public List<Person> People { get; set; }
    }
}
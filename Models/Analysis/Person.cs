using System;

namespace facebook_messages_analyser.Models {
    public class Person {
        public string Name { get; set; }
        public long MessagesSent { get; set; }
        public bool IsActive { get; set; }
        public DateTime FirstMessageSent { get; set; }
        public DateTime LastMessageSent {get; set;}
    }
}
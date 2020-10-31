using System; 

namespace facebook_messages_analyser.Models {

    public class AnalysedMessage {
        public string Sender { get; set; }
        public DateTime Timestamp { get; set; }
        public string Content { get; set; }
    }
}
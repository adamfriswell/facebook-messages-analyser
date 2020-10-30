namespace facebook_messages_analyser.Models {

    public class Message {
        public string SenderName { get; set; }
        public int Timestamp_ms { get; set; }
        public Photos Photos { get; set; }
        public int Call_Duration { get; set; }
        public string Content { get; set; }
        public Reactions[] Reactions { get; set; }
        public string Type {get; set;}
    }
}
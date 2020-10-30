using System;
using System.Linq;
using Newtonsoft.Json;
using facebook_messages_analyser.Models;
using facebook_messages_analyser.Services;

namespace facebook_messages_analyser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var chat = GetChat($"message_1.json");

            string title = StringSanitiser.RemoveUnescapedUnicode(chat.Title);
            title = title.Trim();

            var participants = chat.Participants;
            var participantsNames = string.Join(", ",participants);
            var participantCount = participants.Count;
           
            var totalMessages = GetTotalNumberOfMessages();

            Console.WriteLine($"Chat \"{title}\" has {participantCount} members with a total of {totalMessages} messages sent");

        }

        public static long GetTotalNumberOfMessages()
        {
            long totalMessages = 0;
            int fileNumber = 1;

            while (fileNumber <= 10)
            {
                var chat = GetChat($"message_{fileNumber}.json");
                totalMessages += chat.Messages.Count;
                fileNumber++;
            }

            return totalMessages;
        }

        public static Chat GetChat(string fileName){
            var file = FileService.OpenFile(fileName);
            Chat chat = JsonConvert.DeserializeObject<Chat>(file);

            return chat;
        }
    }
}

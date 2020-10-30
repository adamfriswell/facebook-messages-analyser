using System;
using facebook_messages_analyser.Models;
using System.IO;
using Newtonsoft.Json;
using facebook_messages_analyser.Services;

namespace facebook_messages_analyser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            long totalMessages = 0;
            int fileNumber = 1;

            while(fileNumber<=10){
                var chat = GetChat($"message_{fileNumber}.json");
                totalMessages += chat.Messages.Count;
                fileNumber ++;
            }

            Console.WriteLine(totalMessages);
        }

        public static Chat GetChat(string fileName){
            var file = FileService.OpenFile(fileName);
            Chat chat = JsonConvert.DeserializeObject<Chat>(file);

            return chat;
        }
    }
}

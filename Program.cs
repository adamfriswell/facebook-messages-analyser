using System;
using facebook_messages_analyser.Models;
using System.IO;
using Newtonsoft.Json;

namespace facebook_messages_analyser
{
    public class Program
    {
        static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string path = System.IO.Path.Combine(currentDirectory, "Data", "message_1.json");
            var file = File.ReadAllText(path);

            Chat Chat = JsonConvert.DeserializeObject<Chat>(file);

            Console.WriteLine(Chat.Title);
        
        }
    }
}

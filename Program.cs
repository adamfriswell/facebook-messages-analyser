using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using facebook_messages_analyser.Models;
using facebook_messages_analyser.Services;

namespace facebook_messages_analyser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<ChatFolder> chats = FileService.GetAllChats();
            List<string> chatNames = new List<string>();
            foreach(var c in chats){
                chatNames.Add(c.Name);
            }
            string chatList = string.Join(", ", chatNames);

            ChatFolder selectedChat = new ChatFolder();
            bool nonEmptyChat = true;
            while(nonEmptyChat){
                string ans ="";
                bool chatSelected = false;
                while(!chatSelected){
                    Console.WriteLine($"Please select a chat from: {chatList}");

                    ans = Console.ReadLine();
                
                    if(ans == "Chat2" || ans == "Chat1"){
                        chatSelected = true;
                    }
                }

                selectedChat = chats.Where(c => c.Name == ans).SingleOrDefault();

                if(selectedChat.NumberOfFiles < 1){
                    Console.WriteLine($"{selectedChat.Name} is empty.");
                }
                else{
                    nonEmptyChat = false;
                }
            }

            var chat = GetChat(selectedChat.Name,"message_1.json");

            string title = StringSanitiser.RemoveUnescapedUnicode(chat.Title);
            title = title.Trim();

            var participants = chat.Participants;
            var participantsNames = string.Join(", ",participants);
            var participantCount = participants.Count;
           
            var totalMessages = GetTotalNumberOfMessages(selectedChat.Name,selectedChat.NumberOfFiles);

            Console.WriteLine($"Chat \"{title}\" has {participantCount} members with a total of {totalMessages} messages sent!");
          
            
        }

        public static long GetTotalNumberOfMessages(string chatName, int numberOfFiles)
        {
            long totalMessages = 0;
            int fileNumber = 1;

            while (fileNumber <= numberOfFiles)
            {
                var chat = GetChat(chatName,$"message_{fileNumber}.json");
                totalMessages += chat.Messages.Count;
                fileNumber++;
            }

            return totalMessages;
        }

        public static Chat GetChat(string folderName, string fileName){
            var file = FileService.OpenFile(folderName, fileName);
            Chat chat = JsonConvert.DeserializeObject<Chat>(file);

            return chat;
        }
    }
}

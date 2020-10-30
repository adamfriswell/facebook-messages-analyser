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
            List<ChatInfo> chats = FileService.GetAllChatsInfo();
            List<string> chatNames = new List<string>();
            foreach(var c in chats){
                chatNames.Add(c.Name);
            }
            string chatList = string.Join(", ", chatNames);

            ChatInfo selectedChat = new ChatInfo();
            bool nonEmptyChat = true;
            while(nonEmptyChat){
                string ans ="";
                bool chatSelected = false;
                while(!chatSelected){
                    Console.WriteLine($"Please select a chat from: {chatList} (or Enter to exit)");

                    ans = Console.ReadLine();
                
                    if(chatList.Contains(ans) || string.IsNullOrEmpty(ans)){
                        chatSelected = true;
                    }
                }

                if(!string.IsNullOrEmpty(ans)){
                    selectedChat = chats.Where(c => c.Name == ans).SingleOrDefault();

                    if(selectedChat == null){
                        nonEmptyChat = false; 
                    }
                    if(selectedChat.NumberOfFiles < 1){
                        Console.WriteLine($"{selectedChat.Name} is empty.");
                    }
                
                    ChatAnalysis analysis = Analyse.AnalyseChat(selectedChat.Name,selectedChat.NumberOfFiles);

                    Console.WriteLine($"Chat \"{analysis.Title}\" has {analysis.ParticipantCount} members with a total of {analysis.TotalMessages} messages sent!");
                }
                
                nonEmptyChat = false;
            }
        }
    }
}

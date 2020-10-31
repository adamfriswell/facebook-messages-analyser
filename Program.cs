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
            List<ChatInfo> availableChats = FileService.GetAllChatsInfo();
            string chatList = CommaSeperatedAvailableChats(availableChats);

            ChatInfo selectedChat = new ChatInfo();
            bool chatNotEmptyOrNull = true;
            while (chatNotEmptyOrNull)
            {
                string answer = AskUserToSelectChat(chatList);

                if (!string.IsNullOrEmpty(answer))
                {
                    selectedChat = availableChats.Where(c => c.Name == answer).SingleOrDefault();
                    
                    if (selectedChat.NumberOfFiles < 1)
                    {
                        Console.WriteLine($"{selectedChat.Name} is empty.");
                    }
                    else{
                        ChatAnalysis analysis = Analyse.AnalyseChat(selectedChat.Name, selectedChat.NumberOfFiles);
                        Console.WriteLine($"Chat \"{analysis.Title}\" has {analysis.ParticipantCount} members with a total of {analysis.TotalMessages} messages sent!");
                    }

                    Console.WriteLine($"---------------------------------------------------------------------------------");
                }
                else{
                    chatNotEmptyOrNull = false;
                }
            }
        }

        private static string AskUserToSelectChat(string chatList)
        {
            string answer = "";
            bool chatSelected = false;
            while (!chatSelected)
            {
                Console.WriteLine($"Please select a chat from: {chatList} (or Enter to exit)");
                answer = Console.ReadLine();

                if (chatList.Contains(answer) || string.IsNullOrEmpty(answer))
                {
                    chatSelected = true;
                }
                else{
                    Console.WriteLine("Invalid option, please try again.");
                    Console.WriteLine($"---------------------------------------------------------------------------------");
                }
            }

            return answer;
        }

        private static string CommaSeperatedAvailableChats(List<ChatInfo> chats)
        {
            List<string> chatNames = new List<string>();
            foreach (var c in chats)
            {
                chatNames.Add(c.Name);
            }
            string chatList = string.Join(", ", chatNames);
            return chatList;
        }
    }
}

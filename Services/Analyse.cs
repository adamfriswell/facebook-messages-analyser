using System;
using System.Linq;
using System.Collections.Generic;
using facebook_messages_analyser.Models;

namespace facebook_messages_analyser.Services{
    public static class Analyse{
        public static ChatAnalysis AnalyseChat(string chatName, int numberOfFiles)
        {
            //open first file to get list of active participants and title of chat
            var firstFile = FileService.GetChat(chatName, "message_1.json");
            List<string> participantsList = new List<string>();
            foreach (var p in firstFile.Participants)
            {
                participantsList.Add(p.Name);
            }
            string title = StringSanitiser.RemoveUnescapedUnicode(firstFile.Title).Trim();

            //go through all messages in all files
            List<Person> people = new List<Person>();
            List<AnalysedMessage> messages = new List<AnalysedMessage>();

            int fileNumber = 1;
            while (fileNumber <= numberOfFiles)
            {
                var chat = FileService.GetChat(chatName, $"message_{fileNumber}.json");
                foreach (var msg in chat.Messages)
                {
                    var person = people.Where(p => p.Name == msg.SenderName).SingleOrDefault();
                    if(person == null){
                        people.Add(new Person{
                            Name = msg.SenderName,
                            IsActive = participantsList.Contains(msg.SenderName)
                        });
                    }

                    messages.Add(new AnalysedMessage
                    {
                        Sender = msg.SenderName,
                        Timestamp = TimeConverter.MillisecondsToDateTime(msg.Timestamp),
                        Content = msg.Content
                    });
                }
                fileNumber++;
            }

            //message analysis per participant
            foreach(var p in people){
                List<AnalysedMessage> personMessages = messages.Where(m => m.Sender == p.Name).ToList();
                personMessages = personMessages.OrderBy(m => m.Timestamp).ToList();
                p.MessagesSent = personMessages.Count();
                p.FirstMessageSent = personMessages.First().Timestamp;
                p.LastMessageSent = personMessages.Last().Timestamp;
            }

            //any unaccounted messages
            long participantMessages = 0;
            foreach(var p in people){
                participantMessages += p.MessagesSent;
            }
            long unaccountedMessages = messages.Count() - participantMessages;

            //create analysis 
            messages = messages.OrderBy(m => m.Timestamp).ToList();
            var analysis = new ChatAnalysis()
            {
                Title = title,
                AllMessages = messages,
                TotalMessages = messages.Count(),
                UnaccountedMessages = unaccountedMessages,
                FirstMessageSent = messages.First(),
                LastMessageSent = messages.Last(),
                People = people.OrderByDescending(p => p.MessagesSent).ToList(),
            };

            return analysis;
        }

        public static void GetAnalysisResult(string chatName, int numberOfFiles)
        {
            ChatAnalysis analysis = AnalyseChat(chatName, numberOfFiles);

            var activePeople = analysis.People.Where(p => p.IsActive).ToList();
            var inactivePeople = analysis.People.Where(p => !p.IsActive).ToList();

            Console.Clear();
            Console.WriteLine($"Chat \"{analysis.Title}\" has {activePeople.Count()} active members with a total of {analysis.TotalMessages} messages sent and {inactivePeople.Count()} members left!");

            PrintParticipantsTable(analysis.People);

            Console.WriteLine("Messages:");
            Console.WriteLine($"\t * First message sent by {analysis.FirstMessageSent.Sender} at {analysis.FirstMessageSent.Timestamp}");
            Console.WriteLine($"\t * Last message sent by {analysis.LastMessageSent.Sender} at {analysis.LastMessageSent.Timestamp}");
            Console.WriteLine($"\t * {analysis.UnaccountedMessages} unaccounted messages");
        }

        private static void PrintParticipantsTable(List<Person> people)
        {
            string format = "|{0,20}|{1,10}|{2,15}|{3,22}|{4,22}|";
            int breakSpace = 95;
            Console.WriteLine(new String('-',breakSpace));
            Console.WriteLine(String.Format(format, "Person", "Status", "Messages Sent", "First Message", "Last Message"));
            Console.WriteLine(new String('-',breakSpace));
            foreach (var p in people)
            {
                Console.WriteLine(String.Format(format, p.Name, p.IsActive ? "Active" : "Inactive", p.MessagesSent, p.FirstMessageSent, p.LastMessageSent));
            }
            Console.WriteLine(new String('-',breakSpace));
        }
    }
}
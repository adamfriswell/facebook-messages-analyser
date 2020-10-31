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
                            MessagesSent = 1,
                            IsActive = participantsList.Contains(msg.SenderName)
                        });
                    }
                    else{
                        person.MessagesSent ++;
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

        public static void GetAnalysisResult(string chatName, int numberOfFiles){
            ChatAnalysis analysis = AnalyseChat(chatName, numberOfFiles);

            var activePeople = analysis.People.Where(p => p.IsActive);
            var inactivePeople = analysis.People.Where(p => !p.IsActive);

            Console.WriteLine($"Chat \"{analysis.Title}\" has {activePeople.Count()} active members with a total of {analysis.TotalMessages} messages sent and {inactivePeople.Count()} members left!");

            Console.WriteLine("Participants:");
            Console.WriteLine("\t Active:");
            foreach(var p in activePeople){
                Console.WriteLine($"\t\t * '{p.Name}' sent {p.MessagesSent} messages in the chat");  
            }
            Console.WriteLine("\t Inactive:");
            foreach(var p in analysis.People.Where(p => !p.IsActive)){
                Console.WriteLine($"\t\t * '{p.Name}' sent {p.MessagesSent} messages in the chat");  
            }

            Console.WriteLine("Messages:");
            Console.WriteLine($"\t * First message sent by {analysis.FirstMessageSent.Sender} at {analysis.FirstMessageSent.Timestamp}");
            Console.WriteLine($"\t * Last message sent by {analysis.LastMessageSent.Sender} at {analysis.LastMessageSent.Timestamp}");
            Console.WriteLine($"\t * {analysis.UnaccountedMessages} unaccounted messages");
        }
    }
}
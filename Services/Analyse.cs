using System;
using System.Linq;
using System.Collections.Generic;
using facebook_messages_analyser.Models;

namespace facebook_messages_analyser.Services{
    public static class Analyse{
        public static ChatAnalysis AnalyseChat(string chatName, int numberOfFiles)
        {
            long totalMessages = 0;
            List<Person> peopleData = new List<Person>();
            List<AnalysedMessage> analysedMessages = new List<AnalysedMessage>();

            var firstFile = FileService.GetChat(chatName,"message_1.json");

            string title = StringSanitiser.RemoveUnescapedUnicode(firstFile.Title);
            title = title.Trim();

            totalMessages += firstFile.Messages.Count;
            foreach(var msg in firstFile.Messages){
                analysedMessages.Add(new AnalysedMessage{
                    Sender = msg.SenderName,
                    Timestamp = TimeConverter.MillisecondsToDateTime(msg.Timestamp),
                    Content = msg.Content
                });
            }

            List<Participant> participants = firstFile.Participants;
            foreach(var p in participants){
                peopleData.Add(new Person{
                    Name = p.Name,
                    MessagesSent = firstFile.Messages.Where(m => m.SenderName == p.Name).Count()
                });
            }

            var participantsNames = string.Join(", ",participants);
            var participantCount = participants.Count;

            var analysis = new ChatAnalysis(){
                Title = title,
                ParticipantCount = participantCount
            };

            if(numberOfFiles>1){
                int fileNumber = 1;
                while (fileNumber <= numberOfFiles)
                {
                    var chat = FileService.GetChat(chatName,$"message_{fileNumber}.json");
                    totalMessages += chat.Messages.Count;
                    foreach(var p in peopleData){
                        p.MessagesSent += chat.Messages.Where(m => m.SenderName == p.Name).Count();
                    }
                    foreach(var msg in chat.Messages){
                        analysedMessages.Add(new AnalysedMessage{
                            Sender = msg.SenderName,
                            Timestamp = TimeConverter.MillisecondsToDateTime(msg.Timestamp),
                            Content = msg.Content
                        });
                    }
                    fileNumber++;
                }   
            }

            analysedMessages = analysedMessages.OrderBy(m => m.Timestamp).ToList();

            analysis.TotalMessages = totalMessages;
            analysis.AllMessages = analysedMessages;
            analysis.FirstMessageSent = analysedMessages.First();
            analysis.LastMessageSent = analysedMessages.Last();
            analysis.People = peopleData.OrderByDescending(p => p.MessagesSent).ToList();

            long messagesByParticipants = 0;
            foreach(var p in peopleData){
                messagesByParticipants += p.MessagesSent;
            }
            analysis.UnaccountedMessages = totalMessages - messagesByParticipants;

            return analysis;
        }

        public static void GetAnalysisResult(string chatName, int numberOfFiles){
            ChatAnalysis analysis = AnalyseChat(chatName, numberOfFiles);
            Console.WriteLine($"Chat \"{analysis.Title}\" has {analysis.ParticipantCount} members with a total of {analysis.TotalMessages} messages sent!");

            Console.WriteLine("Participants:");
            foreach(var p in analysis.People){
                Console.WriteLine($"\t * '{p.Name}' sent {p.MessagesSent} messages in the chat");
            }
            Console.WriteLine($"\t * {analysis.UnaccountedMessages} unaccounted messages in the chat");

            Console.WriteLine("Messages:");
            Console.WriteLine($"\t * First message sent by {analysis.FirstMessageSent.Sender} at {analysis.FirstMessageSent.Timestamp}");
            Console.WriteLine($"\t * Last message sent by {analysis.LastMessageSent.Sender} at {analysis.LastMessageSent.Timestamp}");
        }
    }
}
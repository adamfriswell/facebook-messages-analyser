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


            var firstFile = FileService.GetChat(chatName,"message_1.json");

            string title = StringSanitiser.RemoveUnescapedUnicode(firstFile.Title);
            title = title.Trim();

            totalMessages += firstFile.Messages.Count;

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
                    fileNumber++;
                }   
            }

            analysis.TotalMessages = totalMessages;
            analysis.People = peopleData.OrderByDescending(p => p.MessagesSent).ToList();

            return analysis;
        }

        public static void GetAnalysisResult(string chatName, int numberOfFiles){
            ChatAnalysis analysis = AnalyseChat(chatName, numberOfFiles);
            Console.WriteLine($"Chat \"{analysis.Title}\" has {analysis.ParticipantCount} members with a total of {analysis.TotalMessages} messages sent!");
            foreach(var p in analysis.People){
                Console.WriteLine($"* '{p.Name}' sent {p.MessagesSent} messages in the chat");
            }
        }
    }
}
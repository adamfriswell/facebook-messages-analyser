using facebook_messages_analyser.Models;

namespace facebook_messages_analyser.Services{
    public static class Analyse{
           public static ChatAnalysis AnalyseChat(string chatName, int numberOfFiles)
        {
            long totalMessages = 0;

            var firstFile = FileService.GetChat(chatName,"message_1.json");

            string title = StringSanitiser.RemoveUnescapedUnicode(firstFile.Title);
            title = title.Trim();

            var participants = firstFile.Participants;
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
                    fileNumber++;
                }   
            }

            analysis.TotalMessages = totalMessages;

            return analysis;
        }
    }
}
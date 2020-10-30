using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using facebook_messages_analyser.Models;

namespace facebook_messages_analyser.Services{
    public static class FileService{

        public static List<ChatInfo> GetAllChatsInfo(){
            string currentDirectory = Directory.GetCurrentDirectory();
            string path = System.IO.Path.Combine(currentDirectory, "Data");
            string[] chatPaths = Directory.GetDirectories(path);
            
            List<ChatInfo> chats = new List<ChatInfo>();
            foreach(var p in chatPaths){
                string folderName = new DirectoryInfo(p).Name;
                int numberOfFiles = Directory.GetFiles(p,"*",SearchOption.TopDirectoryOnly).Length;

                chats.Add(new ChatInfo{
                    Name = folderName,
                    NumberOfFiles = numberOfFiles
                });
            }
            return chats;
        }
        public static Chat GetChat(string folderName, string fileName){
            var file = FileService.OpenFile(folderName, fileName);
            Chat chat = JsonConvert.DeserializeObject<Chat>(file);

            return chat;
        }
        public static string OpenFile(string folderName, string fileName){
            string currentDirectory = Directory.GetCurrentDirectory();
            string path = System.IO.Path.Combine(currentDirectory, "Data", folderName, fileName);
            var file = File.ReadAllText(path);

            return file;
        }
    }
}
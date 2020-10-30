using System.IO;
using System.Collections.Generic;
using facebook_messages_analyser.Models;

namespace facebook_messages_analyser.Services{
    public static class FileService{

        public static List<ChatFolder> GetAllChats(){
            string currentDirectory = Directory.GetCurrentDirectory();
            string path = System.IO.Path.Combine(currentDirectory, "Data");
            string[] chatPaths = Directory.GetDirectories(path);
            
            List<ChatFolder> chats = new List<ChatFolder>();
            foreach(var p in chatPaths){
                string folderName = new DirectoryInfo(p).Name;
                int numberOfFiles = Directory.GetFiles(p,"*",SearchOption.TopDirectoryOnly).Length;

                chats.Add(new ChatFolder{
                    Name = folderName,
                    NumberOfFiles = numberOfFiles
                });
            }
            return chats;
        }
        public static string OpenFile(string folderName, string fileName){
            string currentDirectory = Directory.GetCurrentDirectory();
            string path = System.IO.Path.Combine(currentDirectory, "Data", folderName, fileName);
            var file = File.ReadAllText(path);

            return file;
        }
    }
}
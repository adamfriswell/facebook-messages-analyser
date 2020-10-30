using System.IO;

namespace facebook_messages_analyser.Services{
    public static class FileService{
        public static string OpenFile(string fileName){
            string currentDirectory = Directory.GetCurrentDirectory();
            string path = System.IO.Path.Combine(currentDirectory, "Data", fileName);
            var file = File.ReadAllText(path);

            return file;
        }
    }
}
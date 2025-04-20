using System.Text.Json;
using GetNews.Core.DomainModel;

namespace GetNews.API.Infrastructure
{
    public class DummyEmailService
    {
        private const string EmailsFolderName = "emails";

        public static async Task Send(Email email, string basePath)
        {
            var fileName = CreateDirAndGetFileName(email.ToEmailAddress, basePath);
            var json = JsonSerializer.Serialize(email);
            await File.WriteAllTextAsync(fileName, json);
        }

        private static string CreateDirAndGetFileName(string emailAddress, string basePath)
        {
            var dir = basePath + "\\" + EmailsFolderName;
            Directory.CreateDirectory(dir);
            var fileName = dir + "\\" + emailAddress + ".json";
            return fileName;
        }
    }
}

using System.Text.Json;
using GetNews.Core.DomainModel;

namespace GetNews.API.Infrastructure
{
    public class DummyEmailService
    {
        private const string EmailsFolderName = "emails";

        public static async Task Send(Email email)
        {
            var fileName = CreateDirAndGetFileName(email.ToEmailAddress);
            var json = JsonSerializer.Serialize(email);
            await File.WriteAllTextAsync(fileName, json);
        }

        private static string CreateDirAndGetFileName(string emailAddress)
        {
            Directory.CreateDirectory(EmailsFolderName);
            var fileName = EmailsFolderName + "\\" + emailAddress + ".json";
            return fileName;
        }
    }
}

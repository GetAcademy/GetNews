using System.Text.Json;
using GetNews.Core.DomainModel;

namespace GetNews.API.Infrastructure
{
    public class SubscriptionFileRepository
    {
        private const string SubscriptionsFolderName = "subscriptions";

        public static async Task<Subscription?> LoadSubscription(string emailAddress, string basePath)
        {
            var fileName = CreateDirAndGetFileName(emailAddress, basePath);
            if (!File.Exists(fileName)) return null;
            var json = await File.ReadAllTextAsync(fileName);
            return JsonSerializer.Deserialize<Subscription>(json);
        }

        public static async Task SaveSubscription(Subscription subscription, string basePath)
        {
            var fileName = CreateDirAndGetFileName(subscription.EmailAddress, basePath);
            var json = JsonSerializer.Serialize(subscription);
            await File.WriteAllTextAsync(fileName, json);
        }

        private static string CreateDirAndGetFileName(string emailAddress, string basePath)
        {
            var dir = basePath + "\\" + SubscriptionsFolderName;
            Directory.CreateDirectory(dir);
            var fileName = dir + "\\" + emailAddress + ".json";
            return fileName;
        }
    }
}

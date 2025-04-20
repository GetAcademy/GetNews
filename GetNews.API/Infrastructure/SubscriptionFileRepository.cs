using System.Text.Json;
using GetNews.Core.DomainModel;

namespace GetNews.API.Infrastructure
{
    public class SubscriptionFileRepository
    {
        private const string SubscriptionsFolderName = "subscriptions";

        public static async Task<Subscription?> GetSubscriptionByEmail(string emailAddress)
        {
            var fileName = CreateDirAndGetFileName(emailAddress);
            if (!File.Exists(fileName)) return null;
            var json = await File.ReadAllTextAsync(fileName);
            return JsonSerializer.Deserialize<Subscription>(json);
        }

        public static async Task SaveSubscription(Subscription subscription)
        {
            var fileName = CreateDirAndGetFileName(subscription.EmailAddress);
            var json = JsonSerializer.Serialize(subscription);
            await File.WriteAllTextAsync(fileName, json);
        }

        private static string CreateDirAndGetFileName(string emailAddress)
        {
            Directory.CreateDirectory(SubscriptionsFolderName);
            var fileName = SubscriptionsFolderName + "\\" + emailAddress + ".json";
            return fileName;
        }
    }
}

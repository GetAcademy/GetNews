using System.Text.Json;
using SubscriptionApi = GetNews.API.ApiModel.Subscription;
using SubscriptionDomain = GetNews.Core.DomainModel.Subscription;

namespace GetNews.API.Infrastructure
{
    public class SubscriptionFileRepository
    {
        private const string SubscriptionsFolderName = "subscriptions";

        public static async Task<SubscriptionDomain> LoadSubscription(string emailAddress, string basePath)
        {
            var fileName = CreateDirAndGetFileName(emailAddress, basePath);
            if (!File.Exists(fileName)) return null;
            var json = await File.ReadAllTextAsync(fileName);
            return JsonSerializer.Deserialize<SubscriptionApi>(json).ToDomainModel();
        }

        public static async Task SaveSubscription(SubscriptionDomain subscription, string basePath)
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

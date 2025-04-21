using System.Text.Json;
using ApiSubscription = GetNews.API.PersistentModel.Subscription;
using DomainSubscription = GetNews.Core.DomainModel.Subscription;

namespace GetNews.API.Infrastructure
{
    public class SubscriptionFileRepository
    {
        private const string SubscriptionsFolderName = "subscriptions";

        public static async Task<DomainSubscription> LoadSubscription(string emailAddress, string basePath)
        {
            var fileName = CreateDirAndGetFileName(emailAddress, basePath);
            if (!File.Exists(fileName)) return null;
            var json = await File.ReadAllTextAsync(fileName);
            var apiSubscription= JsonSerializer.Deserialize<ApiSubscription>(json);
            return Mapper.ToDomainModel(apiSubscription);
        }

        public static async Task SaveSubscription(DomainSubscription subscription, string basePath)
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

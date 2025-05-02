//  loading & Saving subscriptions to/from a file

//  Importing necessary namespaces
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
            /*
             * This methods loads a subscription from a file.
             */
            var fileName = CreateDirAndGetFileName(emailAddress, basePath);

            // Ensure the directory exists
            if (!File.Exists(fileName)) return null;

            var json = await File.ReadAllTextAsync(fileName);
            var apiSubscription= JsonSerializer.Deserialize<ApiSubscription>(json);

            return Mapper.ToDomainModel(apiSubscription);
        }

        public static async Task SaveSubscription(DomainSubscription subscription, string basePath)
        {
            /*
             * This methods saves a subscription to a file.
             */
            
            var apiSubscription = Mapper.ToApiModel(subscription);
            var json = JsonSerializer.Serialize(apiSubscription);
            var fileName = CreateDirAndGetFileName(subscription.EmailAddress, basePath);

            await File.WriteAllTextAsync(fileName, json);
        }

        private static string CreateDirAndGetFileName(string emailAddress, string basePath)
        {
            /*
             * This methods creates a directory for the subscription files if it doesn't exist
             * and returns the file name for the subscription.
             */
            var dir = basePath + "/" + SubscriptionsFolderName;
            
            Directory.CreateDirectory(dir);
            
            var fileName = dir + "/" + emailAddress + ".json";
            
            return fileName;
        }
    }
}

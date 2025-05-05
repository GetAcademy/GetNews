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
            if (!File.Exists(fileName))
            {
                Console.WriteLine("[DEBUG] Fil finnes ikke – ny bruker?");
                return null;
            }

            var json = await File.ReadAllTextAsync(fileName);
            Console.WriteLine($"[DEBUG] Lest JSON:\n{json}");
            var apiSubscription= JsonSerializer.Deserialize<ApiSubscription>(json);
            if (apiSubscription == null)
            {
                Console.WriteLine("[DEBUG] Deserialisering feilet – ugyldig JSON");
                return null;
            }

            var domain = Mapper.ToDomainModel(apiSubscription);
            if (domain == null)
            {
                Console.WriteLine("[DEBUG] Mapper returnerte null");
            }

            return domain;
        }

        public static async Task SaveSubscription(DomainSubscription subscription, string basePath)
        {
            /*
             * This methods saves a subscription to a file.
             */

            var apiSubscription = Mapper.ToApiModel(subscription);
            var json = JsonSerializer.Serialize(apiSubscription, new JsonSerializerOptions { WriteIndented = true });
            var fileName = CreateDirAndGetFileName(subscription.EmailAddress, basePath);

            Console.WriteLine($"[DEBUG] Skriver til fil: {fileName}");
            await File.WriteAllTextAsync(fileName, json);
        }

        private static string CreateDirAndGetFileName(string emailAddress, string basePath)
        {
            /*
             * This methods creates a directory for the subscription files if it doesn't exist
             * and returns the file name for the subscription.
             */
            var dir = Path.Combine(basePath, SubscriptionsFolderName);
            Directory.CreateDirectory(dir);

            var safeEmail = emailAddress.Trim().ToLower();
            return Path.Combine(dir, $"{safeEmail}.json");
        }
    }
}

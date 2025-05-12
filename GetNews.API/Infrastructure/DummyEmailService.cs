//  DummyService

// importing necessary namespaces
using System.Text.Json;
using GetNews.Core.DomainModel;

namespace GetNews.API.Infrastructure
{
    public class DummyEmailService
    {
        /*
         *  This class is a dummy implementation of an email service that saves emails to a file.  
         */

        private const string EmailsFolderName = "emails";

        public static async Task Send(Email email, string basePath)
        {
            /*
             * function to send an email by saving it to a file.

             @param email: The email to be sent.
             @param basePath: The base path where the email will be saved.
             
            */

            var apiEmail = Mapper.ToApiModel(email);
            var json = JsonSerializer.Serialize(apiEmail);
            var fileName = CreateDirAndGetFileName(email.ToEmailAddress, basePath);

            await File.WriteAllTextAsync(fileName, json);
        }

        private static string CreateDirAndGetFileName(string emailAddress, string basePath)
        {
            /*
                @param emailAddress: The email address to be used for naming the file.
                @param basePath: The base path where the email will be saved.

                A functon to create a directory of sent emails and get the file name for the email.
             */

             var dir = basePath + "/" + EmailsFolderName;
            
            Directory.CreateDirectory(dir);
            
            var fileName = dir + "/" + emailAddress + ".json";

            return fileName;
        }
    }
}

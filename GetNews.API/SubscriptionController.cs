// GetNews API

// Importing necessary namespaces
using GetNews.API.ApiModel;
using GetNews.API.Infrastructure;
using GetNews.Core.ApplicationService;
using Microsoft.Extensions.Options;

namespace GetNews.API
{
    public class SubscriptionController
    {
        public static async Task<object> SignUp(SubscriptionSignUp subscriptionSignUp, IOptions<AppConfig> options)
        {
            var basePath = options.Value.BasePath;
            // IO
            var emailAddress = subscriptionSignUp.EmailAddress;
            var subscription = await SubscriptionFileRepository.LoadSubscription(emailAddress, basePath);

            // Logikk uten IO - i kjernen
            var signUpResult = SubscriptionService.SignUp(emailAddress, subscription);

            // IO

            // Return Error message if sign up failed
            if (!signUpResult.IsSuccess)
            {
                return new { IsSuccess = false, Error = signUpResult.Error.ToString() };
            }

            //  Save subscriber if not null
            if (signUpResult.Subscription != null)
            {
                await SubscriptionFileRepository.SaveSubscription(signUpResult.Subscription, basePath);
            }

            //  Send confirmation if email is provided
            if (signUpResult.Email != null)
            {
                await DummyEmailService.Send(signUpResult.Email, basePath);
            }

            return new { IsSuccess = true, SendtEmail = signUpResult.Email != null };
        }
    }
}

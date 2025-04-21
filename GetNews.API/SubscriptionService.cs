using GetNews.API.ApiModel;
using GetNews.API.Infrastructure;
using GetNews.Core.ApplicationLogic;
using Microsoft.Extensions.Options;

namespace GetNews.API
{
    public class SubscriptionService
    {
        public static async Task<object> SignUp(SubscriptionSignUp subscriptionSignUp, IOptions<AppConfig> options) 
        {
            var basePath = options.Value.BasePath;
            // IO
            var emailAddress = subscriptionSignUp.EmailAddress;
            var subscription = await SubscriptionFileRepository.LoadSubscription(emailAddress, basePath);

            // Logikk uten IO - i kjernen
            var signUpResult = SubscriptionLogic.SignUp(emailAddress, subscription);

            // IO
            if (signUpResult.Subscription != null)
            {
                await SubscriptionFileRepository.SaveSubscription(signUpResult.Subscription, basePath);
            }

            if (signUpResult.Email != null)
            {
                await DummyEmailService.Send(signUpResult.Email, basePath);
            }

            return new { Result = signUpResult.Type.ToString() };
        }
    }
}

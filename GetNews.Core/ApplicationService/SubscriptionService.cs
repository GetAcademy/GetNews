using GetNews.Core.DomainModel;

namespace GetNews.Core.ApplicationService
{
    public class SubscriptionService
    {
        public static SignUpResult Signup(string emailAddressStr, Subscription? subscription)
        {
            if (subscription == null)
            {
                var emailAddress = new EmailAddress(emailAddressStr);
                if (!emailAddress.IsValid()) return new SignUpResult(SignUpResultType.InvalidEmailAddress);
                subscription = new Subscription(emailAddressStr);
            }

            return subscription.Status switch
            {
                SubscriptionStatus.Verified 
                    => new SignUpResult(SignUpResultType.AlreadySubscribed),
                SubscriptionStatus.SignedUp or SubscriptionStatus.Unsubscribed
                    => new SignUpResult(SignUpResultType.SignedUp, subscription,
                        Email.CreateConfirmEmail(emailAddressStr, subscription.VerificationCode.Value))

            };
        }
    }
}

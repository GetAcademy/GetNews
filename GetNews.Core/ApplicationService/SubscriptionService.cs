using GetNews.Core.DomainModel;

namespace GetNews.Core.ApplicationService
{
    internal class SubscriptionService
    {
        public static SignUpResult Signup(string emailAddressStr, Subscription? subscription)
        {
            if (subscription == null)
            {
                var emailAddress = new EmailAddress(emailAddressStr);
                if (!emailAddress.IsValid()) return new SignUpResult(SignUpResultType.InvalidEmailAddress);
                subscription = new Subscription(emailAddress);
            }

            switch (subscription.Status)
            {
                case SubscriptionStatus.Verified:
                    return new SignUpResult(SignUpResultType.AlreadySubscribed);
                case SubscriptionStatus.SignedUp or SubscriptionStatus.Unsubscribed:
                {
                    var email = Email.CreateConfirmEmail(emailAddressStr, subscription.VerificationCode.Value);
                    return new SignUpResult(SignUpResultType.SignedUp, subscription, email);
                }
                
            }

            return new SignUpResult(SignUpResultType.SignedUp);
        }
    }
}

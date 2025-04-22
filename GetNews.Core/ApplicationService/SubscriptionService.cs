using GetNews.Core.DomainModel;

namespace GetNews.Core.ApplicationService
{
    public class SubscriptionService
    {
        public static SignUpResult SignUp(string emailAddressStr, Subscription? subscription)
        {
            if (subscription == null)
            {
                var emailAddress = new EmailAddress(emailAddressStr);
                if (!emailAddress.IsValid()) return SignUpResult.Fail(SignUpError.InvalidEmailAddress);
                subscription = new Subscription(emailAddressStr);
            }

            switch (subscription.Status)
            {
                case SubscriptionStatus.Verified:
                    return SignUpResult.Fail(SignUpError.AlreadySubscribed);
                case SubscriptionStatus.SignedUp or SubscriptionStatus.Unsubscribed:
                {
                    var email = Email.CreateConfirmEmail(emailAddressStr, subscription.VerificationCode.Value);
                    return SignUpResult.Ok(subscription, email);
                }
                default:
                    return SignUpResult.Fail(SignUpError.Unknown);
            }
        }
    }
}

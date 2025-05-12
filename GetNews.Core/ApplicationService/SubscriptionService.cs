using GetNews.Core.DomainModel;

namespace GetNews.Core.ApplicationService
{
    public class SubscriptionService
    {

        public static Result<EmailAndSubscription> SignUp(string emailAddressStr, Subscription? subscription)
        {
            var emailAddress = new EmailAddress(emailAddressStr);

            if (!emailAddress.IsValid())
                return Result<EmailAndSubscription>.Fail(SignUpError.InvalidEmailAddress);

            // create a new subscription object.
            if (subscription == null)
            {
                subscription = new Subscription(emailAddressStr);

                var mail = Email.CreateConfirmEmail(emailAddressStr, subscription.VerificationCode);

                return Result<EmailAndSubscription>.Ok(new EmailAndSubscription(mail, subscription));
            }

            switch (subscription.Status)
            {
                case SubscriptionStatus.Verified or SubscriptionStatus.SignedUp:
                    return Result<EmailAndSubscription>.Fail(SignUpError.SignedUp);

                case SubscriptionStatus.Unsubscribed:
                    var mail = Email.CreateConfirmEmail(emailAddressStr, subscription.VerificationCode);
                    return Result<EmailAndSubscription>.Ok(new EmailAndSubscription(mail, subscription));

                default:
                    return Result<EmailAndSubscription>.Fail(SignUpError.Unknown);
            }
        }

        public static Result<Subscription> Confirm(string userMail, Guid verificationCode, Subscription subscription)
        {
            if (subscription.VerificationCode != verificationCode) return Result<Subscription>.Fail(SignUpError.InvalidVertificationCode);

            if (new EmailAddress(subscription.EmailAddress).IsEqual(userMail)) return Result<Subscription>.Fail(SignUpError.InvalidEmailAddress);

            if (subscription.IsVerified && subscription.Status == SubscriptionStatus.Verified) return Result<Subscription>.Fail(SignUpError.AlreadySubscribed);

            subscription.ChangeStatus();

            return Result<Subscription>.Ok(subscription);
        }

        public static Result<Subscription> Unsubscribe(string userMail, Subscription subscription)
        {
            if (new EmailAddress(subscription.EmailAddress).IsEqual(userMail)) return Result<Subscription>.Fail(SignUpError.InvalidEmailAddress);
            if (!(subscription.Status == SubscriptionStatus.Verified || subscription.IsVerified)) return Result<Subscription>.Fail(SignUpError.Unknown);
            
            subscription.ChangeStatus();

            return Result<Subscription>.Ok(subscription);

        }
    }
}

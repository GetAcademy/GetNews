//  SIgnup process for a subscription

using System.Net.Mail;
using GetNews.Core.DomainModel;

namespace GetNews.Core.ApplicationService
{
    public class SubscriptionService
    {

        /// <summary>
        /// When a user signs up for a subscription, the system will check if the email address is valid.
        /// If the email address is valid, the system will check if the user is already subscribed.
        /// If the user is not already subscribed, the system will create a new subscription object and send a confirmation email to the user.
        /// </summary>
        /// <param name="emailAddressStr">The email address of the user</param>
        /// <param name="subscription">The subscription object of the user</param>
        /// <returns>A SignUpResult object containing the result of the sign-up process</returns>

        public static Result<Subscription> SignUp(string emailAddressStr, Subscription? subscription)
        {
            var emailAddress = new EmailAddress(emailAddressStr);

            if (!emailAddress.IsValid())
                return Result<Subscription>.Fail(EmailError.InvalidEmail);

            // create a new subscription object.
            if (subscription == null)
            {
                subscription = new Subscription(emailAddressStr);

                var mail = Email.CreateConfirmEmail(emailAddressStr, subscription.VerificationCode);
                return Result<Subscription>.Ok(subscription);
            }

            // When the subscriber has already signed up, the system will check if the user is already subscribed.
            switch (subscription.Status)
            {
                case SubscriptionStatus.Verified or SubscriptionStatus.SignedUp:
                    return Result<Subscription>.Fail(SubscriptionError.AlreadySignedUp);

                case SubscriptionStatus.Unsubscribed:
                    var mail = Email.CreateConfirmEmail(emailAddressStr, subscription.VerificationCode);
                    return Result<Subscription>.Ok(subscription);

                default:
                    return Result<Subscription>.Fail(Error.Unknown);
            }
        }

        /// <summary>
        /// When a user verifies their subscription, the system will check if the email address is valid.
        /// If the email address is valid, the system will check if the user is already subscribed.
        /// </summary>
        /// <param name="userMail">The email address of the user</param>
        /// <param name="verificationCode">The verification code of the user</param>
        /// <param name="subscription">The subscription object of the user</param>
        /// <returns>A SignUpResult object containing the result of the sign-up process</returns>

        public static Result<Subscription> Confirm(string userMail, Guid verificationCode, Subscription subscription)
        {
            if (subscription.VerificationCode != verificationCode) return Result<Subscription>.Fail(SubscriptionError.InvalidVertificationCode);

            if (new EmailAddress(subscription.EmailAddress).IsEqual(userMail)) return Result<Subscription>.Fail(EmailError.InvalidEmail);

            if (subscription.IsVerified && subscription.Status == SubscriptionStatus.Verified) return Result<Subscription>.Fail(SubscriptionError.AlreadySignedUp);

            subscription.ChangeStatus();

            return Result<Subscription>.Ok(subscription);
        }

        public static Result<Subscription> Unsubscribe(string userMail, Subscription subscription)
        {
            if (new EmailAddress(subscription.EmailAddress).IsEqual(userMail)) return Result<Subscription>.Fail(EmailError.InvalidEmail);
            if (!(subscription.Status == SubscriptionStatus.Verified || subscription.IsVerified)) return Result<Subscription>.Fail(Error.Unknown);
            
            subscription.ChangeStatus();

            return Result<Subscription>.Ok(subscription);

        }
    }
}

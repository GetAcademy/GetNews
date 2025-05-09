//  SIgnup process for a subscription

using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using GetNews.Core.DomainModel;

namespace GetNews.Core.ApplicationService
{
    public class SubscriptionService
    {

        public static SignUpResult SignUp(string emailAddressStr, Subscription? subscription)
        {
            var emailAddress = new EmailAddress(emailAddressStr);

            if (!emailAddress.IsValid())
                return SignUpResult.Fail(SignUpError.InvalidEmailAddress);

            // create a new subscription object.
            if (subscription == null)
            {
                subscription = new Subscription(emailAddressStr);

                var mail = Email.CreateConfirmEmail(emailAddressStr, subscription.VerificationCode);

                return SignUpResult.Ok(subscription, mail);
            }

            // When the subscriber has already signed up, the system will check if the user is already subscribed.
            switch (subscription.Status)
            {
                case SubscriptionStatus.Verified or SubscriptionStatus.SignedUp:
                    return SignUpResult.Fail(SignUpError.SignedUp);

                case SubscriptionStatus.Unsubscribed:
                    var mail = Email.CreateConfirmEmail(emailAddressStr, subscription.VerificationCode);
                    return SignUpResult.Ok(subscription, mail);

                default:
                    return SignUpResult.Fail(SignUpError.Unknown);
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

            if (new EmailAddress(subscription.EmailAddress).IsEqual(userMail)) return Result<Subscription>.Fail(EmailError.InvalidEmailAddress);

            if (subscription.IsVerified && subscription.Status == SubscriptionStatus.Verified) return Result<Subscription>.Fail(SubscriptionError.AlreadySignedUp);

            subscription.ChangeStatus();

            return Result<Subscription>.Ok(subscription);
        }

        public static Result<Subscription> Unsubscribe(string userMail, Subscription subscription)
        {
            if (new EmailAddress(subscription.EmailAddress).IsEqual(userMail)) return Result<Subscription>.Fail(EmailError.InvalidEmailAddress);
            if (!(subscription.Status == SubscriptionStatus.Verified || subscription.IsVerified)) return Result<Subscription>.Fail(Error.Unknown);
            
            subscription.ChangeStatus();

            return Result<Subscription>.Ok(subscription);

        }
    }
}

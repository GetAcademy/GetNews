//  SIgnup process for a subscription
using System.Net.Mail;
using GetNews.Core.DomainModel;

namespace GetNews.Core.ApplicationService
{
    public class SubscriptionService
    {

        public static SignUpResult SignUp(string emailAddressStr, Subscription? subscription)
        {
            /*
                *   When a user signs up for a subscription, the system will check if the email address is valid.
                *   If the email address is valid, the system will check if the user is already subscribed.

                *   @param emailAddressStr: The email address of the user
                *   @param subscription: The subscription object of the user
                *   @return: A SignUpResult object containing the result of the sign-up process
            */

            //  Ensure the email address is not registered


            var emailAddress = new EmailAddress(emailAddressStr);
            if (!emailAddress.IsValid())
                return SignUpResult.Fail(SignUpError.InvalidEmailAddress);

            // Hvis ingen subscription finnes, opprett ny
            if (subscription == null)
            {
                subscription = new Subscription(emailAddressStr);

                var mail = Email.CreateConfirmEmail(emailAddressStr, subscription.VerificationCode);
                return SignUpResult.Ok(subscription, mail);
            }

            // When the subscriber has already signed up, the system will check if the user is already subscribed.
            switch (subscription.Status)
            {
                case SubscriptionStatus.Verified:
                    return SignUpResult.Fail(SignUpError.AlreadySubscribed);

                case SubscriptionStatus.SignedUp:
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

        public static SignUpResult Confirm(string userMail, Guid verificationCode, Subscription subscription)
        {
            if (subscription.VerificationCode != verificationCode) return SignUpResult.Fail(SignUpError.InvalidVertificationCode);

            if (!string.Equals(subscription.EmailAddress, userMail, StringComparison.OrdinalIgnoreCase)) return SignUpResult.Fail(SignUpError.InvalidEmailAddress);

            if (subscription.IsVerified && subscription.Status == SubscriptionStatus.Verified) return SignUpResult.Fail(SignUpError.AlreadySubscribed);

            subscription.ChangeStatus();

            return SignUpResult.Ok(subscription, null);
        }

        public static SignUpResult? Unsubscribe(string userMail, Subscription subscription)
        {
            if (!(new EmailAddress(subscription.EmailAddress).IsEqual(userMail))) return SignUpResult.Fail(SignUpError.InvalidEmailAddress);
            if (!(subscription.Status == SubscriptionStatus.Verified || subscription.IsVerified)) return SignUpResult.Fail(SignUpError.Unknown);
            
            subscription.ChangeStatus();
            return SignUpResult.Ok(subscription, null);

        }

        public static object Confirm(string emailAddress, object value, Subscription subscription)
        {
            throw new NotImplementedException();
        }
    }
}

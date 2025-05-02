//  SIgnup process for a subscription
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
                    var email = Email.CreateConfirmEmail(emailAddressStr, subscription.VerificationCode);
                    return SignUpResult.Ok(subscription, email);
                }
                default:
                    return SignUpResult.Fail(SignUpError.Unknown);
            }
        }
    
        public static SignUpResult ConfirmSubscription(string userMail, Guid verificationCode, Subscription subscription)
        {
            /*
                *   When a user verifies their subscription, the system will check if the email address is valid.
                *   If the email address is valid, the system will check if the user is already subscribed.

                *   @param userMail: The email address of the user
                *   @param verificationCode: The verification code of the user
                *   @param subscription: The subscription object of the user
                *   @return: A SignUpResult object containing the result of the sign-up process
            */

            try
            {
                if (subscription == null) return SignUpResult.Fail(SignUpError.Unknown);
            }
            catch (NullReferenceException)
            {
                return SignUpResult.Fail(SignUpError.Unknown);
            }
            var email = new EmailAddress(userMail);
            if (!email.IsValid()) return SignUpResult.Fail(SignUpError.InvalidEmailAddress);

                        // Ensure the validation code is not correct
            if (subscription.VerificationCode != verificationCode){
                return SignUpResult.Fail(SignUpError.Unknown);
            }

            //  Ensure the status is not verified, then Change the status
            if (subscription.Status == SubscriptionStatus.Verified) return SignUpResult.Fail(SignUpError.AlreadySubscribed);

            if (!subscription.IsVerified
                && subscription.VerificationCode == verificationCode
                && subscription.EmailAddress == userMail) { subscription.Verify(); }
            
            else return SignUpResult.Fail(SignUpError.AlreadySubscribed);

            return SignUpResult.Ok(subscription, null);
        }
    }
}

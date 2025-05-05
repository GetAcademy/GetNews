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
                Console.WriteLine($"[DEBUG] Ny subscription: {subscription.EmailAddress}, kode: {subscription.VerificationCode}");
                var mail = Email.CreateConfirmEmail(emailAddressStr, subscription.VerificationCode);
                return SignUpResult.Ok(subscription, mail);
            }

            // Bruker eksisterer – sjekk status
            Console.WriteLine($"[DEBUG] Eksisterende subscription: {subscription.EmailAddress}, status={subscription.Status}, kode={subscription.VerificationCode}");

            switch (subscription.Status)
            {
                case SubscriptionStatus.Verified:
                    return SignUpResult.Fail(SignUpError.AlreadySubscribed);

                case SubscriptionStatus.SignedUp:
                case SubscriptionStatus.Unsubscribed:
                    var mail = Email.CreateConfirmEmail(emailAddressStr, subscription.VerificationCode);
                    return SignUpResult.Ok(subscription, mail);

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

            var email = new EmailAddress(userMail);
            if (!email.IsValid()) return SignUpResult.Fail(SignUpError.InvalidEmailAddress);
            
            if (subscription == null) return SignUpResult.Fail(SignUpError.Unknown);

            if (subscription.IsVerified)
                return SignUpResult.Fail(SignUpError.AlreadySubscribed);

            if (!string.Equals(subscription.EmailAddress, userMail, StringComparison.OrdinalIgnoreCase))
                return SignUpResult.Fail(SignUpError.Unknown);

            if (subscription.VerificationCode != verificationCode)
                return SignUpResult.Fail(SignUpError.InvalidVertificationCode);

            subscription.Verify();
            return SignUpResult.Ok(subscription, null);
        }

      /*  public static SignUpResult ConfirmUnsubscription(string userMail, Subscription subscription) 
        {
            Console.WriteLine($"[DEBUG] Unsubscribe attempt: email={userMail}");
            Console.WriteLine($"[DEBUG] Loaded subscription: email={subscription?.EmailAddress}, code={subscription?.VerificationCode}, status={subscription?.Status}");

            if (subscription == null)
                return SignUpResult.Fail(SignUpError.Unknown);
            if (string.Equals(subscription.EmailAddress, userMail, StringComparison.OrdinalIgnoreCase))
            { 
                subscription.UnSubscribe();
                return SignUpResult.Ok(subscription, null);
            }
            return SignUpResult.Fail(SignUpError.Unknown);
        }*/
    }
}

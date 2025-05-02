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
        public static VerificationResult Verify(string emailAdressStr, Subscription? subscription, string code)
        {
            if (subscription == null)
                return VerificationResult.Fail(VerificationError.SubscriptionNotFound);
            if (!Guid.TryParse(code, out var parsedCode))
                return VerificationResult.Fail(VerificationError.InvalidCodeFormat);
            if (!subscription.VerificationCode.HasValue)
                return VerificationResult.Fail(VerificationError.VerificationCodeMissing);
            if (subscription.EmailAddress != emailAdressStr)
                return VerificationResult.Fail(VerificationError.EmailMismatch);
            if (subscription.VerificationCode.Value != parsedCode)
                return VerificationResult.Fail(VerificationError.CodeMismatch);

            return VerificationResult.Success();
        }
    }
}

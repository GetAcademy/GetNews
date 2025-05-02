// GetNews Subscription Service

namespace GetNews.Core.DomainModel
{
    public class Subscription
    {
        public string EmailAddress { get; }
        public SubscriptionStatus Status { get; private set; }
        public DateOnly LastStatusChange { get; private set; }
        public Guid VerificationCode { get; private set; }
        public bool IsVerified { get; private set; }

        public Subscription(
            string emailAddress,
            SubscriptionStatus status = SubscriptionStatus.SignedUp,
            Guid verificationCode = default,
            bool isVerified = false,
            DateOnly? lastStatusChange = null
            )
        {
            Status = status;
            IsVerified = isVerified;
            EmailAddress = emailAddress;

            if (!isVerified) VerificationCode = Guid.NewGuid(); else VerificationCode = verificationCode;


            LastStatusChange = lastStatusChange ?? DateOnly.FromDateTime(DateTime.Now);

        }

        public void ChangeStatus(SubscriptionStatus status)
        {
            //  Changes the status of the subscription

            Status = status;
            IsVerified = status == SubscriptionStatus.Verified;
            LastStatusChange = DateOnly.FromDateTime(DateTime.Now);
        }

        public SignUpResult? Verify(Guid verificationCode)
        {
            // Ensure the validation code is not correct
            if (VerificationCode != verificationCode){
                return SignUpResult.Fail(SignUpError.Unknown);
            }

            //  Ensure the status is not verified, then Change the status
            if (!IsVerified && VerificationCode == verificationCode ) { ChangeStatus(SubscriptionStatus.Verified); }
            else return SignUpResult.Fail(SignUpError.AlreadySubscribed);

            return null;

        }

        public void UnSubscribe()
        {
            // Changes the status to unsubscribed
            ChangeStatus(SubscriptionStatus.Unsubscribed);
        }
        public void SetVerificationCode(Guid code)
        {
            VerificationCode = code;
        }
    }
}

namespace GetNews.Core.DomainModel
{
    public class Subscription
    {
        public string EmailAddress { get; }
        public SubscriptionStatus Status { get; private set; }
        public DateOnly LastStatusChange { get; private set; }
        public Guid? VerificationCode { get; }
        public bool IsVerified { get; private set; }

        public Subscription(
            string emailAddress,
            SubscriptionStatus status = SubscriptionStatus.SignedUp,
            Guid? verificationCode = null,
            bool isVerified = false,
            DateOnly? lastStatusChange = null
            )
        {
            EmailAddress = emailAddress;
            Status = status;
            IsVerified = isVerified;

            if (!isVerified) VerificationCode = verificationCode ?? Guid.NewGuid();
            LastStatusChange = lastStatusChange ?? DateOnly.FromDateTime(DateTime.Now);
        }

        public void ChangeStatus(SubscriptionStatus status)
        {
            Status = status;
            IsVerified = status == SubscriptionStatus.Verified;
            LastStatusChange = DateOnly.FromDateTime(DateTime.Now);
        }

        public void Verify(Guid verificationCode)
        {
            // -- Requires a test to be written

            // Ensure the validation code is not correct
            if (VerificationCode != verificationCode)
            {
                throw new InvalidOperationException("Invalid verification code.");
            }

            //  Change the status to verified
            ChangeStatus(SubscriptionStatus.Verified);
            
        }

        public void UnsubScribe()
        {
            // x -> Not tested
            // Changes the status to unsubscribed
            ChangeStatus(SubscriptionStatus.Unsubscribed);        }
    }
}

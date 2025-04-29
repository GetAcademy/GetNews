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
            //  Changes the status of the subscription

            Status = status;
            LastStatusChange = DateOnly.FromDateTime(DateTime.Now);
            IsVerified = status == SubscriptionStatus.Verified || status == SubscriptionStatus.Unsubscribed;
        }

        public void Verify(Guid verificationCode)
        {
            // -- Requires a test to be written
            //  Check for throw exceptions

            // Ensure the validation code is not correct
            if (VerificationCode != verificationCode)
            {
                throw new InvalidOperationException("Invalid verification code.");
            }
            
            //  Ensure that the subscription is not already verified
            if (!IsVerified)
            {
                //  Change the status to verified
            ChangeStatus(SubscriptionStatus.Verified);
            }

            
            
        }

        public void UnSubscribe()
        {
            // -- Requires a test to be written
            // Changes the status to unsubscribed
            ChangeStatus(SubscriptionStatus.Unsubscribed);        
        }
    }
}

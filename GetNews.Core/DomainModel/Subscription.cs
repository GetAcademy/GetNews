// GetNews Subscription Service2

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
            Guid? verificationCode = null,
            bool isVerified = false,
            DateOnly? lastStatusChange = null
        )
        {
            EmailAddress = emailAddress.Trim().ToLower();
            Status = status;
            IsVerified = isVerified;
            VerificationCode = verificationCode ?? Guid.NewGuid();
            LastStatusChange = lastStatusChange ?? DateOnly.FromDateTime(DateTime.Now);
        }

        public void ChangeStatus()
        {
            //  Changes the status of the subscription
            Status = NextStatus();
            IsVerified = Status == SubscriptionStatus.Verified;
            LastStatusChange = DateOnly.FromDateTime(DateTime.Now);
        }
        
        private SubscriptionStatus NextStatus()
        {
            //  Returns the next status of the subscription
            return Status switch
            {
                SubscriptionStatus.SignedUp => SubscriptionStatus.Verified,
                SubscriptionStatus.Verified => SubscriptionStatus.SignedUp,
                _ => throw new ArgumentOutOfRangeException(nameof(Status), Status, null)
            };
        }


    }
}

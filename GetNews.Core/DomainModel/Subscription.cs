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

        public void ChangeStatus(SubscriptionStatus status)
        {
            //  Changes the status of the subscription
            Status = status;
            IsVerified = status == SubscriptionStatus.Verified;
            LastStatusChange = DateOnly.FromDateTime(DateTime.Now);
        }


    }
}

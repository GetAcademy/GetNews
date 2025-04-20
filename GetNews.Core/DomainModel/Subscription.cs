namespace GetNews.Core.DomainModel
{
    internal class Subscription
    {
        public EmailAddress EmailAddress { get; }
        public SubscriptionStatus Status { get; private set; }
        public DateOnly LastStatusChange { get; private set; }
        public Guid? VerificationCode { get; }
        public bool IsVerified { get; private set; }

        public Subscription(
            EmailAddress emailAddress,
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
            LastStatusChange = DateOnly.FromDateTime(DateTime.Now);
        }

        public void Verify(Guid verificationCode)
        {
            IsVerified = verificationCode == VerificationCode;
        }

    }
}

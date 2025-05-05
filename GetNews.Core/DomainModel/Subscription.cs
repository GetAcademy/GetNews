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
            EmailAddress = emailAddress.Trim().ToLower();
            Status = status;
            IsVerified = isVerified;

            // 🔧 FIX: bare generer ny hvis ingen kode ble sendt inn
            VerificationCode = verificationCode != Guid.Empty ? verificationCode : Guid.NewGuid();

            LastStatusChange = lastStatusChange ?? DateOnly.FromDateTime(DateTime.Now);
        }

        private void ChangeStatus(SubscriptionStatus status)
        {
            //  Changes the status of the subscription

            Status = status;
            IsVerified = status == SubscriptionStatus.Verified;
            LastStatusChange = DateOnly.FromDateTime(DateTime.Now);
        }

        public void Verify()
        {
            ChangeStatus(SubscriptionStatus.Verified);
        }

        public void Unsubscribe()
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

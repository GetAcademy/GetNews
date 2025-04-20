using GetNews.Core.DomainModel;

namespace GetNews.API.ApiModel
{
    public class Subscription
    {
        public string EmailAddress { get; set; }
        public SubscriptionStatus Status { get; set; }
        public DateOnly LastStatusChange { get; set; }
        public Guid? VerificationCode { get; set; }
        public bool IsVerified { get; set; }

        public Core.DomainModel.Subscription ToDomainModel()
        {
            return new Core.DomainModel.Subscription(
                EmailAddress, Status, VerificationCode, IsVerified, LastStatusChange);
        }

        public static Subscription FromDomainModel(Core.DomainModel.Subscription subscription)
        {
            return new Subscription
            {
                EmailAddress = subscription.EmailAddress,
                Status = subscription.Status,
                LastStatusChange = subscription.LastStatusChange,
                VerificationCode = subscription.VerificationCode,
                IsVerified = subscription.IsVerified,

            };
        }
    }
}

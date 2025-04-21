using DomainSubscription = GetNews.Core.DomainModel.Subscription;
using ApiSubscription = GetNews.API.PersistentModel.Subscription;
using DomainEmail=GetNews.Core.DomainModel.Email;
using ApiEmail = GetNews.API.ApiModel.Email;

namespace GetNews.API
{
    public class Mapper
    {
        public static DomainSubscription ToDomainModel(ApiSubscription subscription)
        {
            return new DomainSubscription(
                subscription.EmailAddress,
                subscription.Status,
                subscription.VerificationCode,
                subscription.IsVerified,
                subscription.LastStatusChange);
        }

        public static ApiSubscription ToApiModel(DomainSubscription subscription)
        {
            return new ApiSubscription
            {
                EmailAddress = subscription.EmailAddress,
                Status = subscription.Status,
                LastStatusChange = subscription.LastStatusChange,
                VerificationCode = subscription.VerificationCode,
                IsVerified = subscription.IsVerified,

            };
        }

        public static ApiEmail ToApiModel(DomainEmail email)
        {
            return new ApiEmail
            {
                FromEmailAddress = email.FromEmailAddress,
                ToEmailAddress = email.ToEmailAddress,
                Subject = email.Subject,
                Body = email.Body
            };
        }
    }
}

// ErrorCodes

namespace GetNews.Core.DomainModel
{
    public enum Error
    {
        Unknown
    }

    public enum SubscriptionError 
    {
        AlreadySignedUp,
        SubscriptionNotFound,
        InvalidVertificationCode,
        VertificationCodeNotFound,
    }

    public enum EmailError
    {
        InvalidEmailFormat,
        InvalidEmailAddress,
        EmailNotFound,

    }
}
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
        InvalidEmailFormat,
        SubscriptionNotFound,
        InvalidVertificationCode,
        VertificationCodeNotFound,
    }

    public enum EmailError
    {
        InvalidEmailAddress,
        EmailNotFound,

    }
}
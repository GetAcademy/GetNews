// ErrorCodes

namespace GetNews.Core.DomainModel
{
    public enum Error
    {
        Unknown
    }

    public enum SubscriptionError 
    {
        InvalidEmail,
        AlreadySignedUp,
        EmailNotFound,
        SubscriptionNotFound,
        InvalidVertificationCode,
        VertificationCodeNotFound,
        InvalidEmailFormat,
        
        
        
    }
}
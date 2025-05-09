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
        EmailNotFound,
        AlreadySignedUp,
        InvalidEmailFormat,
        SubscriptionNotFound,
        InvalidVertificationCode,
        VertificationCodeNotFound,
        
        
        
        
    }
}
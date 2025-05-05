// Error codes for the sign-up process in the GetNews application.

namespace GetNews.Core.DomainModel
{
    public enum SignUpError
    {
        // This enum defines various error states that can occur during the sign-up process.

        SignedUp,
        InvalidEmailAddress,
        AlreadySubscribed,
        Unknown
    }
}

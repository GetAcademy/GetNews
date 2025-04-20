namespace GetNews.Core.DomainModel
{
    public class SignUpResult
    {
        public Subscription? Subscription { get; }
        public Email? Email { get; }
        public SignUpResultType Type { get; }

        public SignUpResult(SignUpResultType type, Subscription subscription = null, Email email =null )
        {
            Subscription = subscription;
            Email = email;
            Type = type;
        }
    }
}

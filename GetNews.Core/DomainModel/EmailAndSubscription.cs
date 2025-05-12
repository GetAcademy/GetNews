namespace GetNews.Core.DomainModel
{
    public class EmailAndSubscription
    {
        public Email Email { get;  }
        public Subscription Subscription { get;  }

        public EmailAndSubscription(Email email, Subscription subscription)
        {
            Email = email;
            Subscription = subscription;
        }
    }
}

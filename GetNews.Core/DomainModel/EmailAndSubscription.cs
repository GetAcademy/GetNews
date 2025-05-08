namespace GetNews.Core.DomainModel
{
    internal class EmailAndSubscription
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

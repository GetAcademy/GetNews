namespace GetNews.Core.DomainModel
{
    internal class Email
    {
        public string FromEmailAddress { get; }
        public string ToEmailAddress { get; }
        public string Subject { get; }
        public string Body { get; }

        public Email(string fromEmailAddress, string toEmailAddress, string subject, string body)
        {
            FromEmailAddress = fromEmailAddress;
            ToEmailAddress = toEmailAddress;
            Subject = subject;
            Body = body;
        }
        public static Email CreateConfirmEmail(string emailAddress, Guid code)
        {
            return new Email(
                "getnews@dummymail.com",
                emailAddress,
                "Bekreft abonnement på GET News",
                $"Kode: {code}");
        }
    }
}

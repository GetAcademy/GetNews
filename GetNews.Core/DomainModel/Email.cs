//  Initializing the Confirmation Email

namespace GetNews.Core.DomainModel
{
    public class Email
    {
        public string Body { get; }
        public string Subject { get; }
        public string ToEmailAddress { get; }
        public string FromEmailAddress { get; }

        public Email(string fromEmailAddress, string toEmailAddress, string subject, string body)
        {
            Body = body;
            Subject = subject;
            ToEmailAddress = toEmailAddress;
            FromEmailAddress = fromEmailAddress;
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

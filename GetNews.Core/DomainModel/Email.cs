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
        public static Email CreateConfirmEmail(string userEmail, Guid code)
        {
            return new Email(
                "getnews@dummymail.com",
                userEmail,
                "Bekreft abonnement på GET News",
                $"Kode: {code}");
        }

        public static Email UnsubscribeEmail(string userEmail)
        {
            return new Email(
                "getnews@dummymail.com",
                userEmail,
                "Endringer i abonnementet",
                "Vi bekrefter at du har meldt deg av Nyhetsbrevet hos GET News.\n"
                );
        }
    }
}

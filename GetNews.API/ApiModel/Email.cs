namespace GetNews.API.ApiModel
{
    public class Email
    {
        public string FromEmailAddress { get; set; }
        public string ToEmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public Core.DomainModel.Email ToDomainModel()
        {
            return new Core.DomainModel.Email(FromEmailAddress, ToEmailAddress, Subject, Body);
        }

        public static Email FromDomainModel(Core.DomainModel.Email email)
        {
            return new Email
            {
                FromEmailAddress = email.FromEmailAddress,
                ToEmailAddress = email.ToEmailAddress,
                Subject = email.Subject,
                Body = email.Body
            };
        }
    }
}

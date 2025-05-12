//  Subscription Entity

namespace GetNews.API.ApiModel
{
    public class Email
    {
        public string FromEmailAddress { get; set; }
        public string ToEmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}

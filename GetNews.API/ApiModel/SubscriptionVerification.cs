namespace GetNews.API.ApiModel
{
    public class SubscriptionVerification
    {
        public string EmailAddress { get; set; } = string.Empty;
        public string VerificationCode { get; set; } = string.Empty;
    }
}

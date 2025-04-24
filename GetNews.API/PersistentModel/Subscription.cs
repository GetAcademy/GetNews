//  DataStructure for the Subscription entity

//  Importing necessary namespaces
using GetNews.Core.DomainModel;

namespace GetNews.API.PersistentModel
{
    //  Subscription class representing the subscription entity
    public class Subscription
    {
        public string EmailAddress { get; set; }
        public SubscriptionStatus Status { get; set; }
        public DateOnly LastStatusChange { get; set; }
        public Guid? VerificationCode { get; set; }
        public bool IsVerified { get; set; }
    }
}

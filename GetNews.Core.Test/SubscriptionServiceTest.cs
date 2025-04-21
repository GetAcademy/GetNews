using GetNews.Core.ApplicationService;
using GetNews.Core.DomainModel;

namespace GetNews.Core.Test
{
    public class SubscriptionServiceTest
    {
        [Test]
        public void TestNewSignUp()
        {
            var signUpResult = SubscriptionService.SignUp("a@bb.com", null);

            Assert.That(signUpResult.Type, Is.EqualTo(SignUpResultType.SignedUp));
            Assert.That(signUpResult.Email, Is.InstanceOf<Email>());
            Assert.That(signUpResult.Subscription, Is.InstanceOf<Subscription>());
        }

        [Test]
        public void TestSignUpInvalidEmailAddress()
        {
            var signUpResult = SubscriptionService.SignUp("abb.com", null);

            Assert.That(signUpResult.Type, Is.EqualTo(SignUpResultType.InvalidEmailAddress));
            Assert.That(signUpResult.Email, Is.Null);
            Assert.That(signUpResult.Subscription, Is.Null);
        }

        [Test]
        public void TestSignUpAlreadySubscribed()
        {
            var emailAddress = new EmailAddress("a@bb.com");
            var subscription = new Subscription(emailAddress.Value, SubscriptionStatus.Verified);
            var signUpResult = SubscriptionService.SignUp(emailAddress.Value, subscription);

            Assert.That(signUpResult.Type, Is.EqualTo(SignUpResultType.AlreadySubscribed));
            Assert.That(signUpResult.Email, Is.Null);
            Assert.That(signUpResult.Subscription, Is.Null);
        }

        [Test]
        public void TestSignUpWithExistingUnVerified()
        {
            var emailAddress = new EmailAddress("a@bb.com");
            var subscription = new Subscription(emailAddress.Value, SubscriptionStatus.SignedUp);
            var signUpResult = SubscriptionService.SignUp(emailAddress.Value, subscription);

            Assert.That(signUpResult.Type, Is.EqualTo(SignUpResultType.SignedUp));
            Assert.That(signUpResult.Email, Is.InstanceOf<Email>());
            Assert.That(signUpResult.Subscription, Is.InstanceOf<Subscription>());
        }
    }
}
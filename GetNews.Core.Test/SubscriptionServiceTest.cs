using GetNews.Core.DomainModel;
using GetNews.Core.ApplicationService;


namespace GetNews.Core.Test
{
    public class SubscriptionServiceTest
    {
        private EmailAddress userEmail = new EmailAddress("no-replay@getAcademy.no");

        [Test]
        public void TestSignUpWithValidEmailAddress()
        {

            var hexCode = Guid.NewGuid();
            var email = Email.CreateConfirmEmail(userEmail.Value, hexCode);

            using (Assert.EnterMultipleScope())
            {
                //  Ensure the instance is not null
                Assert.That(email, Is.InstanceOf<Email>());
                Assert.That(email.ToEmailAddress, Is.InstanceOf<string>());
                Assert.That(email.FromEmailAddress, Is.InstanceOf<string>());
            
                // Ensure the email has the correct properties
                Assert.That(email.Body, Is.Not.Null);
                Assert.That(email.Subject, Is.Not.Null);
                Assert.That(email.ToEmailAddress, Is.Not.Null);
                Assert.That(email.FromEmailAddress, Is.Not.Null);

                Assert.That(email.ToEmailAddress, Is.EqualTo(userEmail.Value));
                Assert.That(email.FromEmailAddress, Is.EqualTo("getnews@dummymail.com"));
                Assert.That(email.Body, Is.EqualTo($"Kode: {hexCode}"));
                Assert.That(email.Subject, Is.EqualTo("Bekreft abonnement på GET News"));
            }
        }
        
         [Test]
        public void TestUnsubscribedEmail()
        {
            var email = Email.UnsubscribeEmail(userEmail.Value);

            Assert.That(email, Is.InstanceOf<Email>());
            Assert.That(email.ToEmailAddress, Is.InstanceOf<string>());
            Assert.That(email.FromEmailAddress, Is.InstanceOf<string>());

            Assert.That(email.Body, Is.Not.Null);
            Assert.That(email.Subject, Is.Not.Null);
            Assert.That(email.ToEmailAddress, Is.Not.Null);
            Assert.That(email.FromEmailAddress, Is.Not.Null);

            Assert.That(email.ToEmailAddress, Is.EqualTo(userEmail.Value));
            Assert.That(email.FromEmailAddress, Is.EqualTo("getnews@dummymail.com"));
            Assert.That(email.Body, Is.EqualTo($"Vi bekrefter at du har meldt deg av Nyhetsbrevet hos GET News.\n"));
            Assert.That(email.Subject, Is.EqualTo("Endringer i abonnementet"));
        }

        [Test]
        public void TestNewSignUp()
        {
            var signUpResult = SubscriptionService.SignUp("a@bb.com", null);
            
            Assert.That(signupResult.IsSuccess, Is.True);
            Assert.That(signupResult.Email, Is.InstanceOf<Email>());
            Assert.That(signupResult.Subscription, Is.InstanceOf<Subscription>());

        }

        [Test]
        [TestCase("kake@gmail", TestName = "Invalid email address")]
        [TestCase("kakegmail.no", TestName = "Invalid email address")]
        public void TestSignUpInvalidEmailAddress(string userEmail)
        {
            var singupTest = SubscriptionService.SignUp(userEmail, null);

            Assert.That(singupTest.Email, Is.Null);
            Assert.That(singupTest.IsSuccess, Is.False);
            Assert.That(singupTest.Subscription, Is.Null);
            Assert.That(singupTest.Error, Is.EqualTo(EmailError.InvalidEmailAddress.ToString()));
        }

        [Test]
        public void TestSignUpAlreadySubscribed()
        {
            var subscription = new Subscription(userEmail.Value, SubscriptionStatus.Verified);
            var signUpResult = SubscriptionService.SignUp(userEmail.Value, subscription);

            Assert.That(singupTest.Email, Is.Null);
            Assert.That(singupTest.IsSuccess, Is.False);
            Assert.That(singupTest.Subscription, Is.Null);
            Assert.That(signUpResult.Error, Is.EqualTo(SignUpError.AlreadySubscribed));
        }
        
        [Test]
        public void TestSignUpUnsubscribed()
        {
            var subscription = new Subscription(userEmail.Value, SubscriptionStatus.Unsubscribed);
            var signUpResult = SubscriptionService.SignUp(userEmail.Value, subscription);

            //  Ensure the instancek is Null
            InstanceCheck(signUpResult);

            //  Assert the error is already subscribed
            using (Assert.EnterMultipleScope())
            {
                Assert.That(signUpResult.IsSuccess, Is.True);
            }
        }
        
        [TestCase (SubscriptionStatus.SignedUp, false)]
        public void TestSignUpWithExistingUnVerified(SubscriptionStatus status, bool isVerified)
        {
            var subscription = new Subscription(userEmail.Value, status, null, isVerified, lastStatusChange:new DateOnly(2025, 4, 1));

            var SignedUp = SubscriptionService.SignUp(subscription.EmailAddress, null);
            var SignedUp_1 = SubscriptionService.SignUp(subscription.EmailAddress, subscription);


            Assert.That(SignedUp.IsSuccess, Is.True);
            Assert.That(SignedUp_1.IsSuccess, Is.False);
            Assert.That(SignedUp_1.Error, Is.EqualTo(SubscriptionError.AlreadySignedUp.ToString()));
        }
        
        [TestCase (SubscriptionStatus.SignedUp, true)]
        [TestCase (SubscriptionStatus.SignedUp, false)]
        [TestCase (SubscriptionStatus.Verified, false)]
        [TestCase (SubscriptionStatus.Unsubscribed, false)]
        public void TestConfirmation(SubscriptionStatus status, bool isVerified)
        {
            var subscription = new Subscription(userEmail.Value, SubscriptionStatus.SignedUp, Guid.NewGuid(), false, lastStatusChange:new DateOnly(2025, 4, 1));

            var confirm = SubscriptionService.Confirm(subscription.EmailAddress, subscription.VerificationCode, subscription);

            Assert.That(confirm.IsSuccess, Is.True);
            Assert.That(subscription.IsVerified, Is.True);
            Assert.That(subscription.Status, Is.EqualTo(SubscriptionStatus.Verified));
        }

        [TestCase (SubscriptionStatus.Verified, true, "kake@gmail")]
        [TestCase (SubscriptionStatus.SignedUp, true, "kakegmail.no")]
        [TestCase (SubscriptionStatus.SignedUp, false, "kakegmailno")]
        public void TestInvalidConfirmation(SubscriptionStatus status, bool isVerified, string fakeEmail)
        {
            var lastStatusUpdate = new DateOnly(2025, 4, 1);


            var subscription = new Subscription(userEmail.Value, status, null, isVerified, lastStatusUpdate);

            var confirm = SubscriptionService.Confirm(fakeEmail, subscription.VerificationCode, subscription);
            var confirm_1 = SubscriptionService.Confirm(subscription.EmailAddress, Guid.NewGuid(), subscription);
            var confirm_2 = SubscriptionService.Confirm("kake@gmail.no", subscription.VerificationCode, subscription);

            Assert.That(confirm.IsSuccess, Is.False);
            Assert.That(confirm.Error, Is.EqualTo(EmailError.InvalidEmailAddress.ToString()));
            Assert.That(confirm_2.Error, Is.EqualTo(EmailError.InvalidEmailAddress.ToString()));
            Assert.That(confirm_1.Error, Is.EqualTo(SubscriptionError.InvalidVertificationCode.ToString()));
        }

        [TestCase(SubscriptionStatus.Verified, true)]
        [TestCase(SubscriptionStatus.SignedUp, true)]
        [TestCase(SubscriptionStatus.SignedUp, false)]
        [TestCase(SubscriptionStatus.Verified, false)]
        public void TestUnsubscribed(SubscriptionStatus status, bool isVerified)
        {
            var subscription = new Subscription(userEmail.Value, status, null, isVerified, lastStatusChange:new DateOnly(2025, 4, 1));
            var subscription_1 = new Subscription(userEmail.Value, SubscriptionStatus.Verified, null, true, lastStatusChange:new DateOnly(2025, 4, 1));

            SubscriptionService.Unsubscribe(subscription.EmailAddress, subscription);
            SubscriptionService.Unsubscribe(subscription.EmailAddress, subscription_1);
            SubscriptionService.Unsubscribe(subscription.EmailAddress.ToLower(), subscription);
            SubscriptionService.Unsubscribe(subscription.EmailAddress.ToUpper(), subscription);

            Assert.That(subscription.IsVerified, Is.False);
            Assert.That(subscription.Status, Is.EqualTo(SubscriptionStatus.SignedUp));
        }
    }
}
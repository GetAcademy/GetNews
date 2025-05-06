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
                Assert.That(email.Body, Is.EqualTo($"Vi bekrefter at du har meldt deg av Nyhetsbrevet hos GET News.\n"));
                Assert.That(email.Subject, Is.EqualTo("Endringer i abonnementet"));
            }
        }
        [Test]
        public void TestNewSignUp()
        {
            var signUpResult = SubscriptionService.SignUp("a@bb.com", null);
            
            InstanceCheck(signUpResult);

        }

        [Test]
        [TestCase("kake@gmail", TestName = "Invalid email address")]
        [TestCase("kakegmail.no", TestName = "Invalid email address")]
        public void TestSignUpInvalidEmailAddress(string userEmail)
        {
            //  Arrange test
            var singupTest = SubscriptionService.SignUp(userEmail, null);

            //  Assert
            NullCheck(singupTest);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(singupTest.Error, Is.EqualTo(SignUpError.InvalidEmailAddress));
            }
        }

        [Test]
        public void TestSignUpAlreadySubscribed()
        {
            var subscription = new Subscription(userEmail.Value, SubscriptionStatus.Verified);
            var signUpResult = SubscriptionService.SignUp(userEmail.Value, subscription);

            //  Ensure the instance is Null
            NullCheck(signUpResult);

            //  Assert the error is already subscribed
            using (Assert.EnterMultipleScope())
            {
                Assert.That(signUpResult.Error, Is.EqualTo(SignUpError.AlreadySubscribed));
            }
            
            
        }
        
        [Test]
        public void TestSignUpUnsubscribed()
        {
            var subscription = new Subscription(userEmail.Value, SubscriptionStatus.Unsubscribed);
            var signUpResult = SubscriptionService.SignUp(userEmail.Value, subscription);

            //  Ensure the instance is Null
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
            //  Arrange test
            var subscription = new Subscription(userEmail.Value, status, null, isVerified, lastStatusChange:new DateOnly(2025, 4, 1));

            //  Activate the subscription
            var SignedUp = SubscriptionService.SignUp(subscription.EmailAddress, null);
            var SignedUp_1 = SubscriptionService.SignUp(subscription.EmailAddress, subscription);

            //  Assert the subscription is not verified
            using (Assert.EnterMultipleScope())
            {
                Assert.That(SignedUp.IsSuccess, Is.True);
                Assert.That(SignedUp_1.IsSuccess, Is.False);
                
                Assert.That(SignedUp_1.Error, Is.EqualTo(SignUpError.SignedUp));
            }
        }
        
        [TestCase (SubscriptionStatus.SignedUp, true)]
        [TestCase (SubscriptionStatus.SignedUp, false)]
        [TestCase (SubscriptionStatus.Verified, false)]
        [TestCase (SubscriptionStatus.Unsubscribed, false)]
        public void TestConfirmation(SubscriptionStatus status, bool isVerified)
        {
            //  Arrange
            var subscription = new Subscription(userEmail.Value, SubscriptionStatus.SignedUp, Guid.NewGuid(), false, lastStatusChange:new DateOnly(2025, 4, 1));


            //  Activate the subscription
            var confirm = SubscriptionService.Confirm(subscription.EmailAddress, subscription.VerificationCode, subscription);


            //  Assert the subscription is verified
            using (Assert.EnterMultipleScope())
            {
                Assert.That(confirm.IsSuccess, Is.True);
                Assert.That(subscription.IsVerified, Is.True);
                Assert.That(subscription.Status, Is.EqualTo(SubscriptionStatus.Verified));
            }
        }

        [TestCase (SubscriptionStatus.Verified, true, "kake@gmail")]
        [TestCase (SubscriptionStatus.SignedUp, true, "kakegmail.no")]
        [TestCase (SubscriptionStatus.SignedUp, false, "kakegmailno")]
        public void TestInvalidConfirmation(SubscriptionStatus status, bool isVerified, string fakeEmail)
        {
            //  Arrange
            var lastStatusUpdate = new DateOnly(2025, 4, 1);
            //  Initialize the subscription with a valid email address
            var subscription = new Subscription(userEmail.Value, status, null, isVerified, lastStatusUpdate);

            //  Activate the subscription
            var confirm = SubscriptionService.Confirm(fakeEmail, subscription.VerificationCode, subscription);
            var confirm_1 = SubscriptionService.Confirm(subscription.EmailAddress, Guid.NewGuid(), subscription);
            var confirm_2 = SubscriptionService.Confirm("kake@gmail.no", subscription.VerificationCode, subscription);

            //  Assert the subscription is not verified
            using (Assert.EnterMultipleScope())
            {
                Assert.That(confirm.IsSuccess, Is.False);


                Assert.That(confirm.Error, Is.EqualTo(SignUpError.InvalidEmailAddress));
                Assert.That(confirm_1.Error, Is.EqualTo(SignUpError.InvalidVertificationCode));
                Assert.That(confirm_2.Error, Is.EqualTo(SignUpError.InvalidEmailAddress));
            }

        }

        [TestCase(SubscriptionStatus.Verified, true)]
        [TestCase(SubscriptionStatus.SignedUp, true)]
        [TestCase(SubscriptionStatus.SignedUp, false)]
        [TestCase(SubscriptionStatus.Verified, false)]
        public void TestUnsubscribed(SubscriptionStatus status, bool isVerified)

        {
            //  Arrange
            var subscription = new Subscription(userEmail.Value, status, null, isVerified, lastStatusChange:new DateOnly(2025, 4, 1));
            var subscription_1 = new Subscription(userEmail.Value, SubscriptionStatus.Verified, null, true, lastStatusChange:new DateOnly(2025, 4, 1));

            //  Activate the subscription
            SubscriptionService.Unsubscribe(subscription.EmailAddress, subscription);
            SubscriptionService.Unsubscribe(subscription.EmailAddress, subscription_1);
            SubscriptionService.Unsubscribe(subscription.EmailAddress.ToLower(), subscription);
            SubscriptionService.Unsubscribe(subscription.EmailAddress.ToUpper(), subscription);

            //  Assert the subscription is SignedUp
            using (Assert.EnterMultipleScope())
            {
                Assert.That(subscription.IsVerified, Is.False);
                Assert.That(subscription.Status, Is.EqualTo(SubscriptionStatus.SignedUp));
                
            }
        }

        private static void NullCheck(SignUpResult subscription)
        {
            /*
                *   Helper function to ensure the type is null

                *   @param : Subscription type of SignUpResults 
            */

            using (Assert.EnterMultipleScope())
            {
                
                
                Assert.That(subscription.Email, Is.Null);
                Assert.That(subscription.IsSuccess, Is.False);
                Assert.That(subscription.Subscription, Is.Null);
            }
        }
        
        private static void InstanceCheck(SignUpResult subscription)
        {
            /*
                *   Helper function to ensures the type of Email and Subscription

                *   @param : Subscription type of SignUpResults 
            */

            using (Assert.EnterMultipleScope())
            {
                
                Assert.That(subscription.IsSuccess, Is.True);
                Assert.That(subscription.Email, Is.InstanceOf<Email>());
                Assert.That(subscription.Subscription, Is.InstanceOf<Subscription>());
            }
        }
        

    }
}
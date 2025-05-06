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
                public void TestSignUpWithEmailAddress()
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
        public void TestNewSignUp()
        {
            var signUpResult = SubscriptionService.SignUp("a@bb.com", null);
            
            InstanceCheck(signUpResult);

        }

        [Test]
        public void TestSignUpInvalidEmailAddress()
        {
            var singupTest = SubscriptionService.SignUp("abb@com", null);
            var signupTest_2 = SubscriptionService.SignUp("abb.com", null);
            

            //  Ensure the instance is Null
            NullCheck(singupTest);
            NullCheck(signupTest_2);
            

            using (Assert.EnterMultipleScope())
            {
                Assert.That(singupTest.Error, Is.EqualTo(SignUpError.InvalidEmailAddress));
                Assert.That(signupTest_2.Error, Is.EqualTo(SignUpError.InvalidEmailAddress));
            }
            // Check for throw exceptions
            
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
        [Test]
        public void TestSignUpWithExistingUnVerified()
        {
            var subscription = new Subscription(userEmail.Value, SubscriptionStatus.SignedUp, Guid.NewGuid(), false, lastStatusChange:new DateOnly(2025, 4, 1));
            var subscription_1 = new Subscription(userEmail.Value, SubscriptionStatus.SignedUp, Guid.NewGuid(), false, lastStatusChange:new DateOnly(2025, 4, 1));

            var SignedUp = SubscriptionService.SignUp(subscription.EmailAddress, null);
            var SignedUp_1 = SubscriptionService.SignUp(subscription_1.EmailAddress, subscription);


            using (Assert.EnterMultipleScope())
            {
                Assert.That(SignedUp.IsSuccess, Is.True);
                Assert.That(SignedUp_1.IsSuccess, Is.False);
                
                Assert.That(SignedUp_1.Error, Is.EqualTo(SignUpError.SignedUp));
            }

        }
        
        [Test]
        public void TestConfirmation()
        {

            //  Initialize the subscription with a valid email address
            var subscription = new Subscription(userEmail.Value, SubscriptionStatus.SignedUp, Guid.NewGuid(), false, lastStatusChange:new DateOnly(2025, 4, 1));
            var subscription_1 = new Subscription(userEmail.Value, SubscriptionStatus.Verified, Guid.NewGuid(), false, lastStatusChange:new DateOnly(2025, 4, 1));
            var subscription_2 = new Subscription(userEmail.Value, SubscriptionStatus.SignedUp, Guid.NewGuid(), true, lastStatusChange:new DateOnly(2025, 4, 1));


            //  Verify the subscription to ensure the status is verified
            var confirm = SubscriptionService.ConfirmSubscription(subscription.EmailAddress, subscription.VerificationCode, subscription);
            var confirm_1 = SubscriptionService.ConfirmSubscription(subscription_1.EmailAddress, subscription_1.VerificationCode, subscription_1);
            var confirm_2 = SubscriptionService.ConfirmSubscription(subscription_2.EmailAddress, subscription_2.VerificationCode, subscription_2);


            using (Assert.EnterMultipleScope())
            {
                Assert.That(confirm.IsSuccess, Is.True);
                Assert.That(confirm_1.IsSuccess, Is.True);
                Assert.That(confirm_2.IsSuccess, Is.True);

                Assert.That(subscription.IsVerified, Is.True);
                Assert.That(subscription_1.IsVerified, Is.True);
                Assert.That(subscription_2.IsVerified, Is.True);

                Assert.That(subscription.Status, Is.EqualTo(SubscriptionStatus.Verified));
                Assert.That(subscription_1.Status, Is.EqualTo(SubscriptionStatus.Verified));
                Assert.That(subscription_2.Status, Is.EqualTo(SubscriptionStatus.Verified));
 
            }

        }

        [Test]
        public void TestInvalidConfirmation()
        {

            //  Initialize the subscription with a valid email address
            var subscription = new Subscription(userEmail.Value, SubscriptionStatus.Verified, Guid.NewGuid(), true, lastStatusChange:new DateOnly(2025, 4, 1));
            var subscription_1 = new Subscription(userEmail.Value, SubscriptionStatus.SignedUp, Guid.NewGuid(), true, lastStatusChange:new DateOnly(2025, 4, 1));
            var subscription_2 = new Subscription(userEmail.Value, SubscriptionStatus.SignedUp, Guid.NewGuid(), false, lastStatusChange:new DateOnly(2025, 4, 1));


            //  Verify the subscription to ensure the status is verified
            var confirm_3 = SubscriptionService.ConfirmSubscription(subscription.EmailAddress, subscription.VerificationCode, subscription);
            var confirm_4 = SubscriptionService.ConfirmSubscription("kake@gmail.no", subscription_1.VerificationCode, subscription_1);
            var confirm_5 = SubscriptionService.ConfirmSubscription(subscription_2.EmailAddress, Guid.NewGuid(), subscription);

            using (Assert.EnterMultipleScope())
            {

                Assert.That(confirm_3.IsSuccess, Is.False);
                Assert.That(confirm_4.IsSuccess, Is.False);
                Assert.That(confirm_5.IsSuccess, Is.False);

                Assert.That(confirm_3.Error, Is.EqualTo(SignUpError.AlreadySubscribed));
                Assert.That(confirm_4.Error, Is.EqualTo(SignUpError.InvalidEmailAddress));
                Assert.That(confirm_5.Error, Is.EqualTo(SignUpError.InvalidVertificationCode));

                Assert.That(subscription.Status, Is.EqualTo(SubscriptionStatus.Verified));
                Assert.That(subscription_1.Status, Is.EqualTo(SubscriptionStatus.SignedUp));
                Assert.That(subscription_2.Status, Is.EqualTo(SubscriptionStatus.SignedUp));

 
            }

        }

        [Test]
        public void TestUnsubscribed()
        {
            var subscription = new Subscription(userEmail.Value, SubscriptionStatus.Verified, Guid.NewGuid(), true, lastStatusChange:new DateOnly(2025, 4, 1));
            var subscription_1 = new Subscription(userEmail.Value, SubscriptionStatus.Verified, Guid.NewGuid(), true, lastStatusChange:new DateOnly(2025, 4, 1));
            var subscription_2 = new Subscription(userEmail.Value, SubscriptionStatus.Verified, Guid.NewGuid(), true, lastStatusChange:new DateOnly(2025, 4, 1));
        
            var subscription_3 = new Subscription(userEmail.Value, SubscriptionStatus.SignedUp, Guid.NewGuid(), true, lastStatusChange:new DateOnly(2025, 4, 1));
            var subscription_5 = new Subscription(userEmail.Value, SubscriptionStatus.SignedUp, Guid.NewGuid(), false, lastStatusChange:new DateOnly(2025, 4, 1));
            var subscription_4 = new Subscription(userEmail.Value, SubscriptionStatus.Verified, Guid.NewGuid(), false, lastStatusChange:new DateOnly(2025, 4, 1));
            
            var subscription_6 = new Subscription(userEmail.Value, SubscriptionStatus.Unsubscribed, Guid.NewGuid(), false, lastStatusChange:new DateOnly(2025, 4, 1));
            var subscription_7 = new Subscription(userEmail.Value, SubscriptionStatus.Unsubscribed, Guid.NewGuid(), true, lastStatusChange:new DateOnly(2025, 4, 1));

            SubscriptionService.ConfirmUnsubscription(subscription.EmailAddress, subscription);
            SubscriptionService.ConfirmUnsubscription(subscription_3.EmailAddress, subscription_2);
            SubscriptionService.ConfirmUnsubscription(subscription_1.EmailAddress.ToLower(), subscription_1);
            SubscriptionService.ConfirmUnsubscription(subscription_2.EmailAddress.ToUpper(), subscription_2);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(subscription.IsVerified, Is.False);
                Assert.That(subscription_1.IsVerified, Is.False);
                Assert.That(subscription_2.IsVerified, Is.False);
                Assert.That(subscription_3.IsVerified, Is.True);
                Assert.That(subscription_4.IsVerified, Is.False);
                Assert.That(subscription_5.IsVerified, Is.False);
                Assert.That(subscription_6.IsVerified, Is.False);
                Assert.That(subscription_7.IsVerified, Is.True);

                Assert.That(subscription.Status, Is.EqualTo(SubscriptionStatus.Unsubscribed));
                Assert.That(subscription_1.Status, Is.EqualTo(SubscriptionStatus.Unsubscribed));
                Assert.That(subscription_2.Status, Is.EqualTo(SubscriptionStatus.Unsubscribed));

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
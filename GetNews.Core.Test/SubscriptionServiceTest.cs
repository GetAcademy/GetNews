using GetNews.Core.ApplicationService;
using GetNews.Core.DomainModel;

namespace GetNews.Core.Test
{
    public class SubscriptionServiceTest
    {
        private EmailAddress userEmail = new EmailAddress("no-replay@getAcademy.no");

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
        public void TestSignUpWithExistingUnVerified()
        {
            var subscription = new Subscription(userEmail.Value, SubscriptionStatus.SignedUp, Guid.NewGuid(), false, lastStatusChange:new DateOnly(2025, 4, 1));

            var signUpResult = SubscriptionService.SignUp(userEmail.Value, subscription);


            Assert.That(signUpResult.Error, Is.EqualTo(SignUpError.SignedUp));
        }
        
        [Test]
        public void Test_Confirmation()
        {

            var subscription = new Subscription(userEmail.Value, SubscriptionStatus.SignedUp, Guid.NewGuid(), false, lastStatusChange:new DateOnly(2025, 4, 1));

            //  Verify the subscription to ensure the status is verified
            var confirm = SubscriptionService.ConfirmSubscription(subscription.EmailAddress, subscription.VerificationCode, subscription);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(confirm.IsSuccess, Is.True);
                Assert.That(subscription.IsVerified, Is.True);
                Assert.That(subscription.Status, Is.EqualTo(SubscriptionStatus.Verified));

                
            }

        }

        [Test]
        public void TestVerificationCode()
        {

            var subscription = new Subscription(
                userEmail.Value, 
                SubscriptionStatus.SignedUp,
                Guid.NewGuid(),
                false,
               lastStatusChange:new DateOnly(2025, 4, 1)
                );

            var signUpResult = SubscriptionService.SignUp(userEmail.Value, subscription);
            var confirm = SubscriptionService.ConfirmSubscription(subscription.EmailAddress, Guid.NewGuid(), subscription);
            
            using (Assert.EnterMultipleScope())
            {
                

                Assert.That(subscription.IsVerified, Is.False);
                Assert.That(confirm.Error, Is.EqualTo(SignUpError.InvalidVertificationCode));
                Assert.That(subscription.Status, Is.EqualTo(SubscriptionStatus.SignedUp));
                
            }

        }
                [Test]
        public void TestEmailAddress()
        {

            var subscription = new Subscription(
                userEmail.Value, 
                SubscriptionStatus.SignedUp,
                Guid.NewGuid(),
                false,
               lastStatusChange:new DateOnly(2025, 4, 1)
                );

            var signUpResult = SubscriptionService.SignUp(subscription, subscription);
            var confirm = SubscriptionService.ConfirmSubscription(subscription.EmailAddress, Guid.NewGuid(), subscription);
            
            using (Assert.EnterMultipleScope())
            {
                

                Assert.That(subscription.IsVerified, Is.False);
                Assert.That(confirm.Error, Is.EqualTo(SignUpError.InvalidVertificationCode));
                Assert.That(subscription.Status, Is.EqualTo(SubscriptionStatus.SignedUp));
                
            }

        }

        [Test]
        public void Test_Verifed_Subscription()
        {
            var subscription = new Subscription(
                userEmail.Value, 
                SubscriptionStatus.SignedUp,
                Guid.NewGuid(),
                true,
               lastStatusChange:new DateOnly(2025, 4, 1)
                );
            var signUpResult = SubscriptionService.SignUp(userEmail.Value, subscription);

            //  Verify the subscription to ensure the status is verified
            var confirm = SubscriptionService.ConfirmSubscription(userEmail.Value, subscription.VerificationCode, subscription);
            
            using (Assert.EnterMultipleScope())
            {
                Assert.That(subscription.IsVerified, Is.True);
                Assert.That(confirm.Error, Is.EqualTo(SignUpError.AlreadySubscribed));
                Assert.That(subscription.Status, Is.EqualTo(SubscriptionStatus.SignedUp));
                
            }
        }

        [Test]
        public void Test_Status_Subscription()
        {
            var subscription = new Subscription(
                userEmail.Value, 
                SubscriptionStatus.Verified,
                Guid.NewGuid(),
                false,
               lastStatusChange:new DateOnly(2025, 4, 1)
                );

            //  Verify the subscription to ensure the status is verified
            var confirm = SubscriptionService.ConfirmSubscription(userEmail.Value, subscription.VerificationCode, subscription);
            
            using (Assert.EnterMultipleScope())
            {
                Assert.That(subscription.IsVerified, Is.False);
                Assert.That(confirm.Error, Is.EqualTo(SignUpError.AlreadySubscribed));
                Assert.That(subscription.Status, Is.EqualTo(SubscriptionStatus.Verified));
            }
        }

        [Test]
        public void Test_Unsubscribed()
        {
            var subscription = new Subscription(
                userEmail.Value, 
                SubscriptionStatus.Verified,
                Guid.NewGuid(),
                true,
               lastStatusChange:new DateOnly(2025, 4, 1)
                );
            
            //  Verify the subscription to ensure the status is unsubscribed
            SubscriptionService.ConfirmUnsubscribe(userEmail.Value, subscription);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(subscription.IsVerified, Is.False);
                Assert.That(subscription.Status, Is.EqualTo(SubscriptionStatus.Unsubscribed));
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
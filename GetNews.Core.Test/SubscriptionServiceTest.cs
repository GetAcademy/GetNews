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

            //Assert.That(signUpResult.Type, Is.EqualTo(SignUpError.SignedUp));
            InstanceCheck(signUpResult);

        }

        [Test]
        public void TestSignUpInvalidEmailAddress()
        {
            var signUpResult = SubscriptionService.SignUp("abb.com", null);

            //  Ensure the instance is Null
            NullCheck(signUpResult);

            //Assert.That(signUpResult.Type, Is.EqualTo(SignUpError.InvalidEmailAddress));
        }

        [Test]
        public void TestSignUpAlreadySubscribed()
        {
            var subscription = new Subscription(userEmail.Value, SubscriptionStatus.Verified);
            var signUpResult = SubscriptionService.SignUp(userEmail.Value, subscription);

            //  Ensure the instance is Null
            NullCheck(signUpResult);

            // Check for throw exceptions
            //Assert.That(signUpResult.Type, Is.EqualTo(SignUpError.AlreadySubscribed));
            
        }

        [Test]
        public void TestSignUpWithExistingUnVerified()
        {
            var subscription = new Subscription(userEmail.Value, SubscriptionStatus.SignedUp);
            var signUpResult = SubscriptionService.SignUp(userEmail.Value, subscription);

            //  Ensures the type of Email and Subscription
            InstanceCheck(signUpResult);

        }
        
        [Test]
        public void Test_Confirmation()
        {

            var subscription = new Subscription(
                userEmail.Value, 
                SubscriptionStatus.SignedUp,
                Guid.NewGuid(),
                false,
               lastStatusChange:new DateOnly(2025, 4, 1)
                );

            var signUpResult = SubscriptionService.SignUp(userEmail.Value, subscription);
            
            //  Verify the subscription to ensure the status is verified
            var confirm = SubscriptionService.ConfirmSubscription(userEmail.Value, subscription.VerificationCode, subscription);

            using (Assert.EnterMultipleScope())
            {
                
                Assert.That(subscription.IsVerified, Is.True);
                Assert.That(subscription.Status, Is.EqualTo(SubscriptionStatus.Verified));
                Assert.That(confirm.Error, Is.EqualTo(SignUpResult.Ok(subscription, null).Error));
            }

            //  Ensures the integerty type of Email and Subscription
            InstanceCheck(signUpResult);
        }

        [Test]
        public void Test_Generated_verification_code()
        {

            var subscription = new Subscription(
                userEmail.Value, 
                SubscriptionStatus.SignedUp,
                Guid.NewGuid(),
                false,
               lastStatusChange:new DateOnly(2025, 4, 1)
                );

            var signUpResult = SubscriptionService.SignUp(userEmail.Value, subscription);
            //  Verify the subscription to ensure the status is verified
            
            var confirm = SubscriptionService.ConfirmSubscription(userEmail.Value, Guid.NewGuid(), subscription);
            
            using (Assert.EnterMultipleScope())
            {
                Assert.That(confirm.Error, Is.EqualTo(SignUpError.Unknown));

                Assert.That(subscription.IsVerified, Is.False);
                Assert.That(subscription.Status, Is.EqualTo(SubscriptionStatus.SignedUp));
                
            }
            

            //  Ensures the type of Email and Subscription
            InstanceCheck(signUpResult);
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
                SubscriptionStatus.SignedUp,
                Guid.NewGuid(),
                true,
               lastStatusChange:new DateOnly(2025, 4, 1)
                );

            var signUpResult = SubscriptionService.SignUp(userEmail.Value, subscription);

            //  Ensures the type of Email and Subscription
            InstanceCheck(signUpResult);
            
            //  Verify the subscription to ensure the status is unsubscribed
            subscription.UnSubscribe();
            Assert.That(subscription.Status, Is.EqualTo(SubscriptionStatus.Unsubscribed));

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
                Assert.That(subscription.Email, Is.InstanceOf<Email>());
                Assert.That(subscription.Subscription, Is.InstanceOf<Subscription>());
            }
        }
        

    }
}
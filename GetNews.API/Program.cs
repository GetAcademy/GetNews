using GetNews.API.ApiModel;
using GetNews.API.Infrastructure;
using GetNews.Core.ApplicationService;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapPost("/api/subscription", async (SubscriptionSignUp subscriptionSignUp) =>
{
    // IO
    var emailAddress = subscriptionSignUp.EmailAddress;
    var subscription = await SubscriptionFileRepository.GetSubscriptionByEmail(emailAddress);

    // Logikk uten IO - i kjernen
    var signUpResult = SubscriptionService.Signup(emailAddress, subscription);

    // IO
    if (signUpResult.Subscription != null)
    {
        await SubscriptionFileRepository.SaveSubscription(signUpResult.Subscription);
    }

    if (signUpResult.Email != null)
    {
        await DummyEmailService.Send(signUpResult.Email);
    }

    return new { Result = signUpResult.Type.ToString() };
});

app.Run();

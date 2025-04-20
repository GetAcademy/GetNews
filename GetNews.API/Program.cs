using GetNews.API.ApiModel;
using GetNews.API.Infrastructure;
using GetNews.Core.ApplicationService;

var builder = WebApplication.CreateBuilder(args);
var basePath = builder.Environment.ContentRootPath;
var app = builder.Build();
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapPost("/api/subscription", async (SubscriptionSignUp subscriptionSignUp) =>
{
    // IO
    var emailAddress = subscriptionSignUp.EmailAddress;
    var subscription = await SubscriptionFileRepository.LoadSubscription(emailAddress, basePath);

    // Logikk uten IO - i kjernen
    var signUpResult = SubscriptionService.Signup(emailAddress, subscription);

    // IO
    if (signUpResult.Subscription != null)
    {
        await SubscriptionFileRepository.SaveSubscription(signUpResult.Subscription, basePath);
    }

    if (signUpResult.Email != null)
    {
        await DummyEmailService.Send(signUpResult.Email, basePath);
    }

    return new { Result = signUpResult.Type.ToString() };
});

app.Run();

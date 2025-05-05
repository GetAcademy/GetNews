//  Main GetNews API

//  Importing necessary namespaces
using GetNews.API;
using GetNews.Core.DomainModel;

// Adding services to the container
var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<AppConfig>(config => { config.BasePath = AppContext.BaseDirectory; });

// Configure the HTTP request pipeline.
var app = builder.Build();
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapPost("/api/subscription/signup", SubscriptionController.SignUp);
app.MapPost("/api/subscription/verify"), Subscription.Verify);

// Run the application
app.Run();
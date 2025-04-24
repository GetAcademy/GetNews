//  Main GetNews API

//  Importing necessary namespaces
using GetNews.API;

// Adding services to the container
var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<AppConfig>(config => { config.BasePath = AppContext.BaseDirectory; });

// Configure the HTTP request pipeline.
var app = builder.Build();
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapPost("/api/subscription", SubscriptionController.SignUp);

// Run the application
app.Run();
using GetNews.API;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<AppConfig>(config => { config.BasePath = AppContext.BaseDirectory; });
var app = builder.Build();
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapPost("/api/subscription", SubscriptionController.SignUp);

app.Run();

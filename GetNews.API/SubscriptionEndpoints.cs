namespace GetNews.API
{
    public static class SubscriptionEndpoints
    {
        public static void MapSubscriptionEndpoints(this IEndpointRouteBuilder routes) 
        {
            var group = routes.MapGroup("/api/subscription");

            group.MapPost("/signup", SubscriptionController.SignUp);
            group.MapPost("/verify", SubscriptionController.Verify);
            group.MapPost("/unsubscribe", SubscriptionController.Unsubscribe);
        }
    }
}

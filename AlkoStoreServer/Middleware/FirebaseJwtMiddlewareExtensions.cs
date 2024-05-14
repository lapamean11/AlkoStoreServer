namespace AlkoStoreServer.Middleware
{
    public static class FirebaseJwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseFirebaseJwtMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FirebaseJwtMiddleware>();
        }
    }
}

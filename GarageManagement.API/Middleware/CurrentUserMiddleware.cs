namespace GarageManagement.API.Middleware;

public class CurrentUserMiddleware
{
    private readonly RequestDelegate _next;

    public CurrentUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            var roleClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == "Role")?.Value;

            if (Guid.TryParse(userIdClaim, out var userId))
            {
                context.Items["UserId"] = userId;
            }

            if (!string.IsNullOrWhiteSpace(roleClaim))
            {
                context.Items["Role"] = roleClaim;
            }
        }

        await _next(context);
    }
}
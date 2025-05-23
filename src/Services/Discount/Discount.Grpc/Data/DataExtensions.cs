using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public static class DataExtensions
{
    public static async Task<IApplicationBuilder> UseMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DiscountContext>();
        await context.Database.MigrateAsync();
        return app;
    }
}
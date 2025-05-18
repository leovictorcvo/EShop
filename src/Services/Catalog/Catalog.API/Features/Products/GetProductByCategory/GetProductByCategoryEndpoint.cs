namespace Catalog.API.Features.Products.GetProductByCategory;

internal record GetProductByCategoryResponse(IEnumerable<Product> Products);

public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByCategoryQuery(category));
            return Results.Ok(result.Adapt<GetProductByCategoryResponse>());
        })
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithName("GetProductByCategory")
            .WithTags("Products")
            .WithSummary("Gets products by category")
            .WithDescription("Gets products by category from the catalog.");
    }
}
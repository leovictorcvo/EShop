namespace Catalog.API.Features.Products.GetProducts;

internal record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);
internal record GetProductsResponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetProductsQuery>();
            var result = await sender.Send(query);
            var response = result.Adapt<GetProductsResponse>();
            return Results.Ok(response);
        })
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithName("GetProducts")
            .WithTags("Products")
            .WithSummary("Gets all products")
            .WithDescription("Gets all products from the catalog.");
    }
}
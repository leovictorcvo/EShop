namespace Catalog.API.Features.Products.GetProductById;

internal record GetProductByIdResponse(Product Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByIdQuery(id));
            return Results.Ok(result.Adapt<GetProductByIdResponse>());
        })
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithName("GetProductById")
            .WithTags("Products")
            .WithSummary("Gets a product by ID")
            .WithDescription("Gets a product by ID from the catalog.");
    }
}
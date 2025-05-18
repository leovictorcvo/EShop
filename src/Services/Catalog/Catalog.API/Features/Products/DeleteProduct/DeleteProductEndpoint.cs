namespace Catalog.API.Features.Products.DeleteProduct;

internal record DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(id));
            return Results.Ok(result.Adapt<DeleteProductResponse>());
        })
            .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithName("DeleteProduct")
            .WithTags("Products")
            .WithSummary("Deletes a product")
            .WithDescription("Deletes a product from the catalog.");
    }
}
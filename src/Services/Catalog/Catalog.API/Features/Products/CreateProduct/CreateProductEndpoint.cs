namespace Catalog.API.Features.Products.CreateProduct;

internal record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);

internal record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateProductCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateProductResponse>();
            return Results.Created($"/products/{response.Id}", response);
        })
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithName("CreateProduct")
            .WithTags("Products")
            .WithSummary("Creates a new product")
            .WithDescription("Creates a new product in the catalog.");
    }
}
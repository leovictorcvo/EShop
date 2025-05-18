namespace Catalog.API.Features.Products.GetProductById;

internal record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

internal record GetProductByIdResult(Product Product);

internal class GetProductByIdQueryHandler(IDocumentSession session) :
    IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(query.Id, cancellationToken) ??
            throw new ProductNotFoundException(query.Id);

        return new GetProductByIdResult(product);
    }
}
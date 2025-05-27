namespace Ordering.Application.Orders.Queries.GetOrdersByName;

public class GetOrdersByNameQueryHandler(IApplicationDbContext context) : IQueryHandler<GetOrdersByNameQuery, GetOrderByNameResult>
{
    public async Task<GetOrderByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
    {
        var orders = await context.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .Where(o => o.OrderName.Value.Contains(query.Name, StringComparison.OrdinalIgnoreCase))
            .OrderBy(o => o.OrderName.Value)
            .ToListAsync(cancellationToken);

        return new GetOrderByNameResult(orders.ProjectToOrderDto());
    }
}
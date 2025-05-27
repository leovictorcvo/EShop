namespace Ordering.Application.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler(IApplicationDbContext context) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var pageIndex = request.PaginationRequest.PageIndex - 1;
        var pageSize = request.PaginationRequest.PageSize;
        var totalCount = await context.Orders.LongCountAsync(cancellationToken);

        var orders = await context.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .OrderBy(o => o.OrderName.Value)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var result = new PaginationResult<OrderDto>(pageIndex + 1, pageSize, totalCount, orders.ProjectToOrderDto());
        return new GetOrdersResult(result);
    }
}
using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger) :
    DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName, context.CancellationToken) ??
            new Coupon
            {
                ProductName = request.ProductName,
                Description = "No Discount",
                Amount = 0
            };

        logger.LogDebug("Discount retrieved for product with name {ProductName} = {Amount}", coupon.ProductName, coupon.Amount);
        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>() ??
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogDebug("Coupon for product {ProductName} successfully created.", coupon.ProductName);
        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>() ??
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogDebug("Coupon for product {ProductName} successfully updated.", coupon.ProductName);
        return coupon.Adapt<CouponModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName) ??
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount for {request.ProductName} not found"));

        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogDebug("Coupon for product {ProductName} successfully deleted.", coupon.ProductName);
        return new DeleteDiscountResponse { Success = true };
    }
}
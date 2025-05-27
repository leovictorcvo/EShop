namespace Ordering.Application.Orders.Commands.DeleteOrder;
public record DeleteOrderCommand(Guid OrderId) : ICommand<DeleteOrderResult>;

public record DeleteOrderResult(bool IsSuccess);

public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order ID must not be empty.")
            .NotEqual(Guid.Empty).WithMessage("Order ID must not be an empty GUID.");
    }
}
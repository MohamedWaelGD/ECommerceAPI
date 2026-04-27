using FluentValidation;

namespace ECommerceAPI.Application.Features.Cart.Commands.UpdateCartItem;

public class UpdateCartItemCommandValidator : AbstractValidator<UpdateCartItemCommand>
{
    public UpdateCartItemCommandValidator() => RuleFor(x => x.Quantity).GreaterThan(0);
}

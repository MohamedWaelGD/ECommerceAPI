using FluentValidation;

namespace ECommerceAPI.Application.Features.Cart.Commands.AddCartItem;

public class AddCartItemCommandValidator : AbstractValidator<AddCartItemCommand>
{
    public AddCartItemCommandValidator() => RuleFor(x => x.Quantity).GreaterThan(0);
}

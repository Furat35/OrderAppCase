using FluentValidation;
using Ordering.Application.Models.Dtos.Orders;

namespace Ordering.Application.Validations
{
    public class OrderCreateDtoValidator : AbstractValidator<OrderCreateDto>
    {
        public OrderCreateDtoValidator()
        {
            RuleFor(_ => _.CustomerId)
                .NotEmpty()
                .WithMessage("Customer id is required!");

            RuleFor(_ => _.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0");

            RuleFor(_ => _.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0");

            RuleFor(_ => _.Product)
                .NotEmpty()
                .WithMessage("Product is required!");
        }
    }
}

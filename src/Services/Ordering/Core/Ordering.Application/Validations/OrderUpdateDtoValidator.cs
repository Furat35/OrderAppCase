using FluentValidation;
using Ordering.Application.Models.Dtos.Orders;

namespace Ordering.Application.Validations
{
    public class OrderUpdateDtoValidator : AbstractValidator<OrderUpdateDto>
    {
        public OrderUpdateDtoValidator()
        {
            RuleFor(_ => _.Id)
             .NotEmpty()
             .WithMessage("Id is required!")
             .NotEmpty()
             .WithMessage("Id is required!");

            RuleFor(_ => _.Address)
                .NotEmpty()
                .WithMessage("Address is required!")
                .NotEmpty()
                .WithMessage("Address is required!");
        }
    }
}

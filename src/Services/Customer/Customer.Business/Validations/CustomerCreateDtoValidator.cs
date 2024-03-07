using Customer.Business.Models.Dtos.Customer;
using FluentValidation;

namespace Customer.Business.Validations
{
    public class CustomerCreateDtoValidator : AbstractValidator<CustomerCreateDto>
    {
        public CustomerCreateDtoValidator()
        {
            RuleFor(c => c.Name)
                .NotNull()
                .WithMessage("Name can't be empty!")
                .NotEmpty()
                .WithMessage("Name can't be empty!");

            RuleFor(c => c.Email)
                .NotNull()
                .WithMessage("Email can't be empty!")
                .NotEmpty()
                .WithMessage("Email can't be empty!");

            RuleFor(c => c.Address)
                .NotNull()
                .WithMessage("Address can't be empty!")
                .NotEmpty()
                .WithMessage("Address can't be empty!");
        }
    }
}

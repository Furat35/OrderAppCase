using Customer.Business.Models.Dtos.Customer;
using FluentValidation;

namespace Customer.Business.Validations
{
    public class CustomerUpdateDtoValidator : AbstractValidator<CustomerUpdateDto>
    {
        public CustomerUpdateDtoValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("Id can't be empty!");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Name can't be empty!")
                .MaximumLength(50)
                .WithMessage("Name can't contain more than 50 charachters");

            RuleFor(c => c.Email)
                .NotEmpty()
                .WithMessage("Email can't be empty!")
                .MaximumLength(100)
                .WithMessage("Email can't contain more than 50 charachters");

            RuleFor(c => c.Address)
                .NotEmpty()
                .WithMessage("Address can't be empty!");
        }
    }
}

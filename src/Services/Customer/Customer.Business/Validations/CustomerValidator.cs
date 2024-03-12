﻿using FluentValidation;
using Entities = Customer.Entity.Entities;

namespace Customer.Business.Validations
{
    public class CustomerValidator : AbstractValidator<Entities.Customer>
    {
        public CustomerValidator()
        {
            RuleFor(_ => _.Id)
                .NotNull()
                .WithMessage("Id can't be empty!")
                .NotEmpty()
                .WithMessage("Id can't be empty!");

            RuleFor(c => c.Name)
                .NotNull()
                .WithMessage("Name can't be empty!")
                .NotEmpty()
                .WithMessage("Name can't be empty!")
                .MaximumLength(50)
                .WithMessage("Name can't contain more than 50 charachters");

            RuleFor(c => c.Email)
                .NotNull()
                .WithMessage("Email can't be empty!")
                .NotEmpty()
                .WithMessage("Email can't be empty!")
                .MaximumLength(100)
                .WithMessage("Email can't contain more than 50 charachters");

            RuleFor(c => c.Address)
                .NotNull()
                .WithMessage("Address can't be empty!")
                .NotEmpty()
                .WithMessage("Address can't be empty!");
        }
    }
}

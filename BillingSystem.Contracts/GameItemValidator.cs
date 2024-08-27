using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Contracts
{
    public class GameItemValidator : AbstractValidator<GameItem>
    {
        public GameItemValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");

            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Please enter sufficient price");

            RuleFor(x => x.Price).NotNull().WithMessage("Price is required");

            RuleFor(x => x.Quantity).NotEmpty().WithMessage("Quantity is required");

        }
    }
}

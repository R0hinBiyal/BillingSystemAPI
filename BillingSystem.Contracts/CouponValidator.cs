using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Contracts
{
    public class CouponValidator : AbstractValidator<CouponDto>
    {
        public CouponValidator()
        {
            RuleFor(c => c.CouponCode)
                .NotEmpty().WithMessage("Coupon code is required.")
                .MaximumLength(10).WithMessage("Coupon code must be less than 10 characters.");

            RuleFor(c => c.Type)
                .NotEmpty().WithMessage("Type is required")
                .Must(type => type == "percentage" || type == "value").WithMessage("Type must be 'percentage' or 'value'");

            RuleFor(c => c.DiscountValue)
                .GreaterThan(0).WithMessage("Discount value must be greater than zero");

            RuleFor(c => c.ExpiryDate)
                .NotEmpty().WithMessage("Expiry date must be provided");

            RuleFor(c => c.ExpiryDate)
                .GreaterThan(DateTime.Now).WithMessage("Expiry date must be in the future");        
            
        }
    }
}

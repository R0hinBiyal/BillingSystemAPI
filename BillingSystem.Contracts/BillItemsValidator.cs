using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Contracts
{
    public class BillItemsValidator :AbstractValidator<CreateBillDto>
    {
        public BillItemsValidator()
        {
            RuleFor(x => x.Items).NotEmpty().NotNull().WithMessage("Items are Required");

           
        }
    }
}

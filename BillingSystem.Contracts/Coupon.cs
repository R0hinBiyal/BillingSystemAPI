using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Contracts
{
    public class Coupon
    {
        [Key]        
        public Guid id { get; set; } 

        public string CouponCode { get; set; }

        public string Type { get; set; }  


        public double DiscountValue { get; set; } 


        public DateTime ExpiryDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Contracts
{
    public class UpdateItem
    {
        public string? Title { get; set; } = null;

        public int? Quantity { get; set; } = null;
        public double? Price { get; set; } = null;
    }
    //Req items
    public class ItemDto
    {
        public Guid GameId { get; set; }
        public int Quantity { get; set; } = 1;
    } 
    //Resp items
    public class BillItemDto
    {
        public Guid GameId { get; set; }
        public int Quantity { get; set; } 
        public double Price { get; set; }
    }
    //Req
    public class CreateBillDto
    {
        public List<ItemDto> Items { get; set; }
        public Boolean? Type { get; set; }
        public double? Discount { get; set; }

    }

    public class BillResponseDto
    {
        public List<BillItemDto> Items { get; set; }
        public double OriginalPrice { get; set; }
        public double DiscountedPrice { get; set; }
        public double? Discount { get; set; } 
    }

    public class CouponDto
    {
        public string CouponCode { get; set; }
        public string Type { get; set; }
        public double DiscountValue { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class CheckCoupon
    { 
        public string CouponCode { get; set;}
        
        public double Total {  get; set; }  

    }

    public class CouponResponse
    {
        public double OriginalPrice { get; set; }
        public double DiscountedPrice
        {
            get; set;

        }
    }
}

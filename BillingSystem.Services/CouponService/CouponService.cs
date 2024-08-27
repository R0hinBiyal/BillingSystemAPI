using BillingSystem.Contracts;
using BillingSystem.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Services.CouponService
{
    public class CouponService : ICouponService
    {
        private readonly BillingSystemDbContext _context;

        public CouponService(BillingSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Coupon> CreateCoupon(CouponDto couponDto)
        {
            var coupon = new Coupon
            {
                id = Guid.NewGuid(),
                CouponCode = couponDto.CouponCode,
                Type = couponDto.Type,
                DiscountValue = couponDto.DiscountValue,
                ExpiryDate = couponDto.ExpiryDate
            };

            _context.Coupon.Add(coupon);
            await _context.SaveChangesAsync();

            return coupon;
        }

        public async Task<CouponResponse> ApplyCoupon(CheckCoupon checkCoupon)
        {
            if (string.IsNullOrWhiteSpace(checkCoupon.CouponCode))
            {
                throw new ArgumentNullException("Coupon code is required");
            }
            var coupon = await _context.Coupon.FirstOrDefaultAsync(x => x.CouponCode.Equals(checkCoupon.CouponCode));

            if (coupon == null)
            {
                throw new ArgumentException("This coupon is invalid");
            }

            if (coupon.ExpiryDate < DateTime.Now)
            {
                throw new InvalidOperationException("This coupon has expired");
            }
            double discountedTotal = checkCoupon.Total;

            if(coupon.Type== "percentage")
            {
                double discountedAmount = checkCoupon.Total * (coupon.DiscountValue / 100);
                discountedTotal -=  discountedAmount;
            }
            else if(coupon.Type=="value")
            {
                discountedTotal = coupon.DiscountValue >= checkCoupon.Total ? checkCoupon.Total: discountedTotal - coupon.DiscountValue;
                 
            }
            return new CouponResponse
            {
                OriginalPrice = checkCoupon.Total,
                DiscountedPrice = discountedTotal
            };
        }


    }
}

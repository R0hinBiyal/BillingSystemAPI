using BillingSystem.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Services.CouponService
{
    public interface ICouponService
    {
        Task<Coupon> CreateCoupon(CouponDto couponDto);

        Task<CouponResponse> ApplyCoupon(CheckCoupon checkCoupon);


    }
}

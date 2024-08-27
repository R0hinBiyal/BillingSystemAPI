using BillingSystem.Contracts;
using BillingSystem.Db;
using BillingSystem.Services.CouponService;
using BillingSystem.Services.GameItemService;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BillingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly BillingSystemDbContext _billingSystemDbContext;
        private readonly ILogger<ItemController> _logger;
        private readonly IGameItemService _gameItemService;
        private readonly ICouponService _couponService;
        private GameItemValidator gameItemValidator = new GameItemValidator();
        private CouponValidator  couponValidator = new CouponValidator();
        private BillItemsValidator billItemValidator = new BillItemsValidator();



        public ItemController(BillingSystemDbContext billingSystemDbContext, ILogger<ItemController> logger, IGameItemService gameItemService,ICouponService couponService)
        {
            this._billingSystemDbContext = billingSystemDbContext;
            this._logger = logger;
            this._gameItemService = gameItemService;
            this._couponService = couponService;
        }

        [HttpGet]
        public async Task<ApiResponse<List<GameItem>>> GetItems()
        {
            try
            {
                var DataItems = await _gameItemService.GetItems();

                if (DataItems.Count==0) {
                    return new ApiResponse<List<GameItem>>(DataItems, "No Data found");
                }

                return new ApiResponse<List<GameItem>>(DataItems, "Items retrived successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<GameItem>>(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddItem")]
        public async Task<ApiResponse<GameItem>> AddItem([FromBody] GameItem gameItem)
        {

            try
            {
                var result = gameItemValidator.Validate(gameItem);

                if (!result.IsValid)
                {
                    var errorList = result.Errors
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var apiResponse = new ApiResponse<GameItem>(errorList);

                    return apiResponse;
                }


                var data = await _gameItemService.AddItem(gameItem);

                if (data == null)
                {
                    return new ApiResponse<GameItem>("Item Already Exist");
                }

                return new ApiResponse<GameItem>(data, "Game Added Successfully");


            }
            catch (Exception ex)
            {

                return new ApiResponse<GameItem>(ex.Message);


            }

        }
        [HttpPut]
        [Route("{gameId}")]
        public async Task<ApiResponse<GameItem>> UpdateItem([FromRoute] Guid gameId,[FromBody] UpdateItem updateItem)
        {
            try
            {
                var updatedItem = await _gameItemService.UpdateItem(gameId, updateItem.Title, updateItem.Price, updateItem.Quantity );

                return new ApiResponse<GameItem>(updatedItem, "Item updated successfully");

            }
            catch(Exception ex)
            {
                return new ApiResponse<GameItem>(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ApiResponse<GameItem>> DeleteItem(Guid id)
        {
            try
            {
                if (id == null)
                {
                    return new ApiResponse<GameItem>("Id is required");
                }

                var item =await  _gameItemService.RemoveItem(id);

                if(item == null)
                {
                    return new ApiResponse<GameItem>("Id doesnot exist");
                }
                return new ApiResponse<GameItem>(item, "Item has been removed");
            }
            catch(Exception ex)
            {
                return new ApiResponse<GameItem>(ex.Message); 
            }

        }

        [HttpPost]
        [Route("CreateBill")]
        public async Task <ApiResponse<BillResponseDto>> CreateBill([FromBody]CreateBillDto billDto)
        {
            try
            {
                var result = billItemValidator.Validate(billDto);

                if (!result.IsValid)
                {
                return new ApiResponse<BillResponseDto>("Bill data is invalid", HttpStatusCode.BadRequest);
                }

                var billResponse = await _gameItemService.CalculateBill(billDto);
                return new ApiResponse<BillResponseDto>(billResponse, "Bill generated successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<BillResponseDto>(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route ("CreateCoupon")]

        public async Task<ApiResponse<Coupon>> CreateCoupon([FromBody] CouponDto couponDto)
        {
            try
            {
                if (couponDto == null)
                {
                    return new ApiResponse<Coupon>("Coupon data is required");
                }
                var validationResult = couponValidator.Validate(couponDto);

                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return new ApiResponse<Coupon>(errorList, HttpStatusCode.BadRequest);
                }

               

                var Coupon= await _couponService.CreateCoupon(couponDto);
                
                if(Coupon == null)
                {
                    return new ApiResponse<Coupon>("Something went wrong");
                }
                return new ApiResponse<Coupon>(Coupon,"Coupon created successfully");
            }
            catch(Exception ex)
            {
                return new ApiResponse<Coupon>(ex.Message, HttpStatusCode.InternalServerError);
                
            }

        }
      
        [HttpPost]
        [Route("ApplyCoupon")]
        public async Task<ApiResponse<CouponResponse>> ApplyCoupon([FromBody] CheckCoupon checkCoupon)
        {
            try
            {
                var response = await _couponService.ApplyCoupon(checkCoupon);

                if(checkCoupon.Total== response.OriginalPrice)
                {
                    return new ApiResponse<CouponResponse>(response, "Coupon not applied: Total Price should be more than the coupon value ");
                }
                return new ApiResponse<CouponResponse>(response,"Coupon applied successfully");
            }
            catch (ArgumentNullException ex)
            {
                return new ApiResponse<CouponResponse>(ex.Message, HttpStatusCode.BadRequest);
            }
            catch (ArgumentException ex)
            {
                return new ApiResponse<CouponResponse>(ex.Message, HttpStatusCode.BadRequest);
            }
            catch (InvalidOperationException ex)
            {
                return new ApiResponse<CouponResponse>(ex.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CouponResponse>(ex.Message, HttpStatusCode.InternalServerError);
            }
        }



    }
}

using BillingSystem.Contracts;
using BillingSystem.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BillingSystem.Services.GameItemService
{
    public class GameItemService : IGameItemService
    {
        public readonly BillingSystemDbContext _context;

        public GameItemService(BillingSystemDbContext context)
        {
            this._context = context;
        }


        public async Task<List<GameItem>> GetItems()
        {
            var Items = await _context.GameItems.ToListAsync();
            return Items;

        }

        public async Task<GameItem> AddItem(GameItem gameItem)
        {
            if (gameItem == null)
            {
                return null;
            }
            bool ItemExist = await _context.GameItems.AnyAsync(item => item.Title == gameItem.Title);

            if (ItemExist)
            {
                return null;
            }

            gameItem.GameId = Guid.NewGuid();
            _context.GameItems.Add(gameItem);
            await _context.SaveChangesAsync();
            return gameItem;

        }



        public async Task<GameItem> RemoveItem(Guid gameId)
        {
            var item = await _context.GameItems.FindAsync(gameId);

            if (item != null)
            {
                _context.GameItems.Remove(item);
                await _context.SaveChangesAsync();

                return item;
            }

            return null;
        }


        public async Task<GameItem> UpdateItem(Guid gameId,string? Title, double? Price, int? Quantity )
        {
            var item = await _context.GameItems.FirstOrDefaultAsync(x => x.GameId == gameId);

            if (item != null)
            {
                item.Price = (double)(Price != null ? Price : item.Price);
                item.Title = Title != null ? Title : item.Title;
                item.Quantity =(int)(Quantity != null ? Quantity : item.Quantity);

                _context.GameItems.Update(item);
                await _context.SaveChangesAsync(true);

                return item;

            }
            throw new Exception("Item not found");
        }
        public async Task<BillResponseDto> CalculateBill(CreateBillDto billDto)
        {
            
            var billResponse = new BillResponseDto()
            {
                Items = new List<BillItemDto>()
            };
            
            double grandTotal = 0;
            var errorList = new List<string>();

            foreach (var item in billDto.Items)
            {
                var gameItem= await _context.GameItems.FindAsync(item.GameId);
            
                if (gameItem != null)
                {
                    if (gameItem.Quantity < item.Quantity)
                    {
                        if (gameItem.Quantity == 0)
                        {
                            throw new Exception("Unavailable");
                        }
                        throw new Exception($"Not much stock available, Available Quantity: {gameItem.Quantity}");
                    }
                    else
                    {

                        gameItem.Quantity = gameItem.Quantity - item.Quantity;
                        await _context.SaveChangesAsync();
                    }
                    item.Quantity = item.Quantity==0 ? 1:item.Quantity;
                double itemTotalPrice= gameItem.Price * item.Quantity;
                     
                billResponse.Items.Add(new BillItemDto
                {
                    GameId = item.GameId,
                    Quantity = item.Quantity,
                    Price= itemTotalPrice
                }
                );

                    grandTotal += itemTotalPrice;
                }
                else
                {
                    throw new Exception($"{item.GameId} not a valid id");   
                }
            }

            if (errorList.Count>0)
            {
                throw new Exception(string.Join(", ", errorList));
            }

            double discountedPrice = grandTotal;

            if(billDto.Discount.HasValue && billDto.Discount.Value > 0)
            {
                discountedPrice = GetDiscountPrice(billDto.Type.Value, billDto.Discount.Value, grandTotal);

            }

            billResponse.OriginalPrice = grandTotal;
            billResponse.DiscountedPrice = discountedPrice;
            billResponse.Discount = billDto.Discount.Value;

            return billResponse;

        }

        
        private double GetDiscountPrice(Boolean type,double value, double grandTotal)
        {   double discountedPrice = 0;
            if (type)
            {
              if(value>100)
                {
                    throw new Exception("Not a valid discount");
                }
                else {
                        double discountedAmount = grandTotal * (value / 100);
                        discountedPrice = discountedPrice - discountedAmount;
                        return discountedPrice;
                }
            }
            else
            {

                grandTotal=value>=grandTotal?0:grandTotal-value; 
                return grandTotal;
            }
        }

    }
}

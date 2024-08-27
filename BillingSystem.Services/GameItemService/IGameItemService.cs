using BillingSystem.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Services.GameItemService
{
    public interface IGameItemService
    {
        Task<List<GameItem>> GetItems();
        Task<GameItem> AddItem(GameItem gameItem);

        Task <GameItem> RemoveItem(Guid gameId);

        Task<GameItem> UpdateItem(Guid gameId, string? Title, double? Price, int? Quantity);

        Task<BillResponseDto> CalculateBill(CreateBillDto billDto);


        }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillingSystem.Contracts
    {
    public class GameItem
    {
        [Key]
        public Guid GameId { get; set; }
        public string Title { get; set; }

        public int Quantity { get; set; }  

            public double Price { get; set; }

     
        }
    }
    
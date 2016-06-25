using System;
using System.ComponentModel.DataAnnotations;

namespace ApiAuctionShop.Models
{
    public class TodoItem
    {
        public bool Done { get; set; }
        [Key]
        public string ID { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiAuctionShop.Models
{
    public class MobileAuctionSender
    {
        public int ID { get; set; }
        public int duration { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }

        public string description { get; set; }

        public int price { get; set; }

        public string title { get; set; }

        public DateTime addedAuctionTime { get; set; }
    }
}
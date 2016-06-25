using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAuctionShop.Models
{
    public class Signup : IdentityUser
    {
       // [Key]
       // public string ID { get; set; }
        public override string Email { get; set; }
        public string ExpireTokenTime { get; set; }
        public bool IsTokenConfirmed { get; set; }
        public List<Auctions> Auction { get; set; }
    }

    public class Auctions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int duration { get; set; }

        public byte[] image { get; set; }

        public string description { get; set; }

        public int price { get; set; }

        public string title { get; set; }

    }
}
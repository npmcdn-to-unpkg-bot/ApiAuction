using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAuctionShop.Models
{
    public class Signup : IdentityUser
    {
        public override string Email { get; set; }
        public string ExpireTokenTime { get; set; }
        public bool IsTokenConfirmed { get; set; }
        public string Token { get; set; }
        //musi być zadeklarowana bo inaczej Auction traktuje jak zwykły object = null
        public ICollection<Auctions> Auction { get; set; } = new List<Auctions>(); 
    }

    public class Auctions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int duration { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }

        public string description { get; set; }

        public int price { get; set; }

        public string title { get; set; }

        public Signup Signup { get; set; }

        [Column("SignupId")]
        public string SignupId { get; set; }
    }
}
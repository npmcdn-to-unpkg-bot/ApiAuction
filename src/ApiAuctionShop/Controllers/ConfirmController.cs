using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using ApiAuctionShop.Models;
using Microsoft.AspNet.Authorization;
using ApiAuctionShop.Attributes;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Http;
using System.Text;
using ApiAuctionShop.Database;
using ApiAuctionShop.Helpers;

namespace Projekt.Controllers
{
    public class ConfirmController : Controller
    {
        public ApplicationDbContext context;
        public ConfirmController(ApplicationDbContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public string Get(string id)
        {
            var decryptedstring = Encoding.Default.GetString(Convert.FromBase64String(id));

            string[] decryptedstring2 = StringCipher.Decrypt(decryptedstring, Settings.HashPassword);

            var user = context.Logins.Where(s => s.Email == decryptedstring2[0]);

            user.First().IsTokenConfirmed = true;
            user.First().ExpireTokenTime = decryptedstring2[1];

            context.SaveChanges();

            return "Token added " + decryptedstring2[0];
        }
    }
}
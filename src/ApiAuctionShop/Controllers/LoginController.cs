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
using System.Text;
using ApiAuctionShop.Database;
using ApiAuctionShop.Helpers;

namespace Projekt.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        public ApplicationDbContext context;
        public LoginController(ApplicationDbContext _context)
        {
            context = _context;
        }

        [HttpPost]
        public ObjectResult Post([FromBody] Signup value)
        {
            var encrypt = StringCipher.Encrypt(value.Email, Settings.HashPassword);
            var encrypt2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(encrypt));

            var myEmails = context.Logins.Where(s => s.Email == value.Email);

            if (myEmails.Any())
            {
                myEmails.First().IsTokenConfirmed = false;
                context.SaveChanges();
                EmailSender.SendEmailAsync(value.Email, "Token", "http://projektgrupowy.azurewebsites.net/Confirm/Get/" + encrypt2);
            }
            else
            {
                value.Email = value.Email;
                value.IsTokenConfirmed = false;
                value.ExpireTokenTime = null;
                value.Id = context.Logins.Max(record => record.Id) + 1;
                context.Logins.Add(value);
                context.SaveChanges();
                EmailSender.SendEmailAsync(value.Email, "Token", "http://projektgrupowy.azurewebsites.net/Confirm/Get/" + encrypt2);

            }
            return new HttpOkObjectResult(encrypt);
        }

    }
}
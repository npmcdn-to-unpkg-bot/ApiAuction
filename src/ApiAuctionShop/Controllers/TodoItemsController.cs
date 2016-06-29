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
using ApiAuctionShop.Database;

namespace Projekt.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [LoginAuthentication]
    public class TodoItemsController : Controller
    {
        public ApplicationDbContext context;
        public TodoItemsController(ApplicationDbContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public async Task<ObjectResult> Get()
        {
            var list = context.Auctions.ToList();

            var listfixed = new List<MobileAuctionSender>();

            foreach (var el in list)
            {
                MobileAuctionSender dynamic = new MobileAuctionSender
                {
                    description = el.description,
                    duration = el.duration,
                    ID = el.ID,
                    ImageData = el.ImageData,
                    ImageMimeType = el.ImageMimeType,
                    price = el.price,
                    title = el.title
                };
                listfixed.Add(dynamic);
            }

            return new HttpOkObjectResult(listfixed);
        }

        
        //wyciagnac maila i dodac 
        [HttpPost]
        public ObjectResult Post([FromBody] MobileAuctionSender value)
        {
            /**

            tutaj wycagnac jeszcze maila trzeba 
            var user = _userManager.Users.First(d => d.Email == .... );

            user.Auction.Add(_auction);

            await _userManager.UpdateAsync(user);
            **/
            var _auction = new Auctions()
            {
                title = value.title,
                description = value.description,
                duration = value.duration,
                price = value.price,
                ImageData = value.ImageData
            };

            context.Auctions.Add(_auction);
            context.SaveChanges();

            return new HttpOkObjectResult(value);
        }


    }
}
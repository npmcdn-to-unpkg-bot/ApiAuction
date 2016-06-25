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
            var list = context.TodoItems.ToList();
           return new HttpOkObjectResult(list);
        }

        [HttpPost]
        public ObjectResult Post([FromBody] TodoItem value)
        {
            context.TodoItems.Add(value);
            context.SaveChanges();
            return new HttpOkObjectResult(value);
        }


    }
}
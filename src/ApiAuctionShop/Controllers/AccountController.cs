using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authorization;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.Http;
using System.IO;
using Microsoft.AspNet.Identity.EntityFramework;
using ImageProcessor.Imaging;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using System.Drawing;
using ApiAuctionShop.Models;
using ApiAuctionShop.Helpers;
using ApiAuctionShop.Database;

namespace Projekt.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<Signup> _userManager;
        private readonly SignInManager<Signup> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ApplicationDbContext _context;

        public AccountController(
            UserManager<Signup> userManager,
            SignInManager<Signup> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }



        [AllowAnonymous]
        public async Task<IActionResult> Urllogin(string id)
        {
            string[] decryptedstring = StringCipher.Decrypt(id, Settings.HashPassword);
            if (!(decryptedstring[0] == ""))
            {
                var result = await _signInManager.PasswordSignInAsync(decryptedstring[0], decryptedstring[0] + "0D?", isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Login", "Account");
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Signup model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                string encryptedstring = StringCipher.Encrypt(model.Email, Settings.HashPassword);
                await EmailSender.SendEmailAsync(model.Email, "URL do zalogowania", "http://localhost:5000/Account/Urllogin/" + encryptedstring);
                ModelState.AddModelError(string.Empty, "Wyslane");

                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Signup model)
        {
            if (ModelState.IsValid)
            {
                var user = new Signup { UserName = model.Email, Email = model.Email, Auction = new List<Auctions>() { new Auctions() {description = "TEST" }}};
                              
                var result = await _userManager.CreateAsync(user, model.Email + "0D?");
                
                //////////////////TEST ROLES//////////////////////////////////////////
                string name = "User";
                bool roleExist = await _roleManager.RoleExistsAsync(name);
                if (!roleExist)
                {
                    var roleresult = _roleManager.CreateAsync(new IdentityRole(name));
                }
                await _userManager.AddToRoleAsync(user, name);
                //////////////////////////////////////////////////////////////////////

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        //////////////////TEST /////////////////////////
        public IActionResult Image()
        {
            return View();
        }

        [HttpPost]

        public async Task<ActionResult> Image(Auctions auction, IFormFile file = null)
        {
            if (file != null)
            {
                    if(file.ContentType.Contains("image"))
                    { 
                    using (var fileStream = file.OpenReadStream())
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var imageFactory = new ImageFactory())
                            {
                                imageFactory.FixGamma = false;
                                imageFactory.Load(fileStream).Resize(new ResizeLayer(new Size(100, 100)))
                                .Format(new JpegFormat
                                {
                                    Quality = 50
                                })
                                .Quality(50)
                                .Save(ms);
                            }

                            var fileBytes = ms.ToArray();
                            //var imageBase64 = Convert.ToByte(fileBytes);

                            var _auction = new Auctions()
                            {
                                title = auction.title,
                                description = auction.description,
                                duration = auction.duration,
                                price = auction.price,
                                image = fileBytes
                            };

                            var user = await GetCurrentUserAsync();
                            user.Auction.Add(_auction); // sypie
                            


                            var result = await _userManager.UpdateAsync(user);

                            if (result.Succeeded)
                            {
                                return RedirectToAction("Index", "Home");
                            }

                        }
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
        //////////////////////////////////////////////

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        private async Task<Signup> GetCurrentUserAsync()
        {
            return await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
        }
    }

}

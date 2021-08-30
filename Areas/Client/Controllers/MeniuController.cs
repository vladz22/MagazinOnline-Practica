using MagazinOnline.DataAcces.Repository.IRepository;
using MagazinOnline.Modele;
using MagazinOnline.Modele.ViewModels;
using MagazinOnline.Utilitate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MagazinOnline.Areas.Client.Controllers
{
    [Area("Client")]
    public class MeniuController : Controller
    {
        private readonly ILogger<MeniuController> _logger;
        private readonly IUnitOfWork _unit;

        public MeniuController(ILogger<MeniuController> logger,IUnitOfWork unit)
        {
            _logger = logger;
            _unit = unit;
        }
        public IActionResult Details(int id)
        {
            var produsfromdb = _unit.Produs.GetFirstOrDefault(u => u.Id == id,includeProperties:"Categorie");
            ShoppingCart cartobj = new ShoppingCart()
            {
                Produs=produsfromdb,
                ProdusId=produsfromdb.Id
            };
            return View(cartobj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart objcart)
        {
            objcart.Id = 0;
            if(ModelState.IsValid)
            {
                //adaugam la cart
                //cautam id-ul userului logat 
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                objcart.ApplicationUserId = claim.Value;
                //aducem obiectul din baza de date
                ShoppingCart cartfromdb = _unit.ShoppingCart.GetFirstOrDefault(
                    u=>u.ApplicationUserId==objcart.ApplicationUserId && u.ProdusId==objcart.ProdusId,
                    includeProperties:"Produs"
                    );
                if(cartfromdb==null)
                {
                    //nu exista records in baza de date pentru produsul respectiv pentru userul respectiv
                    _unit.ShoppingCart.Adaugare(objcart);
                }
                else
                {
                    cartfromdb.Count += objcart.Count;
                    _unit.ShoppingCart.Update(cartfromdb);//merge si fara asta
                   
                }
                _unit.Save();
                var count = _unit.ShoppingCart.GetAll(a => a.ApplicationUserId == objcart.ApplicationUserId)
                    .ToList().Count();
                //HttpContext.Session.SetObject(StaticDetalii.ssShoppingCart,objcart);//metoda pentru a stoca obiectul in session
                //var obj = HttpContext.Session.GetObject<ShoppingCart>(StaticDetalii.ssShoppingCart); 
                HttpContext.Session.SetInt32(StaticDetalii.ssShoppingCart, count);//adaugam var count la session
                return RedirectToAction("Calculator");
            }
            else
            {
                var produsfromdb = _unit.Produs.GetFirstOrDefault(u => u.Id == objcart.Id, includeProperties: "Categorie");
                ShoppingCart cartobj = new ShoppingCart()
                {
                    Produs = produsfromdb,
                    ProdusId = produsfromdb.Id
                };
                return View(cartobj);
            }
           
        }
        public IActionResult Calculator()
        {
            IEnumerable<Produs> produseLista = _unit.Produs.GetAll(includeProperties: "Categorie");

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim!=null)//null daca user nu este logat
            {
                var count = _unit.ShoppingCart.GetAll(a => a.ApplicationUserId == claim.Value)
                    .ToList().Count();
                HttpContext.Session.SetInt32(StaticDetalii.ssShoppingCart, count);//adaugam var count la session
            }

            return View(produseLista);

        }
        public IActionResult PlacaVideo()
        {
            IEnumerable<Produs> produseLista = _unit.Produs.GetAll(includeProperties: "Categorie");

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)//daca este logat
            {
                var count = _unit.ShoppingCart.GetAll(a => a.ApplicationUserId == claim.Value)
                    .ToList().Count();
                HttpContext.Session.SetInt32(StaticDetalii.ssShoppingCart, count);//adaugam var count la session
            }
            return View(produseLista);

        }
        public IActionResult Telefon()
        {
            IEnumerable<Produs> produseLista = _unit.Produs.GetAll(includeProperties: "Categorie");

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)//null daca user nu este logat
            {
                var count = _unit.ShoppingCart.GetAll(a => a.ApplicationUserId == claim.Value)
                    .ToList().Count();
                HttpContext.Session.SetInt32(StaticDetalii.ssShoppingCart, count);//adaugam var count la session
            }
            return View(produseLista);

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

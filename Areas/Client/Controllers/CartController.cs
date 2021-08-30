using MagazinOnline.DataAcces.Repository.IRepository;
using MagazinOnline.Modele;
using MagazinOnline.Modele.ViewModels;
using MagazinOnline.Utilitate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MagazinOnline.Areas.Client.Controllers
{
    [Area("Client")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartVM shoppingCartVM { get; set; }//merge si cu [bindproperty]

        public CartController(IUnitOfWork unit, IEmailSender emailSender, UserManager<IdentityUser> userManager)
        {
            _unit = unit;
            _emailSender = emailSender;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new Modele.OrderHeader(),
                ListaCart = _unit.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Produs")
            };
            shoppingCartVM.OrderHeader.OrderTotal = 0;
            shoppingCartVM.OrderHeader.ApplicationUser = _unit.ApplicationUser.GetFirstOrDefault
                (u => u.Id == claim.Value);//includeProperties:"Companie"

            foreach (var lista in shoppingCartVM.ListaCart)
            {
                lista.Pret = StaticDetalii.PretInFunctieDeCantitate(lista.Count, lista.Produs.Pret, lista.Produs.Pret5);
                shoppingCartVM.OrderHeader.OrderTotal += (lista.Pret * lista.Count);
                lista.Produs.Descriere = StaticDetalii.ConvertToRawHtml(lista.Produs.Descriere);
                if (lista.Produs.Descriere.Length > 100)
                {
                    lista.Produs.Descriere = lista.Produs.Descriere.Substring(0, 99) + "...";
                }
            }
            return View(shoppingCartVM);
        }

        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> IndexPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _unit.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
            return RedirectToAction("Index");
        }

        public IActionResult Plus(int cartid)
        {
            var cart = _unit.ShoppingCart.GetFirstOrDefault(u => u.Id == cartid, includeProperties: "Produs");
            cart.Count++;
            _unit.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Minus(int cartid)
        {
            var cart = _unit.ShoppingCart.GetFirstOrDefault(u => u.Id == cartid, includeProperties: "Produs");

            if (cart.Count == 1)
            {
                int count = _unit.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
                _unit.ShoppingCart.Stergere(cart);
                _unit.Save();
                //dam update la session
                HttpContext.Session.SetInt32(StaticDetalii.ssShoppingCart, count - 1);
            }
            cart.Count--;
            _unit.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Sters(int cartid)
        {
            var cart = _unit.ShoppingCart.GetFirstOrDefault(u => u.Id == cartid, includeProperties: "Produs");

            int count = _unit.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
            _unit.ShoppingCart.Stergere(cart);
            _unit.Save();
            //dam update la session
            HttpContext.Session.SetInt32(StaticDetalii.ssShoppingCart, count - 1);

            return RedirectToAction("Index");
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new Modele.OrderHeader(),
                ListaCart = _unit.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Produs")
            };

            shoppingCartVM.OrderHeader.ApplicationUser = _unit.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
            foreach (var lista in shoppingCartVM.ListaCart)
            {
                lista.Pret = StaticDetalii.PretInFunctieDeCantitate(lista.Count, lista.Produs.Pret, lista.Produs.Pret5);
                shoppingCartVM.OrderHeader.OrderTotal += (lista.Pret * lista.Count);
            }
            shoppingCartVM.OrderHeader.Nume = shoppingCartVM.OrderHeader.ApplicationUser.Nume;
            shoppingCartVM.OrderHeader.Oras = shoppingCartVM.OrderHeader.ApplicationUser.Oras;
            shoppingCartVM.OrderHeader.Adresa = shoppingCartVM.OrderHeader.ApplicationUser.Adresa;
            shoppingCartVM.OrderHeader.Tara = shoppingCartVM.OrderHeader.ApplicationUser.Tara;
            shoppingCartVM.OrderHeader.CodPostal = shoppingCartVM.OrderHeader.ApplicationUser.CodPostal;

            return View(shoppingCartVM);
        }
        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPost(ShoppingCartVM shoppingCartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            
            shoppingCartVM.OrderHeader.ApplicationUser = _unit.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
            shoppingCartVM.ListaCart = _unit.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Produs");

            
            shoppingCartVM.OrderHeader.PaymentStatus = StaticDetalii.PaymentStatusPending;
            shoppingCartVM.OrderHeader.OrderStatus = StaticDetalii.StatusPending;
            shoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;
            shoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            _unit.OrderHeader.Adaugare(shoppingCartVM.OrderHeader);
            _unit.Save();

            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            foreach (var item in shoppingCartVM.ListaCart)
            {
                item.Pret = item.Produs.Pret;
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProdusId = item.ProdusId,
                    OrderId = shoppingCartVM.OrderHeader.Id,
                    Pret = item.Pret,
                    Count = item.Count
                };
                shoppingCartVM.OrderHeader.OrderTotal = orderDetails.Pret * orderDetails.Count;
                _unit.OrderDetails.Adaugare(orderDetails);
                _unit.Save();
            }
            _unit.ShoppingCart.StergereRange(shoppingCartVM.ListaCart);
            _unit.Save();
            HttpContext.Session.SetInt32(StaticDetalii.ssShoppingCart, 0);
            return RedirectToAction("OrderConfirm", "Cart", new { id = shoppingCartVM.OrderHeader.Id });
        }


        public IActionResult OrderConfirm(int id)
        {
            return View(id);
        }

    }
}

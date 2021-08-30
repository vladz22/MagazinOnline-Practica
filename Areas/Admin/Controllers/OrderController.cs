using MagazinOnline.DataAcces.Repository.IRepository;
using MagazinOnline.Modele;
using MagazinOnline.Utilitate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MagazinOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetalii.Rol_Admin + "," + StaticDetalii.Rol_Angajat)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unit;
        public OrderController(IUnitOfWork unit)
        {
            _unit = unit;
        }
        public IActionResult Index()
        {
            return View();
        }


        #region Api Calls
        [HttpGet]
        public IActionResult GetOrderList(string status)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<OrderHeader> orderHeaderLista;

            if(User.IsInRole(StaticDetalii.Rol_Admin) || User.IsInRole(StaticDetalii.Rol_Angajat))
            {
                orderHeaderLista = _unit.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                orderHeaderLista = _unit.OrderHeader.GetAll(u=>u.ApplicationUserId==claims.Value,includeProperties: "ApplicationUser");
            }
            switch (status)
            {
                case "inprocess":
                    orderHeaderLista = orderHeaderLista.Where(u => u.OrderStatus == StaticDetalii.StatusInProcess ||
                                                                   u.OrderStatus == StaticDetalii.StatusPending); break;
                case "pending":
                    orderHeaderLista = orderHeaderLista.Where(u => u.PaymentStatus == StaticDetalii.PaymentStatusDelayedPayment); break;
                case "completed":
                    orderHeaderLista = orderHeaderLista.Where(u => u.OrderStatus == StaticDetalii.StatusShipped); break;
                case "rejected":
                    orderHeaderLista = orderHeaderLista.Where(u => u.OrderStatus == StaticDetalii.StatusCanceled ||
                                                                  u.OrderStatus == StaticDetalii.StatusRefunded); break;
                default:
                break;
            }
            return Json(new { data=orderHeaderLista});
        }
        #endregion
    }
}

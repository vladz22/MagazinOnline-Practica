using MagazinOnline.DataAcces.Data;
using MagazinOnline.DataAcces.Repository.IRepository;
using MagazinOnline.Modele;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MagazinOnline.Utilitate;

namespace MagazinOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetalii.Rol_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
       

        #region Api Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var listaUser = _db.ApplicationUsers.Include(x=>x.Companie).ToList();
            var userRol = _db.UserRoles.ToList();
            var listaRol = _db.Roles.ToList();

            foreach(var user in listaUser)
            {
                var roleId = userRol.FirstOrDefault(x=>x.UserId==user.Id).RoleId;
                user.Rol = listaRol.FirstOrDefault(x => x.Id == roleId).Name;
                if(user.Companie==null)
                {
                    user.Companie = new Companie()
                    {
                        Nume = ""
                    };
                }
            }

            return Json(new { data = listaUser });
        }
        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var objfromdb = _db.ApplicationUsers.FirstOrDefault(a => a.Id == id);
            if(objfromdb==null)
            {
                return Json(new { succes = false, message = "Eroare in timpul operatiunii" });
            }
            if(objfromdb!=null && objfromdb.LockoutEnd>DateTime.Now)
            {
                //este blocat
                objfromdb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objfromdb.LockoutEnd = DateTime.Now.AddDays(30);
            }

            _db.SaveChanges();
            return Json(new { succes = true, message = "Operatie cu succes" });
        }
        #endregion
    }
}

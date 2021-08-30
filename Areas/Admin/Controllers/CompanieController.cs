using MagazinOnline.DataAcces.Repository.IRepository;
using MagazinOnline.Modele;
using MagazinOnline.Utilitate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazinOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =StaticDetalii.Rol_Admin+","+StaticDetalii.Rol_Angajat)]
    public class CategorieController : Controller
    {
        private readonly IUnitOfWork _unit;

        public CategorieController(IUnitOfWork unit)
        {
            _unit = unit;
        }
        public IActionResult Index()
        {
            return View();
        }
        //Get
        public IActionResult Upsert(int? id)
        {
            Categorie categorie = new Categorie();
            if(id==null)
            {
                //create
                return View(categorie);
            }
            //edit
            categorie = _unit.Categorie.Get(id.GetValueOrDefault());
            if(categorie==null)
            {
                return NotFound();
            }
            return View(categorie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Categorie categorie)
        {
            if (ModelState.IsValid)
            {
                if (categorie.Id == 0)
                {
                    _unit.Categorie.Adaugare(categorie);

                }
                else
                {
                    _unit.Categorie.Update(categorie);
                }
            }
            _unit.Save();
            return RedirectToAction(nameof(Index));//se poate si "Index"
        }

        #region Api Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var lista = _unit.Categorie.GetAll();
            return Json(new { data = lista });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objfromdb = _unit.Categorie.Get(id);
            if (objfromdb == null)
            {
                return Json(new { succes = false, message = "Error la stergere" });
            }
            _unit.Categorie.Stergere(objfromdb);
            _unit.Save();
            return Json(new { succes = true, message = "S-a sters" });
        }       
        #endregion
    }
}

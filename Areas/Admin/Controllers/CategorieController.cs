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
    [Authorize(Roles = StaticDetalii.Rol_Admin)]
    public class CompanieController : Controller
    {
        private readonly IUnitOfWork _unit;

        public CompanieController(IUnitOfWork unit)
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
            Companie companie = new Companie();
            if(id==null)
            {
                //create
                return View(companie);
            }
            //edit
            companie = _unit.Companie.Get(id.GetValueOrDefault());
            if(companie==null)
            {
                return NotFound();
            }
            return View(companie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Companie companie)
        {
            if (ModelState.IsValid)
            {
                if (companie.Id == 0)
                {
                    _unit.Companie.Adaugare(companie);

                }
                else
                {
                    _unit.Companie.Update(companie);
                }
            }
            _unit.Save();
            return RedirectToAction(nameof(Index));//se poate si "Index"
        }

        #region Api Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var lista = _unit.Companie.GetAll();
            return Json(new { data = lista });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objfromdb = _unit.Companie.Get(id);
            if (objfromdb == null)
            {
                return Json(new { succes = false, message = "Error la stergere" });
            }
            _unit.Companie.Stergere(objfromdb);
            _unit.Save();
            return Json(new { succes = true, message = "S-a sters" });
        }       
        #endregion
    }
}

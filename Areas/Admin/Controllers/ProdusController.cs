using MagazinOnline.DataAcces.Repository.IRepository;
using MagazinOnline.Modele;
using MagazinOnline.Modele.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using MagazinOnline.Utilitate;

namespace MagazinOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetalii.Rol_Admin+","+StaticDetalii.Rol_Angajat)]
    public class ProdusController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly IWebHostEnvironment _hostEnv;

        public ProdusController(IUnitOfWork unit,IWebHostEnvironment hostEnv)
        {
            _unit = unit;
            _hostEnv = hostEnv;
        }
        public IActionResult Index()
        {
            return View();
        }
        //Get
        public IActionResult Upsert(int? id)
        {
            ProdusVM produsVM = new ProdusVM()
            {
                Produs = new Produs(),
                CategorieLista = _unit.Categorie.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Nume,
                    Value = i.Id.ToString()
                })
            };
            if(id==null)
            {
                //create
                return View(produsVM);
            }
            //edit
            produsVM.Produs = _unit.Produs.Get(id.GetValueOrDefault());
            if(produsVM.Produs==null)
            {
                return NotFound();
            }
            return View(produsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProdusVM produsVM)
        {
            if (ModelState.IsValid)
            {
                string webrootpath = _hostEnv.WebRootPath;//memoram path in variabila
            var files = HttpContext.Request.Form.Files;//obtinem fisierele uploadate
            if (files.Count > 0)//inseamna ca un fisier a fost uploadat
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webrootpath, @"imagini\produse");//combinam path-urile
                var extension = Path.GetExtension(files[0].FileName);

                if (produsVM.Produs.ImagineUrl != null)
                {
                    //pentru a edita imaginea existenta,o stergem pe cea exista si o uploadam pe cea noua
                    var imaginePath = Path.Combine(webrootpath, produsVM.Produs.ImagineUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(imaginePath))
                    {
                        System.IO.File.Delete(imaginePath);
                    }
                }
                using (var filesStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);//salveaza fisierul in imagini/produse
                }
                produsVM.Produs.ImagineUrl = @"\imagini\produse\" + fileName + extension;//actualizam campul imageurl din produsVM
            }
            else
            {
                //update cand nu schimba imaginea
                if (produsVM.Produs.Id != 0)
                {
                    Produs objfromdb = _unit.Produs.Get(produsVM.Produs.Id);
                    produsVM.Produs.ImagineUrl = objfromdb.ImagineUrl;
                }
            }

            
                if (produsVM.Produs.Id == 0)
                {
                    _unit.Produs.Adaugare(produsVM.Produs);

                }
                else
                {
                    _unit.Produs.Update(produsVM.Produs);
                }
                _unit.Save();
                return RedirectToAction(nameof(Index));//se poate si "Index"
            }
            return View(produsVM);
        }

        #region Api Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var lista = _unit.Produs.GetAll(includeProperties:"Categorie");
            return Json(new { data = lista });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objfromdb = _unit.Produs.Get(id);
            if (objfromdb == null)
            {
                return Json(new { succes = false, message = "Error la stergere" });
            }
            string webrootpath = _hostEnv.WebRootPath;//memoram path in variabila
            var imaginePath = Path.Combine(webrootpath, objfromdb.ImagineUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imaginePath))
            {
                System.IO.File.Delete(imaginePath);
            }
            _unit.Produs.Stergere(objfromdb);
            _unit.Save();
            return Json(new { succes = true, message = "S-a sters" });
        }       
        #endregion
    }
}

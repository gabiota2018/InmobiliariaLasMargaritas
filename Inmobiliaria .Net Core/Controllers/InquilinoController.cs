using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_.Net_Core.Controllers
{
    [Authorize]//el controlador utiliza el sist. por defecto de autorización
    public class InquilinoController : Controller
    {
        private readonly IRepositorio<Inquilino> repositorio;

        public InquilinoController(IRepositorio<Inquilino> repositorio)
        {
            this.repositorio = repositorio;
        }
        // GET: Inquilino
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            ViewData["Title"] = "Listado de Inquilinos";
            return View(lista);
        }

      
        // GET: Inquilino/Create
        public ActionResult Create()
        {
            ViewData["Title"] = "Nuevo Inquilino";
            return View();
        }

        // POST: Inquilino/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino inquilino)
        {
            int rta = -1;
            try
            {
                if (ModelState.IsValid)
                {
                    rta = repositorio.Alta(inquilino);
                    TempData["Id"] = inquilino.IdInquilino;
                    return RedirectToAction(nameof(Index));
                }
                else
                    return View(inquilino);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(inquilino);
            }
        }

        // GET: Inquilino/Edit/5
        public ActionResult Edit(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            ViewData["Title"] = "Actualizar perfil de inquilino";
            return View(p);
        }

        // POST: Inquilino/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Inquilino p = null;
            try
            {
                p = repositorio.ObtenerPorId(id);
                p.Dni = Convert.ToInt32(collection["Dni"]);
                p.Apellido = collection["Apellido"];
                p.Nombre = collection["Nombre"];
                p.Direccion = collection["Direccion"];
                p.Telefono = Convert.ToInt32(collection["Telefono"]);
                repositorio.Modificacion(p);
                TempData["Mensaje"] = "Datos actualizados";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(p);
            }
        }

        // GET: Inquilino/Delete/5
        public ActionResult Delete(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            ViewData["Title"] = "Los datos seran eliminados permanentemente";
            return View(p);
        }

        // POST: Inquilino/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inquilino inquilino)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Los datos fueron eliminados";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(inquilino);
            }
        }

    }
}
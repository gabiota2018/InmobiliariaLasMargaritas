using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_.Net_Core.Controllers
{
  
    public class PagoController : Controller
    {
        private readonly IRepositorioPago repositorio;
        private readonly IRepositorioAlquiler repoAlquiler;

        public PagoController(IRepositorioPago repositorio, IRepositorioAlquiler repoAlquiler)
        {
            this.repositorio = repositorio;
            this.repoAlquiler = repoAlquiler;
        }
        // GET: Pago
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Alquiler");
        }
        public ActionResult VerPagos()
        {
            int IdAlquiler = Convert.ToInt32(TempData["IdAlquiler"]);
            var lista = repositorio.ObtenerPorIdAlquiler(IdAlquiler);
            ViewData["Title"] = "Pagos realizados del Alquiler N° " + IdAlquiler;
            TempData["IdAlquiler"] = TempData["IdAlquiler"];
            return View(lista);
        }
        
        // GET: Pago/Create
        public ActionResult Create()
        {
            Pago p = new Pago();
            Alquiler alquiler = new Alquiler();
            int IdAlquiler = Convert.ToInt32(TempData["IdAlquiler"]);
            int numero = repositorio.ObtenerUltimoNro(IdAlquiler);
            numero++;
            alquiler = repoAlquiler.ObtenerPorId(IdAlquiler);
            ViewData["Title"] = "Nuevo Pago del Alquiler N°:" + IdAlquiler;
            ViewData["Numero"] = numero;
            ViewData["Importe"] = alquiler.Precio;
            TempData["IdAlquiler"] = TempData["IdAlquiler"];
            return View();
        }

        // POST: Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago pago)
        {
            pago.IdAlquiler = Convert.ToInt32(TempData["IdAlquiler"]);
            int rta = -1;
            try
            {
                if (ModelState.IsValid)
                {
                    rta = repositorio.Alta(pago);
                    TempData["Id"] = pago.IdPago;
                    return RedirectToAction(nameof(Index));
                }
                else
                    return View(pago);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(pago);
            }
        }

        // GET: Pago/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Pago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pago/Delete/5
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

        // POST: Pago/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Pago pago)
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
                return View(pago);
            }
        }
    }
}
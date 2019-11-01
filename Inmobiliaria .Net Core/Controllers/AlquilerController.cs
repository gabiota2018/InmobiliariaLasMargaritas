using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_.Net_Core.Controllers
{
    public class AlquilerController : Controller
    {
        private readonly IRepositorioAlquiler repositorio;
        private readonly IRepositorioInmueble repoInmueble;
        private readonly IRepositorioInquilino repoInquilino;
        private readonly IRepositorioPago repoPago;

        public AlquilerController(IRepositorioAlquiler repositorio, IRepositorioInmueble repoInmueble, IRepositorioInquilino repoInquilino, IRepositorioPago repoPago)
        {
            this.repositorio = repositorio;
            this.repoInmueble = repoInmueble;
            this.repoInquilino = repoInquilino;
            this.repoPago = repoPago;
        }
        // GET: Alquiler
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            ViewData["Title"] = "Listado de Alquileres";
            return View(lista);
        }

       
        // GET: Alquiler/Create
        public ActionResult Create()
        {
            ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
            ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
            ViewData["Title"] = "Nuevo Contrato";
            return View();
        }

        // POST: Alquiler/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Alquiler alquiler)
        {
           alquiler.Precio = repoInmueble.DevolverPrecio(alquiler.IdInmueble);
           try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Alta(alquiler);
                    TempData["Id"] = alquiler.IdAlquiler;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
                    ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
                    ViewData["Title"] = "Nuevo Contrato";

                    ViewBag.MensajeError = "Faltan completar datos...";
                    ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
                    ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
                    return View(alquiler);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                ViewBag.MensajeError = "Ups... hubo un error, intenta de nuevo";
                return View(alquiler);
            }
        }

        // GET: Alquiler/Edit/5
        public ActionResult Edit(int id)// RENUEVA EL CONTRATO
        {
            var alquiler = repositorio.ObtenerPorId(id);
            TempData["IdInquilino"] = alquiler.IdInquilino;
            TempData["IdInmueble"] = alquiler.IdInmueble;
            TempData["Precio"] = alquiler.Precio;
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            ViewData["Title"] = "Renovar Contrato";
            return View(alquiler);
        }

        // POST: Alquiler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Alquiler alquiler)// RENUEVA EL CONTRATO
        {

            alquiler.IdInmueble = Convert.ToInt32(TempData["IdInmueble"]);
            alquiler.IdInquilino = Convert.ToInt32(TempData["IdInquilino"]);
            alquiler.Precio = Convert.ToDecimal(TempData["Precio"]);
            try
            {
                if (ModelState.IsValid) 
                {
                    repositorio.Alta(alquiler);
                    TempData["Id"] = alquiler.IdAlquiler;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    alquiler = repositorio.ObtenerPorId(id);
                    ViewData["Title"] = "Renovar Contrato";
                    ViewBag.MensajeError = "Faltan completar datos...";
                    return View(alquiler);
                }
            }
            catch (Exception ex)
            {
                alquiler = repositorio.ObtenerPorId(id);
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                ViewBag.MensajeError = "Ups... hubo un error, intenta de nuevo";
                return View(alquiler);
            }
        }
        public ActionResult VerPagos(int id)
        {
             TempData["IdAlquiler"] = id;
             return RedirectToAction("VerPagos", "Pago");
        }
        
        // GET: Alquiler/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Alquiler/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
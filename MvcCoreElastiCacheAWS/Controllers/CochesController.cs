using Microsoft.AspNetCore.Mvc;
using MvcCoreElastiCacheAWS.Models;
using MvcCoreElastiCacheAWS.Repositories;
using MvcCoreElastiCacheAWS.Services;

namespace MvcCoreElastiCacheAWS.Controllers
{
    public class CochesController : Controller
    {
        private RepositoryCoches repo;
        private ServiceAWSCache serviceCache;

        public CochesController(RepositoryCoches repo,
            ServiceAWSCache serviceCache)
        {
            this.repo = repo;
            this.serviceCache = serviceCache;
        }

        public IActionResult Index()
        {
            List<Coche> coches = this.repo.GetCoches();
            return View(coches);
        }

        public IActionResult Details(int id)
        {
            Coche car = this.repo.FindCoche(id);
            return View(car);
        }

        public async Task<IActionResult> SeleccionarFavorito(int idcoche)
        {
            Coche car = this.repo.FindCoche(idcoche);
            if (car == null)
            {
                ViewData["MENSAJE"] = "no hay CAR";
            }
            else
            {
                ViewData["MENSAJE"] = car.Marca + " " + car.Modelo;
            }
            await this.serviceCache.AddCocheAsync(car);
            //return RedirectToAction("Favoritos");
            return View();
        }

        public async Task<IActionResult> Favoritos()
        {
            List<Coche> coches = await this.serviceCache.GetCochesFavoritosAsync();
            return View(coches);
        }

        public async Task<IActionResult> EliminarFavorito(int idcoche)
        {
            await this.serviceCache.DeleteCocheFavoritoAsync(idcoche);
            return RedirectToAction("Favoritos");
        }
    }
}

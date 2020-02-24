using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AfpEat.Controllers
{
    public class HomeController : Controller
    {
        private AfpEatEntities db = new AfpEatEntities();

        // GET: Restaurants
        public ActionResult Index()
        {
            //on recupere tous les restos de la bdd vers la page d'accueil
            //ViewBag.Restaurants = db.Restaurants.ToList();
            ViewBag.TypeCuisines = db.TypeCuisines.ToList();

            //var typeCuisines=db.TypeCuisines.ToList();
            //return View();
           return View(db.Restaurants.ToList());
        }

        public ActionResult Menu()
        {
            ViewBag.Produits = db.Produits.ToList();
            ViewBag.Menus = db.Menus.ToList();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
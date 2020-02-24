using AfpEat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AfpEat.Controllers
{
    public class SWController : Controller
    {
        // GET: SW
        public ActionResult Index()
        {
            return View();
        }
    
            AfpEatEntities db = new AfpEatEntities();

            [HttpPost]
            public JsonResult AddProduit(int idProduit, string idSession)
            {
                SessionUtilisateur sessionUtilisateur = db.SessionUtilisateurs.Find(Session.SessionID);

                List<ProduitPanier> panier = (List<ProduitPanier>)HttpContext.Application[idSession] ?? new List<ProduitPanier>();

                if (sessionUtilisateur != null)
                {
                    Produit produit = db.Produits.First(p => p.IdProduit == idProduit);

                    ProduitPanier produitPanier = new ProduitPanier()
                    {
                        IdProduit = produit.IdProduit,
                        Nom = produit.Nom,
                        Description = produit.Description,
                        Quantite = produit.Quantite,
                        Prix = produit.Prix,
                        Photo = produit.Photos.FirstOrDefault()?.Nom ?? "default.jpg"
                    };

                    panier.Add(produitPanier);

                    HttpContext.Application[idSession] = panier;
                }

                return Json(panier.Count, JsonRequestBehavior.AllowGet);
            }

            public JsonResult GetProduits(string idSession)
            {
                SessionUtilisateur sessionUtilisateur = db.SessionUtilisateurs.Find(Session.SessionID);

                List<ProduitPanier> panier = (List<ProduitPanier>)HttpContext.Application[idSession] ?? new List<ProduitPanier>();

                if (sessionUtilisateur != null && panier.Count > 0)
                {
                    return Json(panier, JsonRequestBehavior.AllowGet);
                }

                return Json(new EmptyResult(), JsonRequestBehavior.AllowGet);
            }
        }
}
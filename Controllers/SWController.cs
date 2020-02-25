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
                    IdRestaurant = produit.IdRestaurant,
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
        public JsonResult SaveCommande(string idSession)
        {
            SessionUtilisateur sessionUtilisateur = db.SessionUtilisateurs.Find(Session.SessionID);

            List<ProduitPanier> panier = (List<ProduitPanier>)HttpContext.Application[idSession] ?? new List<ProduitPanier>();

            Utilisateur utilisateur = db.Utilisateurs.First(u => u.IdSession == idSession);
            string message = "";


            if (utilisateur != null && utilisateur.Solde > 0 && panier != null && panier.Count > 0)
            {
                decimal prixTotal = 0;
                int idRestaurant = 0;


                foreach (ProduitPanier produitPanier in panier)
                {
                    prixTotal += produitPanier.Prix * produitPanier.Quantite;
                    idRestaurant = produitPanier.IdRestaurant;
                }
                if (prixTotal <= utilisateur.Solde)
                {
                    Commande commande = new Commande();
                    commande.IdUtilisateur = utilisateur.IdUtilisateur;
                    commande.IdRestaurant = idRestaurant;
                    commande.Date = DateTime.Now;
                    commande.Prix = prixTotal;
                    commande.IdEtatCommande = 1;

                    utilisateur.Solde -= prixTotal;

                    //db.Commandes.Add(commande);
                    //db.SaveChanges();

                    foreach (ProduitPanier produitPanier in panier)
                    {
                        CommandeProduit commandeProduit = new CommandeProduit();
                        //commandeProduit.IdCommande = commande.IdCommande;
                        commandeProduit.IdProduit = produitPanier.IdProduit;
                        commandeProduit.Prix = produitPanier.Prix;
                        commandeProduit.Quantite = produitPanier.Quantite;

                        commande.CommandeProduits.Add(commandeProduit);
                    }
                    db.Commandes.Add(commande);
                    db.SaveChanges();
                }
            }
            return Json(new { idutilisateur = utilisateur.IdUtilisateur, JsonRequestBehavior.AllowGet });
        }

        public JsonResult LoginUtilisateur(string idSession, string matricule, string password)

        {
            Utilisateur utilisateur = db.Utilisateurs.FirstOrDefault(u => u.Matricule == matricule && u.Password==password);

            if(utilisateur!=null)
            {
                utilisateur.IdSession = idSession;
                db.SaveChanges();

                return Json(new { error = 0, message = utilisateur.Solde}, JsonRequestBehavior.AllowGet);
            }

            return Json ( new { error = 1, message = "Erreur de connexion" },  JsonRequestBehavior.AllowGet);
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AfpEat
{
    using System;
    using System.Collections.Generic;
    
    public partial class Commande
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Commande()
        {
            this.CommandeProduits = new HashSet<CommandeProduit>();
        }
    
        public int IdCommande { get; set; }
        public int IdUtilisateur { get; set; }
        public int IdRestaurant { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Prix { get; set; }
        public int IdEtatCommande { get; set; }
    
        public virtual EtatCommande EtatCommande { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual Utilisateur Utilisateur { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommandeProduit> CommandeProduits { get; set; }
    }
}
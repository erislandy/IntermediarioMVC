using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntermediarioMVC.Models
{
    public class Provider : Person
    { 
        #region Properties

        [Key]
        public int ProviderId { get; set; }

        #endregion

        #region Navigation properties

        public virtual ICollection<Purchase> Purchases { get; set;}

        #endregion

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntermediarioMVC.Models
{
    public class Pay
    {
        #region Properties
        public int PayId { get; set; }
        public int ProviderId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public bool Certificated { get; set; }

        #endregion  

        #region Navigation Properties
        public virtual Provider Provider { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }


        #endregion

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntermediarioMVC.Models
{
    [NotMapped]
    public class PayDetailsView : Pay
    {
        public List<Sale> AvailableSales { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntermediarioMVC.Models
{
    public class Messenger : Person
    {
        [Key]
        public int MessengerId { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
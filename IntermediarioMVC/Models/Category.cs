using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntermediarioMVC.Models
{
    public class Category
    {
        #region Properties

        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(20, ErrorMessage = "The field {0} contains maximun {1} and minimun {2} characters", MinimumLength = 3)]
        public string Description { get; set; }

        #endregion

        #region Navigation properties

        public virtual ICollection<Product> Products { get; set; }
        #endregion

    }
}
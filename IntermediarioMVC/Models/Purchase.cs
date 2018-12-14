using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntermediarioMVC.Models
{
    public class Purchase
    {
        #region Properties

        [Key]
        public int PurchaseId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        
        public int ProviderId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [Display(Name = "Price input")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "The field {0} is required")]
        public double PriceInput { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "The field {0} is required")]
        public DateTime DatePurchase { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name = "Amount")]
        public double Amount { get; set; }
        public string Remarks { get; set; }

        #endregion

        #region Navigation properties

        public virtual Provider Provider { get; set; }

        public virtual Product Product { get; set; }
        #endregion

    }
}
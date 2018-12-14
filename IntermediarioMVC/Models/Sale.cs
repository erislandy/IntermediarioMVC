using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntermediarioMVC.Models
{
    public class Sale
    {
        #region Properties

        [Key]
        public int SaleId { get; set; }
        public int? PayId { get; set; }
        public int ProductInStockId { get; set; }
        public int? CustomerId { get; set; }
        public int? MessengerId { get; set; }

        [Display(Name = "Date of Sale")]
        [DataType(DataType.Date)]
        public DateTime DateOfSale { get; set; }
        public double Amount { get; set; }

        [Display(Name = "Price of sale")]
        [DataType(DataType.Currency)]
        public double SalePrice { get; set; }

        [Display(Name = "State of sale")]
        public SaleState SaleState { get; set; }
        #endregion

        #region Navigation properties
        public virtual Pay Pay { get; set; }
        public virtual ProductInStock ProductInStock { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Messenger Messenger { get; set; }

        #endregion

    }

    public enum SaleState
    {
        PendingLiquidate, Liquidated, Certificated
    }
}
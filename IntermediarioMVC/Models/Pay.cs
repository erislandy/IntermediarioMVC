using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [NotMapped]
        [DataType(DataType.Currency)]
        public double InputValue
        {
            get
            {
                double total = 0;

                if(Sales != null && Sales.Count > 0)
                {
                    foreach (var sale in Sales)
                    {
                        total += sale.Amount * sale.ProductInStock.PriceInput; 
                    }
                }
                return total;
            } 
        }
        [NotMapped]
        [DataType(DataType.Currency)]

        public double SaleValue
        {
            get
            {
                double total = 0;

                if (Sales != null && Sales.Count > 0)
                {
                    foreach (var sale in Sales)
                    {
                        total += sale.Amount * sale.SalePrice;
                    }
                }
                return total;
            }
        }

        #endregion  

        #region Navigation Properties
        public virtual Provider Provider { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }


        #endregion

    }
}
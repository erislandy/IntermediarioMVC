using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntermediarioMVC.Models
{
    public class ProductInStock
    {
        #region Properties

        [Key]
        public int ProductInStockId { get; set; }
        public int ProviderId { get; set; }
        public int ProductId { get; set; }

        [StringLength(20, ErrorMessage = "The field {0} contains maximun {1} and minimun {2} characters", MinimumLength = 3)]
        public string Code { get; set; }

        [Display(Name = "Price input")]
        [DataType(DataType.Currency)]
        public double PriceInput { get; set; }
        public double Amount { get; set; }
        public StateEnum State { get; set; }

        #endregion


        #region Navigation properties

        public virtual Provider Provider { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<ChangeState> ChangeStates { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }


        #endregion



    }

    /*
    
        1- Available state 
        2- AwaitingForPaid state means produts is awaiting for paid
        3- Moved state means product has been moved out stock
        4- AwaitingForReturn state means product has been returned to provider
            because is a faulty product 
    
    */
    public enum StateEnum
    {
        Available, AwaitingForPaid, Moved, AwaitingForReturn
    }

}
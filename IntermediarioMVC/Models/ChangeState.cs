using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntermediarioMVC.Models
{
    public class ChangeState
    {
        #region Properties
        public int ChangeStateId { get; set; }
        public int ProductInStockId { get; set; }
        public double Amount { get; set; }

        [Display(Name = "Before State")]
        public StateEnum BeforeState { get; set; }

        [Display(Name = "Next State")]
        public StateEnum NextState { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "The field {0} is required")]
        public DateTime Date { get; set; }

        //Navigation Properties
        public virtual ProductInStock ProductInStock { get; set; }

        #endregion
    }
}
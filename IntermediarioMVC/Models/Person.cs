using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntermediarioMVC.Models
{
    public class Person
    {
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(20, ErrorMessage = "The field {0} contains maximun {1} and minimun {2} characters", MinimumLength = 3)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(20, ErrorMessage = "The field {0} contains maximun {1} and minimun {2} characters", MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [Required(ErrorMessage = "The field {0} is required")]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ImagePath { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class RiskDataModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public decimal Value { get; set; }
        public string Make { get; set; }
        [Required]
        public DateTime? DOB { get; set; }
    }

    //public class RequireWhenCategoryAttribute : ValidationAttribute
    //{
    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        var employee = (EmployeeModel)validationContext.ObjectInstance;
    //        if (employee.CategoryId == 1)
    //            return ValidationResult.Success;

    //        var emailStr = value as string;
    //        return string.IsNullOrWhiteSpace(emailStr)
    //            ? new ValidationResult("Value is required.")
    //            : ValidationResult.Success;
    //    }
    //}

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Models
{
    public abstract class BaseModel
    {
        public List<string> ValidationErrorsString { get; set; }
        public List<int> ValidationErrorCodes { get; set; }
        public List<ValidationResult> ValidationResults { get; set; }
    }
}

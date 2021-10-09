using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Models
{
    public interface IBaseModel
    {
        public List<string> ValidationErrorsString { get; set; }
        public List<int> ValidationErrorCodes { get; set; }
        public List<Dictionary<int, string>> ValidationErrors { get; set; }
    }
}

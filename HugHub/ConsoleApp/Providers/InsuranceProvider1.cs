using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Providers
{
    public class InsuranceProvider1 : BaseInsuranceProvider, IInsuranceProvider
    {
        public string InsurerName { get; } = "ddede";
        public string InsurerUrl { get; } = "ddede";
        public bool IsLive { get; } = true;

        public void DataMapper()
        {
            throw new NotImplementedException();
        }

        public void GetPrice()
        {
            throw new NotImplementedException();
        }

        public void InsurerSetup()
        {
            throw new NotImplementedException();
        }

        public void ValidateModel()
        {
            throw new NotImplementedException();
        }
    }
}

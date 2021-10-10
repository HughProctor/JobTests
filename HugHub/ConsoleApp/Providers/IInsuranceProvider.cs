using System;

namespace ConsoleApp.Providers
{
    public interface IInsuranceProvider
    {
        public string InsurerName { get; }
        public string InsurerUrl { get; }
        public bool IsLive { get; }

        public void InsurerSetup();

        public void ValidateModel();
        public void DataMapper();

        public void GetPrice();
    }
}

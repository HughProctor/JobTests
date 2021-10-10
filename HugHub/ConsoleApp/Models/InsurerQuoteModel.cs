using System;

namespace ConsoleApp.Models
{
    public class InsurerQuoteModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public byte[] SiteContent { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public decimal PriceTotal { get; set; }
        public DateTime DateTime_Start { get; set; }
        public DateTime DateTime_Expiry { get; set; }
    }
}

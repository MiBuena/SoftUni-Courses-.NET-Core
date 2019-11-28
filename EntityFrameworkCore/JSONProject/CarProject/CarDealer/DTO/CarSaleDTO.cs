using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    public class CarSaleDTO
    {
        public CarDataDTO Car { get; set; }

        public string CustomerName { get; set; }

        public string Discount { get; set; }

        public string Price { get; set; }

        public string PriceWithDiscount { get; set; }
    }
}

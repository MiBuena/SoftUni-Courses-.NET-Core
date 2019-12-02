using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.DTOs.ExportDTOs
{
    [XmlType("SoldProducts")]
    public class SoldProductsStatisticsDTO
    {
        public SoldProductsStatisticsDTO()
        {
            Products = new List<SoldProductsExportDTO>();
        }

        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public List<SoldProductsExportDTO> Products { get; set; }
    }
}

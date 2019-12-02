using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.DTOs.ExportDTOs
{
    [XmlType("User")]
    public class UsersWithSoldProductsExportDTO
    {
        public UsersWithSoldProductsExportDTO()
        {
            SoldProducts = new List<SoldProductsExportDTO>();
        }

        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlArray("soldProducts")]
        public List<SoldProductsExportDTO> SoldProducts { get; set; }
    }
}

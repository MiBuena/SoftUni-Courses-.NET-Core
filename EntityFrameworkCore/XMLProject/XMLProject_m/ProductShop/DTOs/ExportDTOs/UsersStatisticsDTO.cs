using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.DTOs.ExportDTOs
{
    [XmlType("Users")]
    public class UsersStatisticsDTO
    {
        public UsersStatisticsDTO()
        {
            Users = new List<UsersWithSoldProductsSecondExportDTO>();
        }

        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public List<UsersWithSoldProductsSecondExportDTO> Users { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.DTOs.ImportDTOs
{
    [XmlType("Category")]
    public class CategoryImportDTO
    {
        [XmlElement("name")]
        public string Name { get; set; }
    }
}

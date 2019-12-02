using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.DTOs
{
    [XmlType("user")]
    public class UserDTO
    {
        [XmlAttribute("firstName")]
        public string FirstName { get; set; }

        [XmlAttribute("lastName")]
        public string LastName { get; set; }

        [XmlAttribute("age")]
        public string Age { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace XMLProject
{
    [XmlType("car")]
    public class Car
    {
        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }

        [XmlElement("travelled-distance")]
        public string TravelledDistance { get; set; }
    }
}

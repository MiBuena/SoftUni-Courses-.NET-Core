using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace XMLProject
{
    public class Book
    {

        public Book()
        {

        }

        public Book(string title, string author)
        {
            this.Title = title;
            this.Author = author;
        }

        [XmlElement("make")]
        public string Title { get; set; }

        [XmlAttribute("name")] 
        public string Author { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Entities
{
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Publisher { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public int AvailableQuantity { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace excel_to_analytics.Client.Models
{
    public class Item
    {
        public string Address { get; set; }
        public string Categorie { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4_KhaledMarijn.Classes
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public Customer? User { get; set; }

        /*public OrderStatus Status { get; set; }*/
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4_KhaledMarijn.Classes
{
    public class OrderStatus
    {
        public static readonly string queue = "In queue";
        public static readonly string preparing = "Preparing";
        public static readonly string baking = "Baking";
        public static readonly string delivering = "On the way";
        public static readonly string delivered = "Delivered";

        public List<string> statusList = new List<string>()
        {
            preparing,
            baking,
            delivering,
            delivered
        };
    }
}

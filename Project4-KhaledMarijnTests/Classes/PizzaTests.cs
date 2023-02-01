using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project4_KhaledMarijn.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4_KhaledMarijn.Classes.Tests
{
    [TestClass()]
    public class PizzaTests
    {
        [TestMethod()]
        public void FormatPriceTest()
        {
            Pizza pizza = new Pizza();
            decimal result = pizza.Price = 2.25m;
            Assert.AreEqual("$ 25.25", result);
            Assert.Fail();
        }
    }
}
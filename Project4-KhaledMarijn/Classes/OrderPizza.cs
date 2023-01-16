using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project4_KhaledMarijn.Classes
{
    public class OrderPizza : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion


        private int pizzaId;
        public int PizzaID
        {
            get { return pizzaId; }
            set { pizzaId = value; OnPropertyChanged(); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }

        private int sizeId;
        public int SizeId
        {
            get { return sizeId; }
            set { sizeId = value; OnPropertyChanged(); }
        }

        private int amount;
        public int Amount
        {
            get { return amount; }
            set { amount = value; OnPropertyChanged(); }
        }

        private decimal price;
        public decimal Price
        {
            get { return price; }
            set { price = value; OnPropertyChanged(); }
        }

        private string priceLabel;
        public string PriceLabel
        {
            get { return priceLabel; }
            set { priceLabel = value; OnPropertyChanged(); }
        }

        public string Size
        {
            get => SizeId == 1 ? "small" : SizeId == 2 ? "medium" : "large";
        }
    }
}
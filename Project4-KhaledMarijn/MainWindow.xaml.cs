using MySqlX.XDevAPI.Common;
using Project4_KhaledMarijn.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project4_KhaledMarijn
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            PopulatePizzas();
            PopulateSizes();
            DataContext = this;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region fields
        private readonly Project4DB db = new Project4DB();
        private readonly string serviceDeskMessage = "\n\nSomething went wrong";
        #endregion

        #region Properties
        private ObservableCollection<Pizza> pizzas = new();
        public ObservableCollection<Pizza> Pizzas
        {
            get { return pizzas; }
            set { pizzas = value; OnPropertyChanged(); }
        }

        private ObservableCollection<OrderPizza> orderPizzas = new();
        public ObservableCollection<OrderPizza> OrderPizzas
        {
            get { return orderPizzas; }
            set { orderPizzas = value; OnPropertyChanged(); }
        }

        private ObservableCollection<PizzaSize> sizes = new();
        public ObservableCollection<PizzaSize> Sizes
        {
            get { return sizes; }
            set { sizes = value; OnPropertyChanged(); }
        }

        private OrderPizza? newOrderPizza;
        public OrderPizza? NewOrderPizza
        {
            get { return newOrderPizza; }
            set { newOrderPizza = value; OnPropertyChanged(); }
        }

        private Order? newUser;
        public Order? NewUser
        {
            get { return newUser; }
            set { newUser = value; OnPropertyChanged(); }
        }

        private Order? newOrder;
        public Order? NewOrder
        {
            get { return newOrder; }
            set { newOrder = value; OnPropertyChanged(); }
        }

        private Customer? newOrderUser = new ();
        public Customer? NewOrderUser
        {
            get { return newOrderUser; }
            set { newOrderUser = value; OnPropertyChanged(); }
        }

        private Order? selectedOrder;
        public Order? SelectedOrder
        {
            get { return selectedOrder; }
            set { selectedOrder = value; OnPropertyChanged(); }
        }

        private Pizza? selectedPizza;
        public Pizza? SelectedPizza
        {
            get { return selectedPizza; }
            set { selectedPizza = value; OnPropertyChanged(); }
        }

        private PizzaSize? selectedSize;
        public PizzaSize? SelectedSize
        {
            get { return selectedSize; }
            set { selectedSize = value; OnPropertyChanged(); }
        }

        private int amount;
        public int Amount
        {
            get { return amount; }
            set { amount = value; OnPropertyChanged(); }
        }
        #endregion

        private void PopulatePizzas()
        {
            Pizzas.Clear();
            bool result = db.GetPizzas(Pizzas);
            if (!result)
                MessageBox.Show(serviceDeskMessage);
        }

        private void PopulateSizes()
        {
            Sizes.Clear();
            bool result = db.GetSizes(Sizes);
            if (!result)
                MessageBox.Show(serviceDeskMessage);
        }

        private void AddOrder(object sender, RoutedEventArgs e)
        {
            if (SelectedSize?.SizeID == 1)
            {
                SelectedPizza.Price *= (decimal)0.75;
            }
            else if (selectedSize?.SizeID == 3)
            {
                SelectedPizza.Price *= (decimal)1.25;
            }

            if (SelectedSize != null && (SelectedPizza == null || string.IsNullOrEmpty(SelectedPizza.Name) || Amount <= 0 ||
                                         SelectedPizza.Price <= 0 || string.IsNullOrEmpty(SelectedPizza.PriceLabel) ||
                                         SelectedSize.SizeID <= 0)) return;

            OrderPizza newPizza = new OrderPizza
            {
                Name = SelectedPizza.Name,
                Amount = Amount,
                Price = SelectedPizza.Price,
                PriceLabel = SelectedPizza.PriceLabel,
                SizeId = SelectedSize.SizeID,
            };
            OrderPizzas.Add(newPizza);
            decimal total = Convert.ToDecimal(orderTotal.Text.Substring(1));
            total += newPizza.Price * newPizza.Amount;
            orderTotal.Text = total.ToString("C2");
        }

        private void ConfirmPayment(object sender, RoutedEventArgs e)
        {
            //var regex = new Regex("/^[1-9][0-9]{3}[\\s]?[A-Za-z]{2}$/i");
            if (string.IsNullOrEmpty(NewOrderUser?.FirstName))
            {
                MessageBox.Show("Enter a valid firstname!");
                return;
            }
            if (string.IsNullOrEmpty(NewOrderUser?.LastName))
            {
                MessageBox.Show("Enter a valid lastname!");
                return;
            }
            if (string.IsNullOrEmpty(NewOrderUser?.Address))
            {
                MessageBox.Show("Enter a valid address!");
                return;
            }
            if (string.IsNullOrEmpty(NewOrderUser?.PostalCode) /*|| !regex.IsMatch(NewOrderUser.PostalCode)*/)
            {
                MessageBox.Show("Enter a valid postal code!");
                return;
            }

            if (string.IsNullOrEmpty(NewOrderUser?.City) /* && Regex match 4 cijfers 2 letters postcode*/)
            {
                MessageBox.Show("Enter a valid city!");
                return;
            }

            bool result = NewOrder != null && db.CreateOrder(NewOrder);
            if (!result)
            {
                NewOrder = new();
                //PopulateOrders();
            }
        }

        private void Delete_item(object sender, MouseButtonEventArgs e)
        {
            if (listview1.SelectedItem != null)
            {
               OrderPizzas.RemoveAt(listview1.SelectedIndex);
            }
        }
    }
}

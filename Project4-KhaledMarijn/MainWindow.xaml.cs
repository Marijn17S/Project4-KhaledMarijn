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

        private OrderPizza? selectedOrderPizza;
        public OrderPizza? SelectedOrderPizza
        {
            get { return selectedOrderPizza; }
            set { selectedOrderPizza = value; OnPropertyChanged(); }
        }

        private Order? newUser;
        public Order? NewUser
        {
            get { return newUser; }
            set { newUser = value; OnPropertyChanged(); }
        }

        private Order? newOrder = new();
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

            if (SelectedPizza == null || string.IsNullOrEmpty(SelectedPizza.Name))

            {
                MessageBox.Show("Please Select pizza first");
                return;
            }


            else if (Amount <= 0 || (Amount == null))
            {

                MessageBox.Show("Please type a valid amount");
                return;
            }



            else if (SelectedSize == null || SelectedSize.SizeID <= 0)
            {
                MessageBox.Show("Please select the pizza size");
                return;
            }


            /* if ((SelectedPizza == null || string.IsNullOrEmpty(SelectedPizza.Name) || Amount <= 0 ||
                                          SelectedPizza.Price <= 0 || string.IsNullOrEmpty(SelectedPizza.PriceLabel) ||
                                          SelectedSize == null || SelectedSize.SizeID <= 0)) return; */







            decimal price = SelectedPizza.Price;
            if (SelectedSize?.SizeID == 1)
                price *= (decimal)0.75;
            else if (selectedSize?.SizeID == 3)
                price *= (decimal)1.25;

           

            OrderPizza newPizza = new OrderPizza
            {
                Name = SelectedPizza.Name,
                Amount = Amount,
                Price = price,
                PriceLabel = Convert.ToString($"{price:C2}"),
                SizeId = SelectedSize.SizeID,
                PizzaID = SelectedPizza.PizzaID,
            };
            OrderPizzas.Add(newPizza);
            decimal total = Convert.ToDecimal(orderTotal.Text.Substring(1));
            total += newPizza.Price * newPizza.Amount;
            orderTotal.Text = total.ToString("C2");

            SelectedPizza = null;
            Amount = 0;
            SelectedSize = null;

        }

        private void ConfirmPayment(object sender, RoutedEventArgs e)
        {
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
            if (string.IsNullOrEmpty(NewOrderUser?.PostalCode))
            {
                MessageBox.Show("Enter a valid postal code!");
                return;
            }

            if (string.IsNullOrEmpty(NewOrderUser?.City))
            {
                MessageBox.Show("Enter a valid city!");
                return;
            }

            if (OrderPizzas.Count <= 0)
            {
                MessageBox.Show("Add a pizza to your order.");
                return;
            }

            if (!db.CheckUser(NewOrderUser))
            {
                var result = db.CreateUser(NewOrderUser);
                if (!result.Item1)
                    NewOrderUser = new();

                NewOrderUser.Id = (int)result.Item2;
            }

            OrderWindow orderWindow = new(NewOrderUser);
            NewOrder.UserId = NewOrderUser.Id;
            NewOrder.Status = OrderStatus.queue;
            NewOrder.Date = DateTime.Now;
            var createOrderResult = db.CreateOrder(NewOrder);
            if (!createOrderResult.Item1)
                NewOrder = new();

            NewOrder.Id = (int)createOrderResult.Item2;

            foreach (var pizza in OrderPizzas)
            {
                bool createOrderPizzaResult = pizza != null && db.CreateOrder_Pizza(pizza, NewOrder.Id);
            }

            NewOrder.ChangeStatus(orderWindow);

            NewOrder = new();
            SelectedPizza = null;
            Amount = 0;
            SelectedSize = null;
            orderTotal.Text = 0.ToString("C2");
            OrderPizzas.Clear();

            orderWindow.Show();
        }




        private void RemovePizza(object sender, MouseButtonEventArgs e)
        {
            if (OrderList.SelectedItem != null && SelectedOrderPizza != null)
            {
                decimal total = Convert.ToDecimal(orderTotal.Text.Substring(1));
                total -= SelectedOrderPizza.Price * SelectedOrderPizza.Amount;
                orderTotal.Text = total.ToString("C2");
                OrderPizzas.RemoveAt(OrderList.SelectedIndex);
            }
        }

        private void Orders(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new(NewOrderUser);
            orderWindow.Show();
        }
    }
}

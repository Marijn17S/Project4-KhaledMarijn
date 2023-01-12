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

        private ObservableCollection<Pizza> pizzas = new();
        public ObservableCollection<Pizza> Pizzas
        {
            get { return pizzas; }
            set { pizzas = value; OnPropertyChanged(); }
        }

      /*  private ObservableCollection<Order> newOrder = new();
        public ObservableCollection<Order> NewOrder
        {
            get { return newOrder; }
            set { newOrder = value; OnPropertyChanged(); }
        }
*/

        private Order newOrder = new();
        public Order NewOrder
        {
            get { return newOrder; }
            set
            {
                newOrder = value;
                OnPropertyChanged();
                NewOrder = null;
            }
        }

        private void PopulatePizzas()
        {
            pizzas.Clear();
            bool dbResult = db.GetPizzas(pizzas);
            if (!dbResult)
            {
                MessageBox.Show(dbResult + serviceDeskMessage);
            }
        }

        private void Add_order(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NewOrder.FirstName))
            {
                MessageBox.Show("Type your firstname");
                return;
            }
        }

        private void Confirm_payment(object sender, RoutedEventArgs e)
        {

        }
    }
}

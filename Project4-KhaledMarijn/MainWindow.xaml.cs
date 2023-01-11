using Project4_KhaledMarijn.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        private ObservableCollection<Pizza> pizzas = new();
        public ObservableCollection<Pizza> Pizzas
        {
            get { return pizzas; }
            set { pizzas = value; OnPropertyChanged(); }
        }
        public static string serviceDeskMessage = "Something went wrong";
        private Project4DB db = new Project4DB();

        private void PopulatePizzas()
        {
            pizzas.Clear();
            bool result = db.GetPizzas(pizzas);
            if (!result)
            {
                MessageBox.Show(serviceDeskMessage);
            }
        }
    }
}
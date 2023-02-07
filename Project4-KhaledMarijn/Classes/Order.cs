using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace Project4_KhaledMarijn.Classes
{
    public class Order
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private readonly Project4DB db = new Project4DB();

        DispatcherTimer dt = new();

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public Customer? User { get; set; }

        public string Status { get; set; }

        private int i = 0;

        private OrderWindow orderWindow;

        public void ChangeStatus(OrderWindow orderWindow1)
        {
            orderWindow = orderWindow1;
            dt.Interval = TimeSpan.FromMilliseconds(10000);
            dt.Tick += Dt_Tick;
            dt.Start();
            orderWindow.PopulateOrders();
        }

        private void Dt_Tick(object? sender, EventArgs e)
        {
            OrderStatus os = new OrderStatus();
            if (i < os.statusList.Count)
            {
                Status = os.statusList[i];
                i++;
                db.UpdateStatus(Id, Status);
                orderWindow.PopulateOrders();
            }
            else dt.Stop();
        }
    }
}
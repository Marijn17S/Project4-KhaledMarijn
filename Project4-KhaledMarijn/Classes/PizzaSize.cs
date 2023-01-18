using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project4_KhaledMarijn.Classes
{
    public class PizzaSize
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private int sizeID;
        public int SizeID
        {
            get { return sizeID; }
            set { sizeID = value; OnPropertyChanged(); }
        }

        private string? size;
        public string? Size
        {
            get { return size; }
            set { size = value; OnPropertyChanged(); }
        }
    }
}

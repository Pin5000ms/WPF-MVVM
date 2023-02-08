using ModernDesign.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ModernDesign.MVVM.Model
{
    public class SudokuItem : ObservableObject
    {
        private string number;

        public string Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged();//必須加 才會觸發變更
            }
        }

        public SolidColorBrush BorderBrush { get; set; }
        public SudokuItem(string n, Color c)
        {
            Number = n;
            BorderBrush = new SolidColorBrush();
            BorderBrush.Color = c;
        }
    }
}

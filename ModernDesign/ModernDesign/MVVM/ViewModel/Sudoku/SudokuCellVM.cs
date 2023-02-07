using ModernDesign.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModernDesign.MVVM.ViewModel
{
    public class SudokuCellVM : ObservableObject
    {
        public SudokuCellVM(List<string> input)
        {
            //M = new ObservableCollection<string>();
            //foreach (var item in input)
            //{
            //    M.Add(item);
                
            //}
            //BorderBrush = new ObservableCollection<SolidColorBrush>();
            //for (int i = 0; i < 9; i++)
            //{
            //    BorderBrush.Add(new SolidColorBrush());
            //}
            //foreach (var item in BorderBrush)
            //{
            //    item.Color = Colors.Black;
            //}

            CellItem = new ObservableCollection<SudokuItem>();
            foreach (var num in input)
            {
                CellItem.Add(new SudokuItem(num, Colors.Black));
            }
        }
        //public ObservableCollection<string> M { get; set; }

        //public ObservableCollection<SolidColorBrush> BorderBrush { get; set; }


        public ObservableCollection<SudokuItem> CellItem { get; set; }
    }

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

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
            M = new ObservableCollection<string>();
            foreach (var item in input)
            {
                M.Add(item);
            }
        }
        public ObservableCollection<string> M { get; set; }
    }
}

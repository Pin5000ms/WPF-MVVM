using ModernDesign.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ModernDesign.MVVM.View
{
    /// <summary>
    /// Sudoku.xaml 的互動邏輯
    /// </summary>
    public partial class SudokuView : UserControl
    {
        private SudokuViewModel SudokuVM;
        public SudokuView(SudokuViewModel _sudokuViewModel)
        {
            InitializeComponent();
            DataContext = _sudokuViewModel;
            SudokuVM = _sudokuViewModel;
        }
    }
}

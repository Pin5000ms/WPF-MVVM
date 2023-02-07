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
    /// SudokuCell.xaml 的互動邏輯
    /// </summary>
    public partial class SudokuCell : UserControl
    {
        public SudokuCellVM CellVM;
        public SudokuCell(SudokuCellVM _CellVM)
        {
            InitializeComponent();
            DataContext = _CellVM;
            CellVM = _CellVM;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if((sender as TextBox).Text.Length > 1)
            {
                (sender as TextBox).Text = e.ToString();
            }
        }
    }
}

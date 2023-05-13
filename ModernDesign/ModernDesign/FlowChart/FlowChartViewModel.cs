using ModernDesign.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ModernDesign
{
    public class FlowChartViewModel : ObservableObject
    {
        public FlowChartViewModel()
        {
            var a = new SingleBlockView();
            a.viewModel.BackGround = new SolidColorBrush(Colors.LightPink);
            var b = new SingleBlockView();
            b.viewModel.BackGround = new SolidColorBrush(Colors.LightGreen);
            var c = new SingleBlockView();
            c.viewModel.BackGround = new SolidColorBrush(Colors.LightBlue);


            Blocks.Add(a);
            Blocks.Add(b);
            Blocks.Add(c);
        }
        private ObservableCollection<SingleBlockView> _blocks = new ObservableCollection<SingleBlockView>();
        public ObservableCollection<SingleBlockView> Blocks 
        {
            get
            {
                return _blocks;
            }
            set
            {
                _blocks = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Line> _lines = new ObservableCollection<Line>();
        public ObservableCollection<Line> Lines
        {
            get
            {
                return _lines;
            }
            set
            {
                _lines = value;
                OnPropertyChanged();
            }
        }

        public void DrawLine(double x2, double y2, int currentIdx)
        {
            Lines[currentIdx].X2 = x2;
            Lines[currentIdx].Y2 = y2;
        }
    }
}

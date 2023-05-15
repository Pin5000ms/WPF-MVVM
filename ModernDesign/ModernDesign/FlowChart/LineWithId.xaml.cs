using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ModernDesign
{
    /// <summary>
    /// LineWithId.xaml 的互動邏輯
    /// </summary>
    public partial class LineWithId : UserControl
    {
        public LineWithId()
        {
            InitializeComponent();
        }

        public int ID
        {
            get;
            set;
        }

        public void SetStartEnd(double X1, double Y1, double X2, double Y2)
        {
            line.X1 = X1;
            line.Y1 = Y1;
            line.X2 = X2;
            line.Y2 = Y2;
        }

        public void SetStart(double X1, double Y1)
        {
            line.X1 = X1;
            line.Y1 = Y1;
        }

        public void SetEnd(double X2, double Y2)
        {
            line.X2 = X2;
            line.Y2 = Y2;
        }

        public void SetDefaultHighLightLine()
        {
            line.StrokeThickness = 7;
            line.Stroke = new SolidColorBrush(Colors.OrangeRed);
            line.IsHitTestVisible = false;
            line.Opacity = 0.5;
        }

        public double X1
        {
            get { return line.X1; }
        }
        public double Y1
        {
            get { return line.Y1; }
        }
        public double X2
        {
            get { return line.X2; }
        }
        public double Y2
        {
            get { return line.Y2; }
        }
    }
}

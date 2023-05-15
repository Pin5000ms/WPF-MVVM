using ModernDesign.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ModernDesign
{
    public class SingleBlock : ObservableObject
    {

        public bool DrawingLine;

        private SolidColorBrush backGround;

		public SolidColorBrush BackGround
        {
			get { return backGround; }
			set 
			{ 
				backGround = value;
				OnPropertyChanged();
			}
		}

		//private ObservableCollection<Line> _outputLines;

		//public ObservableCollection<Line> OutputLines
		//{
		//	get { return _outputLines; }
		//	set { _outputLines = value; }
		//}


		public double Left;
		public double Top;


        public double OutputX 
		{
			get { return Left + 110; }
		}

        public double OutputY
        {
            get { return Top + 38; }
        }


        public double InputX
        {
            get { return Left +10; }
        }

        public double InputY
        {
            get { return Top + 38; }
        }


		public int InputLineIndex = -1;
        public List<int> OutputLineIds = new List<int>();

    }
}

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
    public class HomeViewModel : ObservableObject
    {
        public HomeViewModel()
        {
            _homePageBlocks.Add(new Block("A", 400, 200, Colors.BlueViolet));
            _homePageBlocks.Add(new Block("B", 200, 200, Colors.Gold));
            _homePageBlocks.Add(new Block("C", 200, 200, Colors.Aqua));
        }

        private ObservableCollection <Block> _homePageBlocks = new ObservableCollection<Block>();

        //必須用ObservableCollection，內部順序改變才會觸發事件
        public ObservableCollection <Block> HomePageBlocks
        {
            get { return _homePageBlocks; }
            set 
            { 
                _homePageBlocks = value;
                //OnPropertyChanged();
            }
        }
        

    }
    public class Block
    {
        public int blockW { get; set; }//要有 get set Binding才拿的到
        public int blockH { get; set; }
        public string Name { get; set; }

        public Color blockColor { get; set; }
        public Brush brush { get; set; }
        public Block(string name, int w, int h, Color c)
        {
            Name = name;
            blockW = w;
            blockH = h;
            blockColor = c;
            var converter = new BrushConverter();
            brush = (Brush)converter.ConvertFromString(c.ToString());
        }
    }



}

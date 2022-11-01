using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ModernDesign.MVVM.ViewModel
{
    class HomeViewModel
    {
        public HomeViewModel()
        {
            _homePageBlocks.Add(new Block("A", 400, 200, Colors.BlueViolet));
            _homePageBlocks.Add(new Block("B", 200, 200, Colors.Gold));
            _homePageBlocks.Add(new Block("C", 200, 200, Colors.DarkGray));
        }

        private List<Block> _homePageBlocks = new List<Block>();

        public List<Block> HomePageBlocks
        {
            get { return _homePageBlocks; }
            set { _homePageBlocks = value; }
        }
        

    }
    public class Block
    {
        public int blockW { get; set; }
        public int blockH { get; set; }
        public string Name { get; set; }

        public Color blockColor { get; set; }
        public Block(string name, int w, int h, Color c)
        {
            Name = name;
            blockW = w;
            blockH = h;
            blockColor = c;
        }
    }



}

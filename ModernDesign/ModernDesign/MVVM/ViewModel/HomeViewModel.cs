using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernDesign.MVVM.ViewModel
{
    class HomeViewModel
    {
        public HomeViewModel()
        {
            _homePageBlocks.Add(new Block("A", 400, 200));
            _homePageBlocks.Add(new Block("B", 200, 200));
            _homePageBlocks.Add(new Block("C", 200, 200));
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
        public Block(string name, int w, int h)
        {
            Name = name;
            blockW = w;
            blockH = h;
        }
    }



}

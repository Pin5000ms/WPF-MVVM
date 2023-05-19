using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ModernDesign
{
    public class GreenBlockAction : BlockActionBase
    {
        public override void Run(DataFlow data)
        {
            Console.WriteLine("Green");
            Thread.Sleep(50);
            OutputAction?.Invoke(data);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ModernDesign
{
    public class BlockActionBase
    {
        public Action<DataFlow> OutputAction;
        public Action<DataFlow> InputAction;

        public BlockActionBase()
        {
            InputAction = Run;
        }
        public virtual void Run(DataFlow data)
        {
            Console.WriteLine(data.Id);
            Thread.Sleep(50);
            OutputAction?.Invoke(data);
        }
    }
}

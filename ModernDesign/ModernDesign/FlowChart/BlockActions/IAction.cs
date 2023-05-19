using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernDesign
{
    public interface IAction
    {
        Action<DataFlow> InputAction { get; set; }
        Action<DataFlow> OutputAction { get; set; }

        void Run(DataFlow dataFlow);
    }
}

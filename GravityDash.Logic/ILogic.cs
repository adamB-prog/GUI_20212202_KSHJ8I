using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GravityDash.Logic
{
    public interface ILogic
    {
        void Tick();

        void GetKeyDown(KeyEventArgs e);

        void GetKeyUp(KeyEventArgs e);

    }
}

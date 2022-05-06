using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GravityDash.Logic.Input
{
    public interface IInput
    {
        public List<string> ActiveMovements { get; }

        void GetKeyUp(KeyEventArgs e);
        void GetKeyDown(KeyEventArgs e);
    }
}

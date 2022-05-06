using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GravityDash.Logic.Input
{
    public class KeyboardInput : IInput
    {
        public List<string> ActiveMovements { get; private set; }


        public KeyboardInput()
        {
            ActiveMovements = new List<string>();
        }


        public void GetKeyUp(KeyEventArgs e)
        {
            ActiveMovements.Remove(e.Key.ToString());
            foreach (var x in ActiveMovements)
            {
                Debug.WriteLine(x);
            }

            Debug.WriteLine("-----------(UP)");
        }

        public void GetKeyDown(KeyEventArgs e)
        {
            if (!ActiveMovements.Contains(e.Key.ToString()))
            {
                ActiveMovements.Add(e.Key.ToString());
            }

            foreach (var x in ActiveMovements)
            {
                Debug.WriteLine(x);
            }
            Debug.WriteLine("-----------(DOWN)");
        }
    }
}

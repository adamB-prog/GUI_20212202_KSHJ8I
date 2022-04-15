using GravityDash.Logic;
using GravityDash.Logic.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GravityDash.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Stopwatch s = new Stopwatch();
        IGameModel model;
        ILogic logic;
        public MainWindow()
        {
            InitializeComponent();

            model = new GameModel();
            logic = new InGameLogic(model, new KeyboardInput());

            display.SetupModel(model);

            CompositionTarget.Rendering += Render;

            var ts = new Task(() => {

                //double time = 0;
                while (true)
                {
                    logic.Tick(); Thread.Sleep(1000 / 60);

                    //display.vmx = (int)(100 * Math.Sin(time));
                    //time += 0.1;
                    
                }
            }, TaskCreationOptions.LongRunning);
            ts.Start();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
           
            logic.GetKeyDown(e);

        }
        private void Render(object sender, EventArgs e)
        {
            s.Reset();
            s.Start();
            
            display.InvalidateVisual();
            s.Stop();
            Title = ((int)(1 / s.Elapsed.TotalMilliseconds)).ToString();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            
            logic.GetKeyUp(e);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            display.Resize(this.ActualWidth, this.ActualHeight);
        }
    }
}

using GravityDash.Logic;
using GravityDash.Logic.Input;
using GravityDash.Models.Interfaces;
using GravityDash.Renderer;
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
        ViewPort viewport;
        public MainWindow()
        {
            InitializeComponent();

            model = new GameModel();
            logic = new InGameLogic(model, new KeyboardInput());


            viewport = new ViewPort(0, 0, (int)display.ActualWidth, (int)display.ActualHeight, model.PlayerRepository.ReadPlayer(1));

            display.SetupViewPort(viewport);
            display.SetupModel(model);//



            CompositionTarget.Rendering += Render;

            var ts = new Task(() => {

                
                while (true)
                {
                    logic.Tick();
                    viewport.Follow(); 
                    Thread.Sleep(1000 / 60);
                   
                }
            }, TaskCreationOptions.LongRunning);
            ts.Start();

            var spawnerTask = new Task(() =>
            {
                Thread.Sleep(4000);
                while (true)
                {
                    logic.CbSpawner();
                }
            }, TaskCreationOptions.LongRunning);
            spawnerTask.Start();

            var animationTask = new Task(() =>
            {

                while (true)
                {
                    logic.PlayerAnimation();
                }
            }, TaskCreationOptions.LongRunning);
            animationTask.Start();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
           
            logic.GetKeyDown(e);

        }
        private void Render(object sender, EventArgs e)
        {
            s.Reset();
            s.Start();
            //viewport.Move();
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

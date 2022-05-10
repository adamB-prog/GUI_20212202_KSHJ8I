using GravityDash.Data;
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
        ScoreData data = new ScoreData();
        Stopwatch s = new Stopwatch();
        IGameModel model;
        ILogic logic;
        IViewPort viewport;
        CancellationTokenSource source = new CancellationTokenSource();
        public MainWindow()
        {
            InitializeComponent();

            // NewGame();
            UpdateScoreList();
            highscore_label.Content = data.GetHighScore();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (logic is not null)
            {
                logic.GetKeyDown(e);
            }
           

        }
        private void Render(object sender, EventArgs e)
        {
            
            if (logic is not null && logic.GameOver && gameover_label.Visibility == Visibility.Hidden)
            {
                GameOver();
            }
            
            display.InvalidateVisual();
            stopwatch_label.Content = string.Format("{0}:{1}.{2}",s.Elapsed.Minutes, s.Elapsed.Seconds < 10 ? "0" + s.Elapsed.Seconds: s.Elapsed.Seconds, s.Elapsed.Milliseconds);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (logic is not null)
            {
                logic.GetKeyUp(e);
            }   
            
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            display.Resize(this.ActualWidth, this.ActualHeight);
        }

        private void NewGame()
        {
            model = new GameModel();
            logic = new InGameLogic(model, new KeyboardInput());
            source = new CancellationTokenSource();

            viewport = new ViewPort(0, 0, (int)display.ActualWidth, (int)display.ActualHeight, model.PlayerRepository.ReadPlayer(1));

            display.SetupViewPort((ViewPort)viewport);
            display.SetupModel(model);

            CompositionTarget.Rendering += Render;
            HideUI();

            s.Reset();
            s.Start();

            var ts = new Task(() => {


                while (!logic.GameOver)
                {
                    logic.Tick();
                    viewport.Follow();
                    Thread.Sleep(1000 / 60);

                }
            }, TaskCreationOptions.LongRunning);           
            ts.Start();


            CancellationTokenSource source22 = source;
            var spawnerTask = new Task(() =>
            {          
                Thread.Sleep(2000);
                while (!source22.IsCancellationRequested)
                {
                    logic.CbSpawner(source22.Token);          
                }
            }, source22.Token, TaskCreationOptions.LongRunning);
            spawnerTask.Start();

            var animationTask = new Task(() =>
            {
                while (!logic.GameOver)
                {
                    logic.PlayerAnimation();
                }
            }, TaskCreationOptions.LongRunning);
            animationTask.Start();       
        }

        private void GameOver()
        {
            source.Cancel();
            s.Stop();
            data.AddScore(s.Elapsed);
            data.SaveScores();
            


            CompositionTarget.Rendering -= Render;
            highscore_label.Content = data.GetHighScore();
            UpdateScoreList();
            ShowUI();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
            mePlayer.IsEnabled = false;

            HideUI();
        }
        private void ShowUI()
        {
            highscorelist_textblock.Visibility = Visibility.Visible;
            scores_label.Visibility = Visibility.Visible;
            gameover_label.Visibility = Visibility.Visible;
            newgame_button.Visibility = Visibility.Visible;
        }
        private void HideUI()
        {
            gameover_label.Visibility = Visibility.Hidden;
            newgame_button.Visibility = Visibility.Hidden;
            highscorelist_textblock.Visibility = Visibility.Hidden;

            scores_label.Visibility = Visibility.Hidden;
        }

        private void UpdateScoreList()
        {
            highscorelist_textblock.Text = data.GetScoreList();
        }

        private void mePlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            mePlayer.Position = TimeSpan.Zero;
        }
    }
}

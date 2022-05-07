using GravityDash.Logic.Input;
using GravityDash.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GravityDash.Logic
{
    public class InGameLogic : ILogic
    {
        IGameModel model;
        IInput input;
        const int MAXFALLSPEED = 5;
        const float GRAVITY = 0.05f;

        public bool GameOver { get; set; }


        public int Animation = 2; //0: idle,   1: running left,   2: running right,   3: jump
        int lastAnimation = 0;
        int lastFrame = 0;
        public CannonBall cbToShoot { get; set; }//repo
        Random rnd = new Random();
        public InGameLogic(IGameModel model, IInput input)
        {
            this.input = input;
            this.model = model;
            this.GameOver = false;
        }
        public void PlayerAnimation()
        {
            
            if (Animation == lastAnimation)//continue with next frame
            {
                if(Animation == 3)
                {
                    //17-20 jobbra ugrik
                    //21-24 balra ugrik
                    for (int i = 17; i < 21; i++)
                    {
                        model.PlayerRepository.UpdatePlayer(input.ActiveMovements.Contains("A") ? (i + 4) : i);
                        Thread.Sleep(250);
                    }
                }
                else if (Animation == 0)
                {
                    //13-16 között váltogatni
                    if (lastFrame == 17)
                        lastFrame = 13;
                    model.PlayerRepository.UpdatePlayer(lastFrame);
                    lastFrame++;
                    Thread.Sleep(100);
                }
                else if (Animation == 2)
                {
                    //1-6 között váltogatni
                    if (lastFrame == 7)
                        lastFrame = 1;
                    model.PlayerRepository.UpdatePlayer(lastFrame);
                    lastFrame++;
                    Thread.Sleep(100);
                }
                else if(Animation == 1)
                {
                    //7-12 között vált
                    if (lastFrame == 13)
                        lastFrame = 7;
                    model.PlayerRepository.UpdatePlayer(lastFrame);
                    lastFrame++;
                    Thread.Sleep(100);
                }
            }
            else
            {
                lastAnimation = Animation;
                if (Animation == 0)
                {
                    lastFrame = 13;
                    model.PlayerRepository.UpdatePlayer(lastFrame++);
                    Thread.Sleep(100);
                }
                if (Animation == 2)
                {
                    lastFrame = 1;
                    model.PlayerRepository.UpdatePlayer(lastFrame++);
                    Thread.Sleep(100);
                }
                if (Animation == 1)
                {
                    lastFrame = 7;
                    model.PlayerRepository.UpdatePlayer(lastFrame++);
                    Thread.Sleep(100);
                }
            }
        }
    

        public void CbSpawner(CancellationToken token)
        {
            Point playerCoordinates = new Point(model.PlayerRepository.ReadPlayer(1).X, model.PlayerRepository.ReadPlayer(1).Y);
            int cbX, cbY, angle;
            int spawnOption = rnd.Next(0, 5);
            Debug.WriteLine($"{spawnOption}. spawnoption");
            if (spawnOption == 0)
            {
                //balról jön
                cbX = (int)playerCoordinates.X - 200 + (rnd.Next(0,2)==1 ? -1* rnd.Next(0,25) : rnd.Next(0, 25));
                cbY = (int)playerCoordinates.Y - 50 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 5) : rnd.Next(0, 5));
                angle = 0;
                Spawn(cbX, cbY, angle);
            }
            else if (spawnOption == 1)
            {
                //jobbról jön
                cbX = (int)playerCoordinates.X + 200 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 25) : rnd.Next(0, 25));
                cbY = (int)playerCoordinates.Y - 50 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 5) : rnd.Next(0, 5));
                angle = 180;
                Spawn(cbX, cbY, angle);
            }
            else if (spawnOption == 2 && playerCoordinates.Y > 500)
            {
                //combo balról
                cbX = (int)playerCoordinates.X - 200 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 25) : rnd.Next(0, 25));
                cbY = (int)playerCoordinates.Y - 50 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 5) : rnd.Next(0, 5));
                angle = 285;
                Spawn(cbX, cbY, angle);
                cbX = (int)playerCoordinates.X - 200 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 25) : rnd.Next(0, 25));
                cbY = (int)playerCoordinates.Y - 50 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 5) : rnd.Next(0, 5));
                angle = 0;
                Spawn(cbX, cbY, angle);
                cbY = (int)playerCoordinates.Y - 90 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 5) : rnd.Next(0, 5));
                Spawn(cbX, cbY, angle);

            }
            else if (spawnOption == 3 && playerCoordinates.Y > 500)
            {
                //combo jobbról
                cbX = (int)playerCoordinates.X + 200 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 25) : rnd.Next(0, 25));
                cbY = (int)playerCoordinates.Y - 50 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 5) : rnd.Next(0, 5));
                angle = 265;
                Spawn(cbX, cbY, angle);
                cbX = (int)playerCoordinates.X + 200 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 25) : rnd.Next(0, 25));
                cbY = (int)playerCoordinates.Y - 50 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 5) : rnd.Next(0, 5));
                angle = 180;
                Spawn(cbX, cbY, angle);
                cbY = (int)playerCoordinates.Y - 90 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 5) : rnd.Next(0, 5));
                Spawn(cbX, cbY, angle);
            }
            else if (spawnOption >= 4 &&  playerCoordinates.Y > 500)
            {
                //Szönyegbomba
                cbX = (int)playerCoordinates.X - 300 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 25) : rnd.Next(0, 25));
                cbY = (int)playerCoordinates.Y - 50 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 5) : rnd.Next(0, 5));
                angle = 0;
                Spawn(cbX, cbY, angle, 500);
                cbX = (int)playerCoordinates.X - 300 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 25) : rnd.Next(0, 25));
                cbY = (int)playerCoordinates.Y - 100 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 5) : rnd.Next(0, 5));
                angle = 0;
                Spawn(cbX, cbY, angle, 500);
                cbX = (int)playerCoordinates.X - 300 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 25) : rnd.Next(0, 25));
                cbY = (int)playerCoordinates.Y - 150 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 5) : rnd.Next(0, 5));
                angle = 0;
                Spawn(cbX, cbY, angle, 600);
                cbX = (int)playerCoordinates.X - 300 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 25) : rnd.Next(0, 25));
                cbY = (int)playerCoordinates.Y - 200 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 5) : rnd.Next(0, 5));
                angle = 0;
                Spawn(cbX, cbY, angle, 700);
                cbX = (int)playerCoordinates.X - 300 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 25) : rnd.Next(0, 25));
                cbY = (int)playerCoordinates.Y - 250 + (rnd.Next(0, 2) == 1 ? -1 * rnd.Next(0, 5) : rnd.Next(0, 5));
                angle = 0;
                Spawn(cbX, cbY, angle, 800);
            }
        }

        private void Spawn(int cbX, int cbY, int angle, int sleepTime = 0)
        {
            cbToShoot = new CannonBall(cbX, cbY, angle);
            double rad = (cbToShoot.Angle) * (Math.PI / 180);
            double dx = (Math.Cos(rad) < 0.0001 && Math.Cos(rad) > -0.0001 ? 0 : Math.Cos(rad));
            double dy = Math.Sin(rad);
            Vector2 v = new Vector2((float)dx * 10, (float)(rad > 0 && rad < Math.PI ? dy * 5 : 0));
            cbToShoot.Velocity = v;
            model.LevelRepository.UpdateCbToShoot(cbToShoot);
            if (sleepTime == 0)
                Thread.Sleep(rnd.Next(850, 1151));
            else
                Thread.Sleep(sleepTime);
            model.LevelRepository.AddCb(cbToShoot);
            cbToShoot = null;
            CannonBall defaultCb = new CannonBall(-50, -50, 0);
            model.LevelRepository.UpdateCbToShoot(defaultCb);
        }

        public void Tick()
        {
            CalculateNewVectors();
            foreach (var x in model.PlayerRepository.ReadAllPlayer())
            {
                x.X += x.Velocity.X;
                x.Y += x.Velocity.Y;
                if (OutOfLevel(x))
                {
                    GameOver = true;
                }
            }
            foreach (var x in model.LevelRepository.level.CannonBalls.Where(cb => !cb.Ignore))
            {
                x.Move();
                Size levelArea = new Size(model.LevelRepository.level.Width * 32, model.LevelRepository.level.Height * 32);
                if (x.Outside(levelArea))
                {
                    CannonBall defaultCb = new CannonBall(-50, -50, 0);
                    x.SetToDefault(defaultCb);
                }
            }

            foreach (var platform in model.LevelRepository.level.Platforms)
            {
                foreach (var player in model.PlayerRepository.ReadAllPlayer())
                {
                    if (player.IsCollision(platform))
                        player.Collision(platform);
                }

                foreach (var cb in model.LevelRepository.level.CannonBalls.Where(cb => !cb.Ignore))
                {
                    if (platform.IsCollision(cb))
                    {
                        cb.Collision(platform);
                    }
                }
            }

            foreach (var cb in model.LevelRepository.level.CannonBalls.Where(cb => !cb.Ignore))
            {
                foreach (var cb2 in model.LevelRepository.level.CannonBalls.Where(cb => !cb.Ignore))
                {
                    if(cb != cb2)
                    {
                        if (cb.IsCollision(cb2))
                            cb2.Collision(cb);
                    }
                }
                foreach (var player in model.PlayerRepository.ReadAllPlayer())
                {
                    if (cb.IsCollision(player))
                        cb.Collision(player);
                }
            }
        }

        private bool OutOfLevel(Player x)
        {
            return x.Y > 1000;
        }

        public void GetKeyDown(KeyEventArgs e)
        {
            input.GetKeyDown(e);
        }
        public void GetKeyUp(KeyEventArgs e)
        {
            input.GetKeyUp(e);
        }
        private void CalculateNewVectors()
        {
            foreach (var player in model.PlayerRepository.ReadAllPlayer())
            {
                //Gravitáció
                player.Velocity = Vector2.Add(player.Velocity, new Vector2(0, GRAVITY)).Y < MAXFALLSPEED ? Vector2.Add(player.Velocity, new Vector2(0, GRAVITY)) : new Vector2(player.Velocity.X, MAXFALLSPEED);

                if (input.ActiveMovements.Contains("A") && input.ActiveMovements.Contains("D")) 
                {
                    if (player.Velocity.X == -5 || player.Velocity.X == 5)
                        player.Velocity = new Vector2(0, player.Velocity.Y);
                    Animation = 0;
                }
                else if (input.ActiveMovements.Contains("A"))
                {
                    if (player.Velocity.X > 0)
                    {
                        player.Velocity = Vector2.Add(player.Velocity, new Vector2(-0.1f, 0));
                    }
                    else { player.Velocity = new Vector2(-5, player.Velocity.Y); }

                    if (!input.ActiveMovements.Contains("W"))
                        Animation = 1;
                }
                else if (input.ActiveMovements.Contains("D"))
                {
                    if (player.Velocity.X < 0)
                    {
                        player.Velocity = Vector2.Add(player.Velocity, new Vector2(0.1f, 0));
                    }
                    else { player.Velocity = new Vector2(5, player.Velocity.Y); }

                    if (!input.ActiveMovements.Contains("W"))
                        Animation = 2;
                }
                else 
                {
                    if(!input.ActiveMovements.Contains("W"))
                        Animation = 0; 
                }

                if (!input.ActiveMovements.Contains("A"))
                {
                    if (player.Velocity.X == -5)
                        player.Velocity = new Vector2(0, player.Velocity.Y);
                }
                if (!input.ActiveMovements.Contains("D"))
                {
                    if (player.Velocity.X == 5)
                        player.Velocity = new Vector2(0, player.Velocity.Y);
                }

                if (input.ActiveMovements.Contains("W"))
                {
                    if (player.CanJump)
                    {
                        player.Velocity = new Vector2(player.Velocity.X, -3);
                        player.CanJump = false;
                        Animation = 3;
                    }
                }

                if (input.ActiveMovements.Contains("S"))
                {
                    if(player.Radius == 16)//default radius
                    {
                        player.Radius = 12;
                        player.Y += 4;
                    }
                }
                else
                {
                    if(player.Radius == 12)
                    {
                        player.Radius = 16;
                        player.Y -= 4;
                    } 
                }
            }
        }
    }
}
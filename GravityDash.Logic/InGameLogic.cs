using GravityDash.Logic.Input;
using GravityDash.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace GravityDash.Logic
{
    public class InGameLogic : ILogic
    {
        IGameModel model;
        IInput input;
        const int MAXFALLSPEED = 5;
        const float GRAVITY = 0.05f;
        public CannonBall cbToShoot { get; set; }//repo
        public InGameLogic(IGameModel model, IInput input)
        {
            this.input = input;
            this.model = model;
        }

        public void Tick()
        {
            CalculateNewVectors();
            HandleNumberInputs();//
            foreach (var x in model.PlayerRepository.ReadAllPlayer())
            {
                x.X += x.Velocity.X;
                x.Y += x.Velocity.Y;
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
                Debug.WriteLine($"Active cb count: {model.LevelRepository.level.CannonBalls.Where(cb => !cb.Ignore).Count()}");
            }

            //Collision check and resolve  model.PlayerRepository.ReadAllPlayer()
            foreach (var platform in model.LevelRepository.level.Platforms)
            {
                foreach (var player in model.PlayerRepository.ReadAllPlayer())
                {
                    if (player.IsCollision(platform)) //TODO: player osztaly kezelje le Collision() metodussal átláthatóságért
                    {
                        if (player.X > platform.X + platform.Width)
                        {
                            player.X = platform.X + platform.Width + player.Radius;
                            player.Velocity = new Vector2(0, player.Velocity.Y);
                            player.CanJump = true;
                        }
                        else if (player.X < platform.X)
                        {
                            player.X = platform.X - player.Radius;
                            player.Velocity = new Vector2(0, player.Velocity.Y);
                            player.CanJump = true;
                        }
                        else if (player.Y < platform.Y)
                        {
                            player.Y = platform.Y - (player.Radius);
                            player.Velocity = new Vector2(player.Velocity.X, 0);
                            if (player.CanJump == false)
                                player.CanJump = true;
                        }
                        else if (player.Y > platform.Y)
                        {
                            player.Y = platform.Y + platform.Height + player.Radius;
                            player.Velocity = new Vector2(player.Velocity.X, 0);
                        }

                    }
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

        public void GetKeyDown(KeyEventArgs e)
        {
            input.GetKeyDown(e);
        }
        public void GetKeyUp(KeyEventArgs e)
        {
            input.GetKeyUp(e);
        }

        private void HandleNumberInputs()//
        {
            //Debug.WriteLine(cbToShoot == null ? "Nulla" : "Van CB");
            if (input.ActiveMovements.Contains("D1"))
            {
                if (cbToShoot == null)
                {
                    cbToShoot = new CannonBall(-50, -50, 90);
                }
            }
            if (input.ActiveMovements.Contains("D2"))
            {
                if (cbToShoot == null)
                {
                    cbToShoot = new CannonBall(-50, -50, 180);
                }

            }
            if (input.ActiveMovements.Contains("D3"))
            {
                if (cbToShoot == null)
                {
                    cbToShoot = new CannonBall(-50, -50, 0);
                }
            }
            if (input.ActiveMovements.Contains("Q"))
            {
                if (cbToShoot != null)
                {
                    cbToShoot = new CannonBall(-50, -50, 0);
                    model.LevelRepository.UpdateCbToShoot(cbToShoot);
                    cbToShoot = null;
                }
            }
        }
        public void CbMove(Point position)
        {
            
            cbToShoot.X = position.X;
            cbToShoot.Y = position.Y;
            model.LevelRepository.UpdateCbToShoot(cbToShoot);
            //Debug.WriteLine($"{model.LevelRepository.ReadCbToShoot().X}:{model.LevelRepository.ReadCbToShoot().Y}");
        }

        public void CbRotate(int delta)
        {
            if (delta > 0)
            {
                cbToShoot.Angle += 15;
                model.LevelRepository.UpdateCbToShoot(cbToShoot);
            }
            else
            {
                cbToShoot.Angle -= 15;
                model.LevelRepository.UpdateCbToShoot(cbToShoot);
            }
        }

        public void CbShoot()
        {
            double rad = (cbToShoot.Angle) * (Math.PI / 180);
            double dx = (Math.Cos(rad) < 0.0001 && Math.Cos(rad) > -0.0001 ? 0 : Math.Cos(rad));
            double dy = Math.Sin(rad);
            Vector2 v = new Vector2((float)dx * 10, (float)(rad > 0 && rad < Math.PI ? dy * 5 : 0));
            cbToShoot.Velocity = v;
            model.LevelRepository.AddCb(cbToShoot);

            cbToShoot = null;
            CannonBall defaultCb = new CannonBall(-50, -50, 0);
            model.LevelRepository.UpdateCbToShoot(defaultCb);
        }

        private void CalculateNewVectors()
        {
            //TODO: JUST FOR ME, not for everybody(when networking comes)
            foreach (var player in model.PlayerRepository.ReadAllPlayer())
            {
                //Gravitáció
                player.Velocity = Vector2.Add(player.Velocity, new Vector2(0, GRAVITY)).Y < MAXFALLSPEED ? Vector2.Add(player.Velocity, new Vector2(0, GRAVITY)) : new Vector2(player.Velocity.X, MAXFALLSPEED);

                if (input.ActiveMovements.Contains("A") && input.ActiveMovements.Contains("D")) 
                {
                    if (player.Velocity.X == -5 || player.Velocity.X == 5)
                        player.Velocity = new Vector2(0, player.Velocity.Y);
                }
                else if (input.ActiveMovements.Contains("A"))
                {
                    if (player.Velocity.X > 0)
                    {
                        player.Velocity = Vector2.Add(player.Velocity, new Vector2(-0.1f, 0));
                    }
                    else { player.Velocity = new Vector2(-5, player.Velocity.Y); }
                }
                else if (input.ActiveMovements.Contains("D"))
                {
                    if (player.Velocity.X < 0)
                    {
                        player.Velocity = Vector2.Add(player.Velocity, new Vector2(0.1f, 0));
                        Debug.WriteLine($"VelocityX: {player.Velocity.X}");
                    }
                    else { player.Velocity = new Vector2(5, player.Velocity.Y); }
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
                    }
                }
                if (input.ActiveMovements.Contains("S"))
                {
                    //TODO: change characterSkin to the crouching one
                    if(player.Radius == 16) //default radius
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
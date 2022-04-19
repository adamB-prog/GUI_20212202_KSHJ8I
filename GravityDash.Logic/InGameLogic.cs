using GravityDash.Logic.Input;
using GravityDash.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public InGameLogic(IGameModel model, IInput input)
        {
            this.input = input;
            this.model = model;

        }

        public void Tick()
        {
            CalculateNewVectors();
            foreach (var x in model.PlayerRepository.ReadAllPlayer())
            {
                x.X += x.Velocity.X;
                x.Y += x.Velocity.Y;
            }

            //Collision check and resolve
            foreach (var player in model.PlayerRepository.ReadAllPlayer())
            {
                foreach (var platform in model.LevelRepository.level.Platforms)
                {
                    if (player.IsCollision(platform))
                    {
                        //Debug.WriteLine("COLLISION");
                        if(player.X > platform.X + platform.Width)//player collides from the right side
                        {
                            player.X = platform.X + platform.Width + player.Radius;
                            player.Velocity = new Vector2(0, player.Velocity.Y);
                            player.CanJump = true;
                        }
                        else if(player.X < platform.X) //player collides from the left side
                        {
                            player.X = platform.X - player.Radius;
                            player.Velocity = new Vector2(0, player.Velocity.Y);
                            player.CanJump = true;
                        }
                        else if(player.Y < platform.Y)//player is above the platform
                        {
                            player.Y = platform.Y - (player.Radius);
                            player.Velocity = new Vector2(player.Velocity.X, 0);
                            if (player.CanJump == false)
                                player.CanJump = true;
                        }
                        else if(player.Y>platform.Y)//payer is below the platform
                        {
                            player.Y = platform.Y + platform.Height + player.Radius;
                            player.Velocity = new Vector2(player.Velocity.X, 0);
                        }
                        
                    }
                        
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
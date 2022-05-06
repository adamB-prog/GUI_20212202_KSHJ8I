using GravityDash.Logic.Input;
using GravityDash.Models;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Input;
using System.Windows.Media;


namespace GravityDash.Logic
{
    public class InGameLogic : ILogic
    {
        IGameModel model;
        IInput input;
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




                x.X += (int)x.Velocity.X;
                x.Y += (int)x.Velocity.Y;
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
                int vx = 0, vy = 0;
                if (input.ActiveMovements.Contains("W"))
                {
                    vy -= 3;
                }
                if (input.ActiveMovements.Contains("S"))
                {
                    vy += 3;
                }
                if (input.ActiveMovements.Contains("A"))
                {
                    vx -= 3;
                }
                if (input.ActiveMovements.Contains("D"))
                {
                    vx += 3;
                }

                player.Velocity = new Vector2(vx, vy);
            }
        }
    }
}
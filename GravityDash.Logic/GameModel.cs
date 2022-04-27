using GravityDash.Models;
using GravityDash.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravityDash.Logic
{
    public class GameModel : IGameModel
    {
        public LevelRepository LevelRepository { get; set; }
        public PlayerRepository PlayerRepository { get; set; }

        public GameModel()
        {
            PlayerRepository = new PlayerRepository();

            LevelRepository = new LevelRepository();
        }
    }
}

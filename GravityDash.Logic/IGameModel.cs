using GravityDash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GravityDash.Repository;

namespace GravityDash.Logic
{
    public interface IGameModel
    {
        LevelRepository LevelRepository { get; set; }


        PlayerRepository PlayerRepository { get; set; }

    }
}

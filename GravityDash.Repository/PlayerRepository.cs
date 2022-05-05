using GravityDash.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GravityDash.Repository
{
    public class PlayerRepository
    {

        IList<Player> players;
        public PlayerRepository()
        {
            players = new List<Player>();
            //For TEST PURPOSES
            players.Add(new Player()
            {
                X = 800,
                Y = 350,
                ID = 1,
                Name = "Slampa",
                //Character = new SolidColorBrush(Color.FromArgb(133, 255, 0, 0)),
                Character = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Asset", "player.png"), UriKind.RelativeOrAbsolute))),
                Radius = 16,
                CanJump = true
            }
           );
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            players.Remove(player);
        }

        public Player ReadPlayer(int id)
        {
            return players.FirstOrDefault(x => x.ID == id);
        }

        public IEnumerable<Player> ReadAllPlayer()
        {
            return players;
        }

        public void UpdatePlayer(Player player)
        {
            var old = ReadPlayer(player.ID);
            old.X = player.X;
            old.Y = player.Y;
            old.Velocity = old.Velocity;
        }
    }
}

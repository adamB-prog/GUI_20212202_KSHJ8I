using GravityDash.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace GravityDash.Repository
{
    public class LevelRepository
    {
        public Level level { get; private set; }
        public LevelRepository()
        {

            //FOR TEST PURPOSES
            level = LoadLevel("Asset/test.tmx");
            level.Brushes = LoadBrushes("Asset/tileset_64x64(new).png");
            SetupPlatforms();
        }

        void SetupPlatforms()
        {
            List<Platform> l = new List<Platform>();
            string[] rows = File.ReadAllLines(Path.Combine("Asset", "platforms01.txt"));
            foreach (var row in rows)
            {
                string[] data = row.Split(',');
                l.Add(new Platform(
                    int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3])
                    ));
            }
            level.Platforms = l;
        }

        public Level LoadLevel(string path)
        {
            XDocument file = XDocument.Load(path);

            Level level = new Level();

            level.Width = int.Parse(file.Root.Attribute("width").Value);
            level.Height = int.Parse(file.Root.Attribute("height").Value);
            foreach (var item in file.Descendants("layer"))
            {
                if (item.Attribute("name").Value == "Collision")
                {
                    level.CollisionLayer = FormatCopy(item.Value, level.Width, level.Height);
                }
                else if (item.Attribute("name").Value == "Drawing")
                {
                    level.DrawingLayer = FormatCopy(item.Value, level.Width, level.Height);
                }
            }

            return level;
        }
        private int[,] FormatCopy(string data, int width, int height)
        {
            int[,] level = new int[height, width];
            string[] split = data.Split(",");
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    level[i, j] = int.Parse(split[i * width + j]);

                }
            }

            return level;
        }


        public Brush[] LoadBrushes(string path)
        {
            ImageBrush[] brushes = new ImageBrush[72];
            #region test
            //{
            //    new SolidColorBrush(Color.FromArgb(255,0,0,255)),
            //    new SolidColorBrush(Color.FromArgb(255,255,0,0)),
            //    new SolidColorBrush(Color.FromArgb(255,0,255,0)),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(Color.FromArgb(255,0,0,255)),
            //    new SolidColorBrush(Color.FromArgb(255,255,0,128)),
            //    new SolidColorBrush(Color.FromArgb(255,255,0,50)),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(Color.FromArgb(255,255,0,255)),
            //    new SolidColorBrush(Color.FromArgb(255,128,0,128)),
            //    new SolidColorBrush(Color.FromArgb(255,0,255,255)),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //    new SolidColorBrush(),
            //};
            #endregion


            for (int i = 1; i < 72; i++)
            {
                brushes[i - 1] = new ImageBrush(new BitmapImage(new Uri("Asset/asd/" + i + ".png", UriKind.RelativeOrAbsolute)));
            }

            return brushes;
        }

        public int ReadLevelData(int width, int height)
        {
            return level.DrawingLayer[width, height];
        }
    }
}


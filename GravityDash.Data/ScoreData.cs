using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GravityDash.Data
{
    public class ScoreData
    {
        public IList<TimeSpan> Scores { get; set; }


        public ScoreData()
        {
            StreamReader s = new StreamReader("Scores.txt");
            Scores = new List<TimeSpan>();
            while (!s.EndOfStream)
            {
                TimeSpan time = TimeSpan.Parse(s.ReadLine());
                Scores.Add(time);
            }

            Scores = Scores.OrderByDescending(x => x.TotalMilliseconds).ToList();
        }

        public void SaveScores()
        {
            IList<string> scores = new List<string>();

            for (int i = 0; i < Scores.Count; i++)
            {
                scores.Add(Scores[i].ToString());
            }
            File.Delete("Scores.txt");
            File.WriteAllLines("Scores.txt", scores);
        }

        public void AddScore(TimeSpan score)
        {
            Scores.Add(score);
            Scores = Scores.OrderByDescending(x => x.TotalMilliseconds).ToList();
        }

        public string GetHighScore()
        {
            if (Scores.Count == 0)
            {
                return "0:00.00";
            }
            TimeSpan score = Scores[0];
            return string.Format("HighScore: {0}:{1}.{2}", score.Minutes, score.Seconds < 10 ? "0" + score.Seconds : score.Seconds, score.Milliseconds);
        }

        public string GetScoreList()
        {
            string scores = "";
            for (int i = 0; i < Scores.Count; i++)
            {
                scores += string.Format("{0}:{1}.{2}\n", Scores[i].Minutes, Scores[i].Seconds < 10 ? "0" + Scores[i].Seconds : Scores[i].Seconds, Scores[i].Milliseconds);
            }

            return scores;
        }
    }
}

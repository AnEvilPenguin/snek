using System;
using System.Collections.Generic;
using System.Text;

namespace Snek
{
    public class HighScore
    {
        public int Score { get; }

        public DateTime Date { get; }

        public string GameOverReason { get; }

        public HighScore(int score, string gameOverReason)
        {
            this.Score = score;
            this.Date = DateTime.Now;
            this.GameOverReason = gameOverReason;
        }

        // TODO toString?
    }

    public class Scores
    {
        public int Version { get; }

        public List<HighScore> scores { get; set; } = new List<HighScore>();

        public Scores()
        {
            Version = 1;
        }

        public void addScore(int score, string gameOverReason)
        {
            this.scores.Add(new HighScore(score, gameOverReason));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (HighScore score in this.scores)
            {
                sb.AppendLine($"{score.Date} - {score.Score} - {score.GameOverReason}");
            }

            return sb.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

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

        [JsonConstructorAttribute]
        public HighScore(int score, DateTime date, string gameOverReason)
        {
            this.Score = score;
            this.Date = date;
            this.GameOverReason = gameOverReason;
        }
    }

    public class Scores
    {
        public int Version { get; }

        public List<HighScore> scores { get; set; } = new List<HighScore>();

        public Scores()
        {
            this.Version = 1;
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

        public List <HighScore> GetScores(int top = 5)
        {
            var sorted = this.scores
                .OrderByDescending(s => s.Score)
                .Take(top)
                .ToList();

            return sorted;
        }
    }
}

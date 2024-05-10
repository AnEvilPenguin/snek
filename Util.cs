namespace Snek
{
    using System;
    using System.Collections.Generic;
    using Godot;

    internal class Util
    {
        internal static Vector2 GetSnappedPosition(int x, int y, int tileSize = 64)
        {
            Vector2 position = new Vector2(x, y);

            // Snapped allows us to round the position to the nearest element
            position = position.Snapped(Vector2.One * tileSize);

            // This centers the player on the tile
            position += Vector2.One * tileSize / 2;

            return position;
        }

        internal static Vector2 GetSnappedPosition(Vector2 position, int tileSize = 64) =>
            GetSnappedPosition((int)position.X, (int)position.Y, tileSize);
    }

    public class HighScore
    {
        public int score;
        public DateTime date;

        public HighScore(int score)
        {
            this.score = score;
            this.date = DateTime.Now;
        }

        // TODO toString?
    }

    public class Scores
    {
        public const int version = 1;

        public List<HighScore> scores;

        public Scores()
        {
            // TODO think about how to instantiate this from a save manager
        }
    }

    internal class SaveManager
    {
        private Scores _scores = new Scores();

        public SaveManager() { }

        public void AddScore(int score)
        {
            // TODO add to list if not full
            // TODO consider dropping scores if more than 5?
        }

        // TODO get top 5 scores
        // TODO get most recent 5 scores?

        // TODO work out how to display high scores in UI.
        // Look at GridContainer and ItemList
        // Build these items into the control
    }
}

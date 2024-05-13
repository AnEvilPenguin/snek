using System;
using System.Text.Json;
using System.IO;

namespace Snek
{
    internal class SaveManager
    {
        private Scores _scores;

        private string _path;

        private string _fileName;

        public SaveManager()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var path = Path.Combine(appData, "EvilPenguinIndustries\\Snek");

            Directory.CreateDirectory(path);

            this._path = path;

            this._fileName = "Saves.json";

            string jsonString = File.ReadAllText(Path.Combine(path, this._fileName));

            this._scores = JsonSerializer.Deserialize<Scores>(jsonString) ?? new Scores();
        }

        public void AddScore(int score, string gameOverReason)
        {
            this._scores.addScore(score, gameOverReason);
            // TODO add to list if not full
            // TODO consider dropping scores if more than 5?
        }

        // TODO get top 5 scores
        // TODO get most recent 5 scores?

        public string getScores()
        {
            return this._scores.ToString();
        }

        public void Persist()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(this._scores, options);

            File.WriteAllText(Path.Combine(_path, _fileName), jsonString);
        }

        // TODO work out how to display high scores in UI.
        // Look at GridContainer and ItemList
        // Build these items into the control
    }
}

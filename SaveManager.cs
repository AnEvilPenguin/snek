namespace Snek
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;

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

            string jsonString = string.Empty;

            string fullpath = Path.Combine(path, this._fileName);

            if (!File.Exists(fullpath))
            {
                return;
            }

            try
            {
                jsonString = File.ReadAllText(fullpath);
            }
            catch (Exception ex)
            {
                Godot.GD.Print(ex.ToString());
            }

            this._scores = JsonSerializer.Deserialize<Scores>(jsonString) ?? new Scores();
        }

        public void AddScore(int score, string gameOverReason) => 
            this._scores.addScore(score, gameOverReason);

        public List<HighScore> GetScores(int top) =>
            this._scores.GetScores(top);

        public void Persist()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(this._scores, options);

            File.WriteAllText(Path.Combine(this._path, this._fileName), jsonString);
        }
    }
}

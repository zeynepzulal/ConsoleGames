using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ConsoleGames
{
    public class Score
    {
        public string GameName { get; set; }
        public int Level { get; set; }
        public int Points { get; set; }
        public string Date { get; set; }
        public bool LevelCompleted { get; set; }

        public bool Save(Game currentGame, Game[] gameArray, string pathHighScoreFile)
        {
            this.GameName = currentGame.Name;
            this.Date = DateTime.Now.ToString();

            // check if new score is new high score
            if (Score.NewScoreIsHighScore(this, currentGame.HighScore, currentGame.TheHigherTheBetter))
            {
                currentGame.HighScore = this;
                Score.WriteHighScores(gameArray, pathHighScoreFile);
                return true;
            }

            return false;
        }

        private static bool NewScoreIsHighScore(Score score, Score currentHighScore, bool theHigherTheBetter)
        {
            if (currentHighScore == null)
            {
                return true;
            }

            if (score.Level > currentHighScore.Level)
            {
                return true;
            }
            else if (score.Level < currentHighScore.Level)
            {
                return false;
            }
            else // i.e. same level: score.Level == currentHighScore.Level
            {
                if (theHigherTheBetter)
                {
                    if (score.Points > currentHighScore.Points)
                    {
                        return true;
                    }
                }
                else
                {
                    if (score.Points < currentHighScore.Points)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static void WriteHighScores(Game[] gameArray, String pathHighScoreFile)
        {
            List<Score> highScoreList = new List<Score>();

            foreach (Game game in gameArray)
            {
                if (game.HighScore != null)
                {
                    highScoreList.Add(game.HighScore);
                }
            }
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize<List<Score>>(highScoreList, serializeOptions);

            String path = Path.GetDirectoryName(pathHighScoreFile);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllText(pathHighScoreFile, json);
        }

        public static void DeleteHighScores(String pathHighScoreFile)
        {
            if (File.Exists(pathHighScoreFile))
            {
                File.Delete(pathHighScoreFile);
            }
        }

        public static void ReadHighScores(Game[] gameArray, String pathHighScoreFile)
        {
            List<Score> highScoreList = new List<Score>();

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            if (File.Exists(pathHighScoreFile))
            {
                string jsonString = File.ReadAllText(pathHighScoreFile);
                highScoreList = JsonSerializer.Deserialize<List<Score>>(jsonString,serializeOptions);
            }
            else
            {
                highScoreList.Clear(); // make sure list is empty
            }

            // set highscores for each game in gameArray
            foreach (Game game in gameArray)
            {
                foreach (Score highScore in highScoreList)
                {
                    if (highScore != null)
                    {
                        if (game.Name == highScore.GameName)
                        {
                            game.HighScore = highScore;
                        }
                    }
                }
            }
        }
    }
}

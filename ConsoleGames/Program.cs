using System;
using System.IO;

// Update Framework: Right click on ConsoleGames on Projektmappe / Eigenschaften / Allgemein / Zielframework

namespace ConsoleGames
{
    public static class Program
    {
        static Game[] gameArray = new Game[] {
            new Games.GuessNumber()
        };

        static Game currentGame;

        static string pathHighScoreFile = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ConsoleGames", "highscore.json");

        static void StartScreen()
        {
            Console.WriteLine("Cooler Start Screen muss noch implementiert werden.");
            Console.WriteLine("Drücke eine Taste, um zum Startmenu zu gelangen.");
            Console.ReadKey();
            StartMenu();
        }
        static void StartMenu()
        {
            // START MENU
            Console.Clear();
            Console.WriteLine("(1) Wähle Game");
            Console.WriteLine("(2) Highscores anschauen");
            Console.WriteLine("(3) Highscores Löschen");
            Console.WriteLine("(q) Uf Wiederluege!");
            Console.WriteLine();
            Console.WriteLine("Wähle Zahl: ");

            ConsoleKeyInfo option = Console.ReadKey();

            if (option.Key == ConsoleKey.D1)
            {
                SelectGameMenu();
            }
            else if (option.Key == ConsoleKey.D2)
            {
                HighScoreMenu();
            }
            else if (option.Key == ConsoleKey.D3)
            {
                DeleteHighScores();
            }
            else if (option.Key == ConsoleKey.Q)
            {
                // QUIT GAME
                Environment.Exit(0);
            }
            StartMenu(); // no valid input: Reload start menu
        }

        private static void DeleteHighScores()
        {
            Console.Clear();
            Console.WriteLine("Bist du sicher, dass du alle HighScores löschen möchtest? (y/n)");
            ConsoleKeyInfo option = Console.ReadKey();
            if (option.Key == ConsoleKey.Y)
            {
                // delete HS saved in game objects
                foreach (Game game in gameArray)
                {
                    game.HighScore = null;
                }
                // delete file
                Score.DeleteHighScores(pathHighScoreFile);
            }
            StartMenu();
        }

        private static void HighScoreMenu()
        {
            Console.Clear();
            foreach (Game game in gameArray)
            {
                if (game.HighScore != null)
                {
                    Console.WriteLine("game: " + game.HighScore.GameName + " , level: " + game.HighScore.Level + " , level completed: " + game.HighScore.LevelCompleted + ", points: " + game.HighScore.Points + ", date: " + game.HighScore.Date);
                }
            }
            Console.WriteLine();
            Console.WriteLine("Zurück zum Menu");
            Console.ReadKey();
        }

        static void SelectGameMenu()
        {
            // MENU TO SELECT A GAME
            Console.Clear();
            Console.WriteLine("Wähle ein Game:");
            for (int i = 0; i < gameArray.Length; i++)
            {
                Console.WriteLine("(" + (i + 1) + ") " + gameArray[i].Name);
            }

            Console.WriteLine();
            Console.WriteLine("(m) Zurück zum Menu");

            string input = " ";
            if (gameArray.Length <= 9)
            {
                ConsoleKeyInfo inpKey = Console.ReadKey();
                try
                {
                    input = inpKey.KeyChar.ToString().ToUpper();
                }
                catch
                {
                    SelectGameMenu();
                }
            }
            else
            {
                try
                {
                    input = Console.ReadLine().ToUpper();
                }
                catch
                {
                    SelectGameMenu();
                }
            }

            Console.Clear();

            if (input == "M")
            {
                StartMenu();
            }
            try
            {
                int i = int.Parse(input) - 1;
                currentGame = gameArray[i];
                Console.Clear();

                NewOrResumeGameMenu();
            }
            catch (Exception)
            {
                Console.WriteLine("Kein erlaubter Input");
            }
            SelectGameMenu();
        }

        private static void NewOrResumeGameMenu()
        {
            Score score = new Score();

            Console.Clear();
            Console.WriteLine("(1) Weiterspielen");
            Console.WriteLine("(2) Level auswählen");
            Console.WriteLine("(3) Game neu starten");
            Console.WriteLine("(4) Game Beschreibung");
            Console.WriteLine("(5) Game Regeln");
            Console.WriteLine("(6) Credits");
            Console.WriteLine("(m) Zurück zum Menu");
            Console.WriteLine();
            Console.WriteLine("Wähle Zahl: ");
            ConsoleKeyInfo option = Console.ReadKey();

            // RESUME GAME
            if (option.Key == ConsoleKey.D1)
            {
                if (currentGame.HighScore != null)
                {
                    if (currentGame.HighScore.LevelCompleted == true)
                    {
                        Console.Clear();
                        score = PlayCurrentGame(currentGame.HighScore.Level + 1);
                    }
                    else
                    {
                        Console.Clear();
                        score = PlayCurrentGame(currentGame.HighScore.Level);
                    }
                }
                else
                {
                    Console.Clear();
                    score = PlayCurrentGame(1);
                }
                score.Save(currentGame, gameArray, pathHighScoreFile);
            }
            else if (option.Key == ConsoleKey.D2)
            {
                SelectLevelMenu();
            }
            else if (option.Key == ConsoleKey.D3)
            {
                Console.Clear();
                score = PlayCurrentGame(1); // play first level
                score.Save(currentGame, gameArray, pathHighScoreFile);
            }
            else if (option.Key == ConsoleKey.D4)
            {
                GameDescription();
            }
            else if (option.Key == ConsoleKey.D5)
            {
                GameRules();
            }
            else if (option.Key == ConsoleKey.D6)
            {
                GameCredits();   
            }
            StartMenu(); // no valid input: Reload start menu
        }

        private static Score PlayCurrentGame(int level)
        {
            try
            {
                return currentGame.Play(level);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Etwas ging schief beim Game " + currentGame.Name + ".");
                Console.WriteLine("Fehler:");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Mit irgendeiner Taste geht es weiter.");
                Console.ReadKey();
                StartMenu();
                return new Score();
            }
        }

        private static void GameCredits()
        {
            Console.Clear();
            Console.WriteLine("Credits für Game " + currentGame.Name + ":");
            Console.WriteLine(currentGame.Credits);
            Console.WriteLine("Programmiert im Jahr: " + currentGame.Year);
            Console.WriteLine("\nDrücke eine Taste um zum Game-Menu zurück zu kehren.");
            Console.ReadKey();
            NewOrResumeGameMenu();
        }

        private static void GameDescription()
        {
            Console.Clear();
            Console.WriteLine("Beschreibung für Game " + currentGame.Name + ":");
            Console.WriteLine(currentGame.Description);
            Console.WriteLine("\nDrücke eine Taste um zum Game-Menu zurück zu kehren.");
            Console.ReadKey();
            NewOrResumeGameMenu();
        }

        private static void GameRules()
        {
            Console.Clear();
            Console.WriteLine("Regeln für Game " + currentGame.Name + ":");
            Console.WriteLine(currentGame.Rules);
            Console.WriteLine("\nDrücke eine Taste um zum Game-Menu zurück zu kehren.");
            Console.ReadKey();
            NewOrResumeGameMenu();
        }

        private static void SelectLevelMenu()
        {
            // SELECT LEVEL
            int highestLevelToPlay;
            if (currentGame.HighScore != null)
            {
                if (currentGame.HighScore.LevelCompleted == true & currentGame.HighScore.Level < currentGame.LevelMax)
                {
                    highestLevelToPlay = currentGame.HighScore.Level + 1;
                }
                else
                {
                    highestLevelToPlay = currentGame.HighScore.Level;
                }
            }
            else
            {
                highestLevelToPlay = 1;
            }

            Console.Clear();
            Console.WriteLine("Wähle ein Level:\n");
            for (int i = 1; i <= highestLevelToPlay; i++)
            {
                Console.WriteLine("(" + i + ") Spiele Level " + i);
            }
            Console.WriteLine("\n(m) Zurück zum Menu");

            ConsoleKeyInfo input = Console.ReadKey();

            if (input.Key == ConsoleKey.M)
            {
                StartMenu();
            }
            try
            {
                int i = int.Parse(input.KeyChar.ToString());
                if ((i >= 1) && (i <= highestLevelToPlay))
                {
                    Score score = new Score();
                    Console.Clear();
                    score = PlayCurrentGame(i);
                    score.Save(currentGame, gameArray, pathHighScoreFile);
                }
                Console.WriteLine("\n\nKein erlaubter Input.");
                Console.WriteLine("Drücke eine beliebige Taste um fortzufahren.");
                Console.ReadKey();
                SelectLevelMenu();
            }
            catch (Exception)
            {
                Console.WriteLine("Kein erlaubter Input.");
                Console.WriteLine("Drücke eine beliebige Taste um fortzufahren.");
                Console.ReadKey();
                SelectLevelMenu();
            }
        }

        static void Main(string[] args)
        {
            Score.ReadHighScores(gameArray, pathHighScoreFile);
            StartScreen();
        }
    }
}
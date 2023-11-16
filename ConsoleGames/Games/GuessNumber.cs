using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace ConsoleGames.Games
{
    public class GuessNumber : Game
    {
        // PUBLIC PROPERTIES (Eigenschaften)
        public override string Name => "Guess Number";
        public override string Description => "Gegeben ist eine Zahl zwischen 1 und 100 (inkl. Grenzen).\nErrate diese Zahl.";
        public override string Rules => "Errate die gesuchte Zahl in möglichst wenig Versuchen.";
        public override string Credits => "Andreas Schaerer, sca@ksr.ch";
        public override int Year => 2019;
        public override bool TheHigherTheBetter => false;
        public override int LevelMax => 4;
        public override Score HighScore { get; set; }

        //---------------------------------------------------//
        // PRIVATE FIELDS
        //---------------------------------------------------//

        private int[] levelBundary = new int[] { 51, 101, 151, 201 };
        private int?[] maxAttempts = new int?[] { 7, 10, 15, null }; // max nr of attempts per level

        //---------------------------------------------------//
        // PUBLIC METHODS
        //---------------------------------------------------//

        /// <summary>
        /// Public method <c>Play</c> that is called by framework (in Program.cs) to start the game.
        /// </summary>
        /// <remarks>
        /// If player is not successful (fail or quit), set score.points=-1.
        /// If sucessfull, score.points is positive int representing score achieved
        /// </remarks>
        /// <param name="level">the level of the game</param>
        /// <returns>
        /// Score object that contains info about score achieved.
        /// </returns>
        public override Score Play(int level = 1)
        {
            Score score = new Score();
            score.LevelCompleted = false;

            Random random = new Random();
            int nrToGuess;
            int attempts = 0;
            bool levelCompleted = false; // number guessed in at most max attempts

            if (level > LevelMax) level = LevelMax;
            if (maxAttempts.Length != LevelMax || levelBundary.Length != LevelMax)
            {
                // ensure that settings are consistent
                throw new Exception();
            }

            // set number range for levels
            nrToGuess = random.Next(1, levelBundary[level - 1]);
            
            DisplayInitial(level);
            while (true)
            {
                int guess = AskForNumber(out bool quit);
                if (quit) break;
                ResultEval result = EvaluateGuess(level, nrToGuess, guess, ref attempts, ref levelCompleted);
                DisplayResult(level, nrToGuess, result, attempts, ref levelCompleted);
                if (result == ResultEval.Found)
                {
                    score.LevelCompleted = levelCompleted;
                    score.Level = level;
                    score.Points = attempts; // the lower the better
                    break;
                }
            }
            return score;
        }

        //---------------------------------------------------//
        // PRIVATE METHODS FOR MODEL
        //---------------------------------------------------//

        /// <summary>
        /// Private method <c>EvaluateGuess</c> evaluates guess and checks if level completed.
        /// </summary>
        /// <remarks>
        /// Guessed number is evaluated: too small/correct/too big?
        /// When level completed (correct guess) check if level
        /// completed successfully (<= max attempts)
        /// </remarks>
        /// <param name="level">the level of the game</param>
        /// <param name="nrToGuess">number player is supposed to guess</param>
        /// <param name="guess">number player guessed</param>
        /// <param name="attempts">number of attempts so far</param>
        /// <param name="levelCompleted">bool level completed successfully, passed as ref</param>
        /// <returns>
        /// int with value -1 (guess too small), 0 (correct guess) or 1 (guess too big)
        /// </returns>
        private ResultEval EvaluateGuess(int level, int nrToGuess, int guess, ref int attempts, ref bool levelCompleted) // MODEL METHOD
        {
            // check if guessed correctly or not
            ResultEval result;
            if (guess < nrToGuess)
            {
                result = ResultEval.TooSmall;
            }
            else if (guess > nrToGuess)
            {
                result = ResultEval.TooBig;
            }
            else // correct guess
            {
                result = ResultEval.Found;
            }

            // check if level completed (successfully)
            attempts++;
            if (result == ResultEval.Found)
            {
                if (attempts <= maxAttempts[level - 1])
                {
                    levelCompleted = true;
                }
                if (level == LevelMax) levelCompleted = true;
            }
            return result;
        }

        //---------------------------------------------------//
        // PRIVATE METHODS FOR VIEW
        //---------------------------------------------------//
        private enum ResultEval
        {
            TooSmall,
            Found,
            TooBig
        }

        /// <summary>
        /// Private method <c>AskForNumber</c> asks player to guess a number and checks if input is valid
        /// </summary>
        /// <param name="level">the level of the game</param>
        /// <returns>
        /// int guessed number
        /// </returns>
        private int AskForNumber(out bool quit) // VIEW METHOD
        {
            // check input
            int guess;
            quit = false;
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "q")
                {
                    quit = true;
                    return -1;
                }
                try
                {
                    guess = int.Parse(input);
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Eingabe keine gültige Zahl. Und nochmals!");
                    continue;
                }
            }
            Console.Clear();
            return guess;
        }

        /// <summary>
        /// Private method <c>DisplayInitial</c> that shows instructions when starting game
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="level">the level of the game</param>
        private void DisplayInitial(int level)
        {
            if (level <= 3)
            {
                Console.WriteLine("Errate eine Zahl! Level: " + level);
                Console.WriteLine("Errate eine gesuchte Zahl zwischen 1 und " + (levelBundary[level - 1] - 1) + " (inkl. der Grenzen):");
                Console.WriteLine("Um zum nächsten Level zu gelanden, musst du die Zahl in höchstens " + maxAttempts[level - 1] + " Versuchen erraten.");
            }
            else if (level == levelBundary.Length)
            {
                Console.WriteLine("Errate eine gesuchte Zahl zwischen 1 und " + (levelBundary[level - 1] - 1) + " (inkl. der Grenzen):");
                Console.WriteLine("Du bist bereits im höchsten Level. Versuche die gesuchte Zahl in möglichst wenigen Versuchen zu erraten.");
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Private method <c>DisplayResult</c> displays game state after guessing a number
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="level">the level of the game</param>
        /// <param name="nrToGuess">number player is supposed to guess</param>
        /// <param name="result">result (-1,0,1) from evaluation of input</param>
        /// <param name="attempts">number of attempts so far</param>
        /// <param name="levelCompleted">bool level completed successfully, passed as ref</param>
        private void DisplayResult(int level, int nrToGuess, ResultEval result, int attempts, ref bool levelCompleted) // VIEW METHOD
        {
            if (result == ResultEval.TooSmall)
            {
                Console.WriteLine("Die eingegebene Zahl ist zu klein");
            }
            else if (result == ResultEval.TooBig)
            {
                Console.WriteLine("Die eingegebene Zahl ist zu gross");
            }
            else if (result == ResultEval.Found)
            {
                Console.WriteLine("Richtig! Die gesuchte Zahl ist: " + nrToGuess);
                Console.WriteLine("Dafür hast du " + attempts + " Versuche benötigst");
                if (level < LevelMax)
                {
                    if (levelCompleted)
                    {
                        Console.WriteLine("Gratuliere, du hast das Level erfolgreich absolviert.");
                    }
                    else
                    {
                        Console.WriteLine("Leider hat du zu viele Versuche benötigt, um das Level zu bestehen.");
                    }
                }
                else // already in highest level
                {
                    Console.WriteLine("Du hast bereits das höchste Level erreicht. Versuche, dieses in möglichst wenigen Schritten zu absolvieren.");
                }
                Console.WriteLine("Drücke irgend eine Taste um fortzufahren.");
                Console.ReadKey();
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
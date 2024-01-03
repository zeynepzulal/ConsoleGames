using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGames.Games
{
    internal class Hangman : Game
    {
        public override string Name => "Hangman";
        public override string Description => "Try to guess the secret word.";
        public override string Rules => "If you can't guess the right letters, man will be hangt .";
        public override string Credits => "Zeynep Keskin";
        public override int Year => 2019;
        public override bool TheHigherTheBetter => false;
        public override int LevelMax => 4;
        public override Score HighScore { get; set; }

        public override Score Play(int level)
        {
            // Variable declarations allowed here
            char[] validCharachters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string[] hangman = new string[] {@" 
____
|/   |
|   
|    
|    
|    
|
|_____
",
 @"
 ____
|/   |
|
|
|
|
|
|_____
",
        @"
 ____
|/   |
|   (_)
|    
|    
|    
|
|_____
",
        @"
 ____
|/   |
|   (_)
|    |
|    |    
|    
|
|_____
",
        @"
 ____
|/   |
|   (_)
|   \|
|    |
|    
|
|_____
",
        @"
 ____
|/   |
|   (_)
|   \|/
|    |
|    
|
|_____
",
        @"
 ____
|/   |
|   (_)
|   \|/
|    |
|   / 
|
|_____
",
        @"
 ____
|/   |
|   (_)
|   \|/
|    |
|   / \
|
|_____
",
        @"
 ____
|/   |
|   (_)
|   /|\
|    |
|   | |
|
|_____
" };


            // Variable declarations allowed here
            // resetting variables
            int hangmanIndex = 0;
            List<char> triedChar = new List<char>();
            string secretWord = ReadSecretWord(validCharachters);  // Player 1: Enter the secret word to be guessed by player 2
            int isGameOn = 2;
            while (true)                 // Player 2: Make your guesses
            {
                char yourChar = ReadOneChar(validCharachters); // Handle input of one char. 
                string recreatedWord = EvaluateTheSituation(secretWord, yourChar, ref triedChar, ref hangmanIndex, hangman, ref isGameOn);  // Game Logic goes here
                HangTheMan(ref hangmanIndex, hangman, ref isGameOn, recreatedWord, secretWord);            // Screen output goes here
                if (isGameOn == 1 || isGameOn == 0)
                {
                    Console.Read();
                    break;
                }
            }
            
            return new Score();
            
        }
        static string ReadSecretWord(char[] validChar)
        {
            while (true)
            {
                Console.WriteLine("Enter your secret word");
                string word = Console.ReadLine();
                bool valid = true;
                word = word.ToUpper();
                if (word.Length < 3)
                {
                    Console.WriteLine("Do not enter words of less than 3 characters");
                    valid = false;
                }
                for (int i = 0; i < word.Length; i++)
                {
                    if (!validChar.Contains(word[i]))
                    {
                        Console.WriteLine("Invalid character!");
                        valid = false;
                        break;
                    }
                }
                if (!valid)
                {
                    continue;
                }
                return word;
            }
        }

        static char ReadOneChar(char[] validChar)
        {
            Console.WriteLine("Guess a letter");
            char c = Console.ReadKey().KeyChar.ToString().ToUpper().ToCharArray()[0];
            if (!validChar.Contains(c))
            {
                Console.WriteLine("Invalid character!");
                return ' ';
            }
            return c;
        }

        static string EvaluateTheSituation(string word, char letter, ref List<char> triedCharachter, ref int indexHangman, string[] Man, ref int Status)
        {
            string recreatedWord = "";
            triedCharachter.Add(letter);
            for (int i = 0; i < word.Length; i++)
            {
                if (triedCharachter.Contains(word[i]))
                {
                    recreatedWord += word[i];
                }
                else
                {
                    recreatedWord += " _ ";
                }
            }

            if (!word.Contains(letter))
            {
                indexHangman++;
            }
            if (Man[indexHangman] == Man[Man.Length - 1])
            {
                Status = 0;
            }
            if (word == recreatedWord)
            {
                Status = 1;
            }
            return recreatedWord;
        }

        static void HangTheMan(ref int indexHangman, string[] Man, ref int Status, string recreatedWord, string word)
        {
            if (Status == 1)
            {
                Console.WriteLine("You won!");
            }
            if (Status == 0)
            {
                Console.WriteLine("You lost! Secret word was " + word);
            }
            Console.WriteLine(Man[indexHangman]);
            Console.WriteLine(recreatedWord);
        }
    }
}

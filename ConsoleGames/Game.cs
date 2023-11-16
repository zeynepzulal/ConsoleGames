namespace ConsoleGames
{
    public abstract class Game
    {
        //---------------------------------------------------//
        // PUBLIC PROPERTIES (Eigenschaften)
        //---------------------------------------------------//
        public abstract string Name { get; } // name of game, 'get' means: can be viewed publicly but not set
        public abstract string Description { get; } // short descripition of game
        public abstract string Rules { get; } // rules of game, how to play, exit
        public abstract string Credits { get; } // name(s) of author(s) incl. email
        public abstract int Year { get; } // year in which game was created
        public abstract int LevelMax { get; } // max number of levels
        public abstract bool TheHigherTheBetter { get; } // is a higher score desirable?
        public abstract Score HighScore { get; set; } // get; set means: can both be viewed and set publicly, don't touch this property, framework deals with it

        // NO additional public properties allowed, all additional ones must be private

        //---------------------------------------------------//

        // PRIVATE FIELDS
        // here write your private fields

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
        public abstract Score Play(int level);


        // NO additional public methods allowed, all additional ones must be private

        //---------------------------------------------------//
        // PRIVATE METHODS FOR MODEL
        //---------------------------------------------------//
        // these methods must NOT contain Console.... commands


        //---------------------------------------------------//
        // PRIVATE METHODS FOR VIEW
        //---------------------------------------------------//
        // these methods are responsible for input and output, contain Console.... commands
    }
}




using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

class MovieAgeCheck
{
    
    // Class of type 'Film'
    // Used during the generation of the NowShowing.txt file, and generating a working array of the current films.  
    class Movie
    {
        // Maximum characters in the film title. If changed, also change in DisplayFilmList() WriteLine column width.
        private int maxMovieLength = 30;    
        private char[] symbolInput = new char[] { '*', '@', '#', '%', '^', '*' };  
        private string movieName;
        private string ageLimit;

        // Functions for initialization of the class members.      
        // Method to assign a Movie title       
        public void InputName()
        {
            string inputString;
            do
            { 
                Console.Write("  Please Enter The Movie's Name:  ");
                inputString = Console.ReadLine().Trim();

                string inputStringTest = inputString;
                foreach (char s in symbolInput)
                {

                    foreach (char c in inputStringTest)
                    {
                        if (c == s)
                        {
                            Console.WriteLine($"  --------------------------------------");
                            Console.WriteLine($"  | Please type letters instead of {s} |");
                            Console.WriteLine($"  --------------------------------------");
                            inputString = "";
                            break;
                        };
                    };
                }
                if (inputString.Length > maxMovieLength)
                {
                    Console.WriteLine($"  ----------------------------------------------------------------");
                    Console.WriteLine($"  | The maximum film title lenght is {maxMovieLength} characters |");
                    Console.WriteLine($"  ----------------------------------------------------------------");
                    inputString = "";
                }
            } 
            while (inputString == "");
            movieName = inputString;
        }
        
        // Method to assign a Movie Age Rating to 
        public void InputAge()
        {
            string inputString;
            bool inputValid = false;

            do
            {
                Console.Write($"  Please Enter The Age Limit Or Rating for {movieName}:  ");

                // Options for Age Limit and Rating
                inputString = Console.ReadLine().Trim().ToUpper();
                switch (inputString)
                {
                    case "G":
                    case "PG":
                    case "PG-13":
                    case "R":
                    case "NC-17":
                    case "10":
                    case "13":
                    case "15":
                    case "17":

                        inputValid = true;
                        break;
                    default:
                        Console.WriteLine($"\n{inputString} is an invalid age restriction. Valid age restrictions are:");
                        Console.WriteLine("\tG ( General Audience, any age is good )");
                        Console.WriteLine("\tPG ( We will take PG as 10 years or older )");
                        Console.WriteLine("\tPG-13 ( We will take PG-13 as 13 years or older )");
                        Console.WriteLine("\tR ( We will take R as 15 years or older )");
                        Console.WriteLine("\tNC-17 ( We will take NC-17 as 17 years or older )\n");
                        break;
                }
            } 
            while (!inputValid);
            ageLimit = inputString;
        }

        //funcions relating to the accessing of class members.
        // Will throw exception if filmName is null.      
        // Return string Film.filmName
        public string MovieName()
        {
            if (movieName == null)
            {
                throw new Exception("  Unable to access  Movie name.");
            }
            return movieName;
        }
        
        // Will throw exception if ageLimit is null.      
        // Return string Film.ageLimit
        public string AgeLimit()
        {
            if (ageLimit == null)
            {
                throw new Exception("  Unable to access Movie age.");
            }
            return ageLimit;
        }
       
        // Function to obtain derived string containing both FilmName and AgeLimit, separated by '*' character.     
        // <returns>string in form "filmName*ageLimit"</returns>
        public string MovieAgeString()
        {
            string movie;
            // film = FilmName() + "  " + "{" + AgeLimit() + "}";  // Age in Curly Braces
            movie = MovieName() + "*" + AgeLimit() ;
            return movie;
        }

    }

    /************************************************************************************************************************************************************/

    // Method to run the program initialization sequence.
    // Prints current film list to screen, asks user to confirm, regenerate, or terminate program.
    // If file does not contain appropriate strings, catches exception from method GenerateNowShowingArray, and terminates program.
    // Returns string ["Film Title","Age limit"]
    static string[,] Admin()                                                                 ////////// MAIN CALLS FOR ADMIN //////////
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\t\t\t******************************************");
        Console.WriteLine("\t\t\t******************************************");
        Console.WriteLine("\t\t\t*****  WELCOME TO MOVIEPLEX THEATRE  *****");
        Console.WriteLine("\t\t\t******************************************");
        Console.WriteLine("\t\t\t******************************************\n\n");

        Console.WriteLine("  ***************************");
        Console.WriteLine("  *   ADMINISTRATOR LOGIN   *");
        Console.WriteLine("  ***************************\n");

        

        // Checking Admin Password upto 5 Attempts
        int i;
        int count = 5;
        for (i = 1; i <= 5; i++)
        {
            Console.WriteLine("  Please Enter The Admin Password:  ");
            string password = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(password))
            {
                Console.WriteLine("  ----------------------------------------------------------");
                Console.WriteLine("  | Password can't be empty! Input your Password once more |");
                Console.WriteLine("  ----------------------------------------------------------");
                password = Console.ReadLine().Trim();
            }

            count--;
            if (password == "ADMIN!13" && count != 0)
            {
                break;
            }
            else if (password != "ADMIN!13" && count == 0)
            {
                Console.Write("\n  You have entered an Invalid Password\n");
                Console.Write($"  You have {count} attempts to enter the password OR Press A to go back to the Admin Login  ");
                Console.WriteLine("\n\n");

                while (Console.ReadKey().Key == ConsoleKey.A)
                {
                    Console.Clear();
                    return Admin();
                }
            }
        }
       


        // Checks for existing Movie list file, if doesnot exists create the file
        string[,] nowShowingArray;
        do
        {
            if (!File.Exists(@"MovieShowing.txt")) CreateMovieFile();
            try
            {
                nowShowingArray = CreateMovieArray();
            }
            catch (Exception)
            {
                Console.WriteLine("  Invalid MovieShowing.txt File\n");

                return null;
            }
            
            Console.WriteLine("  Currently Showing\n");
            GuestMovieShowing(nowShowingArray);
        }
        while (!RenewList());

        Console.Clear();
        return nowShowingArray;

    }

    /************************************************************************************************************************************************************/

    // Prompt for admin to begin user program sequence, renew NowShowing.txt file   
    // TRUE to begin main program, otherwise returns FALSE
        static bool RenewList()                                                                 ////////// ADMIN CONSOLE ( 3rd Call ) //////////
        {
            char choose;
            do
            {
                Console.Write("  Your Movies Are Playing Today Are Listed Above. Are You Satisfied?(Y/N)?  ");
                choose = Console.ReadLine().ToUpper()[0];
                if (choose == 'N')
                {
                    string confirm;
                    Console.Write("\n  Are you sure you want to delete the current File and create a new Movie list?\n  Type 'delete' to Remove Current Movie List:  ");
            
                    confirm = Console.ReadLine().ToLower();
                    if (confirm == "delete")    //string to confirm deletion. Change prompt above if string chaged.
                    {
                        File.Delete(@"MovieShowing.txt");
                    }
                    else choose = ' '; // If char other than Y and N, reloop of outer do..while loop.
                }
            }
            while (choose != 'Y' && choose != 'N');

            return choose == 'Y';
        }

    /************************************************************************************************************************************************************/

    // Display currently film list.  
    // Parameter is list which is string["Film Title","Age limit"]                              ////////// GUEST CONSOLE ( 1st call )//////////
        static void GuestMovieShowing(string[,] list)
        {
            for (int i = 0; i < list.GetLength(0); i++)
            {
                Console.WriteLine($"\t{i + 1,1}:{list[i, 0],-30}{list[i, 1]}");    //
            }

        }

    /************************************************************************************************************************************************************/

    // Reads NowShowing.txt and converts to a 2D string array  
    // Returns string[,] in form [FilmName,AgeLimit]
        static string[,] CreateMovieArray()                                                      ////////// ADMIN CONSOLE ( 1st Call ) //////////
        {
            var MoviesList = new List<string>();

            StreamReader read = new StreamReader($"MovieShowing.txt");
            int currentLine = 0;
            while (!read.EndOfStream)
            {
                string line = read.ReadLine();
                MoviesList.Add(line);
                currentLine++;
            }

            // Taking FilmsList and converting in 2D array FilmsArray using Split() fn.
            // filmslist.count - Number of movies rows counted by StreamReader
            var MoviesArray = new string[MoviesList.Count, 2];

            //Splits raw string from file at character '*'
            for (int i = 0; i < MoviesList.Count; i++)
            {
                MoviesArray[i, 0] = MoviesList[i].Split('*')[0];
                MoviesArray[i, 1] = MoviesList[i].Split('*')[1];
                if (MoviesArray[i, 0].Length > 30)
                {
                    throw new Exception("  Invalid MovieShowing.txt");
                }
                switch (MoviesArray[i, 1])
                {
                    case "G":
                    case "PG":
                    case "PG-13":
                    case "R":
                    case "NC-17":
                    case "10":
                    case "13":
                    case "15":
                    case "17":
                        break;
                    default:
                        throw new Exception("  Invalid MovieShowing.txt file");
                }
            }
            read.Close();
            return MoviesArray;
        }

    /************************************************************************************************************************************************************/


    // Contains const to limit number of films showing.
    // Admin Enter Movies and Age Limit by making OBJECT of FILM class and Calling InputName() and InputFilmAge()
    // And writing data in NOWSHOWING.txt file

    static void CreateMovieFile()                                                                ////////// ADMIN CONSOLE ( 2nd Call ) //////////
    {
        var movies = new List<Movie>();
        const int maxMoviesShowing = 10;


        Console.Clear();
        Console.WriteLine("\t\t\t******************************************");
        Console.WriteLine("\t\t\t******************************************");
        Console.WriteLine("\t\t\t*****  WELCOME TO MOVIEPLEX THEATRE  *****");
        Console.WriteLine("\t\t\t******************************************");
        Console.WriteLine("\t\t\t******************************************\n\n");

        // Admin add Max 10 Movies
        Console.WriteLine("  Welcome MoviePlex Administrator\n");
        Console.WriteLine($"  The maximum number of Movies is {maxMoviesShowing}.");
        Console.WriteLine("  How many Movies are playing today?  ");
        int num = int.Parse(Console.ReadLine());


        if (num >= 1 && num <= maxMoviesShowing)
        {
            for (int i = 1; i <= num; i++)
            {
                Movie m = new Movie();

                m.InputName();
                m.InputAge();
                Console.WriteLine(" ");
                movies.Add(m);
            }
        }
        else if ( num < 1 || num > maxMoviesShowing || num == 0)
        {
            Console.WriteLine("  -------------------------------------------------");
            Console.WriteLine("  | You Can Enter Maximum Movies Between 1 and 10 |");
            Console.WriteLine("  -------------------------------------------------");
            Console.ReadLine();
        }


        // Writing data into the file by Calling FilmAgeString
        StreamWriter write = new StreamWriter(@"MovieShowing.txt");
        for (int i = 0; i < movies.Count; i++)
        {
            write.WriteLine(movies[i].MovieAgeString());
        }
        write.Close();

    }

    /************************************************************************************************************************************************************/

    // User selects Film from displayed list of currently showing films.
    // Entering "*admin" will leave method and reenter initialization method.
    // <param name="list">string[,] in format [FilmName,AgeLimit]</param>
    // Parameter userexit when true will exit user interface method</param>
    // Returns int user film selection index
        static int GuestChooseMovie(in string[,] list, out bool userexit)                       ////////// GUEST CONSOLE ( 1st call ) //////////
        {
            int movieSelection = 0;
            do
            {
                userexit = false;

                // Validate User Input for putting Numbers only
                Console.Write("\n  Which Movie Would You Like To Watch:  ");
                try
                {
                    string input = Console.ReadLine().Trim().ToLower();
                    if (input == "*admin")
                    {
                        userexit = true;
                        return movieSelection = 0;
                    }
                    movieSelection = int.Parse(input);
                }
                catch (Exception)
                {
                    Console.WriteLine("  ------------------------------");
                    Console.WriteLine("  | Please enter numbers only! |");
                    Console.WriteLine("  ------------------------------");
                }

                // If Number exceeds List of Movies or <= 0, then
                if (movieSelection > list.GetLength(0) || movieSelection <= 0)
                {
                    Console.WriteLine($"  -------------------------------------------------------------------");
                    Console.WriteLine($"  | Please enter a Movie Selection between 1 and {list.GetLength(0)} |");
                    Console.WriteLine($"  -------------------------------------------------------------------");

                }
            }
            while (movieSelection <= 0 || movieSelection > list.GetLength(0));

            return movieSelection;
        }

    /************************************************************************************************************************************************************/

    // Check Age entered by Guest among classified films
    // string[,] list is converted into format [FilmName,AgeLimit]
    // Parameter is selection
        static void GuestConfirmAge(string[,] list, int selection)                                 ////////// GUEST CONSOLE ( 2nd call ) //////////
        {
            const int maxAge = 50;    // Maximum age a user can enter
            char choice;
            var ageLimit = (list[selection - 1, 1]) switch          // Select only Age Item from list
            {
                "G" => 0,
                "PG" => 10,
                "PG-13" => 13,
                "R" => 15,
                "NC-17" => 17,
                "10" => 10,
                "13" => 13,
                "15" => 15,
                "17" => 17,
                _ => throw new Exception("  Not mentioned in the list of movies."),
            };

            // Checking Guest Age For General Audiance - G rating
            if (ageLimit == 0)
            {
                Console.WriteLine($"  The Movie {list[selection - 1, 0]} is Suitable for all Ages!\n\n");
                Console.WriteLine($"\t\t------------------------");
                Console.WriteLine($"\t\t||| ENJOY THE MOVIE! |||");              
                Console.WriteLine($"\t\t------------------------");

                do
                {
                    Console.WriteLine("\n");
                    Console.Write("  Press M To Go Back To Guest Main Menu  \n");
                    Console.Write("  Press S To Go Back To Start Page  \n");
                    choice = Console.ReadLine().ToUpper()[0];

                    if (Console.ReadKey().Key == ConsoleKey.S)
                    {
                        Admin();
                    }
                    else if (Console.ReadKey().Key == ConsoleKey.M)
                    {
                        break;
                    }                
                } while (choice != 'S' && choice != 'M');
            }  

        

          // Check Guest Age For Remaining Ratings
            int userAge;
            do
            {
                Console.Write("\n  Please Enter Your Age For Verification:  ");
                try
                {
                    userAge = int.Parse(Console.ReadLine().Trim());
                }
                catch (Exception)
                {
                    Console.WriteLine("\n");
                    Console.WriteLine("  ------------------------------------------------");
                    Console.WriteLine("  | Please enter Appropriate Rating or Age Limit |");
                    Console.WriteLine("  ------------------------------------------------\n");
                userAge = -1;
                }
            }
            while (userAge <= 0 || userAge > maxAge);

            if (userAge < ageLimit)
            {
                Console.WriteLine("\n");
                Console.WriteLine($"  You Are Too Young To See {list[selection - 1, 0]}");      // Select only Movie Item from The List
                Console.WriteLine($"  Please Select a Different Movie to Watch ");
            }
            else
            {
                Console.WriteLine("\n");
                Console.WriteLine("\t\t------------------------");
                Console.WriteLine("\t\t||| ENJOY THE MOVIE! |||");
                Console.WriteLine("\t\t------------------------");
            }

            // Options For Guest Page and Start Page
           char option;
           do
           {
              Console.WriteLine("\n");
              Console.Write("  Press M To Go Back To Guest Main Menu  \n");    
              Console.Write("  Press S To Go Back To Start Page  \n");    
              option = Console.ReadLine().ToUpper()[0];

              if ( option == 'S')
              {
                 Console.Clear();
                 Admin();
              }
              else if (option == 'M')
              {
                 break;
              }

           } while ( option != 'S' && option != 'M');
        }

         


    

    /************************************************************************************************************************************************************/

    // Primary user facing method. Displayes currently showing films using DisplayNowShowing().
    // Calls ChooseMovie() and ConfirmAge() for user prompts.
    // Pauses after each user awaiting any key, then clears console and repeats.

    // <param name="moviesList">string[,] in format [FilmName,AgeLimit]</param>
    static void Guest(in string[,] moviesList)                                                      ////////// MAIN CALLS FOR GUEST //////////
        {
            bool quitChecking;

            do
            {
                Console.WriteLine("\t\t\t******************************************");
                Console.WriteLine("\t\t\t******************************************");
                Console.WriteLine("\t\t\t*****  WELCOME TO MOVIEPLEX THEATRE  *****");
                Console.WriteLine("\t\t\t******************************************");
                Console.WriteLine("\t\t\t******************************************\n\n");
                Console.WriteLine("  *******************");
                Console.WriteLine("  *  WELCOME GUEST  *");
                Console.WriteLine("  *******************\n");

                Console.WriteLine($"Currently these movies are playing Today. Please choose from the following movies:\n");
                GuestMovieShowing(moviesList);
                int selectedMovie = GuestChooseMovie(moviesList, out quitChecking);
                if (quitChecking) break;
                GuestConfirmAge(moviesList, selectedMovie);
                Console.WriteLine("  Press any key to continue.");
                Console.ReadKey();
                Console.Clear();
            }
            while (!quitChecking);
            Console.Clear();

        }

    /************************************************************************************************************************************************************/

    // MoviePlex Main Menu
    /*  private static bool MainMenu()
      {

          Console.Clear();

          string[,] list;
          Console.WriteLine("                ******************************************");
          Console.WriteLine("                *****  WELCOME TO MOVIEPLEX THEATRE  *****");
          Console.WriteLine("                ******************************************\n\n");

          Console.WriteLine("  Please Select From The Following Options");
          Console.WriteLine("  1: ADMINISTRATOR");
          Console.WriteLine("  2: GUESTS");

          Console.Write("\n  Selection: ");

          // Choose between Administrator and Guest Portal
          switch (Console.ReadLine())
          {
              case "1":
                  list = Admin();
                  return true;
              case "2":
                  Guest(list);
                  return true;
              default:
                  return true;
          }


      }  */

    /************************************************************************************************************************************************************/

    // Calls InitializeAgeCheck() ADMINISTRATOR
    // Calls Guest() for GUEST

    static void Main()                                      ////////// MAIN FUNCTION //////////
    {
      
        //Working array of current films list.                   
        string[,] list;
        list = Admin();
        Guest(list);
        Console.ReadKey();

    }
}


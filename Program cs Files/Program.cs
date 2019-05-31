using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacks_or_Better
{
    class Program
    {
        /*enum Suites
        {
            Hearts= '♥',
            Diamonds = '♦',
            Clubs = '♣',
            Spades = '♠'
        }*/

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8; // UTF8 is needed for displaying card suits
            GameManager game = new GameManager();
            bool play = true;

            Console.WriteLine("Lets play some \"Jacks or better\" poker.\n");
            while (play)
            {
                game.StartGame();
                string playerAnswer = "";

                //Ask for input until it is either y or n.
                while (playerAnswer.ToLower() != "y" && playerAnswer.ToLower() != "n")
                {
                    Console.WriteLine("\nPlay again? (Y/N)");
                    playerAnswer = Console.ReadLine();
                    if (playerAnswer.ToLower() == "n")
                        play = false;

                }
            }
            Console.WriteLine("Thank you for playing.");
        }
    }
}

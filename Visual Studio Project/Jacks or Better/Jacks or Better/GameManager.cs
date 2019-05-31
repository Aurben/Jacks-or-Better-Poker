using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacks_or_Better
{
    enum HandValues
    {
        RoyalFlush = 800,
        StraightFlush = 50,
        FourOfAKind = 25,
        FullHouse = 9,
        Flush = 6,
        Straight = 4,
        ThreeOfAKind = 3,
        TwoPair = 2,
        JacksOrBetter = 1,
        Nothing = 0,
    }

    class GameManager
    {
        public List<Card> PlayerHand { get; set; }
        Deck deck;
        public HandValues handValue { get; private set; }

        public GameManager()
        {
            deck = new Deck();
            PlayerHand = new List<Card>();
            handValue = HandValues.Nothing;
        }

        public void StartGame()
        {
            deck.RefillDeck();
            deck.Shuffle();
            DrawNewHand();
            CalculateHandValue();
            PrintHand();
            AskForRedraw();
            CalculateResult();
        }

        private void DrawNewHand()
        {
            PlayerHand.Clear();

            Console.WriteLine("Drawing Cards...\n");

            //Draw 5 cards
            for (int i = 0; i < 5; i++)
            {
                PlayerHand.Add(deck.DrawCard());
            }
        }

        private void CalculateResult()
        {
            if (handValue != HandValues.Nothing)
                Console.WriteLine($"You ended the game with {handValue}. You won {(int)handValue} times your bet!");
            else
                Console.WriteLine("You didn't get anything. better luck next time.");
        }

        private void AskForRedraw()
        {
            string input = "";
            bool commandRead = false;

            while (!commandRead)
            {
                Console.WriteLine("Would you like to redraw some cards? Type their numbers seperated by spaces if yes. Otherwise type 'n'");
                input = Console.ReadLine();

                if (input.ToLower() == "n")
                    return;

                //Get the entered numbers
                var commands = input.Split(' ');

                commandRead = RedrawHand(commands);
            }
        }

        //Tries to redraw hand with the specified cards to redraw. Returns true if successful, otherwise false
        private bool RedrawHand(string[] input)
        {
            var cardNumbers = new List<int>();

            //If didn't enter anything ask again.
            if (input.Length == 1 && input[0] == "")
            {
                Console.WriteLine("Please give an answer.");
                return false;
            }
            //If entered more than 5 values, ask for input again
            if (input.Length > 5)
            {
                Console.WriteLine("Enter no more than 5 numbers.");
                return false;
            }

            try
            {
                //Convert each string to a number
                for (int i = 0; i < input.Length; i++)
                {
                    int value = -1;
                    //If provided variables aren't integers, abort command
                    if (!int.TryParse(input[i], out value))
                    {
                        Console.WriteLine("Provided variables need to be numbers.");
                        return false;
                    }

                    //If entered number isn't 1-5, abort command
                    if (value < 1 || value > 5)
                    {
                        Console.WriteLine("Provided numbers need to be 1-5.");
                        return false;
                    }

                    //if number has already been entered, abort command
                    if (cardNumbers.Contains(value))
                    {
                        Console.WriteLine("Enter each number only once.");
                        return false;
                    }
                    cardNumbers.Add(value);
                }
                RedrawHand(cardNumbers);
                PrintHand();

                return true;
            }
            catch
            {
                Console.WriteLine("There was a problem reading your input.");
                return false;
            }
        }

        private void RedrawHand(List<int> cardNumbers)
        {
            foreach (var number in cardNumbers)
            {
                PlayerHand.RemoveAt(number - 1);
                PlayerHand.Insert(number - 1, deck.DrawCard());
            }
        }

        public void PrintHand()
        {
            const int space = 4; //The size of the area in symbols given to display each cards information

            Console.WriteLine("Your hand's score is: " + handValue);

            //Display each cards position in the hand
            for (int i = 1; i <= 5; i++)
            {
                Console.Write($"{i,space}");
            }
            Console.Write('\n');

            //Display each card
            foreach (var card in PlayerHand)
            {
                Console.Write($"{card,space}");
            }
            Console.Write("\n");
        }
        
        public void CalculateHandValue()
        {
            var tempHand = new List<Card>(PlayerHand); 
            handValue = HandValues.Nothing;

            //For testing
            /*Card[] cards = { new Card(11,Card.SuitType.Clubs), new Card(11,Card.SuitType.Clubs), new Card(2,Card.SuitType.Clubs),
                                        new Card(8,Card.SuitType.Clubs),new Card(7,Card.SuitType.Spades )};
            tempHand = new List<Card>(cards);
            PlayerHand = tempHand;
            PrintHand();*/

            //Sort by card value ascending
            tempHand.Sort((a, b) => a.Value.CompareTo(b.Value));

            //Check for straights
            if (HasStraight(tempHand))
            {
                handValue = HandValues.Straight;
            }            

            //Check for Flush
            if(HasFlush())
            {
                //If a straight was found as well, then it is either a royal flush or a straight flush
                if (handValue == HandValues.Straight)
                {
                    //Check if it is a 10 J Q K A straight
                    if (tempHand[0].Value == 10)
                        handValue = HandValues.RoyalFlush;
                    else handValue = HandValues.StraightFlush;
                }
                else //Otherwise it is just a flush
                    handValue = HandValues.Flush;
            }

            //Check for matching value cards. Cards are already sorted by value
            var pairValue = CheckForPairs(tempHand);

            //Set the hand score to the highest value
            if (handValue < pairValue)
                handValue = pairValue;
        }

        private bool HasStraight(List<Card> sortedHand)
        {
            for (int i = 1; i < 5; i++)
            {
                //Check if the cards are in order
                if (sortedHand[i - 1].Value + 1 != sortedHand[i].Value)
                {
                    //On the last card check if it is an A 2 3 4 5 straight
                    if (i != 4 || sortedHand[4].Value != 14 || sortedHand[0].Value != 2)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool HasFlush()
        {
            for (int i = 1; i < 5; i++)
            {
                //If suits don't match
                if (PlayerHand[i - 1].Suit != PlayerHand[i].Suit)
                {
                    return false;
                }
            }
            return true;            
        }

        private HandValues CheckForPairs(List<Card> sortedHand)
        {
            HandValues pairValue = HandValues.Nothing;
            int matchValue = 0; // Stores the value of the matched card. Used if it is only a pair to check for Jacks or Better.
            int[] matches = CountPairs(sortedHand, ref matchValue);

            //Checks if the hand value is a pair or better, not counting hands of a single pair worse than jacks.
            bool handFound = matches[0] != 0 && !(matchValue <= 10 && matches[1]==0 && matches[0]==1);

            //if a hand is found, check if its a two/three/four of a kind or a full house/two pair hand
            if (handFound)
            {
                //if more than two types of cards were matched it's either a full house or two pair
                if (matches[1] != 0)
                {
                    if (matches[0] + matches[1] == 3)
                    {
                        pairValue = HandValues.FullHouse;
                    }
                    else pairValue = HandValues.TwoPair;
                }
                else
                {
                    switch (matches[0])
                    {
                        case 1:                           
                            pairValue = HandValues.JacksOrBetter;
                            break;
                        case 2:
                            pairValue = HandValues.ThreeOfAKind;
                            break;
                        case 3:
                            pairValue = HandValues.FourOfAKind;
                            break;
                        default:
                            Console.WriteLine("Single card pair check error.");
                            break;
                    }
                }
            }
            return pairValue;
        }

        //Returns the number of pair matches for 1 or 2 types of cards
        private int[] CountPairs(List<Card> sortedHand, ref int matchValue)
        {

            int[] matches = new int[2];
            int matchIndex = 0; //Index indicating which type of pair is being checked in the matches array.

            for (int i = 1; i < 5; i++)
            {
                if (sortedHand[i - 1].Value == sortedHand[i].Value)
                {
                    matches[matchIndex]++;
                    matchValue = sortedHand[i].Value;
                }
                else if (matches[matchIndex] != 0)
                {
                    matchIndex++;
                }
            }
            return matches;
        }
    }
}

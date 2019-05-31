using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacks_or_Better
{
    class Deck
    {
        Stack<Card> CardStack { get; set; }
        List<Card> RemovedCards { get; set; } //Cards taken from the deck. Used for puttings cards back in.

        public Deck()
        {
            var cardList = new List<Card>();
            RemovedCards = new List<Card>();

            //Go through each suit
            foreach (var suit in Enum.GetValues(typeof(Card.SuitType)).Cast<Card.SuitType>())
            {
                //Go through each value
                for (int i = 2; i < 15; i++)
                {
                    Card card = new Card(i, suit);
                    cardList.Add(card);
                }
            }
            CardStack = new Stack<Card>(cardList);
        }

        public Card DrawCard()
        {
            var card = CardStack.Pop();
            RemovedCards.Add(card);
            return card;
        }

        public void RefillDeck()
        {
            foreach (var card in RemovedCards)
            {
                CardStack.Push(card);
            }
            RemovedCards.Clear();
        }

        public void Shuffle()
        {
            Random random = new Random();
            CardStack = new Stack<Card>(CardStack.OrderBy(x => random.Next()));
        }

        //Displays deck with a maximum of 13 cards per line
        public void Print()
        {
            Console.WriteLine("Printing deck.");
            int i =0;
            foreach (var card in CardStack)
            {
                Console.Write($"{card,-4}");
                i++;
                if (i >= 13)
                {
                    i = 0;
                    Console.Write('\n');
                }
            }
        }
    }
}

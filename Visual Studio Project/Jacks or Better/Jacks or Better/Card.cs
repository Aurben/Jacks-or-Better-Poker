using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacks_or_Better
{
    class Card: IComparable<Card>
    {
        public enum SuitType // requires console output encoding to be set to UTF8 to display suit symbols
        {
            Hearts = '♥',
            Diamonds = '♦',
            Clubs = '♣',
            Spades = '♠'
        }

        public SuitType Suit { get; private set; }
        public int Value { get; private set; }
        
        public Card(int value, SuitType suit)
        {
            Value = value; // Number from 2 to 14, where 11 is Jack, 12 - Queen, 13 - King, and 14 - Ace
            Suit = suit;
        }

        public override string ToString()
        {
            return (char)Suit + ValueToString();
        }

        //Represents the value of faced cards and the ace by a letter, otherwise shows its number value
        string ValueToString()
        {
            switch(Value)
            {
                case 11:
                    return "J";
                case 12:
                    return "Q";
                case 13:
                    return "K";
                case 14:
                    return "A";
                default: return Value.ToString();
            }
        }

        //By default, sorting is done by ascending value
        public int CompareTo(Card other)
        {
            return Value.CompareTo(other.Value);
        }
        
    }
}

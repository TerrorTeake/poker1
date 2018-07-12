using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WinState
{
    PLAYER1,
    PLAYER2,
    TIE
}

public enum CardType
{
    NULL,
    HEARTS,
    CLUBS,
    DIAMONDS,
    SPADES
}

public enum CardValue
{
    TWO = 2,
    THREE = 3,
    FOUR = 4,
    FIVE = 5,
    SIX = 6,
    SEVEN = 7,
    EIGHT = 8,
    NINE = 9,
    TEN = 10,
    JACK = 11,
    QUEEN = 12,
    KING = 13,
    ACE = 14
}

public enum CardSets
{
    HIGHCARD,
    ONEPAIR,
    TWOPAIR,
    THREEOFAKIND,
    STRAIGHT,
    FLUSH,
    FULLHOUSE,
    FOUROFAKIND,
    STRAIGHTFLUSH,
    ROYALFLUSH
}

public class CardManager : MonoBehaviour
{
}

[System.Serializable]
public class Card
{
    public Card(CardType cardType, CardValue cardValue)
    {
        type = cardType;
        value = cardValue;
    }

    public CardType type;
    public CardValue value;

    public string Name
    {
        get
        {
            return type + " " + value;
        }
    }

    public static Card GetHighCard(Card[] hand, out int value)
    {
        Card highestCard = null;
        int highestValue = 0;

        foreach (Card card in hand)
        {
            if ((int)card.value > highestValue)
            {
                highestCard = card;
                highestValue = (int)highestCard.value;
            }
        }
        value = highestValue;
        return highestCard;
    }

    public static bool CheckPairs(Card[] hand, out bool twoPairs, out int value)
    {
        value = 0;
        twoPairs = false;
        bool hasPair = false;
        Card[] firstPair = new Card[2];
        Card[] secondPair = new Card[2];
        foreach (Card card in hand)
        {
            Card otherCard = System.Array.Find(hand, x => x.value == card.value && x != card && card != firstPair[1] && card != secondPair[1]);
            if (otherCard != null)
            {
                if (firstPair[0] == null)
                {
                    firstPair[0] = card;
                    firstPair[1] = otherCard;
                    hasPair = true;
                    value = (int)card.value;
                }
                else if (secondPair[0] == null)
                {
                    secondPair[0] = card;
                    secondPair[1] = otherCard;
                    value = firstPair[0].value > secondPair[0].value ? (int)firstPair[0].value : (int)secondPair[0].value;
                    twoPairs = true;
                    hasPair = true;
                }
            }
        }
        return hasPair;
    }

    public static bool ThreeOfAKind(Card[] hand, out int value)
    {
        value = 0;

        Card[] threeOfAKind = new Card[3];
        Card secondCard = null;
        Card thirdCard = null;

        foreach(Card card in hand)
        {
            secondCard = System.Array.Find(hand, x => x.value == card.value && x != secondCard && card != secondCard);
            if (secondCard != null)
            {
                thirdCard = System.Array.Find(hand, x => x.value == card.value && x != secondCard && card != secondCard && x != card);
                if (thirdCard != null)
                {
                    threeOfAKind[0] = card;
                    threeOfAKind[1] = secondCard;
                    threeOfAKind[2] = thirdCard;
                    value = (int)card.value;
                    return true;
                } 
            } 
        }
        return false;
    }

    public static bool Straight(Card[] hand, out int value)
    {
        Card[] Straight = new Card[5];

        foreach (Card card in hand)
        {
            for (int i = 0; i < hand.Length; i++)
            {
                Card check = System.Array.Find(hand, x => x.value == card.value + i);

                if (check != null)
                {
                    Straight[i] = check;
                    if (i == 4)
                    {
                        value = (int)Straight[4].value;
                        return true;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        value = 0;
        return false;
    }

    public static bool Straight(Card[] hand, out Card[] straight, out int value)
    {
        value = 0;
        Card[] straightHand = new Card[5];

        foreach (Card card in hand)
        {
            for (int i = 0; i < hand.Length; i++)
            {
                Card check = System.Array.Find(hand, x => x.value == card.value + i);

                if (check != null)
                {
                    straightHand[i] = check;
                    if (i == 4)
                    {
                        straight = straightHand;
                        value = (int)straightHand[4].value;
                        return true;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        straight = straightHand;
        return false;
    }

    public static bool Flush(Card[] hand, out int value)
    {
        Card[] flush = new Card[5];
        CardType type = CardType.NULL;

        foreach(Card card in hand)
        {
            if (type == CardType.NULL)
            {
                type = card.type;
                continue;
            }

            if (card.type != type)
            {
                value = 0;
                return false;
            }
        }
        GetHighCard(hand, out value);
        return true;
    }

    public static bool FullHouse(Card[] hand, out int value)
    {
        int threeOfAKindValue;
        int twoPairValue;
        bool twoPair;

        if (ThreeOfAKind(hand, out threeOfAKindValue) && CheckPairs(hand, out twoPair, out twoPairValue))
        {
            value = threeOfAKindValue > twoPairValue ? threeOfAKindValue : twoPairValue;
            return true;
        }
        value = 0;
        return false;
    }

    public static bool StraightFlush(Card[] hand, out int value)
    {
        Card[] straight;
        CardType type = CardType.NULL;
        if (Straight(hand, out straight, out value))
        {
            foreach(Card card in straight)
            {
                if (type == CardType.NULL)
                {
                    type = card.type;
                    continue;
                }

                if (card.type != type) 
                {
                    return false;
                }

            }
        }
        else
        {
            return false;
        }
        return true;
    }

    public static bool FourOfAKind(Card[] hand, out int value)
    {
        value = 0;

        Card[] fourOfAKind = new Card[4];
        Card secondCard = null;
        Card thirdCard = null;
        Card fourthCard = null;

        foreach (Card card in hand)
        {
            secondCard = System.Array.Find(hand, x => x.value == card.value && x != secondCard && card != secondCard);
            if (secondCard != null)
            {
                thirdCard = System.Array.Find(hand, x => x.value == card.value && x != secondCard && card != secondCard && x != card);
                if (thirdCard != null)
                {
                    fourthCard = System.Array.Find(hand, x => x.value == card.value && x != secondCard && card != secondCard && x != card && x != thirdCard && card != thirdCard);
                    if (fourthCard != null)
                    {
                        fourOfAKind[0] = card;
                        fourOfAKind[1] = secondCard;
                        fourOfAKind[2] = thirdCard;
                        fourOfAKind[3] = fourthCard;
                        value = (int)card.value;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public static bool RoyalFlush(Card[] hand)
    {
        Card[] RoyalFlush = new Card[5];
        for (int i = 0; i < hand.Length; i++)
        {
            Card card = System.Array.Find(hand, x => x.value == CardValue.TEN + i);
            if (card != null && i == 4)
            {
                return true;
            }
            else if (card == null)
            {
                break;
            }
        }
        return false;
    }

    public static CardSets HighestSet(Card[] hand, out int value)
    {
        if (RoyalFlush(hand))
        {
            value = 0;
            return CardSets.ROYALFLUSH;
        }

        if (StraightFlush(hand, out value))
        {
            return CardSets.STRAIGHTFLUSH;
        }

        if (FourOfAKind(hand, out value))
        {
            return CardSets.FOUROFAKIND;
        }

        if (FullHouse(hand, out value))
        {
            return CardSets.FULLHOUSE;
        }

        if (Flush(hand, out value))
        {
            return CardSets.FLUSH;
        }

        if (Straight(hand, out value))
        {
            return CardSets.STRAIGHT;
        }

        if (ThreeOfAKind(hand, out value))
        {
            return CardSets.THREEOFAKIND;
        }

        bool twoPairs;
        if (CheckPairs(hand, out twoPairs, out value))
        {
            if (!twoPairs)
            {
                return CardSets.ONEPAIR;
            }
            else
            {
                return CardSets.TWOPAIR;
            }
        }
        GetHighCard(hand, out value);
        return CardSets.HIGHCARD;
    }

    public static void CheckVictory(Card[] p1Hand, Card[] p2Hand)
    {
        int p1Value;
        int p2Value;

        CardSets p1Set = HighestSet(p1Hand, out p1Value);
        CardSets p2Set = HighestSet(p2Hand, out p2Value);

        //WriteRoundToDisk(p1Set, p2Set, p1Value, p2Value, 0);

        Debug.Log(p1Value + " " + p2Value);

        Debug.Log("P1 " + p1Set + " P2 " + p2Set);

        if (p1Set > p2Set)
        {
            PlayerInfo.player1Wins++;
        }
        else if (p2Set > p1Set)
        {
            PlayerInfo.player2Wins++;
        }
        else if (p2Set == p1Set)
        {
            if (p1Value > p2Value)
            {
                PlayerInfo.player1Wins++;
            }
            else if (p2Value > p1Value)
            {
                PlayerInfo.player2Wins++;
            }
            else if (p1Value == p2Value)
            {
                PlayerInfo.ties++;
            }
        }
    }

    public static void CheckVictory(Card[] p1Hand, Card[] p2Hand, out WinState state)
    {
        int p1Value;
        int p2Value;
        state = WinState.TIE;

        CardSets p1Set = HighestSet(p1Hand, out p1Value);
        CardSets p2Set = HighestSet(p2Hand, out p2Value);

        Debug.Log(p1Value + " " + p2Value);

        Debug.Log("P1 " + p1Set + " P2 " + p2Set);

        if (p1Set > p2Set)
        {
            PlayerInfo.player1Wins++;
            state = WinState.PLAYER1;
        }
        else if (p2Set > p1Set)
        {
            PlayerInfo.player2Wins++;
            state = WinState.PLAYER2;
        }
        else if (p2Set == p1Set)
        {
            if (p1Value > p2Value)
            {
                PlayerInfo.player1Wins++;
                state = WinState.PLAYER1;
            }
            else if (p2Value > p1Value)
            {
                PlayerInfo.player2Wins++;
                state = WinState.PLAYER2;
            }
            else if (p1Value == p2Value)
            {
                PlayerInfo.ties++;
                state = WinState.TIE;
            }
        }
    }
}

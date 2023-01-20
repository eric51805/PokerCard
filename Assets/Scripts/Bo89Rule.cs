using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class Bo89Rule
{
    
    public static bool Compare2Result(Bo89Result result1, Bo89Result result2)
    {
        bool result = true;

        switch (result1.resultType)
        {
            case Bo89ResultType.RoyalStraightFlush:

                if (result2.resultType == Bo89ResultType.RoyalStraightFlush)
                {

                }
                else
                {

                }

                break;
        }

        return result;
    }

    public static Bo89Result GetResult(List<PokerCard> pokerCards, Bo89ResultSet bo89ResultSet)
    {
        Bo89Result result = new Bo89Result();
        result.resultType = Bo89ResultType.None;

        pokerCards = pokerCards.OrderBy(s => s.number).ToList();

        PokerCard card1 = null;
        PokerCard card2 = null;
        PokerCard card3 = null;

        card1 = pokerCards[0];
        card2 = pokerCards[1];

        result.pokerCards.Add(card1);
        result.pokerCards.Add(card2);

        foreach (var card in pokerCards)
            result.totalPoint += card.GetNumber();

        result.totalPoint = result.totalPoint % 10;

        if (pokerCards.Count == 2)
        {
            if (card1.type == card2.type)
            {
                result.resultType = Bo89ResultType.Flush;
                result.resultInfo = bo89ResultSet.bo89ResultInfos.First(s => s.resultType == Bo89ResultType.Flush);
                result.result = String.Format(result.resultInfo.GetResultFormat(), result.totalPoint);
            }

            if (card1.number == card2.number)
            {
                result.resultType = Bo89ResultType.Pairs;
                result.resultInfo = bo89ResultSet.bo89ResultInfos.First(s => s.resultType == Bo89ResultType.Pairs);
                result.result = String.Format(result.resultInfo.GetResultFormat(), result.totalPoint);
            }
        }
        else if (pokerCards.Count == 3)
        {
            card3 = pokerCards[2];
            result.pokerCards.Add(card3);

            var isThreeOfFlush = false;
            var isStraight = false;

            if (card1.type == card2.type && card2.type == card3.type)
            {
                result.resultType = Bo89ResultType.ThreeOfFlush;
                result.resultInfo = bo89ResultSet.bo89ResultInfos.First(s => s.resultType == Bo89ResultType.ThreeOfFlush);
                result.result = String.Format(result.resultInfo.GetResultFormat(), result.totalPoint);
                isThreeOfFlush = true;
            }

            if (card1.number +1 == card2.number && card2.number +1 == card3.number)
            {
                result.resultType = Bo89ResultType.Straight;
                result.resultInfo = bo89ResultSet.bo89ResultInfos.First(s => s.resultType == Bo89ResultType.Straight);
                result.result = String.Format(result.resultInfo.GetResultFormat(), card1.name, card2.name, card3.name);
                isStraight = true;
            }

            if (card1.number == 1 && card2.number == 12 && card3.number == 13)
            {
                result.resultType = Bo89ResultType.Straight;
                result.resultInfo = bo89ResultSet.bo89ResultInfos.First(s => s.resultType == Bo89ResultType.Straight);
                result.result = String.Format(result.resultInfo.GetResultFormat(), card1.name, card2.name, card3.name);
                isStraight = true;
            }

            if (card1.number == card2.number && card2.number == card3.number)
            {
                result.resultType = Bo89ResultType.ThreeOfAKind;
                result.resultInfo = bo89ResultSet.bo89ResultInfos.First(s => s.resultType == Bo89ResultType.ThreeOfAKind);
                result.result = String.Format(result.resultInfo.GetResultFormat(), card1.number);
            }

            if (isThreeOfFlush && isStraight)
            {
                // 同花順
                result.resultType = Bo89ResultType.StraightFlush;
                result.resultInfo = bo89ResultSet.bo89ResultInfos.First(s => s.resultType == Bo89ResultType.StraightFlush);
                result.result = String.Format(result.resultInfo.GetResultFormat(), card1.name, card2.name, card3.name);

                // 皇家同花順
                if (card1.number == 1 && card2.number == 12 && card3.number == 13)
                {
                    result.resultType = Bo89ResultType.RoyalStraightFlush;
                    result.resultInfo = bo89ResultSet.bo89ResultInfos.First(s => s.resultType == Bo89ResultType.RoyalStraightFlush);
                    result.result = String.Format(result.resultInfo.GetResultFormat(), card1.name, card2.name, card3.name);
                }
            }
        }      

        if (result.resultType == Bo89ResultType.None)
        {
            Bo89ResultInfo bo89ResultInfo_None = bo89ResultSet.bo89ResultInfos.First(s => s.resultType == Bo89ResultType.None);
            result.result = String.Format(bo89ResultInfo_None.GetResultFormat(), result.totalPoint);
        }

        return result;
    }

    public enum Bo89ResultType
    {
        // 皇家同花順
        RoyalStraightFlush,
        // 同花順
        StraightFlush,
        // 三條
        ThreeOfAKind,
        // 順子
        Straight,
        // 三同花
        ThreeOfFlush,
        // 兩張一對
        Pairs,
        // 同花
        Flush,
        // 沒有牌型
        None,
    }

    public class Bo89Result
    {
        public int totalPoint;
        public Bo89ResultType resultType;
        public List<PokerCard> pokerCards;
        public Bo89ResultInfo resultInfo;
        public string result;

        public Bo89Result()
        {
            pokerCards = new List<PokerCard>();
        }
    }

    [Serializable]
    public class Bo89ResultInfo
    {
        public Bo89ResultType resultType;
        public double odds;
        public string result;

        public string GetResultFormat()
        {
            return result.Replace("\\n", "\n");
        }
    }
}

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
        PokerCard result1MaxCard = result1.GetMaxCard();
        PokerCard result2MaxCard = result2.GetMaxCard();

        switch (result1.resultType)
        {
            case Bo89ResultType.RoyalStraightFlush:

                switch (result2.resultType)
                {
                    case Bo89ResultType.RoyalStraightFlush:
                        // 如果都是皇家同花順只要比花色就好，因為點數都是一樣的
                        result = Compare2Type(result1.pokerCards[0].type, result2.pokerCards[0].type);
                        break;
                }

                break;

            case Bo89ResultType.StraightFlush:

                switch (result2.resultType)
                {
                    case Bo89ResultType.RoyalStraightFlush:
                        result = false;
                        break;
                    case Bo89ResultType.StraightFlush:
                        // 如果手牌是最大點數的排型
                        if (result1.IsKQA())
                        {
                            // 如果對方也是最大點數的排型
                            if (result2.IsKQA())
                            {
                                // 就比 Ace 花色
                                result = Compare2Type(result1.pokerCards[0].type, result2.pokerCards[0].type);
                            }
                        }
                        else if (result1.pokerCards[2].number == result1.pokerCards[2].number)
                        {
                            // 如果是同個點數的同花順就比花色
                            result = Compare2Type(result1.pokerCards[0].type, result2.pokerCards[0].type);
                        }
                        else
                        {
                            // 不同點數就比點數
                            result = result1.pokerCards[2].number > result1.pokerCards[2].number;
                        }
                        break;
                }

                break;

            case Bo89ResultType.ThreeOfAKind:

                switch (result2.resultType)
                {
                    case Bo89ResultType.RoyalStraightFlush:
                    case Bo89ResultType.StraightFlush:
                        result = false;
                        break;
                    case Bo89ResultType.ThreeOfAKind:
                        // 三條不存在同花色，只比點數
                        result = result1.pokerCards[2].number > result1.pokerCards[2].number;
                        break;
                }

                break;

            case Bo89ResultType.Straight:

                switch (result2.resultType)
                {
                    case Bo89ResultType.RoyalStraightFlush:
                    case Bo89ResultType.StraightFlush:
                    case Bo89ResultType.ThreeOfAKind:
                        result = false;
                        break;
                    case Bo89ResultType.Straight:
                        // 如果手牌是最大點數的排型
                        if (result1.IsKQA())
                        {
                            // 如果對方也是最大點數的排型
                            if (result2.IsKQA())
                            {
                                // 就比 Ace 花色
                                result = Compare2Type(result1.pokerCards[0].type, result2.pokerCards[0].type);
                            }
                        }
                        else if (result1.pokerCards[2].number == result1.pokerCards[2].number)
                        {
                            // 如果是同個點數的順子就比最大的那張的花色
                            result = Compare2Type(result1.pokerCards[2].type, result2.pokerCards[2].type);
                        }
                        else
                        {
                            // 不同點數就比點數
                            result = result1.pokerCards[2].number > result1.pokerCards[2].number;
                        }
                        break;
                }

                break;

            case Bo89ResultType.ThreeOfFlush:

                switch (result2.resultType)
                {
                    case Bo89ResultType.RoyalStraightFlush:
                    case Bo89ResultType.StraightFlush:
                    case Bo89ResultType.ThreeOfAKind:
                    case Bo89ResultType.Straight:
                        result = false;
                        break;
                    case Bo89ResultType.ThreeOfFlush:
                        // 三同花 遇到 三同花
                        if (result2.totalPoint > result1.totalPoint)
                        {
                            // 點數要超過才會贏
                            result = false;
                        }
                        else if (result2.totalPoint == result1.totalPoint)
                        {
                            // 點數一樣的話，比最大的那張
                            result = result1MaxCard.compareNumber > result2MaxCard.compareNumber;
                        }
                        break;
                    case Bo89ResultType.Pairs:
                    case Bo89ResultType.Flush:
                    case Bo89ResultType.None:
                        // 三同花 遇上兩張
                        if (result2.totalPoint >= result1.totalPoint)
                        {
                            // 點數只要一樣以上，就會贏
                            result = false;
                        }
                        else if (result2.totalPoint == result1.totalPoint)
                        {
                            // 點數一樣的話 兩張獲勝
                            result = result1.pokerCards.Count <= result2.pokerCards.Count;
                        }
                        break;
                }

                break;

            case Bo89ResultType.Pairs:

                switch (result2.resultType)
                {
                    case Bo89ResultType.RoyalStraightFlush:
                    case Bo89ResultType.StraightFlush:
                    case Bo89ResultType.ThreeOfAKind:
                    case Bo89ResultType.Straight:
                        result = false;
                        break;
                    case Bo89ResultType.ThreeOfFlush:
                        // 當 對子 遇到 三同花
                        if (result2.totalPoint > result1.totalPoint)
                        {
                            // 點數要超過才會贏
                            result = false;
                        }
                        else if (result2.totalPoint == result1.totalPoint)
                        {
                            // 點數一樣的話 兩張獲勝
                            result = result1.pokerCards.Count <= result2.pokerCards.Count;
                        }
                        break;
                    case Bo89ResultType.Pairs:
                        // 當 對子 遇到 對子 只要比對大小就好
                        result = result1MaxCard.compareNumber > result2MaxCard.compareNumber;
                        break;
                    case Bo89ResultType.Flush:
                    case Bo89ResultType.None:
                        // 當對子遇到 同花 或 沒牌型 只要一樣以上就會贏
                        result = result1.totalPoint >= result2.totalPoint;
                        break;
                }

                break;

            case Bo89ResultType.Flush:

                switch (result2.resultType)
                {
                    case Bo89ResultType.RoyalStraightFlush:
                    case Bo89ResultType.StraightFlush:
                    case Bo89ResultType.ThreeOfAKind:
                    case Bo89ResultType.Straight:
                        result = false;
                        break;
                    case Bo89ResultType.ThreeOfFlush:
                        // 當 兩張同花 遇到 三張同花
                        // 點數大於 贏
                        // 點數等於 兩張贏三張 贏
                        result = result1.totalPoint >= result2.totalPoint;
                        break;
                    case Bo89ResultType.Pairs:
                        // 當 兩張同花 遇到 兩張對子
                        // 點數大於 贏
                        // 點數等於 對子 > 同花
                        result = result1.totalPoint > result2.totalPoint;
                        break;
                    case Bo89ResultType.Flush:
                        // 當 兩張同花 遇到 同花
                        // 比較 最大張 贏
                        result = result1MaxCard.compareNumber > result2MaxCard.compareNumber;
                        break;
                    case Bo89ResultType.None:
                        // 當 兩張同花 沒牌型的情況 (兩張或三張)
                        // 點數大於 才能贏
                        result = result1.totalPoint >= result2.totalPoint;
                        break;
                }

                break;

            case Bo89ResultType.None:

                switch (result2.resultType)
                {
                    case Bo89ResultType.RoyalStraightFlush:
                    case Bo89ResultType.StraightFlush:
                    case Bo89ResultType.ThreeOfAKind:
                    case Bo89ResultType.Straight:
                        result = false;
                        break;
                    case Bo89ResultType.ThreeOfFlush:
                        // 當 沒有牌型 遇到 三張同花 / 對子
                        // 點數大於 才能贏
                        // 點數等於 和 兩張 => 贏，三張 => 輸
                        if (result1.pokerCards.Count == 2)
                        {
                            result = result1.totalPoint >= result2.totalPoint;
                        }
                        else if (result1.pokerCards.Count == 3)
                        {
                            result = result1.totalPoint > result2.totalPoint;
                        }
                        break;
                    case Bo89ResultType.Pairs:
                    case Bo89ResultType.Flush:
                        // 當 沒有牌型 遇到 兩張 對子 / 同花
                        // 點數大於 贏
                        result = result1.totalPoint > result2.totalPoint;
                        break;
                    case Bo89ResultType.None:
                        // 當 沒有牌型 遇到 沒有牌型
                        if (result1.pokerCards.Count == 2)
                        {
                            // 兩張遇兩張
                            if (result2.pokerCards.Count == 2)
                            {
                                // 點數必須要超過才會贏
                                if (result2.totalPoint > result1.totalPoint)
                                {
                                    result = false;
                                }
                                else if (result2.totalPoint == result1.totalPoint)
                                {
                                    // 持平的話要比最大張
                                    result = result1MaxCard.compareNumber > result2MaxCard.compareNumber;
                                }
                            }
                            // 兩張遇三張
                            else if (result2.pokerCards.Count == 3)
                            {
                                // 點數必須要超過才會贏
                                if (result2.totalPoint > result1.totalPoint)
                                {
                                    result = false;
                                }
                            }
                        }
                        else if (result1.pokerCards.Count == 3)
                        {
                            // 三張遇兩張
                            if (result2.pokerCards.Count == 2)
                            {
                                // 點數只要持平以上就可以贏
                                if (result2.totalPoint >= result1.totalPoint)
                                {
                                    result = false;
                                }
                            }
                            // 三張遇三張
                            else if (result2.pokerCards.Count == 3)
                            {
                                // 點數必須要超過才會贏
                                if (result2.totalPoint > result1.totalPoint)
                                {
                                    result = false;
                                }
                                // 持平的話要比最大張
                                else if (result2.totalPoint == result1.totalPoint)
                                {
                                    result = result1MaxCard.compareNumber > result2MaxCard.compareNumber;
                                }
                            }
                        }
                        break;
                }

                break;
        }

        return result;
    }

    public static bool Compare2Type(PokerCardDefine pokerCardDefine1, PokerCardDefine pokerCardDefine2)
    {
        bool result = true;

        switch (pokerCardDefine1)
        {
            case PokerCardDefine.Spade:
                switch (pokerCardDefine2)
                {
                    case PokerCardDefine.Joker:
                        result = false;
                        break;
                }
                break;
            case PokerCardDefine.Heart:
                switch (pokerCardDefine2)
                {
                    case PokerCardDefine.Joker:
                    case PokerCardDefine.Spade:
                        result = false;
                        break;
                }
                break;
            case PokerCardDefine.Dimand:
                switch (pokerCardDefine2)
                {
                    case PokerCardDefine.Joker:
                    case PokerCardDefine.Spade:
                    case PokerCardDefine.Heart:
                        result = false;
                        break;
                }
                break;
            case PokerCardDefine.Club:
                switch (pokerCardDefine2)
                {
                    case PokerCardDefine.Joker:
                    case PokerCardDefine.Spade:
                    case PokerCardDefine.Heart:
                    case PokerCardDefine.Dimand:
                        result = false;
                        break;
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
            result.totalPoint += ToBo89Number(card.number);

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

            if (card1.number + 1 == card2.number && card2.number + 1 == card3.number)
            {
                result.resultType = Bo89ResultType.Straight;
                result.resultInfo = bo89ResultSet.bo89ResultInfos.First(s => s.resultType == Bo89ResultType.Straight);
                result.result = String.Format(result.resultInfo.GetResultFormat(), card1.name, card2.name, card3.name);
                isStraight = true;
            }

            if (result.IsKQA())
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
                if (result.IsKQA())
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

    public static float GetTypeWeighted(PokerCardDefine pokerCardDefine)
    {
        switch (pokerCardDefine)
        {
            case PokerCardDefine.Joker:
                return 99f;
            case PokerCardDefine.Spade:
                return 0.9f;
            case PokerCardDefine.Heart:
                return 0.8f;
            case PokerCardDefine.Dimand:
                return 0.7f;
            case PokerCardDefine.Club:
                return 0.6f;
            default:
                return 0;
        }
    }

    public static int ToBo89Number(int number)
    {
        return number > 10 ? 10 : number;
    }

    public enum Bo89ResultType
    {
        /// 皇家同花順
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
        public PokerCard addCard;

        public Bo89Result()
        {
            pokerCards = new List<PokerCard>();
        }

        public bool IsKQA()
        {
            bool result = false;

            if (pokerCards.Count == 3)
            {
                pokerCards = pokerCards.OrderBy(s => s.number).ToList();
                result = pokerCards[0].number == 1 && pokerCards[1].number == 12 && pokerCards[2].number == 13;
            }

            return result;
        }

        public PokerCard GetMaxCard()
        {
            foreach (PokerCard pokerCard in pokerCards)
            {
                if (pokerCard.number == 1)
                {
                    pokerCard.compareNumber = 14 + GetTypeWeighted(pokerCard.type);
                }
                else
                {
                    pokerCard.compareNumber = pokerCard.number + GetTypeWeighted(pokerCard.type);
                }
            }

            return pokerCards.OrderBy(s => s.compareNumber).Last();
        }
    }

    [Serializable]
    public class Bo89ResultInfo
    {
        public Bo89ResultType resultType;
        public string name;
        public double odds;
        public string result;

        public string GetResultFormat()
        {
            return result.Replace("\\n", "\n");
        }
    }
}

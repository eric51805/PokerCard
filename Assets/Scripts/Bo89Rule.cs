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
                        // 狦常琌產抖璶ゑ︹碞翴计常琌妓
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
                        // 狦も礟琌程翴计逼
                        if (result1.IsKQA())
                        {
                            // 狦癸よ琌程翴计逼
                            if (result2.IsKQA())
                            {
                                // 碞ゑ Ace ︹
                                result = Compare2Type(result1.pokerCards[0].type, result2.pokerCards[0].type);
                            }
                        }
                        else if (result1.pokerCards[2].number == result1.pokerCards[2].number)
                        {
                            // 狦琌翴计抖碞ゑ︹
                            result = Compare2Type(result1.pokerCards[0].type, result2.pokerCards[0].type);
                        }
                        else
                        {
                            // ぃ翴计碞ゑ翴计
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
                        // 兵ぃ︹ゑ翴计
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
                        // 狦も礟琌程翴计逼
                        if (result1.IsKQA())
                        {
                            // 狦癸よ琌程翴计逼
                            if (result2.IsKQA())
                            {
                                // 碞ゑ Ace ︹
                                result = Compare2Type(result1.pokerCards[0].type, result2.pokerCards[0].type);
                            }
                        }
                        else if (result1.pokerCards[2].number == result1.pokerCards[2].number)
                        {
                            // 狦琌翴计抖碞ゑ程ê眎︹
                            result = Compare2Type(result1.pokerCards[2].type, result2.pokerCards[2].type);
                        }
                        else
                        {
                            // ぃ翴计碞ゑ翴计
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
                        //  笿 
                        if (result2.totalPoint > result1.totalPoint)
                        {
                            // 翴计璶禬筁穦墓
                            result = false;
                        }
                        else if (result2.totalPoint == result1.totalPoint)
                        {
                            // 翴计妓杠ゑ程ê眎
                            result = result1MaxCard.compareNumber > result2MaxCard.compareNumber;
                        }
                        break;
                    case Bo89ResultType.Pairs:
                    case Bo89ResultType.Flush:
                    case Bo89ResultType.None:
                        //  笿ㄢ眎
                        if (result2.totalPoint >= result1.totalPoint)
                        {
                            // 翴计璶妓碞穦墓
                            result = false;
                        }
                        else if (result2.totalPoint == result1.totalPoint)
                        {
                            // 翴计妓杠 ㄢ眎莉秤
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
                        // 讽 癸 笿 
                        if (result2.totalPoint > result1.totalPoint)
                        {
                            // 翴计璶禬筁穦墓
                            result = false;
                        }
                        else if (result2.totalPoint == result1.totalPoint)
                        {
                            // 翴计妓杠 ㄢ眎莉秤
                            result = result1.pokerCards.Count <= result2.pokerCards.Count;
                        }
                        break;
                    case Bo89ResultType.Pairs:
                        // 讽 癸 笿 癸 璶ゑ癸碞
                        result = result1MaxCard.compareNumber > result2MaxCard.compareNumber;
                        break;
                    case Bo89ResultType.Flush:
                    case Bo89ResultType.None:
                        // 讽癸笿  ┪ ⊿礟 璶妓碞穦墓
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
                        // 讽 ㄢ眎 笿 眎
                        // 翴计 墓
                        // 翴计单 ㄢ眎墓眎 墓
                        result = result1.totalPoint >= result2.totalPoint;
                        break;
                    case Bo89ResultType.Pairs:
                        // 讽 ㄢ眎 笿 ㄢ眎癸
                        // 翴计 墓
                        // 翴计单 癸 > 
                        result = result1.totalPoint > result2.totalPoint;
                        break;
                    case Bo89ResultType.Flush:
                        // 讽 ㄢ眎 笿 
                        // ゑ耕 程眎 墓
                        result = result1MaxCard.compareNumber > result2MaxCard.compareNumber;
                        break;
                    case Bo89ResultType.None:
                        // 讽 ㄢ眎 ⊿礟薄猵 (ㄢ眎┪眎)
                        // 翴计 墓
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
                        // 讽 ⊿Τ礟 笿 眎 / 癸
                        // 翴计 墓
                        // 翴计单 ㎝ ㄢ眎 => 墓眎 => 块
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
                        // 讽 ⊿Τ礟 笿 ㄢ眎 癸 / 
                        // 翴计 墓
                        result = result1.totalPoint > result2.totalPoint;
                        break;
                    case Bo89ResultType.None:
                        // 讽 ⊿Τ礟 笿 ⊿Τ礟
                        if (result1.pokerCards.Count == 2)
                        {
                            // ㄢ眎笿ㄢ眎
                            if (result2.pokerCards.Count == 2)
                            {
                                // 翴计ゲ斗璶禬筁穦墓
                                if (result2.totalPoint > result1.totalPoint)
                                {
                                    result = false;
                                }
                                else if (result2.totalPoint == result1.totalPoint)
                                {
                                    // キ杠璶ゑ程眎
                                    result = result1MaxCard.compareNumber > result2MaxCard.compareNumber;
                                }
                            }
                            // ㄢ眎笿眎
                            else if (result2.pokerCards.Count == 3)
                            {
                                // 翴计ゲ斗璶禬筁穦墓
                                if (result2.totalPoint > result1.totalPoint)
                                {
                                    result = false;
                                }
                            }
                        }
                        else if (result1.pokerCards.Count == 3)
                        {
                            // 眎笿ㄢ眎
                            if (result2.pokerCards.Count == 2)
                            {
                                // 翴计璶キ碞墓
                                if (result2.totalPoint >= result1.totalPoint)
                                {
                                    result = false;
                                }
                            }
                            // 眎笿眎
                            else if (result2.pokerCards.Count == 3)
                            {
                                // 翴计ゲ斗璶禬筁穦墓
                                if (result2.totalPoint > result1.totalPoint)
                                {
                                    result = false;
                                }
                                // キ杠璶ゑ程眎
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
                // 抖
                result.resultType = Bo89ResultType.StraightFlush;
                result.resultInfo = bo89ResultSet.bo89ResultInfos.First(s => s.resultType == Bo89ResultType.StraightFlush);
                result.result = String.Format(result.resultInfo.GetResultFormat(), card1.name, card2.name, card3.name);

                // 產抖
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
        /// 產抖
        RoyalStraightFlush,
        // 抖
        StraightFlush,
        // 兵
        ThreeOfAKind,
        // 抖
        Straight,
        // 
        ThreeOfFlush,
        // ㄢ眎癸
        Pairs,
        // 
        Flush,
        // ⊿Τ礟
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

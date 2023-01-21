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
                        // �p�G���O�Ӯa�P�ᶶ�u�n����N�n�A�]���I�Ƴ��O�@�˪�
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
                        // �p�G��P�O�̤j�I�ƪ��ƫ�
                        if (result1.IsKQA())
                        {
                            // �p�G���]�O�̤j�I�ƪ��ƫ�
                            if (result2.IsKQA())
                            {
                                // �N�� Ace ���
                                result = Compare2Type(result1.pokerCards[0].type, result2.pokerCards[0].type);
                            }
                        }
                        else if (result1.pokerCards[2].number == result1.pokerCards[2].number)
                        {
                            // �p�G�O�P���I�ƪ��P�ᶶ�N����
                            result = Compare2Type(result1.pokerCards[0].type, result2.pokerCards[0].type);
                        }
                        else
                        {
                            // ���P�I�ƴN���I��
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
                        // �T�����s�b�P���A�u���I��
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
                        // �p�G��P�O�̤j�I�ƪ��ƫ�
                        if (result1.IsKQA())
                        {
                            // �p�G���]�O�̤j�I�ƪ��ƫ�
                            if (result2.IsKQA())
                            {
                                // �N�� Ace ���
                                result = Compare2Type(result1.pokerCards[0].type, result2.pokerCards[0].type);
                            }
                        }
                        else if (result1.pokerCards[2].number == result1.pokerCards[2].number)
                        {
                            // �p�G�O�P���I�ƪ����l�N��̤j�����i�����
                            result = Compare2Type(result1.pokerCards[2].type, result2.pokerCards[2].type);
                        }
                        else
                        {
                            // ���P�I�ƴN���I��
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
                        // �T�P�� �J�� �T�P��
                        if (result2.totalPoint > result1.totalPoint)
                        {
                            // �I�ƭn�W�L�~�|Ĺ
                            result = false;
                        }
                        else if (result2.totalPoint == result1.totalPoint)
                        {
                            // �I�Ƥ@�˪��ܡA��̤j�����i
                            result = result1MaxCard.compareNumber > result2MaxCard.compareNumber;
                        }
                        break;
                    case Bo89ResultType.Pairs:
                    case Bo89ResultType.Flush:
                    case Bo89ResultType.None:
                        // �T�P�� �J�W��i
                        if (result2.totalPoint >= result1.totalPoint)
                        {
                            // �I�ƥu�n�@�˥H�W�A�N�|Ĺ
                            result = false;
                        }
                        else if (result2.totalPoint == result1.totalPoint)
                        {
                            // �I�Ƥ@�˪��� ��i���
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
                        // �� ��l �J�� �T�P��
                        if (result2.totalPoint > result1.totalPoint)
                        {
                            // �I�ƭn�W�L�~�|Ĺ
                            result = false;
                        }
                        else if (result2.totalPoint == result1.totalPoint)
                        {
                            // �I�Ƥ@�˪��� ��i���
                            result = result1.pokerCards.Count <= result2.pokerCards.Count;
                        }
                        break;
                    case Bo89ResultType.Pairs:
                        // �� ��l �J�� ��l �u�n���j�p�N�n
                        result = result1MaxCard.compareNumber > result2MaxCard.compareNumber;
                        break;
                    case Bo89ResultType.Flush:
                    case Bo89ResultType.None:
                        // ���l�J�� �P�� �� �S�P�� �u�n�@�˥H�W�N�|Ĺ
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
                        // �� ��i�P�� �J�� �T�i�P��
                        // �I�Ƥj�� Ĺ
                        // �I�Ƶ��� ��iĹ�T�i Ĺ
                        result = result1.totalPoint >= result2.totalPoint;
                        break;
                    case Bo89ResultType.Pairs:
                        // �� ��i�P�� �J�� ��i��l
                        // �I�Ƥj�� Ĺ
                        // �I�Ƶ��� ��l > �P��
                        result = result1.totalPoint > result2.totalPoint;
                        break;
                    case Bo89ResultType.Flush:
                        // �� ��i�P�� �J�� �P��
                        // ��� �̤j�i Ĺ
                        result = result1MaxCard.compareNumber > result2MaxCard.compareNumber;
                        break;
                    case Bo89ResultType.None:
                        // �� ��i�P�� �S�P�������p (��i�ΤT�i)
                        // �I�Ƥj�� �~��Ĺ
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
                        // �� �S���P�� �J�� �T�i�P�� / ��l
                        // �I�Ƥj�� �~��Ĺ
                        // �I�Ƶ��� �M ��i => Ĺ�A�T�i => ��
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
                        // �� �S���P�� �J�� ��i ��l / �P��
                        // �I�Ƥj�� Ĺ
                        result = result1.totalPoint > result2.totalPoint;
                        break;
                    case Bo89ResultType.None:
                        // �� �S���P�� �J�� �S���P��
                        if (result1.pokerCards.Count == 2)
                        {
                            // ��i�J��i
                            if (result2.pokerCards.Count == 2)
                            {
                                // �I�ƥ����n�W�L�~�|Ĺ
                                if (result2.totalPoint > result1.totalPoint)
                                {
                                    result = false;
                                }
                                else if (result2.totalPoint == result1.totalPoint)
                                {
                                    // �������ܭn��̤j�i
                                    result = result1MaxCard.compareNumber > result2MaxCard.compareNumber;
                                }
                            }
                            // ��i�J�T�i
                            else if (result2.pokerCards.Count == 3)
                            {
                                // �I�ƥ����n�W�L�~�|Ĺ
                                if (result2.totalPoint > result1.totalPoint)
                                {
                                    result = false;
                                }
                            }
                        }
                        else if (result1.pokerCards.Count == 3)
                        {
                            // �T�i�J��i
                            if (result2.pokerCards.Count == 2)
                            {
                                // �I�ƥu�n�����H�W�N�i�HĹ
                                if (result2.totalPoint >= result1.totalPoint)
                                {
                                    result = false;
                                }
                            }
                            // �T�i�J�T�i
                            else if (result2.pokerCards.Count == 3)
                            {
                                // �I�ƥ����n�W�L�~�|Ĺ
                                if (result2.totalPoint > result1.totalPoint)
                                {
                                    result = false;
                                }
                                // �������ܭn��̤j�i
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
                // �P�ᶶ
                result.resultType = Bo89ResultType.StraightFlush;
                result.resultInfo = bo89ResultSet.bo89ResultInfos.First(s => s.resultType == Bo89ResultType.StraightFlush);
                result.result = String.Format(result.resultInfo.GetResultFormat(), card1.name, card2.name, card3.name);

                // �Ӯa�P�ᶶ
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
        /// �Ӯa�P�ᶶ
        RoyalStraightFlush,
        // �P�ᶶ
        StraightFlush,
        // �T��
        ThreeOfAKind,
        // ���l
        Straight,
        // �T�P��
        ThreeOfFlush,
        // ��i�@��
        Pairs,
        // �P��
        Flush,
        // �S���P��
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

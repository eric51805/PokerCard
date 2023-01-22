using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Bo89Rule;

public class HandCardController : MonoBehaviour
{
    [SerializeField]
    private PokerCardSet pokerCardSet;

    [SerializeField]
    private Bo89ResultSet bo89ResultSet;

    [SerializeField]
    private GameObject pokerCardPrefab;

    [SerializeField]
    private GridLayoutGroup gridLayoutGroup;

    [SerializeField]
    private GameObject me;

    [SerializeField]
    private TextMeshProUGUI resultLabel;

    [SerializeField]
    private TextMeshProUGUI possibleLabel;

    [SerializeField]
    private TextMeshProUGUI strategyLabel;

    [SerializeField]
    private TextMeshProUGUI detailLabel;

    [SerializeField]
    private RectTransform content;

    private PokerCardView[] pokerCardViews;

    private int handCardCount;
    private List<PokerCardView> handCardViews = new List<PokerCardView>();
    private List<int> handCardIndexList = new List<int>();
    private List<int> blackCardIndexList = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        var cardSize = gridLayoutGroup.cellSize;

        pokerCardViews = new PokerCardView[pokerCardSet.pokerCards.Length];
        int index = 0;
        foreach (var card in pokerCardSet.pokerCards)
        {
            card.index = index;

            if (card.type == PokerCardDefine.Joker)
            {
                index++;
                continue;
            };

            GameObject newCard = Instantiate(pokerCardPrefab);
            newCard.transform.SetParent(transform);
            newCard.transform.localPosition = new Vector3();

            PokerCardView pokerCardView = newCard.GetComponent<PokerCardView>();
            pokerCardView.Init(card, OnCardClick);
            pokerCardView.SetCardBack(false);
            pokerCardView.SetCardSize(cardSize);

            pokerCardViews[index] = pokerCardView;
            index++;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartDealing()
    {

    }

    public void OnCardClick(PokerCard _pokerCard)
    {
        possibleLabel.text = "";
        detailLabel.text = "";
        strategyLabel.text = "";

        var pokerCard = pokerCardViews[_pokerCard.index];

        if (pokerCard != null)
        {
            if (handCardIndexList.Contains(_pokerCard.index))
            {
                // �^����P
                PokerCardView pokerCardView = handCardViews.First(s => s.pokerCard.index == _pokerCard.index);
                if (pokerCardView != null)
                {
                    pokerCard.SetCardBack();
                    handCardIndexList.Remove(pokerCard.pokerCard.index);
                    blackCardIndexList.Remove(pokerCard.pokerCard.index);
                    handCardViews.Remove(pokerCardView);
                    DestroyImmediate(pokerCardView.gameObject);
                    handCardCount -= 1;
                }
            }
            else if (blackCardIndexList.Contains(_pokerCard.index))
            {
                pokerCard.SetCardBack();
                blackCardIndexList.Remove(pokerCard.pokerCard.index);
            }
            else if (handCardCount < 2)
            {
                // ���P
                pokerCard.SetCardBack(true, true);

                GameObject newCard = Instantiate(pokerCardPrefab);
                newCard.transform.SetParent(me.transform);
                newCard.transform.localPosition = new Vector3();

                PokerCardView pokerCardView = newCard.GetComponent<PokerCardView>();
                pokerCardView.Init(pokerCard.pokerCard, OnCardClick);
                pokerCardView.SetCardBack(false);

                handCardViews.Add(pokerCardView);
                handCardIndexList.Add(pokerCard.pokerCard.index);
                blackCardIndexList.Add(pokerCard.pokerCard.index);

                handCardCount += 1;
            }
            else
            {
                // ���P
                pokerCard.SetCardBack(true, false);
                blackCardIndexList.Add(pokerCard.pokerCard.index);
            }
        }

        if (handCardCount <= 1)
        {
            resultLabel.text = "�п�P";
        }
        else if (handCardCount >= 2)
        {
            var pokerCardList = handCardViews.Select(s => s.pokerCard).ToList();
            var bo89Result = Bo89Rule.GetResult(pokerCardList, bo89ResultSet);
            resultLabel.text = bo89Result.result;

            CalcRestCard(bo89Result);
        }
    }

    public void CalcRestCard(Bo89Result twoCardResult)
    {
        var result = "";
        var handCardList = handCardViews.Select(s => s.pokerCard).ToList();
        var restResultList = new List<Bo89Result>();

        int count = 0;
        foreach (var view in pokerCardViews)
        {
            if (view == null) continue;

            if (blackCardIndexList.Contains(view.pokerCard.index))
                continue;

            handCardList.Add(view.pokerCard);
            Bo89Result bo89Result = GetResult(handCardList, bo89ResultSet);
            bo89Result.addCard = view.pokerCard;
            handCardList.Remove(view.pokerCard);

            restResultList.Add(bo89Result);

            count += 1;
        }

        SortBo89Result(restResultList);

        int changeToBigger = 0;
        int changeToSpecial = 0;

        foreach (Bo89Result bo89Result in restResultList)
        {
            var tempResult = "";
            var bo89ResultInfo = bo89ResultSet.bo89ResultInfos.Where(s => s.resultType == bo89Result.resultType).First();
            var isBiggerThanOriginal = Compare2Result(bo89Result, twoCardResult);
            var change = "";

            change = isBiggerThanOriginal ? "(�j)" : "(�p)";

            if (isBiggerThanOriginal)
                changeToBigger++;

            switch (bo89Result.resultType)
            {
                case Bo89ResultType.None:
                    tempResult = $"{bo89Result.addCard.name} �� {bo89Result.totalPoint} �I {change}\n";
                    break;
                case Bo89ResultType.ThreeOfFlush:
                    tempResult = $"{bo89Result.addCard.name} �� {bo89Result.totalPoint} �I {bo89ResultInfo.name} {change}\n";
                    break;
                case Bo89ResultType.Straight:
                case Bo89ResultType.ThreeOfAKind:
                case Bo89ResultType.StraightFlush:
                case Bo89ResultType.RoyalStraightFlush:
                    tempResult = $"{bo89Result.addCard.name} �� {bo89ResultInfo.name} {change}\n";
                    break;
            }

            if (bo89Result.resultType != Bo89ResultType.None)
                changeToSpecial++;

            result += tempResult;
        }

        var percentOfChangeToBigger = Convert.ToDouble(changeToBigger) / Convert.ToDouble(restResultList.Count);
        var percentOfChangeToSmaller = 1 - percentOfChangeToBigger;
        var percentOfChangeToBiggerStr = (percentOfChangeToBigger * 100).ToString("N2");
        var percentOfChangeToSmallerStr = (percentOfChangeToSmaller * 100).ToString("N2");

        var percentOfChangeToSpecial = Convert.ToDouble(changeToSpecial) / Convert.ToDouble(restResultList.Count);
        var percentOfChangeToSpecialStr = (percentOfChangeToSpecial * 100).ToString("N2");

        detailLabel.text = $"{percentOfChangeToBiggerStr} % �ܤj\n";
        detailLabel.text += $"{percentOfChangeToSmallerStr} % �ܤp\n";
        detailLabel.text += $"\n";
        detailLabel.text += $"{percentOfChangeToSpecialStr} % ���P��";

        strategyLabel.text = (percentOfChangeToBigger * 100) > 50 ? "��" : "����";

        content.sizeDelta = new Vector2(content.sizeDelta.x, 42.6f * count);
        possibleLabel.text = result;
    }

    private void SortBo89Result(List<Bo89Result> source)
    {
        for (int i = 0; i < source.Count; i++)
        {
            for (int j = 0; j < source.Count; j++)
            {
                if (Bo89Rule.Compare2Result(source[i], source[j]))
                {
                    var tempResult1 = source[i];
                    var tempResult2 = source[j];
                    source[i] = tempResult2;
                    source[j] = tempResult1;
                }
            }
        }
    }

    public void Clear()
    {
        foreach (int pokerCardIndex in handCardIndexList)
        {
            var pokerCard = pokerCardViews[pokerCardIndex];
            PokerCardView pokerCardView = handCardViews.First(s => s.pokerCard.index == pokerCardIndex);

            pokerCard.SetCardBack();
            blackCardIndexList.Remove(pokerCardView.pokerCard.index);
            handCardViews.Remove(pokerCardView);
            DestroyImmediate(pokerCardView.gameObject);
            handCardCount -= 1;
        }

        foreach (int pokerCardIndex in blackCardIndexList)
        {
            var pokerCard = pokerCardViews[pokerCardIndex];
            pokerCard.SetCardBack();
        }

        handCardCount = 0;
        handCardViews = new List<PokerCardView>();
        handCardIndexList = new List<int>();
        blackCardIndexList = new List<int>();

        possibleLabel.text = "";
        detailLabel.text = "";
        strategyLabel.text = "";
        resultLabel.text = "�п�P";

        content.sizeDelta = new Vector2(content.sizeDelta.x, 0);
    }
}

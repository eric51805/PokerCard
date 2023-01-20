using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private PokerCardView[] pokerCardViews;

    private int handCardCount;
    private List<PokerCardView> handCardViews = new List<PokerCardView>();
    private List<int> handCardIndex = new List<int>();

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
        var pokerCard = pokerCardViews[_pokerCard.index];

        if (pokerCard != null)
        {
            if (handCardIndex.Contains(_pokerCard.index))
            {
                // 回收手牌
                PokerCardView pokerCardView = handCardViews.First(s => s.pokerCard.index == _pokerCard.index);
                if (pokerCardView != null)
                {
                    pokerCard.SetCardBack();
                    handCardIndex.Remove(pokerCard.pokerCard.index);
                    handCardViews.Remove(pokerCardView);
                    DestroyImmediate(pokerCardView.gameObject);
                    handCardCount -= 1;
                }
            }
            else if (handCardCount < 3)
            {
                // 取牌
                pokerCard.SetCardBack();

                GameObject newCard = Instantiate(pokerCardPrefab);
                newCard.transform.SetParent(me.transform);
                newCard.transform.localPosition = new Vector3();

                PokerCardView pokerCardView = newCard.GetComponent<PokerCardView>();
                pokerCardView.Init(pokerCard.pokerCard, OnCardClick);
                pokerCardView.SetCardBack(false);

                handCardViews.Add(pokerCardView);
                handCardIndex.Add(pokerCard.pokerCard.index);

                handCardCount += 1;
            }
            else
            {
                // 出過的牌
            }
        }

        if (handCardCount <= 1)
        {
            resultLabel.text = "請選牌";
        }
        else if (handCardCount >= 2)
        {
            var pokerCardList = handCardViews.Select(s => s.pokerCard).ToList();
            var bo89Result = Bo89Rule.GetResult(pokerCardList, bo89ResultSet);
            resultLabel.text = bo89Result.result;
        }
    }
}

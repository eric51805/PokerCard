using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerCardSetController : MonoBehaviour
{

    [SerializeField]
    private PokerCardSet pokerCardSet;

    [SerializeField]
    private GameObject pokerCardPrefab;


    // Start is called before the first frame update
    void Start()
    {
        foreach(var card in pokerCardSet.pokerCards)
        {
            if (card.type == PokerCardDefine.Joker) continue;

            GameObject newCard = Instantiate(pokerCardPrefab);
            newCard.transform.SetParent(this.transform);
            newCard.transform.localPosition = new Vector3();

            PokerCardView pokerCardView = newCard.GetComponent<PokerCardView>();
            pokerCardView.Init(card, null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDealing()
    {

    }
}

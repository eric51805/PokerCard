using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PokerCardView : MonoBehaviour
{
    [SerializeField]
    private Image front;

    [SerializeField]
    private Image back;

    [SerializeField]
    private Sprite handCard;

    [SerializeField]
    private Sprite otherhandCard;

    public PokerCard pokerCard;

    private Action<PokerCard> onCardClick;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(PokerCard pokerCard, Action<PokerCard> onCardClick)
    {
        this.pokerCard = pokerCard;
        front.sprite = pokerCard.sprite;
        this.onCardClick = onCardClick;
    }

    public void SetCardBack(bool show, bool isHandCard = true)
    {
        back.enabled = show;
        back.sprite = isHandCard ? handCard : otherhandCard;
    }

    public bool SetCardBack()
    {
        back.enabled = !back.enabled;

        return back.enabled;
    }

    public void SetCardSize(Vector2 vector2)
    {
        var rect = GetComponent<RectTransform>();
        var rectFront = front.gameObject.GetComponent<RectTransform>();
        var rectBack = back.gameObject.GetComponent<RectTransform>();

        rect.sizeDelta = vector2;
        rectFront.sizeDelta = vector2;
        rectBack.sizeDelta = vector2;
    }

    public void OnPointerClick(BaseEventData eventData)
    {
        onCardClick?.Invoke(pokerCard);
    }
}

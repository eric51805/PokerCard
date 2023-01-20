using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PokerCard
{
    [HideInInspector]
    public int index;
    public PokerCardDefine type;
    public int number;
    public string name;
    public Sprite sprite;

    public int GetNumber()
    {
        return number > 10 ? 10 : number;
    }
}

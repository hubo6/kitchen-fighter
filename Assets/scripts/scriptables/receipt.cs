using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class receipt : ScriptableObject
{
    // Start is called before the first frame update
    public Transform prefab;
    public RECEIPT_MSK msk;
    public Sprite icon;
    public int stat = 0;
}

public enum RECEIPT_MSK { 
    PLATE = 1,
    TOMATO,
    TOMATO_SLICE,
    CHEESE,
    CHEESE_SLICE,
    MEAT_PIE,
}
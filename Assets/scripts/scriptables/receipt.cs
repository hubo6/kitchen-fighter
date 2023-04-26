using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class receipt : ScriptableObject
{
    // Start is called before the first frame update
    public Transform prefab;
    public RECEIPT_MSK msk;
    public ITEM_TYPE type;
    public Sprite icon;
    public float height;
    public int stat = 0;
}



public enum ITEM_TYPE { 
PLATE = 0,
RAW,
PROCESSED,
BURNT,
}
public enum RECEIPT_MSK { 
    NONE = 0,
    TOMATO,
    CHEESE,
    MEAT_PIE,
}
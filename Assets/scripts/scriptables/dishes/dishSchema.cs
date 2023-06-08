using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class dishSchema : ScriptableObject
{
    public List<itemCnf> dishOrder;
    public float waitingTime;
    public string dishName;
    public Image icon;
}

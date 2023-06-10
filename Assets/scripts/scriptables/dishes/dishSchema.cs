using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class dishSchema : ScriptableObject
{
    public List<itemCnf> dishOrder;
    public float waitingTime;
    public string dishName;
    public Image icon;
    public int msk = 0;

    private void Awake() {
        foreach (var d in dishOrder)
            msk |= 1 << (int)d.msk;
    }
}

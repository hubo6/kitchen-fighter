using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class dishSchema : ScriptableObject
{
    // Start is called before the first frame update
    public List<itemCnf> dishOrder;
    public float waitingTime;
    public string dishName;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class receiptCnf : ScriptableObject
{
    // Start is called before the first frame update
    public receipt input;
    public receipt output;
    public int progress;
}

[CreateAssetMenu()]
public class receiptCnf2 : ScriptableObject {
    // Start is called before the first frame update
    public receipt input;
    public receipt output;
    public int progress;
    public receipt last;
    public int timeout;
}

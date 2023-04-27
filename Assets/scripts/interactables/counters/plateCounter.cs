using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plateCounter : counter {
    // Start is called before the first frame update
    [SerializeField] itemCnf _receipt;
    [SerializeField] int _max;
    [SerializeField] plateGen _gen;
    public  override item holding() {
        return _gen.cur.Last?.Value;
    }

    public override bool receive(item i)
    {
        if (i.receipt.type != ITEM_TYPE.PLATE)
            return false;
        return _gen.add(i);
    }

    public override item remove(item i = null)
    {
        return _gen.remove();
    }
}

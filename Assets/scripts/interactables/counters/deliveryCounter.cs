using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deliveryCounter : counter {
    // Start is called before the first frame update
    [SerializeField] List<item> _clearedCache = new List<item>();
    public override bool receive(item i) {
        if (i.cnf.type != ITEM_TYPE.PLATE)
            return false;
        var p = i as plate;
        if (p.msk == 0)
            return false;


        var msk = p.clear(_clearedCache);
        deliveryMgr.ins.validate(msk, _clearedCache);
        foreach (var c in _clearedCache)
            Destroy(c.gameObject);
        Destroy(p.gameObject);

        return true;
    }
}

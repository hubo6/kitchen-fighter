using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deliveryCounter : counter {
    // Start is called before the first frame update
    [SerializeField] List<item> _clearedCache = new List<item>();
    [SerializeField] List<ITEM_MSK> _clearedMskCache = new List<ITEM_MSK>();
    public override bool receive(item i) {
        if (i.cnf.type != ITEM_TYPE.PLATE)
            return false;
        var p = i as plate;
        if (p.msk == 0)
            return false;


        var msk = p.clear(_clearedCache, _clearedMskCache);
        deliveryMgr.ins.validate(msk, _clearedCache, _clearedMskCache);
        foreach (var c in _clearedCache)
            Destroy(c.gameObject);
        Destroy(p.gameObject);

        return true;
    }
}

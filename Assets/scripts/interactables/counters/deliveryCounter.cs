using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deliveryCounter : counter {
    // Start is called before the first frame update

    public override bool receive(item i) {
        if (i.cnf.type != ITEM_TYPE.PLATE)
            return false;
        var p = i as plate;
        if (p.msk == 0)
            return false;
        p.clear(out List<item> cleared);
        //check the recipes here;
        //
        foreach (var c in cleared)
            Destroy(c.gameObject);
        Destroy(p.gameObject);

        return true;
    }
}

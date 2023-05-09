using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class itemCnfV2 {
    public itemCnf[] _refV1;
}

public class DishSchema {
    public List<itemCnf> dishOrder = new List<itemCnf>();
    public int msk = 0;
    public DishSchema(IEnumerator<itemCnf> itr) {
        while (itr.MoveNext()) {
            dishOrder.Add(itr.Current);
            msk |= 1 << (int)itr.Current.msk;
        };
    }
}

public class gameMgr : MonoSingleton<gameMgr> {
    // Start is called before the first frame update
    [SerializeField] itemCnfV2[] _testDishCnfsV2; 
    [SerializeField] Dictionary<int, DishSchema> _dishCnfsMap= new Dictionary<int, DishSchema>();
    public void onInteractableChged(interactable p, interactable n) {
        Debug.Log($"highlight: {p?.transform.tag} -> {n?.transform.tag}");
        p?.highlight(false);
        n?.highlight(true);
    }

    public bool plateReArrange(plate p) {
        DishSchema sch = null;
        foreach (var d in _dishCnfsMap) {
            if ((p.msk & d.Key) == p.msk) {
                sch = d.Value;
                break;
            }
        }
        if (sch != null)
            p.rearrange(sch);
        return sch != null;
    }

    public void Start() {
        foreach (var cnfv1 in _testDishCnfsV2) { 
            var dish = new DishSchema(cnfv1._refV1.Cast<itemCnf>().GetEnumerator());
            _dishCnfsMap.Add(dish.msk, dish); 
        }
 
    }


    public bool changeOwnerShip(owner src, owner dst) {
        var ret = false;
        var srcHolding = src.holding();
        var dstHolding = dst.holding();
        do {
            if (srcHolding == null && dstHolding == null)
                break;
            if (srcHolding.cnf.type == ITEM_TYPE.PLATE) {
                break;
            }
            if (dstHolding.cnf.type == ITEM_TYPE.PLATE) {
                break;
            }


        } while (false);
        return ret;
    }
}

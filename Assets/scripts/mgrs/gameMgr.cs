using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameMgr : MonoSingleton<gameMgr>
{
    // Start is called before the first frame update
    public void onInteractableChged(interactable p, interactable n)
    {
        Debug.Log($"highlight: {p?.transform.tag} -> {n?.transform.tag}");
        p?.highlight(false);
        n?.highlight(true);
    }

    public bool plateRedish(plate p) {
        return false;
    }


    public bool changeOwnerShip(owner src, owner dst) {
        var ret = false;
        var srcHolding = src.holding();
        var dstHolding = dst.holding();
        do
        {
            if (srcHolding == null && dstHolding == null)
                break;
            if (srcHolding.receipt.type == ITEM_TYPE.PLATE) {
                break;
            }
            if (dstHolding.receipt.type == ITEM_TYPE.PLATE) {
                break;
            }


        } while (false);
        return ret;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class trashCan : counter
{
    // Start is called before the first frame update

    protected IEnumerator Fade(Transform item) {
        while (item.position.y > 0)
        {
            item.position -= Vector3.up * Time.deltaTime;
            yield return null;
        }
        Destroy(item.gameObject);

    } 

    public override bool receive(item i)
    {
        if (i == null)
            return false;
        if (i.receipt.msk == RECEIPT_MSK.PLATE)
        {

            if (((plate)i).clear(out List<item> cleared)) {
                foreach (var c in cleared) {
                    i.transform.SetParent(_objAnchor);
                    i.transform.localPosition = Vector3.zero;
                    StartCoroutine(Fade(i.transform));
                }
            }
            return false;

        } else {
            i.transform.SetParent(_objAnchor);
            i.transform.localPosition = Vector3.zero;
            StartCoroutine(Fade(i.transform));
            return true;
        }



        //var ret = base.receive(i);
        //if (ret) {
        //    StartCoroutine(Fade(i.transform));
        //    _holding = null;
        //}
        //return ret;
    }
}

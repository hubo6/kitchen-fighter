using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class trashCan : counter {
    // Start is called before the first frame update

    protected IEnumerator fade(Transform item) {
        while (item.position.y > 0)
        {
            item.position -= Vector3.up * Time.deltaTime;
            yield return null;
        }
        Destroy(item.gameObject);

    }

    public override bool receive(item i)
    {
        var ret = false;
        do
        {
            if (i == null)
                break;
            if (i.receipt.type == ITEM_TYPE.PLATE)
            {
                ((plate)i).clear(out List<item> cleared);
                float offset = 0;
                foreach (var c in cleared) {
                    c.transform.SetParent(_objAnchor);
                    c.transform.localPosition = Vector3.zero + offset * Vector3.up;
                    offset += c.receipt.height;
                    StartCoroutine(fade(c.transform));
                }
                break;
            }
            i.transform.SetParent(_objAnchor);
            i.transform.localPosition = Vector3.zero;
            StartCoroutine(fade(i.transform));
            ret = true;
        } while (false);
        return ret;
      
       
        //i.transform.SetParent(_objAnchor);
        //i.transform.localPosition = Vector3.zero;
        //StartCoroutine(fade(i.transform));
        //return true;
        //var ret = base.receive(i);
        //if (ret) {
        //    StartCoroutine(fade(i.transform));
        //    _holding = null;
        //}
        //return ret;
    }
    public override item remove(item i) {
        return null;
    }
}

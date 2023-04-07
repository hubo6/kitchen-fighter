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
        var ret = base.receive(i);
        if (ret) {
            StartCoroutine(Fade(_holding.transform));
            _holding = null;
        }
        return ret;
    }
}

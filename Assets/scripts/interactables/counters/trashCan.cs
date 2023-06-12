using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class trashCan : counter {
    // Start is called before the first frame update
    [SerializeField] List<item> _clearedCache = new List<item>();
    public static new event Action<Transform> onDrop;
    protected IEnumerator fade(Transform item) {
        while (item.position.y > 0) {
            item.position -= Vector3.up * Time.deltaTime;
            yield return null;
        }
        Destroy(item.gameObject);

    }

    public override bool receive(item i) {
        var dump = false;
        var ret = false;
        do {
            if (i.cnf.type == ITEM_TYPE.PLATE) {
                var p = (plate)i;
                if (p.msk == 0)
                    break;
                ((plate)i).clear(_clearedCache);
                float offset = 0;
                foreach (var c in _clearedCache) {
                    c.transform.SetParent(_objAnchor);
                    c.transform.localPosition = Vector3.zero + offset * Vector3.up;
                    offset += c.cnf.height;
                    StartCoroutine(fade(c.transform));
                }
                dump = true;
                break;
            }
            {
                i.transform.SetParent(_objAnchor);
                i.transform.localPosition = Vector3.zero;
                StartCoroutine(fade(i.transform));
                dump = ret = true;
                break;
            }
        } while (false);
        if(dump) onDrop?.Invoke(transform);
        return ret;
    }
    public override item remove(item i) {
        return null;
    }
}

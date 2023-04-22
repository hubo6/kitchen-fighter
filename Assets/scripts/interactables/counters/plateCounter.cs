using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plateCounter : counter {
    // Start is called before the first frame update
    [SerializeField] receipt _receipt;
    [SerializeField] int _max;
    [SerializeField] plateGen _gen;
    public  override item holding() {
        return _gen.cur.Last?.Value;
    }
//    [SerializeField] plateGen _gen;
  
    //public override void Start()
    //{
    //    base.Start();
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
    //public override bool interact(owner src)
    //{
    //    var ret = false;
    //    do
    //    {
    //        if (src._holding()?.receipt.objName == "plate") {
    //            if (_gen.cur.Count < _gen._max) { 
    //                _gen.
    //            }
    //        }
    //        if (_gen.cur.Count == 0)
    //            break;
    //        var srcRecv = src.receive(_gen.cur.Last.Value);
    //        if (!srcRecv)
    //            break;
    //        _gen.remove();
    //        ret = true;

    //    } while (false);
    //    return ret;
    //}
    public override bool receive(item i)
    {
        return _gen.add(i);
    }

    public override item remove(item i = null)
    {
        return _gen.remove();
    }
}

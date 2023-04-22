using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plateCounter : counter
{
    // Start is called before the first frame update
    [SerializeField] receipt _receipt;
    [SerializeField] int _max;
  
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override bool interact(owner src)
    {
        var ret = false;
        do
        {
            var plates = _holding as plateGen;
            if (plates.cur.Count == 0)
                break;
            var srcRecv = src.receive(plates.cur.Last.Value);
            if (!srcRecv)
                break;
            plates.take();
            ret = true;

        } while (false);
        return ret;
    }
    public override bool receive(item i)
    {
        return false;
    }

    public override item remove(item i = null)
    {
        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class heatPanCounter : counter
{
    // Start is called before the first frame update
    [SerializeField] receiptCnf[] _receiptCnfs;
    [SerializeField] progressBar _progressBar;
    [SerializeField] receiptCnf _holdingReciptCnf;


    protected IEnumerator progress(receiptCnf cnf) {
        yield return null;
        
    }

    public override void Start()
    {
        if (_progressBar == null)
            throw new System.Exception($"_progress absent in {transform.name}.");
    }

    public override bool receive(item item)
    {
        //var ret = base.receive(item);

        receiptCnf reciptCnf = null;
        for (var i = 0; i < _receiptCnfs.Length; i++)
        {
            if (_receiptCnfs[i].input == item.receipt)
            {
                reciptCnf = _receiptCnfs[i];
                break;
            }
        }
        var ret = (reciptCnf != null) && base.receive(item);
        return ret;


        //if (ret)
        //{
        //    _holdingReciptCnf = null;
        //    for (var i = 0; i < _receiptCnfs.Length; i++)
        //    {
        //        if (_receiptCnfs[i].input == _holding.receipt)
        //        {
        //            _holdingReciptCnf = _receiptCnfs[i];
        //           // StartCoroutine(progress(_holdingReciptCnf));
        //            break;
        //        }
        //    }
        //}
        //return _holdingReciptCnf != null;
    }

    public override item remove(item i)
    {
        var ret = base.remove(i);
        if (ret)
        {
            _progressBar.display(false);
            _holdingReciptCnf = null;
        }
        return ret;
    }
}

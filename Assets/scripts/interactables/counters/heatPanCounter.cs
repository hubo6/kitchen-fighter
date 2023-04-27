using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class heatPanCounter : counter
{
    // Start is called before the first frame update
    [SerializeField] processableCnf[] _receiptCnfs;
    [SerializeField] progressBar _progressBar;
    [SerializeField] processableCnf _holdingReciptCnf;
    [SerializeField] GameObject _glowShim;
    [SerializeField] GameObject _splash;

    protected processableCnf getReceiptCnfByInput(itemCnf receipt) {
        processableCnf reciptCnf = null;
        for (var i = 0; i < _receiptCnfs.Length; i++)
        {
            if (_receiptCnfs[i].input == receipt)
            {
                reciptCnf = _receiptCnfs[i];
                break;
            }
        }
        return reciptCnf;
    }

    protected IEnumerator progress(processableCnf cnf) {
        yield return null;
        
    }

    public override void Start()
    {
        if (_progressBar == null)
            throw new System.Exception($"_progress absent in {transform.name}.");
    }

    public override bool receive(item item)
    {
        var ret = item.receipt.msk == ITEM_MSK.MEAT_PIE && base.receive(item);
        if(ret)
            _holdingReciptCnf = getReceiptCnfByInput(item.receipt);
        return ret;
    }

    public override item remove(item i = null)
    {
        var ret = base.remove(i);
        if (ret)
        {
            _progressBar.display(false);
            _holdingReciptCnf = null;
        }
        return ret;
    }

    private void Update() {
        if (!(_holdingReciptCnf && holding())) {
            _glowShim.SetActive(false);
            _splash.SetActive(false);
            return;
        }

       _glowShim.SetActive(true);
        var progress = holding().updateProgress(Time.deltaTime);
        if(holding().receipt.stat == 0)
            _progressBar.progress(progress / _holdingReciptCnf.progress);
        //if (_holding.itemCnf.stat == 1) //warning
        //    _progressBar.progress(progress / _holdingCnf.progress);
       _splash.SetActive(holding().receipt.stat == 1);
        if (progress < _holdingReciptCnf.progress)
            return;

        if (holding().receipt.stat == 0)
            _progressBar.display(false);

        var nextRecv = Instantiate(_holdingReciptCnf.output.prefab).GetComponent<item>();
        Destroy(remove()?.gameObject);
        receive(nextRecv);
    }
}

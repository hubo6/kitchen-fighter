using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class heatPanCounter : counter
{
    // Start is called before the first frame update
    [SerializeField] receiptCnf[] _receiptCnfs;
    [SerializeField] progressBar _progressBar;
    [SerializeField] receiptCnf _holdingReciptCnf;
    [SerializeField] GameObject _glowShim;
    [SerializeField] GameObject _splash;

    protected receiptCnf getReceiptCnfByInput(receipt receipt) {
        receiptCnf reciptCnf = null;
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
        receiptCnf reciptCnf = getReceiptCnfByInput(item.receipt);
        var ret = (reciptCnf != null) && base.receive(item);

        if (ret) 
            _holdingReciptCnf = reciptCnf;
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
        //if (_holding.receipt.stat == 1) //warning
        //    _progressBar.progress(progress / _holdingReciptCnf.progress);
       _splash.SetActive(holding().receipt.stat == 1);
        if (progress < _holdingReciptCnf.progress)
            return;

        if (holding().receipt.stat == 0)
            _progressBar.display(false);

        var nextRecv = Instantiate(_holdingReciptCnf.output.prefab).GetComponent<item>();
        Destroy(remove()?.gameObject);
        receive(nextRecv);

        var nextReceiptCnf = getReceiptCnfByInput(_holdingReciptCnf.output);
        if (!nextReceiptCnf)
            throw new System.Exception($"update burnning {transform.name} with {holding().receipt}");
    }
}

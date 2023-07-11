using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class heatPanCounter : counter {
    // Start is called before the first frame update
    [SerializeField] processableCnf[] _processableCnfs;
    [SerializeField] Dictionary<itemCnf, processableCnf> _processableCnfsMap = new Dictionary<itemCnf, processableCnf>();
    [SerializeField] progress _progress;
    [SerializeField] alert _alert;
    [SerializeField] processableCnf _holdingCnf;
    [SerializeField] GameObject _glowShim;
    [SerializeField] GameObject _splash;
    [SerializeField] AudioSource _audio;

    //protected IEnumerator progress(processableCnf cnf) {
    //    yield return null;

    //}

    public override void Start() {
        Assert.IsNotNull(_progress);
        Assert.IsNotNull(_audio);
        Assert.IsNull(Array.Find(_processableCnfs, i => {
            return i.input.msk != ITEM_MSK.MEAT_PIE && (i.input.type != ITEM_TYPE.RAW || i.input.type != ITEM_TYPE.PROCESSED);
        }));
        _progress.display(false);
        _alert.display(false);
        foreach (var p in _processableCnfs) _processableCnfsMap.Add(p.input, p);
    }

    public override bool receive(item item) {
        var ret = item.cnf.msk == ITEM_MSK.MEAT_PIE && base.receive(item);
        if (ret) 
            _processableCnfsMap.TryGetValue(item.cnf, out _holdingCnf);
        if(item.cnf.msk == ITEM_MSK.MEAT_PIE && item.cnf.type == ITEM_TYPE.RAW)
            _audio.Play();
        else
            _audio.Stop();
        _progress.display(_holding.cnf.stat == 0);
        return ret;
    }

    public override item remove(item i = null) {
        var ret = base.remove(i);
        if (ret) {
            _progress.display(false);
            _alert.display(false);
            _holdingCnf = null;
            _audio.Stop();
        }
        return ret;
    }

    private void Update() {
        if (!(_holdingCnf && holding())) {
            _glowShim.SetActive(false);
            _splash.SetActive(false);
            return;
        }

        _glowShim.SetActive(true);
        var curProgress = holding().updateProgress(Time.deltaTime);
        //onSizzle?.Invoke(transform);
        var ratio = curProgress / _holdingCnf.progress;
        if (holding().cnf.stat == 0)
            _progress.update(ratio);
        //if (_holding.itemCnf.stat == 1) //warning
        //    _progress.update(update / _holdingCnf.update);
        _splash.SetActive(holding().cnf.stat == 1);
        _alert.display(!(ratio < progress.warningThreshold));
        if (curProgress < _holdingCnf.progress)
            return;

        if (holding().cnf.stat == 0)
            _progress.display(false);

        if (IsServer)
            onSpawnItemServerRpc((int)_holdingCnf.output.type, (int)_holdingCnf.output.msk, holding().NetworkObject, netRef());

        //var nextRecv = Instantiate(_holdingCnf.output.prefab).GetComponent<item>();
        //Destroy(remove()?.gameObject);
        //receive(nextRecv);
    }

    [ServerRpc(RequireOwnership = false)]
    void onSpawnItemServerRpc(int type, int msk, NetworkObjectReference toDel, NetworkObjectReference parent) {
        toDel.TryGet(out var toDelNetObj);
        toDelNetObj.Despawn();
        var cnf = deliveryMgr.ins.itemCnfsCache[(ITEM_TYPE)type][(ITEM_MSK)msk];
        var item = Instantiate(cnf.prefab);
        var netObj = item.GetComponent<NetworkObject>();
        netObj.Spawn(true);
        onSpawnItemClientRpc(type, msk, netObj, parent);
    }

    [ClientRpc]
    void onSpawnItemClientRpc(int type, int msk, NetworkObjectReference child, NetworkObjectReference parent) {
        parent.TryGet(out var netRefParent);
        child.TryGet(out var netRefchild);
        Assert.IsNotNull(netRefParent); Assert.IsNotNull(netRefchild);
        remove(holding());
        netRefParent.GetComponent<heatPanCounter>().receive(netRefchild.GetComponent<item>());
    }
}

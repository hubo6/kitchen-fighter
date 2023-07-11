using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class cuttingCounter : counter {
    // Start is called before the first frame update
    [SerializeField] processableCnf[] _cnfs;
    [SerializeField] Dictionary<ITEM_MSK, processableCnf> _cnfsMap = new Dictionary<ITEM_MSK, processableCnf>();
    [SerializeField] progress _progress;
    [SerializeField] protected counterAnim _counterAnim;
    [SerializeField] processableCnf _holdingCnf;
    public static event Action<Transform> onChop;
    public static new void resetEvt() {
        onChop = null;
    }
    public override void Start() {
        Assert.IsNotNull(_progress);
        _counterAnim = GetComponentInChildren<counterAnim>();
        Assert.IsNotNull(_counterAnim);
        _progress.gameObject.SetActive(false);
        foreach (var c in _cnfs) _cnfsMap[c.input.msk] = c;
        _counterAnim.OnAnimEvt += () => {
            var ratio = holding().updateProgress(1);
            Debug.Log($"cutting for {holding().cnf} -> {_holdingCnf.output} update {ratio}");
            onChop?.Invoke(transform);
            _progress.display(ratio < _holdingCnf.progress);
            if (ratio < _holdingCnf.progress) {
                _progress.update((float)ratio / _holdingCnf.progress);
                return;
            }
            if (IsServer)
                onSpawnItemServerRpc((int)_holdingCnf.output.type, (int)_holdingCnf.output.msk, holding().NetworkObject, this.NetworkObject);
        };
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
        netRefParent.GetComponent<cuttingCounter>().receive(netRefchild.GetComponent<item>());

    }

    // Update is called once per frame
    void Update() {

    }

    public override bool receive(item i) {
        var ret = base.receive(i);
        if (ret) {
            _holdingCnf = null;
            if (i.cnf.type == ITEM_TYPE.RAW)
                _cnfsMap.TryGetValue(i.cnf.msk, out _holdingCnf);
        }
        return ret;
    }

    public override item remove(item i) {
        var ret = base.remove(i);
        if (ret) {
            _progress.display(false);
            _holdingCnf = null;
        }
        return ret;
    }

    public override void process() {
        if (holding() == null || _holdingCnf == null)
            return;
        if (_counterAnim.playing())
            return;
        _counterAnim.play();
    }
}

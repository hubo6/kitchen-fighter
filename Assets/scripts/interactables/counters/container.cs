using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

public class container : counter {
    [SerializeField] itemCnf _itemCnf;
    [SerializeField] protected counterAnim _counterAnim;
    // Start is called before the first frame update
    public override void Start() {
        base.Start();
        _counterAnim = GetComponentInChildren<counterAnim>();
        Assert.IsNotNull(_itemCnf);
        Assert.IsNotNull(_counterAnim);
        _counterAnim.OnAnimEvt += () => {
            if(IsServer)
                onSpawnItemServerRpc((int)_itemCnf.type, (int)_itemCnf.msk, this.NetworkObject);
        };
    }

    //public override void OnNetworkSpawn() {
    //    _counterAnim.OnAnimEvt += () => {
    //        onSpawnItemServerRpc((int)_itemCnf.type, (int)_itemCnf.msk, this.NetworkObject);
    //    };
    //}


    public override bool interact(owner src) {
        var ret = false;
        do {
            if (base.interact(src))
                break;
            if (holding() != null)
                break;
            if (_counterAnim.playing()) {
                Debug.LogWarning($"{transform.name} is busy of interaction.");
                break;
            }
            onOpenLidServerRpc();
            ret = true;

        } while (false);
        return ret;
    }


    [ServerRpc(RequireOwnership = false)]

    void onOpenLidServerRpc() {
        onOpenLidClientRpc();
    }
    [ClientRpc]

    void onOpenLidClientRpc() {
        _counterAnim.play();
    }

    [ServerRpc(RequireOwnership = false)]

    void onSpawnItemServerRpc(int type, int msk, NetworkObjectReference parent) {
        var cnf = deliveryMgr.ins.itemCnfsCache[(ITEM_TYPE)type][(ITEM_MSK)msk];
        var prefab = Instantiate(cnf.prefab);
        prefab.GetComponent<NetworkObject>().Spawn(true);
        parent.TryGet(out NetworkObject netRef);
        recvClientRpc(prefab.GetComponent<item>().NetworkObject, parent);
    }

    [ClientRpc]
    void recvClientRpc(NetworkObjectReference child, NetworkObjectReference parent) {
        parent.TryGet(out var netRefParent);
        child.TryGet(out var netRefchild);
        Assert.IsNotNull(netRefParent); Assert.IsNotNull(netRefchild);
        netRefParent.GetComponent<container>().receive(netRefchild.GetComponent<item>());
    }



    public void onClosed() {
    }
}

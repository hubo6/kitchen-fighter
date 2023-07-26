
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class dishSchemaCounter {
    public dishSchema schemaRef;
    public float timepass;
}



public class deliveryMgr : netSingleton<deliveryMgr> {
    // Start is called before the first frame update
    [SerializeField] dishSchema[] _dishSchemas;
    public dishSchema[] dishSchemas { get => _dishSchemas; }
    public Dictionary<ITEM_TYPE, Dictionary<ITEM_MSK, itemCnf>> itemCnfsCache { get => _itemCnfsCache; }

    [SerializeField] Dictionary<int, dishSchema> _dishCnfsMap = new Dictionary<int, dishSchema>();
    //[SerializeField] Dictionary<dishSchemaCounter, dishSchemaCounter> _waitingMap = new Dictionary<dishSchemaCounter, dishSchemaCounter>();
    [SerializeField] List<dishSchemaCounter> _waitingList = new List<dishSchemaCounter>();
    [SerializeField] float _genGapSec = 5.0f;
    [SerializeField] float _timeStamp = 0f;
    [SerializeField] List<dishSchemaCounter> _toDel = new List<dishSchemaCounter>();
    [SerializeField] List<dishSchemaCounter> _toUpdate = new List<dishSchemaCounter>();
    [SerializeField] itemCnf[] _itemCnfs;
    public event Action<dishSchemaCounter> onAdd;
    public event Action<int> onRm;
    public event Action<List<dishSchemaCounter>> onUpdate;
    public event Action<bool> onDeliver;
    public int max;

    [SerializeField] Dictionary<ITEM_TYPE, Dictionary<ITEM_MSK, itemCnf>> _itemCnfsCache = new();

    public bool plateReArrange(plate p) {
        dishSchema sch = null;
        foreach (var d in _dishCnfsMap) {
            if ((p.msk & d.Key) == p.msk) {
                sch = d.Value;
                break;
            }
        }
        if (sch != null)
            p.rearrange(sch);
        return sch != null;
    }

    public void Start() {
        foreach (var dish in _dishSchemas)
            _dishCnfsMap.Add(dish.msk, dish);

        foreach (var cnf in _itemCnfs) {
            if (!_itemCnfsCache.ContainsKey(cnf.type))
                _itemCnfsCache.Add(cnf.type, new Dictionary<ITEM_MSK, itemCnf>());
            _itemCnfsCache[cnf.type][cnf.msk] = cnf;
        }
    }

    public bool validate(int msk, List<item> items) {
        dishSchemaCounter ret = null;
        var idx = 0;
        foreach (var dish in _waitingList) {
            if (dish.schemaRef.msk != msk) {
                idx++;
                continue;
            }
            if (dish.schemaRef.dishOrder.Count != items.Count) {
                idx++;
                continue;
            }
            //var itr0 = dish.schemaRef.dishOrder.GetEnumerator();

            //var match = false;
            //while (itr0.MoveNext()) {
            //    if (items.Any())
            //        break;
            //}
            //if (!match) {
            //    dishIdx++;
            //    continue;
            //}
            ret = dish;
            break;
        }

        deliveryFinishedServerRpc(idx, idx < _waitingList.Count);
        //if (dishIdx < _waitingList.Count) {
        //    deliveryFinishedServerRpc(dishIdx);
        //    //_waitingList.Remove(ret);
        //    //onRm?.Invoke(dishIdx);
        //}
        return ret != null;
    }

    [ServerRpc(RequireOwnership = false)]

    void deliveryFinishedServerRpc(int dishIdx, bool succ) {
        deliveryFinishClientRpc(dishIdx, succ);
       
    }
    [ClientRpc]
    void deliveryFinishClientRpc(int idx, bool succ) {
        if (succ) {
            onRm?.Invoke(idx);
            _waitingList.RemoveAt(idx);
        }
         
        onDeliver?.Invoke(succ);
      
    }

    [ClientRpc]

    void recipeAddClientRpc(int dishIdx) {
        var dish = new dishSchemaCounter() {
            schemaRef = ins.dishSchemas[dishIdx],
            timepass = 0
        };
        _waitingList.Add(dish);
        onAdd?.Invoke(dish);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer)
            return;
        if (!gameMgr.ins.running())
            return;
        _timeStamp += Time.deltaTime;
        _toDel.Clear();
        _toUpdate.Clear();
        foreach(var item in _waitingList) {
            if (item.timepass >= item.schemaRef.waitingTime) {
                _toDel.Add(item);
            } else {
                item.timepass += Time.deltaTime;
                _toUpdate.Add(item);
            }
        }
        if (_toDel.Count > 0) {
           // recipeRmClientRpc(_toDel);
        }
           
        if (_toUpdate.Count > 0) 
            onUpdate?.Invoke(_toUpdate);
        if (_timeStamp > _genGapSec) {
            _timeStamp = 0;
            if (_waitingList.Count < max) {
                recipeAddClientRpc(Random.Range(0, ins.dishSchemas.Length));
            }
        }
    }
}

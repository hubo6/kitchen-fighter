
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
    [SerializeField] Dictionary<int, dishSchema> _dishCnfsMap = new Dictionary<int, dishSchema>();
    //[SerializeField] Dictionary<dishSchemaCounter, dishSchemaCounter> _waitingMap = new Dictionary<dishSchemaCounter, dishSchemaCounter>();
    [SerializeField] List<dishSchemaCounter> _waitingList = new List<dishSchemaCounter>();
    [SerializeField] float _genGapSec = 5.0f;//test
    [SerializeField] float _timeStamp = 0f;//test
    [SerializeField] List<dishSchemaCounter> _toDel = new List<dishSchemaCounter>();
    [SerializeField] List<dishSchemaCounter> _toUpdate = new List<dishSchemaCounter>();
    public event Action<dishSchemaCounter> onAdd;
    public event Action<int> onRm;
    public event Action<List<dishSchemaCounter>> onUpdate;
    public event Action<dishSchemaCounter> onDeliver;
    public int max;

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
            //    idx++;
            //    continue;
            //}
            ret = dish;
            break;
        }
        onDeliver?.Invoke(ret);
        if (idx < _waitingList.Count) {
            deliverySuccessServerRpc(idx);
            //_waitingList.Remove(ret);
            //onRm?.Invoke(idx);
        }
        return ret != null;
    }

    [ServerRpc(RequireOwnership = false)]

    void deliverySuccessServerRpc(int idx) {
        deliverySuccessClientRpc(idx);
       
    }
    [ClientRpc]
    void deliverySuccessClientRpc(int idx) {
        onRm?.Invoke(idx);
        _waitingList.RemoveAt(idx);
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

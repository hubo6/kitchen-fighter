
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using static UnityEngine.InputSystem.InputAction;
using Random = UnityEngine.Random;

public class dishSchemaCounter {
    public dishSchema schemaRef;
    public float timepass;
}



public class deliveryMgr : MonoSingleton<deliveryMgr> {
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
    public event Action<List<dishSchemaCounter>> onRm;
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

    public bool validate(int msk, List<item> items, List<ITEM_MSK> withOrder) {
        if (items.Count != withOrder.Count)
            return false;
        dishSchemaCounter ret = null;
        foreach (var dish in _waitingList) {
            if (dish.schemaRef.msk != msk)
                continue;
            if (dish.schemaRef.dishOrder.Count != items.Count)
                continue;
            var itr0 = dish.schemaRef.dishOrder.GetEnumerator();
            var itr1 = withOrder.GetEnumerator();
            var match = false;
            while (itr1.MoveNext() && itr0.MoveNext()) {
                if (!(match = (itr1.Current == itr0.Current.msk)))
                    break;
            }
            if (!match)
                continue;
            ret = dish;
            break;
        }
        onDeliver?.Invoke(ret);
        if (ret != null) {
            _waitingList.Remove(ret);
            onRm?.Invoke(new List<dishSchemaCounter>() { ret });
        }
        return ret != null;
    }
    // Update is called once per frame
    void Update()
    {
        if (gameMgr.ins.stage != gameMgr.STAGE.STARTED)
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
            onRm?.Invoke(_toDel);
            _toDel.ForEach((i) => _waitingList.Remove(i));
        }
           
        if (_toUpdate.Count > 0) 
            onUpdate?.Invoke(_toUpdate);
        if (_timeStamp > _genGapSec) {
            _timeStamp = 0;
            if (_waitingList.Count < max) {
                var dish = new dishSchemaCounter() { 
                    schemaRef = ins.dishSchemas[Random.Range(0, ins.dishSchemas.Length)], 
                    timepass = 0
                };
                //_waitingMap.Add(dish, dish);
                _waitingList.Add(dish);
                onAdd?.Invoke(dish);
            }
        }
    }
}

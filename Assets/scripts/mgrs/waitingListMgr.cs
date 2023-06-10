
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;
using Random = UnityEngine.Random;

public class dishSchemaCounter {

    public dishSchema schemaRef;
    public float timepass;

}



public class waitingListMgr : MonoSingleton<waitingListMgr> {
    // Start is called before the first frame update
    [SerializeField] Dictionary<dishSchemaCounter, dishSchemaCounter> _waiting = new Dictionary<dishSchemaCounter, dishSchemaCounter>();
    [SerializeField] float _genGapSec = 1.0f;//test
    [SerializeField] float _timeStamp = 0f;//test
    [SerializeField] List<dishSchemaCounter> _toDel = new List<dishSchemaCounter>();
    [SerializeField] List<dishSchemaCounter> _toUpdate = new List<dishSchemaCounter>();
    public event Action<dishSchemaCounter> onAdd;
    public event Action<List<dishSchemaCounter>> onRm;
    public event Action<List<dishSchemaCounter>> onUpdate;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timeStamp += Time.deltaTime;

        _toDel.Clear();
        _toUpdate.Clear();
        var itr = _waiting.GetEnumerator();
        foreach(var key in _waiting.Keys.ToList()) {
            if (key.timepass >= key.schemaRef.waitingTime) {
                _toDel.Add(key);
                _waiting.Remove(key);
            } else {
                key.timepass += Time.deltaTime;
                _toUpdate.Add(key);
            }
        }
        if (_toDel.Count > 0) 
            onRm?.Invoke(_toDel);
        if (_toUpdate.Count > 0) 
            onUpdate?.Invoke(_toUpdate);
        if (_timeStamp > _genGapSec) {
            _timeStamp = 0;
            var dish = new dishSchemaCounter() { schemaRef = gameMgr.ins.dishSchemas[Random.Range(0, gameMgr.ins.dishSchemas.Length)], timepass = 0};
            _waiting.Add(dish,dish);
            onAdd?.Invoke(dish);
        }
    }
}

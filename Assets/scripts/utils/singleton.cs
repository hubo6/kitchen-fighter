using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

public class monoSingleton<T> : MonoBehaviour where T : Component {
    // Start is called before the first frame update

    static T _ins;
    public virtual void Awake() {
        if (_ins == null)
            _ins = this as T;
    }
    public static T ins {
        get {
            //if (_ins == null) {
            //    var type = typeof(T);
            //    var objs = FindObjectsOfType(type) as T[];
            //    if (objs.Length > 1)
            //        throw new System.Exception($"error: {type.Name} instance duplicated.");
            //    if (objs.Length == 0) {
            //        var gobj = new GameObject(type.Name);
            //        _ins = gobj.AddComponent<T>();
            //    } else
            //        _ins = objs[0];
            //}

            Assert.IsNotNull(_ins);
            return _ins;
        }

    }
}

public class netSingleton<T> : NetworkBehaviour where T : Component {
    // Start is called before the first frame update

    static T _ins;

    public virtual void Awake() {
        if (_ins == null) {
            DontDestroyOnLoad(this);
            _ins = this as T;
        }
    }

    public static T ins {
        get {
            //if (_ins == null) {
            //    var type = typeof(T);
            //    var objs = FindObjectsOfType(type) as T[];
            //    if (objs.Length > 1)
            //        throw new System.Exception($"error: {type.Name} instance duplicated.");
            //    if (objs.Length == 0) {
            //        var gobj = new GameObject(type.Name);
            //        _ins = gobj.AddComponent<T>();
            //    } else
            //        _ins = objs[0];
            //}
            Assert.IsNotNull(_ins);
            return _ins;
        }

    }

}


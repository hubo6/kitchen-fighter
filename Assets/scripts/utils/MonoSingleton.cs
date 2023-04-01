using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton <T> : MonoBehaviour where T :Component
{
    // Start is called before the first frame update

    static T _ins;
    public static T ins 
    {
        get {
            if (_ins == null) {
                var type = typeof(T);
                var objs = FindObjectsOfType(type) as T[];
                if (objs.Length  > 1)
                    throw new System.Exception($"error: {type.Name} instance duplicated.");
                if (objs.Length == 0)
                {
                    var gobj = new GameObject(type.Name);
                    _ins = gobj.AddComponent<T>();
                }
                else
                    _ins = objs[0];
            }
            return _ins;
        }
    
    }

}

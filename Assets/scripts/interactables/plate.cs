using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class plate : item, owner
{
    [SerializeField] Dictionary<string, int> _cnf = new Dictionary<string, int>();
    [SerializeField] Dictionary<string, int> _contained = new Dictionary<string, int>();
    public item holding()
    {
        return this;
    }

    public bool receive(item i)
    {
        throw new System.NotImplementedException();
    }

    public item remove(item i = null)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

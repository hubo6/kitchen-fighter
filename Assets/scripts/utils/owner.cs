using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface owner {
    bool receive(item i);
    item remove(item i = null);
    item holding();
}

using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

public class NetAnimator : NetworkAnimator {
    // Start is called before the first frame update

    protected override bool OnIsServerAuthoritative() {
        return false;
    }
}

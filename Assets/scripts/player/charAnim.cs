using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Animator))]
public class charAnim : NetworkBehaviour {
    // Start is called before the first frame update
    [SerializeField] player _player;
    [SerializeField] Animator _animator;
    void Start() {
        Assert.IsNotNull(_animator);
    }

    // Update is called once per frame
    void Update() {
        if (!IsOwner)
            return;
        _animator.SetBool("walking", _player.inputV3 != Vector3.zero);
    }
}

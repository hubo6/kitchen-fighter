using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class charAnim : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] player _player;
    [SerializeField] Animator _animator;
    void Start() {
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() {
        _animator.SetBool("walking", _player.inputV3 != Vector3.zero);
    }
}

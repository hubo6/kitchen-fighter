using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class progressBar : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] Image _progress;
    [SerializeField] Transform _sign;
    [SerializeField] Animator _animator;
    public float warningThreshold = 0.65f;
    void Start() {
        Assert.IsNotNull(_progress);
        if(_sign != null)
            _sign.gameObject.SetActive(false);
        if (_animator != null)
            _animator.SetBool("warning", false);
    }

    public void progress(float p) {
        if (!displaying()) display();
        _progress.fillAmount = Mathf.Abs(p) % 1f;
        if (_progress.fillAmount > warningThreshold) {
            if(_sign != null && !_sign.gameObject.activeSelf)
                _sign.gameObject.SetActive(true);
            if(_animator != null && !_animator.GetBool("warning"))
                _animator.SetBool("warning", true);
        }
    }

    public void display(bool display = true) {
        gameObject.SetActive(display);
        if (!display  && _sign != null && _sign.gameObject.activeSelf)
            _sign?.gameObject.SetActive(display);
        if (!display && _animator != null)
            _animator.SetBool("warning", display);
    }

    public bool displaying() {
        return gameObject.activeSelf;
    }
}

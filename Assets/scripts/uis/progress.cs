using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class progress : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] Image _progress;
    //[SerializeField] Transform _sign;
    [SerializeField] Animator _animator;
    public static float warningThreshold = 0.7f;
    void Start() {
        Assert.IsNotNull(_progress);
        //Assert.IsNotNull(_animator);
        //if(_sign != null)
        //    _sign.gameObject.SetActive(false);
        //if (_animator != null)
        //_animator.SetBool("warning", false);
    }

    public void update(float p) {
        _progress.fillAmount = Mathf.Abs(p) % 1f;
        if (_progress.fillAmount > warningThreshold) {
            //if(_sign != null && !_sign.gameObject.activeSelf)
            //    _sign.gameObject.SetActive(true);
            if (_animator != null && !_animator.GetBool("warning"))
                _animator.SetBool("warning", true);
        } else {
            if (_animator != null && _animator.GetBool("warning"))
                _animator.SetBool("warning", false);
        }
    }

    public void display(bool display = true) {
        if(gameObject.activeSelf != display)
            gameObject.SetActive(display);
    }

    public bool displaying() {
        return gameObject.activeSelf;
    }
}

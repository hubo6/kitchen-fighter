using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class progressBar : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] Image _progress;

    void Start() {
        if (_progress == null) throw new System.Exception($"_progress absent in {transform.name}.");
    }

    public void progress(float p) {
        if (!displaying()) display();
        _progress.fillAmount = Mathf.Abs(p) % 1f;
    }

    public void display(bool display = true) {
        gameObject.SetActive(display);
    }

    public bool displaying() {
        return gameObject.activeSelf;
    }
}

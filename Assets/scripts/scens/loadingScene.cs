using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadingScene : MonoBehaviour {
    [SerializeField] Image _process;
    [SerializeField] TextMeshProUGUI _txt;
    // Start is called before the first frame update
    void Start() {
        Assert.IsNotNull(_process);
        Assert.IsNotNull(_txt);
        staticResetMgr.resetEnv();
        StartCoroutine(loading(3));
    }

    protected IEnumerator loading(int secs) {
        for (var i = 0; i < secs; i++) {
            yield return new WaitForSeconds(1);
            var ratio = (float)i / secs;
            _process.fillAmount = ratio;
            _txt.SetText(String.Format("Loading......{0:P2}.", ratio));
        }
        SceneManager.LoadScene(gameMgr.SCENE.GAME.ToString());
    }
    // Update is called once per frame
    void Update() {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseUi : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] Button _btnOpt;
    [SerializeField] Button _btnResume;
    [SerializeField] Button _btnQuit;
    [SerializeField] settingUi _setting;
    void Start() {
        Assert.IsNotNull(_btnOpt); Assert.IsNotNull(_btnResume); Assert.IsNotNull(_btnQuit); Assert.IsNotNull(_setting);
        _btnResume.onClick.AddListener(() => {
            gameMgr.ins.togglePauseServerRpc();
        });
        _btnQuit.onClick.AddListener(() => SceneManager.LoadScene(gameMgr.SCENE.MAIN.ToString()));
        _btnOpt.onClick.AddListener(() => {
            gameObject.SetActive(false);
            _setting.gameObject.SetActive(true);
        });
    }

    // Update is called once per frame
    void Update() {

    }
}

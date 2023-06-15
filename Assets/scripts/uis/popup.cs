using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class popup : MonoBehaviour {
    // Start is called before the first frame update

    [SerializeField] Transform _gameOver;
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] Transform _options;


    [SerializeField] Button _btn_opt;
    [SerializeField] Button _btn_resume;
    [SerializeField] Button _btn_quit;


    void Start() {
        Assert.IsNotNull(_gameOver);
        Assert.IsNotNull(_options);
        Assert.IsNotNull(_score);
        Assert.IsNotNull(_btn_opt);
        Assert.IsNotNull(_btn_resume);
        Assert.IsNotNull(_btn_quit);
        gameObject.SetActive(false);
        _score.gameObject.SetActive(false);
        _options.gameObject.SetActive(false);
        _btn_resume.onClick.AddListener(() => gameMgr.ins.togglePause());
        _btn_quit.onClick.AddListener(() => SceneManager.LoadScene(gameMgr.SCENE.MAIN.ToString()));
        gameMgr.ins.onStageChg += (gameMgr.STAGE s) => {
            if (s == gameMgr.STAGE.END) {
                gameObject.SetActive(true);
                _options.gameObject.SetActive(false);
                _score.gameObject.SetActive(true);
            } else if (s == gameMgr.STAGE.PAUSED) {
                gameObject.SetActive(true);
                _options.gameObject.SetActive(true);
                _score.gameObject.SetActive(false);
            } else if (s == gameMgr.STAGE.STARTED) {
                gameObject.SetActive(false);
                _options.gameObject.SetActive(false);
                _score.gameObject.SetActive(false);
            }
        };
    }

    // Update is called once per frame
    void Update() {

    }
}

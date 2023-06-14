using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button _start;
    [SerializeField] Button _opt;
    [SerializeField] Button _quit;
    void Start()
    {
        Assert.IsNotNull(_start);
        Assert.IsNotNull(_opt);
        Assert.IsNotNull(_quit);

        _start.onClick.AddListener(() => {
            SceneManager.LoadScene(gameMgr.SCENE.LOADING.ToString());
        });
        _quit.onClick.AddListener(() => {
            Application.Quit();
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

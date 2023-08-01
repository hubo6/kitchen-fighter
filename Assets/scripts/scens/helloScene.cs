using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class helloScene : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(fade(2));
    }

    protected IEnumerator fade(int secs) {
        yield return new  WaitForSeconds(secs);
        SceneManager.LoadScene(gameMgr.SCENE.GAME.ToString());
    }


    // Update is called once per frame
    void Update() {

    }
}

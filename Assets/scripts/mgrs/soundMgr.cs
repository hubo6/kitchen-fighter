using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Assertions;

public class soundMgr : MonoSingleton<soundMgr>
{
    // Start is called before the first frame update
    [SerializeField] soundCnf _cnf;
    void Start()
    {
        Assert.IsNotNull(_cnf);
        var objs = GameObject.FindGameObjectsWithTag("Counter");
        foreach (var i in objs) {
            var counter = i.GetComponent<counter>();
            counter.onPick += onPick;
            counter.onDrop += onDrop;
        }

        objs = GameObject.FindGameObjectsWithTag("Container");
        foreach (var i in objs) {
            var counter = i.GetComponent<counter>();
            counter.onPick += onPick;
            counter.onDrop += onDrop;
        }

        objs = GameObject.FindGameObjectsWithTag("TrashCan");
        foreach (var i in objs) {
            var counter = i.GetComponent<trashCan>();
            counter.onDrop += onDump;
        }

        //var player = obj.GetComponent<player>();
        //player.onPick += onPick;
        //player.onDrop += onDrop;

    }


    void onPick(Transform t) {
        play(_cnf.pick, t.position);
    }

    void onDrop(Transform t) {
        play(_cnf.drop, t.position);
    }

    void onContainer() { 
    }

    void onHeadPanSizzle() { 
    }

     void onMove() {
        
    }

    void onChop() { }

    void onDump(Transform t) {
        play(_cnf.dump, t.position);
    }

    void onSuccess() { 
    }

    void onFail() { 
    }

    void onWarning() { 
    }


    void play(AudioClip[] clips, Vector3 pos, float vol = 1f, bool random = true) {
        AudioSource.PlayClipAtPoint(clips[random ? Random.Range(0, clips.Length) : 0], pos, vol);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

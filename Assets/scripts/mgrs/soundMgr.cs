using UnityEngine;
using UnityEngine.Assertions;

public class soundMgr : monoSingleton<soundMgr>
{
    // Start is called before the first frame update
    [SerializeField] soundCnf _cnf;
    //[SerializeField] float _music_vol = 0.5f;
    [SerializeField] float _effect_vol = 0.5f;
    [SerializeField] AudioSource _music;

    public float music_vol { get => _music.volume; set => _music.volume = value; }
    public float effect_vol { get => _effect_vol; set => _effect_vol = value; }

    void Start()
    {
        Assert.IsNotNull(_cnf);
        Assert.IsNotNull(_music);
        counter.onPick += onPick;
        counter.onDrop += onDrop;

        cuttingCounter.onChop += onChop;
        trashCan.onDrop += onDump;
    }


    void onPick(Transform t) {
        play(_cnf.pick, t.position);
    }

    void onDrop(Transform t) {
        play(_cnf.drop, t.position);
    }

    void onContainer() { 
    }

    void onHeadPanSizzle(Transform t) {
        play(_cnf.heatpanSizzle, t.position);
    }

     public void onMove(Transform t) {
        play(_cnf.move, t.position);
    }

    void onChop(Transform t) {
        play(_cnf.chop, t.position);
    }

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
        AudioSource.PlayClipAtPoint(clips[random ? Random.Range(0, clips.Length) : 0], pos, effect_vol);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

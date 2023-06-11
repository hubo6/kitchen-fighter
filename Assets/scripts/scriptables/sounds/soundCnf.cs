using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class soundCnf : ScriptableObject
{
    // Start is called before the first frame update
    public AudioClip[] chop;
    public AudioClip[] move;
    public AudioClip[] pick;
    public AudioClip[] drop;
    public AudioClip[] dump;
    public AudioClip[] container;
    public AudioClip[] heatpanSizzle;
    public AudioClip[] success;
    public AudioClip[] fail;
    public AudioClip[] warning;
}

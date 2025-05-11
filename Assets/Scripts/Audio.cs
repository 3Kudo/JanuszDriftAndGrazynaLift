using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField]
    AudioClip engine;
    [SerializeField]
    AudioClip startup;
    [SerializeField]
    AudioClip crash;
    [SerializeField]
    AudioClip treeCrash;
    [SerializeField]
    AudioClip obstacleCrash;
    [SerializeField]
    AudioClip[] wifeVoice;
    [SerializeField]
    bool breakPlayer;
    [SerializeField]
    float startUpTime;
    AudioSource AS;
    

    private bool isPlayingBreak=false;
    // Start is called before the first frame update
    void Start()
    {
        AS = GetComponentInChildren<AudioSource>();

    }

    public void mooseCrash()
    {
        if (breakPlayer) return;
        AudioSource.PlayClipAtPoint(crash, transform.position, 1f);
    }
    public void startEngine()
    {
        if (breakPlayer) return;
        AudioSource.PlayClipAtPoint(startup, transform.position, 1f);
        AS.clip = engine;
        AS.loop = true;
        AS.Play();

    }

    public void wifeVoicePlay()
    {
        if (breakPlayer) return;
        int voice = Random.Range(0, wifeVoice.Length);
        AudioSource.PlayClipAtPoint(wifeVoice[voice], transform.position, 1f);

    }

    public void playTreeCrash()
    {
        if (breakPlayer) return;
        AudioSource.PlayClipAtPoint(treeCrash, transform.position, 1f);
    }

    public void playObstacleCrash()
    {
        if (breakPlayer) return;
        AudioSource.PlayClipAtPoint(obstacleCrash,transform.position, 1f);
    }
    /*
    public void playBreak(bool play)
    {
        if (!breakPlayer) return;
        if (play)
        {
            if (!isPlayingBreak)
            {
                AS.clip = breakSlam;
                AS.loop = true;
                AS.Play();
                isPlayingBreak = true;
            }
        }
        else
        {
            AS.Stop();
        }


    }*/



    // Update is called once per frame                 GameObject.Find("Car").GetComponentInChildren<Audio>().wifeVoicePlay();        
    void Update()
    {
        if (breakPlayer) return;
        AS.pitch =.5f+ GetComponentInParent<CarScript>().speed / 30;
        if (startUpTime <= 0)
        {
            AS.volume = .3f;
        }
        else startUpTime -= Time.deltaTime;
    }
}

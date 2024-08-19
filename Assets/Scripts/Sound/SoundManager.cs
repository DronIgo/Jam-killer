using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    // TimedObjectDestroyer;             // float timewhendestroy


    public GameObject fishCaught;
    public GameObject baitDeploy;
    public GameObject fishGetsAway;

    public GameObject fishTackle;
    private GameObject fishTackleObject;


    public GameObject punch;
    public GameObject bottleCrash;


    public AudioClip backgroundAudio;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        PlayMusic(backgroundAudio);
    }

    public void OnFishCaught()
    {
        Instantiate(fishCaught);
    }

    public void OnBaltDeploy()
    {
        Instantiate(baitDeploy);
    }

    public void OnFishGetAway()
    {
        Instantiate(fishGetsAway);
    }

    public void OnBottleCrash()
    {
        Instantiate(bottleCrash);
    }


    public void OnFishTackleStart()
    {
        if (fishTackleObject != null)
        {
            Debug.LogError("Fish Tackle already started!");
            return;
        }
        fishTackleObject = Instantiate(fishTackle);
    }

    public void OnFishTackleEnd()
    {
        Destroy(fishTackleObject);
        fishTackleObject = null;
    }

    public void PlayMusic(AudioClip musicClip)

    {
        if (audioSource.clip == musicClip) return;

        audioSource.clip = musicClip;
        audioSource.Play();
        Debug.Log("Begin playing music...");
    }

    public void StopMusic()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }


    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}

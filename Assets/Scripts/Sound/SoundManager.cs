using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    // TimedObjectDestroyer;             // float timewhendestroy

    [Header("Settings")]
    public float fadeDuration = 5.0f; // Время на затухание и нарастание
    public GameObject fishCaught;
    public GameObject baitDeploy;
    public GameObject fishGetsAway;

    public GameObject fishTackle;
    private GameObject fishTackleObject;


    public GameObject punch;
    public GameObject bottleCrash;


    public AudioSource sailAudio;

    public AudioSource fishingAudio;

    public AudioSource insideAudio;
    public AudioSource pressureAudio;

    // public AudioSource currentAudioSource; TODO: ?

    void Awake()
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
        
        sailAudio = transform.Find("ChillFishing").gameObject.GetComponent<AudioSource>();
        fishingAudio = transform.Find("FishGyatt").gameObject.GetComponent<AudioSource>();
        insideAudio = transform.Find("InsideTheFish").gameObject.GetComponent<AudioSource>();
        pressureAudio = transform.Find("HydrostaticPressure").gameObject.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.isInOcean)
            StartSailing();
    }

    public void StartSailing()
    {
        Debug.Log("start sailing");
        sailAudio.Play();
    }

    public void StartFishing()
    {
        StopWithFade(sailAudio, fishingAudio);
    }

    public void StopFishing()
    {
        StopWithFade(fishingAudio, sailAudio);
    }

    public void StopWithFade(AudioSource audioSourceOld, AudioSource audioSourceNew)
    {
        StartCoroutine(FadeCurrentMusic(audioSourceOld, audioSourceNew));

    }
    private IEnumerator FadeCurrentMusic(AudioSource audioSourceOld, AudioSource audioSourceNew)
    {
        audioSourceNew.volume = 0;
        audioSourceNew.Play();

        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            audioSourceOld.volume = Mathf.Lerp(1, 0, time / fadeDuration);
            audioSourceNew.volume = Mathf.Lerp(0, 1, time / fadeDuration);
            yield return null;
        }
        
        audioSourceOld.Stop();
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

}

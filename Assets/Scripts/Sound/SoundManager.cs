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
    public float pressureFadeDuration = 10.0f; // Время на затухание и нарастание
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
    public AudioSource currentAudio;

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
        else
            StartInsideFishSailing();
    }

    public void StartSailing()
    {
        currentAudio = sailAudio;
        Debug.Log("start sailing");
        currentAudio.Play();
    }

    public void StartInsideFishSailing()
    {
        currentAudio = insideAudio;
        Debug.Log("start sailing inside fish");
        currentAudio.Play();
    }

    public void StartPressure()
    {
        StopWithFade(currentAudio, pressureAudio, pressureFadeDuration);
    }

    public void StartFishing()
    {
        StopWithFade(currentAudio, fishingAudio, fadeDuration);
    }

    public void StopFishing()
    {
        StopWithFade(currentAudio, sailAudio, fadeDuration);
    }

    public void StopWithFade(AudioSource audioSourceOld, AudioSource audioSourceNew, float duration)
    {
        currentAudio = audioSourceNew;
        StartCoroutine(FadeCurrentMusic(audioSourceOld, audioSourceNew, duration));

    }
    private IEnumerator FadeCurrentMusic(AudioSource audioSourceOld, AudioSource audioSourceNew,  float duration)
    {
        audioSourceNew.volume = 0;
        audioSourceNew.Play();

        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            audioSourceOld.volume = Mathf.Lerp(1, 0, time / duration);
            audioSourceNew.volume = Mathf.Lerp(0, 1, time / duration);
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
        Debug.Log("Fish tackle disabled");
        // if (fishTackleObject != null)
        // {
        //     Debug.LogError("Fish Tackle already started!");
        //     return;
        // }
        // fishTackleObject = Instantiate(fishTackle);
    }

    public void OnFishTackleEnd()
    {
        Debug.Log("Fish tackle disabled");
        // Destroy(fishTackleObject);
        // fishTackleObject = null;
    }

}

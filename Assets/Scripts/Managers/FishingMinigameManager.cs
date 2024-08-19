using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMinigameManager : MonoBehaviour
{
    public GameObject minigameObject;
    public FishingMinigame minigame;
    public FishManager fishManager;
    void Start()
    {
        minigameObject.SetActive(false);
        minigame = minigameObject.GetComponent<FishingMinigame>();
    }

    public void InitMinigame(Fish fish)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.StartFishing();
            SoundManager.instance.OnFishTackleStart();
        }
        else
        {
            SoundManager.instance.StartSailing();
            Debug.LogWarning("SoundManager doesn't exists!");
        }

        minigame.SetParamsFromFish(fish);

        StartCoroutine("PlayMinigame");
    }

    private IEnumerator PlayMinigame()
    {
        minigameObject.SetActive(true);
        minigame.InitiateGame();
        while (!minigame.win && !minigame.lose)
        {
            if (!FishingRod.rodActive) {
                minigameObject.SetActive(false);
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        if (SoundManager.instance != null)
        {
            SoundManager.instance.StopFishing();

            if (minigame.win)
            {
                SoundManager.instance.OnFishCaught();
            }
            else
            {
                SoundManager.instance.OnFishGetAway();
            }
        }
        else
        {
            Debug.LogWarning("SoundManager doesn't exists!");
        }

        if (minigame.win)
        {
            GameManager.instance.AddFish(fishManager.currentFishInMinigameAI.fishComponent.type);
            if (fishManager.currentFishInMinigameGO != null)
                fishManager.DeleteFish(fishManager.currentFishInMinigameGO);
        }
        if (minigame.lose)
        {
            fishManager.currentFishInMinigameAI.ForceSetState(FishAI.FishState.FUCKING_DONE);
            fishManager.currentFishInMinigameAI.SetGoal(new FishGoalRandomPoint(fishManager.currentFishInMinigameAI));
        }
        minigameObject.SetActive(false);
        fishManager.minigameActive = false;
        fishManager.ResetFishBehaviour();
      

    }
}

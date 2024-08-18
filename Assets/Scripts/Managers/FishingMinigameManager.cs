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
        if (minigame.win)
        {
            if (fishManager.currentFishInMinigameGO != null)
                fishManager.DeleteFish(fishManager.currentFishInMinigameGO, true);
            //GameManager.instance.AddFish();
            Debug.Log("Слушай, а ловко ты это придумал. Молодец!");
        }
        if (minigame.lose)
        {
            fishManager.currentFishInMinigameAI.ForceSetState(FishAI.FishState.FUCKING_DONE);
            fishManager.currentFishInMinigameAI.SetGoal(new FishGoalRandomPoint(fishManager.currentFishInMinigameAI));
            Debug.Log("Результаты теста - вы реальный лошпед");
        }
        minigameObject.SetActive(false);
        fishManager.minigameActive = false;
        fishManager.ResetFishBehaviour();
    }
}

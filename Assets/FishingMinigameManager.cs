using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMinigameManager : MonoBehaviour
{
    public GameObject minigameObject;
    public FishingMinigame minigame;
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
        while (!minigame.win || !minigame.lose)
        {
            yield return new WaitForEndOfFrame();
        }
        if (minigame.win)
        {
            Debug.Log("Слушай, а ловко ты это придумал. Молодец!");
        }
        if (minigame.lose)
        {
            Debug.Log("Результаты теста - вы реальный лошпед");
        }
    }
}

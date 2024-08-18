using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public List<FishType> fishTypes;
    public List<GameObject> fishAlive = new();
    public List<FishAI> fishAliveAI = new();

    private int totalProbability = 0;

    public int maxNumberOfChecksInFrame = 5;
    private int currentCheckIndex = 0;

    public int maxTotalNumOfFish = 30;
    public int desiredNumOfFish = 8;

    public Transform player;

    public float distDispawn = 40.0f;
    public float minSpawnDist = 4f;
    public float maxSpawnDist = 10f;

    public FishingMinigameManager minigameManager;

    // Start is called before the first frame update
    void Start()
    {
        totalProbability = 0;
        foreach (FishType type in fishTypes)
            totalProbability += type.probability;
        //init player
        player = GameManager.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckFishDespawn();
        if (fishAlive.Count < desiredNumOfFish)
        {
            SummonFish(desiredNumOfFish - fishAlive.Count);
        }
    }

    #region Fish population managment
    void SpawnRandomFish()
    {
        int prob = Random.Range(0, totalProbability);
        foreach (var type in fishTypes)
        {
            prob -= type.probability;
            if (prob < 0)
            {
                SpawnFish(type);
                return;
            }
        }
    }

    void SummonFish(int amount)
    {
        Debug.Log("SummonFish: " + amount);
        for (int i = 0; i < amount; ++i)
        {
            SpawnRandomFish();
        }
    }


    void DeleteFish(GameObject fish)
    {
        int index = fishAlive.FindIndex((GameObject a) => { return a == fish; });
        if (index != -1)
            DeleteFish(index);
    }

    void DeleteFish(int index)
    {
        if (index < currentCheckIndex)
            currentCheckIndex -= 1;
        fishAlive[index].GetComponent<Fish>().Destroy();
        fishAlive.RemoveAt(index);
        fishAliveAI.RemoveAt(index);
    }
    void CheckFishDespawn()
    {
        for (int i = 0; i < maxNumberOfChecksInFrame; ++i)
        {
            if (fishAlive.Count == 0)
                return;
            if (currentCheckIndex >= fishAlive.Count)
                currentCheckIndex = 0;
   
            if (CheckFishDespawnByIndex(currentCheckIndex))
                DeleteFish(currentCheckIndex);
            else
                currentCheckIndex++;
        }
    }

    bool CheckFishDespawnByIndex(int index)
    {
        return Vector3.Distance(fishAlive[index].transform.position, player.position) > distDispawn;
    }

    void SpawnFish(FishType type)
    {
        Vector3 position = Utils.GetRandomPointInArea(player.position, minSpawnDist, maxSpawnDist);
        position.z = 1;
        SpawnFish(position, type);
    }

    void SpawnFish(Vector3 position, FishType type)
    {
        if (fishAlive.Count >= maxTotalNumOfFish)
            return;
        GameObject fish = Instantiate(type.fishPrefab, position, Quaternion.identity);
        //set FishAI
        FishAI fishAI;
        {
            try
            {
                fishAI = fish.GetComponentInChildren<FishAI>();
            }
            catch
            {
                Debug.LogWarning("FishPrefab in fishType: " + type.name + " doens't have FishAI component. Dumb ass fish");
                return;
            }
            fishAI.playerShip = player.gameObject;
            fishAI.behaviourType = type.behaviourType;
            fishAI.ResetBehaviour();
        }
        //set Size
        switch (type.size)
        {
            case FishType.FISH_SIZE.BIG:
                fish.transform.localScale = new Vector3(8.0f, 8.0f, 1.0f);
                break;
            case FishType.FISH_SIZE.MEDIUM:
                fish.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                break;
            case FishType.FISH_SIZE.SMALL:
                fish.transform.localScale = new Vector3(0.15f, 0.15f, 1.0f);
                break;
            default:
                fish.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                break;
        }
        //Configurate Fish component
        {
            Fish fishComponent = fish.GetComponent<Fish>();
            fishComponent.fishManager = this;
            fishComponent.type = type;
        }
        fish.transform.parent = this.transform;
        fishAlive.Add(fish);
        fishAliveAI.Add(fishAI);
    }

    #endregion

    public bool minigameActive = true;
    public void InitiateMinigame(Fish fish) 
    {
        foreach (FishAI fishAI in fishAliveAI)
        {
            fishAI.ForceSetState(FishAI.FishState.NEUTRAL);
            fishAI.SetGoal(new FishGoalRandomPoint(fishAI));
        }
        minigameManager.InitMinigame(fish);
    }

    public void InitateLoadLevel(Fish fish)
    {

    }
}

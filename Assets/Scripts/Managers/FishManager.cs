using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    //public class 
    public List<FishType> fishTypes;

    public List<System.Tuple<GameObject, FishAI>> fishAlive_go_ai = new();

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
        if (fishAlive_go_ai.Count < desiredNumOfFish)
        {
            SummonFish(desiredNumOfFish - fishAlive_go_ai.Count);
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

    int FindFishIndex(GameObject fish)
    {
        return fishAlive_go_ai.FindIndex((System.Tuple<GameObject, FishAI> a) => { return a.Item1 == fish; });
    }
    public void DeleteFish(GameObject fish, bool instantDestroy = false)
    {
        int index = FindFishIndex(fish);
        if (index != -1)
            DeleteFish(index);
    }

    void DeleteFish(int index, bool instantDestroy = false)
    {
        if (index < currentCheckIndex)
            currentCheckIndex -= 1;
        if (instantDestroy)
            Destroy(fishAlive_go_ai[index].Item1);
        else
            fishAlive_go_ai[index].Item1.GetComponent<Fish>().Destroy();
        fishAlive_go_ai.RemoveAt(index);
    }
    void CheckFishDespawn()
    {
        for (int i = 0; i < maxNumberOfChecksInFrame; ++i)
        {
            if (fishAlive_go_ai.Count == 0)
                return;
            if (currentCheckIndex >= fishAlive_go_ai.Count)
                currentCheckIndex = 0;
   
            if (CheckFishDespawnByIndex(currentCheckIndex))
                DeleteFish(currentCheckIndex);
            else
                currentCheckIndex++;
        }
    }

    bool CheckFishDespawnByIndex(int index)
    {
        return Vector3.Distance(fishAlive_go_ai[index].Item1.transform.position, player.position) > distDispawn;
    }

    void SpawnFish(FishType type)
    {
        Vector3 position = Utils.GetRandomPointInArea(player.position, minSpawnDist, maxSpawnDist);
        position.z = 1;
        SpawnFish(position, type);
    }

    void SpawnFish(Vector3 position, FishType type)
    {
        if (fishAlive_go_ai.Count >= maxTotalNumOfFish)
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
        fishAlive_go_ai.Add(new System.Tuple<GameObject, FishAI>(fish, fishAI));
    }

    #endregion

    public bool minigameActive = true;
    public GameObject currentFishInMinigameGO;
    public FishAI currentFishInMinigameAI;
    public void InitiateMinigame(Fish fish) 
    {
        if (minigameActive)
            return;
        
        int index = FindFishIndex(fish.gameObject);
        currentFishInMinigameGO = fishAlive_go_ai[index].Item1;
        currentFishInMinigameAI = fishAlive_go_ai[index].Item2;
        foreach (var f in fishAlive_go_ai)
        {
            FishAI fishAI = f.Item2;

            if (fishAI == currentFishInMinigameAI)
            {
                fishAI.SetGoal(new FishDoNothingGoal(currentFishInMinigameAI));
            }
            else
            {
                fishAI.ForceSetState(FishAI.FishState.NEUTRAL);
                fishAI.SetGoal(new FishGoalRandomPoint(fishAI));
            }
        }
        minigameManager.InitMinigame(fish);
        minigameActive = true;
    }

    public void ResetFishBehaviour()
    {
        foreach (var f in fishAlive_go_ai)
        {
            if (f.Item2 == currentFishInMinigameAI)
            {
                continue;
            }
            FishAI fishAI = f.Item2;
            fishAI.ResetBehaviour();
        }
    }

    public void InitateLoadLevel(Fish fish)
    {

    }
}

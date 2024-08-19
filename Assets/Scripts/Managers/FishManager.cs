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
        //Debug.Log("SummonFish: " + amount);
        for (int i = 0; i < amount; ++i)
        {
            SpawnRandomFish();
        }
    }

    int FindFishIndex(GameObject fish)
    {
        return fishAlive_go_ai.FindIndex((System.Tuple<GameObject, FishAI> a) => { return a.Item1 == fish; });
    }
    public void DeleteFish(GameObject fish)
    {
        int index = FindFishIndex(fish);
        if (index != -1)
            DeleteFish(index);
    }

    void DeleteFish(int index)
    {
        if (index < currentCheckIndex)
            currentCheckIndex -= 1;
        Destroy(fishAlive_go_ai[index].Item1);
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
        return fishAlive_go_ai[index].Item2.distanceToPlayer > distDispawn;
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
        //Configurate Fish component
        {
            Fish fishComponent = fish.GetComponent<Fish>();
            fishComponent.fishManager = this;
            fishComponent.type = type;
        }
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
            fishAI.playerShip = GameManager.instance.player;
            fishAI.playerCenter = GameManager.instance.playerCenter;
            fishAI.behaviourType = type.behaviourType;
            fishAI.attackEffect = type.attackPrefab;
            fishAI.ResetBehaviour();
        }
        //set FishMover speed 
        {
            fish.GetComponentInChildren<FishMover>().maxSpeed = type.speed;
        }
        fish.transform.parent = this.transform;
        fishAlive_go_ai.Add(new System.Tuple<GameObject, FishAI>(fish, fishAI));
    }

    #endregion

    public static bool minigameActive = false;
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

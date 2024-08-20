using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    
    //public class 
    public List<FishType> regularFishTypes;

    public List<FishType> mediumFish;
    public List<FishType> smallFish;
    public List<FishType> bigFish;

    int currentNumSmall = 0;
    int currentNumMedium = 0;
    int currentNumBig = 0;

    public List<System.Tuple<GameObject, FishAI>> fishAlive_go_ai = new();

    private int totalProbability = 0;
    private int totalProbabilityBig = 0;
    private int totalProbabilitySmall = 0;
    private int totalProbabilityMedium = 0;

    public int maxNumberOfChecksInFrame = 5;
    private int currentCheckIndex = 0;

    public int maxTotalNumOfFish = 30;
    public int desiredNumOfFish = 8;

    public int maxNumOfMediumFish = 3;
    public int maxNumOfBigFish = 2;
    public int minNumOfMediumFish = 1;
    public int minNumOfBigFish = 1;

    public Transform player;

    public float distDispawn = 40.0f;
    public float minSpawnDist = 4f;
    public float maxSpawnDist = 10f;

    public bool insideFish;
    public float minY = -14.5f;
    public float maxY = 14.5f;

    public FishingMinigameManager minigameManager;

    public static FishManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void UpdateTotalProbability()
    {
        totalProbability = 0;
        totalProbabilityBig = 0;
        totalProbabilityMedium = 0;
        totalProbabilitySmall = 0;
        foreach (FishType type in regularFishTypes)
            totalProbability += type.probability;

        foreach (FishType type in smallFish)
            totalProbabilitySmall += type.probability;
        foreach (FishType type in bigFish)
            totalProbabilityBig += type.probability;
        foreach (FishType type in mediumFish)
            totalProbabilityMedium += type.probability;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateTotalProbability();
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

    public void UpdateFishLists(List<FishType> newFishTypes)
    {
        regularFishTypes.Clear();
        regularFishTypes = new List<FishType>(newFishTypes);
        smallFish.Clear();
        mediumFish.Clear();
        bigFish.Clear();
        foreach (var fish in regularFishTypes)
        {
            if (fish.size == FishType.FISH_SIZE.BIG)
                bigFish.Add(fish);
            if (fish.size == FishType.FISH_SIZE.MEDIUM)
                mediumFish.Add(fish);
            if (fish.size == FishType.FISH_SIZE.SMALL)
                smallFish.Add(fish);
        }
        UpdateTotalProbability();
    }

    #region Fish population managment
    //void SpawnRandomFish()
    //{
    //    int prob = Random.Range(0, totalProbability);
    //    foreach (var type in regularFishTypes)
    //    {
    //        prob -= type.probability;
    //        if (prob < 0)
    //        {
    //            SpawnFish(type);
    //            return;
    //        }
    //    }
    //}

    void SpawnRandomFish(FishType.FISH_SIZE size)
    {
        int totalProb = 0;
        List<FishType> fishPool = new List<FishType>();
        switch (size)
        {
            case FishType.FISH_SIZE.BIG:
                totalProb = totalProbabilityBig;
                fishPool = bigFish;
                break;
            case FishType.FISH_SIZE.MEDIUM:
                totalProb = totalProbabilityMedium;
                fishPool = mediumFish;
                break;
            case FishType.FISH_SIZE.SMALL:
                totalProb = totalProbabilitySmall;
                fishPool = smallFish;
                break;
        }

        int prob = Random.Range(0, totalProb);
        foreach (var type in fishPool)
        {
            prob -= type.probability;
            if (prob < 0)
            {
                SpawnFish(type);
                return;
            }
        }
    }

    void SummonRandomFish(int amount, FishType.FISH_SIZE size)
    {
        switch(size)
        {
            case FishType.FISH_SIZE.BIG:
                if (bigFish.Count == 0)
                    return;
                break;
            case FishType.FISH_SIZE.MEDIUM:
                if (mediumFish.Count == 0)
                    return;
                break;
            case FishType.FISH_SIZE.SMALL:
                if (smallFish.Count == 0)
                    return;
                break;
        }


        for (int i = 0; i < amount; ++i)
        {
            SpawnRandomFish(size);
        }
    }

    void SummonFish(int amount)
    {
        if (regularFishTypes.Count == 0)
            return;

        int minMed = Mathf.Max(minNumOfMediumFish - currentNumMedium, 0);
        int maxMed = Mathf.Max(maxNumOfMediumFish - currentNumMedium, 0);
        int minBig = Mathf.Max(minNumOfBigFish - currentNumBig);
        int maxBig = Mathf.Max(maxNumOfBigFish - currentNumBig);

        int med = Random.Range(minMed, maxMed + 1);
        int big = Random.Range(minBig, maxBig + 1);
        int small = Mathf.Max(desiredNumOfFish - currentNumBig - currentNumMedium - currentNumSmall - med - big, 0);

        SummonRandomFish(small, FishType.FISH_SIZE.SMALL);
        SummonRandomFish(med, FishType.FISH_SIZE.MEDIUM);
        SummonRandomFish(big, FishType.FISH_SIZE.BIG);
        //for (int i = 0; i < amount; ++i)
        //{
        //    SpawnRandomFish();
        //}
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
        switch (fishAlive_go_ai[index].Item2.fishComponent.type.size)
        {
            case FishType.FISH_SIZE.BIG:
                currentNumBig--;
                break;
            case FishType.FISH_SIZE.MEDIUM:
                currentNumMedium--;
                break;
            case FishType.FISH_SIZE.SMALL:
                currentNumSmall--;
                break;
        }
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
        switch (type.size)
        {
            case FishType.FISH_SIZE.BIG:
                currentNumBig++;
                break;
            case FishType.FISH_SIZE.MEDIUM:
                currentNumMedium++;
                break;
            case FishType.FISH_SIZE.SMALL:
                currentNumSmall++;
                break;
        }

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
            if (!minigameActive)
                fishAI.ResetBehaviour();
            else
                fishAI.currentState = FishAI.FishState.NEUTRAL;
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

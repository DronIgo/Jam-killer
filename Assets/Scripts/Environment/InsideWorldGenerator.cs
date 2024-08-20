using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InsideWorldGenerator : MonoBehaviour
{
    public float chunkWidth = 15f;
    public float chunkHeight = 15f;
    public Vector2Int currentChunkPos;

    public int wallTopEdge = 2;
    public int wallBottomEdge = -2;

    private int topEdge;
    private int bottomEdge;

    public ChunkType topWallChunk;
    public ChunkType bottomWallChunk;

    public Dictionary<Vector2Int, ChunkType> generatedChunks;
    public Dictionary<Vector2Int, GameObject> existingGameObjects;

    public ChunkType emptyChunkType;

    public List<ChunkType> chunkTypes;

    public int generationRadius = 2;
    public int garbageRadius = 3;

    // external components
    public Transform player;

    private void UpdateCurrentChunkPos()
    {
        float x = player.position.x;
        float y = player.position.y;

        currentChunkPos.x = Mathf.FloorToInt(x / chunkWidth);
        currentChunkPos.y = Mathf.FloorToInt(y / chunkHeight);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameManager.instance.player.transform;
        }


        currentChunkPos = new();
        generatedChunks = new();
        existingGameObjects = new();

        topEdge = Mathf.FloorToInt(player.position.y / chunkHeight) + wallTopEdge;
        bottomEdge = Mathf.FloorToInt(player.position.y / chunkHeight) + wallBottomEdge;

        // generate start chunk
        //Debug.Log("Creating first chunk...");
        UpdateCurrentChunkPos();


        GameObject chunkGameObject = PlaceChunk(currentChunkPos,emptyChunkType.prefab);
        generatedChunks.Add(currentChunkPos, emptyChunkType);
        existingGameObjects.Add(currentChunkPos, chunkGameObject);

        //Debug.Log("Creating near chunks...");
        GenerateChunkArea(currentChunkPos, generationRadius);
    }

    private ChunkType GetRandomChunkType()
    {
        float total = 0;

        foreach(var chunkType in chunkTypes)
            total += chunkType.probability;

        float randomPoint = Random.value * total;

        for (int i = 0; i < chunkTypes.Count; i++)
        {
            if (randomPoint < chunkTypes[i].probability)
                return chunkTypes[i];

            randomPoint -= chunkTypes[i].probability;
        }

        return chunkTypes[^1];

    }

    private GameObject PlaceChunk(Vector2Int chunk, GameObject chunkPrefab)
    {
        int x = chunk.x;
        int y = chunk.y;

        Vector3 chunkPosition = new(x * chunkWidth, y * chunkHeight, 0);

        GameObject gameObject = Instantiate(chunkPrefab, chunkPosition, Quaternion.identity);

        // to hide gameObject and avoid clogging up the stage
        gameObject.transform.parent = this.transform;

        //Debug.Log("Place chunk on " + "("+ chunkPosition.x + ", " + chunkPosition.y + ")");

        return gameObject;
    }

    private void GenerateNewChunk(Vector2Int chunkPos, bool random, ChunkType chunkType)
    {
        if (random)
            chunkType = GetRandomChunkType();

        GameObject chunkGameObject = PlaceChunk(chunkPos, chunkType.prefab);
        generatedChunks.Add(chunkPos, chunkType);
        existingGameObjects.Add(chunkPos, chunkGameObject);
    }

    private void CreateIfNotExist(Vector2Int chunk, bool random, ChunkType chunkType)
    {
        if (generatedChunks.ContainsKey(chunk))
        {
            // if it already placed continue...
            if (!existingGameObjects.ContainsKey(chunk))
            {
                // otherwise place it with correct chunkPrefab
                GameObject chunkObject = PlaceChunk(chunk, generatedChunks[chunk].prefab);
                existingGameObjects.Add(chunk, chunkObject);
            }
            return;
        }

        GenerateNewChunk(chunk, random, chunkType);
    }

    private void GenerateChunkArea(Vector2Int startChunk, int radius)
    {
        int startX = startChunk.x;
        int startY = startChunk.y;

        // get chunk in "radius"
        List<Vector2Int> chunks = new();
        List<Vector2Int> topWalls = new();
        List<Vector2Int> bottomWalls = new();

        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                // skip the startChunk
                if (i == startX && j == startY)
                    continue;

                if (startY + j >= topEdge)
                {
                    Vector2Int wall = new(startX + i, topEdge);
                    topWalls.Add(wall);
                    continue;
                }

                if (startY + j <= bottomEdge)
                {
                    Vector2Int wall = new(startX + i, bottomEdge);
                    bottomWalls.Add(wall);
                    continue;
                }

                Vector2Int nearChunk = new(startX + i, startY + j);
                chunks.Add(nearChunk);

                // Debug.Log("Near chunk: (" + nearChunk.x +"," + nearChunk.y + ")");
            }
        }

        // create nonexisting chunks
        foreach (var chunk in chunks)
        {
            CreateIfNotExist(chunk, true, null);
        }

        foreach (var wall in topWalls)
        {
            CreateIfNotExist(wall, false, topWallChunk);
        }

        foreach (var wall in bottomWalls)
        {
            CreateIfNotExist(wall, false, bottomWallChunk);
        }

    }

    private void CollectGarbage(Vector2Int startChunk, int radius)
    {
        // Debug.Log("Collecting garbage...");
        int x = startChunk.x;
        int y = startChunk.y;

        for (int i = -radius; i <= radius; i++)
        {
            List<Vector2Int> chunkToDestroy = new()
            {
                new(x + radius, y + i),
                new(x - radius, y + i),
                new(x + i, y + radius),
                new(x + i, y - radius)
            };
            // Debug.Log("Circle chunk1: (" + chunk1.x +"," + chunk1.y + ")");
            // Debug.Log("Circle chunk2: (" + chunk2.x +"," + chunk2.y + ")");

            foreach (var chunk in chunkToDestroy)
            {
                // if object is here continue...
                if (!existingGameObjects.ContainsKey(chunk))
                    continue;

                //Debug.Log("Cleaning chunk: (" + chunk.x +"," + chunk.y + ")");
                Destroy(existingGameObjects[chunk]);
                existingGameObjects.Remove(chunk);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrentChunkPos();

        // Debug.Log("Chunk: (" + currentChunkPos.x +"," + currentChunkPos.y + ")");
        GenerateChunkArea(currentChunkPos, generationRadius);

        CollectGarbage(currentChunkPos, garbageRadius);
    }
}

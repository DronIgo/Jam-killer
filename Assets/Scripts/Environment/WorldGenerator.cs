using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{

    public float chunkWidth = 15f;
    public float chunkHeight = 15f;
    public Vector2Int currentChunkPos;

    public Dictionary<Vector2Int, ChunkType> generatedChunks;
    public Dictionary<Vector2Int, GameObject> existingGameObjects;

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
            Debug.LogError("You should setup player for World Generator!!!!!!!!!!!!!!!!!!!!!!!!!");
            Application.Quit();
        }


        currentChunkPos = new();
        generatedChunks = new();
        existingGameObjects = new();

        // generate start chunk
        Debug.Log("Creating first chunk...");
        UpdateCurrentChunkPos();
        GenerateNewChunk(currentChunkPos);

        Debug.Log("Creating near chunks...");
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

        Debug.Log("Place chunk on " + "("+ chunkPosition.x + ", " + chunkPosition.y + ")");

        return gameObject;
    }

    private void GenerateNewChunk(Vector2Int chunkPos)
    {
        ChunkType chunkType = GetRandomChunkType();
        GameObject chunkGameObject = PlaceChunk(chunkPos, chunkType.chunkPrefab);
        generatedChunks.Add(chunkPos, chunkType);
        existingGameObjects.Add(chunkPos, chunkGameObject);
    }

    private void GenerateChunkArea(Vector2Int startChunk, int radius)
    {
        int startX = startChunk.x;
        int startY = startChunk.y;

        // get chunk in "radius"
        List<Vector2Int> chunks = new();
        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                // skip the startChunk
                if (i == startX && j == startY)
                    continue;

                Vector2Int nearChunk = new(startX + i, startY + j);
                chunks.Add(nearChunk);

                // Debug.Log("Near chunk: (" + nearChunk.x +"," + nearChunk.y + ")");
            }
        }

        // create nonexisting chunks
        foreach (var chunk in chunks)
        {
            if (generatedChunks.ContainsKey(chunk))
            {
                // if it already placed continue...
                if (existingGameObjects.ContainsKey(chunk))
                    continue;

                // otherwise place it with correct chunkPrefab
                GameObject chunkObject = PlaceChunk(chunk, generatedChunks[chunk].chunkPrefab);
                existingGameObjects.Add(chunk, chunkObject);
            }

            GenerateNewChunk(chunk);
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

                Debug.Log("Cleaning chunk: (" + chunk.x +"," + chunk.y + ")");
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

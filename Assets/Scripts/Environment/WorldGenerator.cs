using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{

    public float chunkWidth = 15f;
    public float chunkHeight = 15f;
    public Vector2Int currentChunkPos;

    public Dictionary<Vector2Int, bool> generatedChunks;


    // external components
    public Transform player;
    public List<GameObject> chunkPrefabList;

    private void updateCurrentChunkPos()
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


        // generate start chunk
        Debug.Log("Creating start chunk...");
        updateCurrentChunkPos();
        PlaceChunk(currentChunkPos, chunkPrefabList[1]);

        Debug.Log("Creating near chunks...");
        CheckChunkArea(currentChunkPos, 1);

    }

    private GameObject GetRandomChunkPrefab()
    {
        float total = 0;

        float[] probabilities = { 0.2f, 0.5f, 0.3f }; // TODO: choose externally

        foreach(float prob in probabilities)
            total += prob;

        float randomPoint = Random.value * total;

        for (int i = 0; i < chunkPrefabList.Count; i++)
        {
            if (randomPoint < probabilities[i])
                return chunkPrefabList[i];

            randomPoint -= probabilities[i];
        }

        return chunkPrefabList[^1];

    }

    private void PlaceChunk(Vector2Int chunk, GameObject chunkPrefab)
    {
        int x = chunk.x;
        int y = chunk.y;

        Vector3 chunkPosition = new(x * chunkWidth, y * chunkHeight, 0);

        GameObject go = Instantiate(chunkPrefab, chunkPosition, Quaternion.identity);
        go.transform.parent = this.transform;
        Debug.Log("Place chunk on " + "("+ chunkPosition.x + ", " + chunkPosition.y + ")");
    }

    private void CheckChunkArea(Vector2Int startChunk, int radius)
    {
        int startX = startChunk.x;
        int startY = startChunk.y;
    
        // get chunk in "radius"
        List<Vector2Int> chunks = new List<Vector2Int>();
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
                continue;
            
            
            GameObject chunkPrefab = GetRandomChunkPrefab();
            PlaceChunk(chunk, chunkPrefab);    
            generatedChunks.Add(chunk, true);
        }


    }

    // Update is called once per frame
    void Update()
    {

        updateCurrentChunkPos();


        CheckChunkArea(currentChunkPos, 1);

        // Debug.Log("Chunk: (" + chunkX +"," + chunkY + ")");

    }
}

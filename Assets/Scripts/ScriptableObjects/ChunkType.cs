using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChunkType", menuName = "ScriptableObjects/ChunkType", order = 4)]
public class ChunkType : ScriptableObject
{
    public float probability;

    public GameObject prefab;
}

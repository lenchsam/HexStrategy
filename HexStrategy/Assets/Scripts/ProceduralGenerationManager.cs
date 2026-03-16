using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerationManager : SerializedMonoBehaviour
{
    static ProceduralGenerationManager Instance;

    [Header("MapSettings")]
    [SerializeField] private int _seed = 12345;
    [SerializeField] private int _mapWidth = 100;
    [SerializeField] private int _mapHeight = 100;
    [SerializeField] private float _scale = 1.0f;

    [Header("Prefabs")]
    //serialized dictionary so prefabs can be assigned in the inspector
    //key is terrain type e.g. "Water", "Grass" etc
    public Dictionary<string, GameObject> _terrainPrefabs;

    private HeightMap _heightMap;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _heightMap = new HeightMap(_mapWidth, _mapHeight, _seed);

        _heightMap.Generate(_scale);

        GenerateMap();
    }

    private void GenerateMap()
    {
        GameObject mapParent = new GameObject("Map");

        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                float heightValue = _heightMap.GetHeight(x, y);

                
            }
        }
    }
}

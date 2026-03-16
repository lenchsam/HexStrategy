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

    [Header("Height Thresholds")]
    [SerializeField] private float _waterHeight = 0.3f;
    [SerializeField] private float _grassHeight = 0.5f;
    [SerializeField] private float _grasstransitionHeight = 0.53f;
    [SerializeField] private float _grasslevel2Height = 0.8f;
    [SerializeField] private float _mountainHeight = 1.0f;

    [Header("Hex Settings")]
    [SerializeField] private float _hexOuterRadius = 1f;

    [Header("Prefabs")]
    //this is serialised using odin inspector so data is assigned in editor
    //key is terrain type e.g. "Water", "Grass" etc
    public Dictionary<string, GameObject> _terrainPrefabs;

    private HeightMap _heightMap;
    private HeightMap _temperatureMap;

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
        _temperatureMap = new HeightMap(_mapWidth, _mapHeight, _seed + 1);

        _heightMap.Generate(_scale);
        _temperatureMap.Generate(_scale);

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

                Vector3 hexPosition = HexHelper.GetWorldPosition(x, y, _hexOuterRadius);

                hexPosition.y = GetHeightStep(heightValue);

                Instantiate(GetPrefabForHeight(heightValue), hexPosition, Quaternion.identity, mapParent.transform);
            }
        }
    }

    private float GetHeightStep(float height)
    {
        if (height < _waterHeight) return 0f;               //water   
        if (height < _grassHeight) return 0f;               //grass level 1
        if (height < _grasstransitionHeight) return 0.5f;   //grass transition  
        if (height < _grasslevel2Height) return 1f;         //grass level 2
        return 2f;                                          //mountain            
    }

    private GameObject GetPrefabForHeight(float height)
    {
        if (height < _waterHeight) return _terrainPrefabs["Water"];
        if (height < _grassHeight) return _terrainPrefabs["Grass"];             //level 1 of grass
        if (height < _grasstransitionHeight) return _terrainPrefabs["Grass"];   //transition between level 1 and 2
        if (height < _grasslevel2Height) return _terrainPrefabs["Grass"];       //level 2 of grass
        return _terrainPrefabs["Mountain"];               
    }
}

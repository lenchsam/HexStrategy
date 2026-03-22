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
    [SerializeField] [Range(0, 0.2f)] private readonly float _scale = 0.1f;

    [Header("Height Thresholds")]
    [SerializeField] private readonly float _waterHeight = 0.3f;
    [SerializeField] private readonly float _grassHeight = 0.5f;
    [SerializeField] private readonly float _grasstransitionHeight = 0.53f;
    [SerializeField] private readonly float _grasslevel2Height = 0.8f;

    [Header("Temperature Settings")]
    [SerializeField][Range(0, 0.2f)] private readonly float _temperatureScale = 0.2f;
    [SerializeField, Range(0f, 1f)] private float _coldThreshold = 0.3f;
    [SerializeField, Range(0f, 1f)] private float _hotThreshold = 0.6f;
    [SerializeField] private Dictionary<string, Material> _biomeMaterialPrefabs;


    //1.155 is the scale for the hex prefabs to fit together without gaps
    private const float _hexOuterRadius = 1.155f;

    [Header("Prefabs")]
    //this is serialised using odin inspector so data is assigned in editor
    //key is terrain type e.g. "Water", "Grass" etc
    [SerializeField] private Dictionary<string, GameObject> _terrainPrefabs;

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
        _temperatureMap.Generate(_temperatureScale);

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
                float tempValue = _temperatureMap.GetHeight(x, y);

                Vector3 hexPosition = HexHelper.GetWorldPosition(x, y, _hexOuterRadius);
                hexPosition.y = GetHeightStep(heightValue);

                GameObject basePrefab = GetBasePrefab(heightValue);

                GameObject tile = Instantiate(basePrefab, hexPosition, Quaternion.identity, mapParent.transform);

                Material biomeMaterial = GetBiomeMaterial(heightValue, tempValue);
                MeshRenderer hexRenderer = tile.GetComponentInChildren<MeshRenderer>();

                if (hexRenderer != null)
                    hexRenderer.material = biomeMaterial;
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

    private GameObject GetBasePrefab(float height)
    {
        if (height < _waterHeight) return _terrainPrefabs["Water"];
        if (height >= _grasslevel2Height) return _terrainPrefabs["Mountain"];

        return _terrainPrefabs["Grass"];
    }

    private Material GetBiomeMaterial(float height, float temp)
    {
        if (temp < _coldThreshold)
        {
            return _biomeMaterialPrefabs["Tundra"];
        }
        else if (temp > _hotThreshold)
        {
            return _biomeMaterialPrefabs["Desert"];
        }
        else
        {
            return _biomeMaterialPrefabs["Forest"];
        }
    }
}

using UnityEngine;

public class TemperatureMap
{
    private float[,] _temperatureArray;
    private int _width;
    private int _height;

    private readonly Vector2 _seedOffset;

    public TemperatureMap(int width, int height, int seed)
    {
        _width = width;
        _height = height;

        _temperatureArray = new float[_width, _height];

        System.Random rand = new System.Random(seed);
        _seedOffset = new Vector2(rand.Next(-100000, 100000), rand.Next(-100000, 100000));
    }

    public void Generate()
    {

    }

    public float GetTemperature(int x, int y)
    {
        return _temperatureArray[x, y];
    }
}

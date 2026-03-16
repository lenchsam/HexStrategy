using UnityEngine;

public class HeightMap
{
    private float[,] _heightArray;
    private int _width;
    private int _height;

    private readonly Vector2 _seedOffset;

    public HeightMap(int width, int height, int seed)
    {
        _width = width;
        _height = height;

        _heightArray = new float[_width, _height];

        //gets a random offset for the noise
        //ensures different maps for different seeds
        //system.random means that same seed will always produce the same offset
        System.Random rand = new System.Random(seed);
        _seedOffset = new Vector2(rand.Next(-100000, 100000), rand.Next(-100000, 100000));
    }

    public void Generate(float scale)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                float noiseX = (x + _seedOffset.x) * scale;
                float noiseY = (y + _seedOffset.y) * scale;
                _heightArray[x, y] = Mathf.PerlinNoise(noiseX, noiseY);
            }
        }
    }

    public float GetHeight(int x, int y)
    {
        return _heightArray[x, y];
    }
    public float[,] GetHeightMap()
    {
        return _heightArray;
    }
}

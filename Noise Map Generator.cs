using UnityEngine;

public class NoiseMapGenerator : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 20;
    public Vector2 offset;

    void Start()
    {
        GenerateNoiseMap();
    }

    void GenerateNoiseMap()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, 20, height);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }

        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale + offset.x;
        float yCoord = (float)y / height * scale + offset.y;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}

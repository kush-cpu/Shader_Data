using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
    public GameObject[] customMeshes;
    public int levelWidth = 10;
    public int levelLength = 10;
    public Vector3 rotationRange = new Vector3(0, 360, 0); // Rotation range in degrees
    public Vector3 translationRange = new Vector3(1, 0, 1); // Translation range in world units
    public NoiseMapGenerator noiseMapGenerator;

    private List<GameObject> meshPool = new List<GameObject>();

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        if (noiseMapGenerator == null)
        {
            Debug.LogError("NoiseMapGenerator is not assigned.");
            return;
        }

        for (int x = 0; x < levelWidth; x++)
        {
            for (int z = 0; z < levelLength; z++)
            {
                // Choose a random mesh from the customMeshes array
                GameObject selectedMesh = GetRandomMesh();

                // Use noise map for position adjustments
                Vector3 noiseMapPosition = GetNoiseMapPosition(x, z);
                Vector3 finalPosition = new Vector3(x, 0, z) + noiseMapPosition;

                // Instantiate or reuse a mesh from the pool
                GameObject instantiatedMesh = GetPooledMesh(selectedMesh);

                // Set the position and rotation of the mesh
                instantiatedMesh.transform.position = finalPosition;
                instantiatedMesh.transform.rotation = Quaternion.Euler(GetRandomRotation());
            }
        }
    }

    GameObject GetPooledMesh(GameObject meshPrefab)
    {
        // Check if there is an available mesh in the pool
        foreach (GameObject pooledMesh in meshPool)
        {
            if (!pooledMesh.activeInHierarchy)
            {
                pooledMesh.SetActive(true);
                return pooledMesh;
            }
        }

        // If no available mesh is found, instantiate a new one and add it to the pool
        GameObject newMesh = Instantiate(meshPrefab);
        meshPool.Add(newMesh);

        return newMesh;
    }

    Vector3 GetNoiseMapPosition(int x, int z)
    {
        float noiseValue = noiseMapGenerator.GetNoiseValue(x, z);
        float adjustedX = noiseValue * translationRange.x;
        float adjustedZ = noiseValue * translationRange.z;

        return new Vector3(adjustedX, 0, adjustedZ);
    }

    GameObject GetRandomMesh()
    {
        if (customMeshes.Length == 0)
        {
            Debug.LogError("No custom meshes assigned.");
            return null;
        }

        return customMeshes[Random.Range(0, customMeshes.Length)];
    }

    Vector3 GetRandomRotation()
    {
        float randomX = Random.Range(rotationRange.x, rotationRange.y);
        float randomY = Random.Range(rotationRange.x, rotationRange.y);
        float randomZ = Random.Range(rotationRange.x, rotationRange.y);

        return new Vector3(randomX, randomY, randomZ);
    }
}


using UnityEngine;

public class RoadObjectSpawnManager : MonoBehaviour
{
    //static RoadObjectSpawnManager sharedInstance;
    [SerializeField] GameObject[] roadObjects;

    private readonly float zSpawn = 170.0f;
    private readonly float xSpawnRange = 7;
    private readonly float yObstacleSpawn = 0.2f;
    private float spawnTimeInterval = 1.0f;

    //public static RoadObjectSpawnManager SharedInstance
    //{
    //    get { return sharedInstance; }
    //}

    void Start()
    {
        //if (sharedInstance == null)
        //{
        //    sharedInstance = this;
        //}

        //GameManager.GameAcceleration += RestartSpawning;

        MakeStartObjects();
        StartSpawning();
    }

    void MakeStartObjects()
    {
        int roadObjectZStep = 12;
        for (int startObjectZCoordinate = 36; startObjectZCoordinate < zSpawn; startObjectZCoordinate += roadObjectZStep)
        {
            int randomIndex = Random.Range(0, roadObjects.Length);
            Instantiate(roadObjects[randomIndex], GetRandomObjectPosition(startObjectZCoordinate), roadObjects[randomIndex].transform.rotation);
        }
    }

    void SpawnRandomObject()
    {
        if (GameManager.IsGameActive)
        {
            int randomIndex = Random.Range(0, roadObjects.Length);
            Instantiate(roadObjects[randomIndex], GetRandomObjectPosition(zSpawn), roadObjects[randomIndex].transform.rotation);
        }
    }

    Vector3 GetRandomObjectPosition(float zCoordinate)
    {
        float randomX = Random.Range(xSpawnRange, -xSpawnRange);
        Vector3 spawnPos = new Vector3(randomX, yObstacleSpawn, zCoordinate);
        return spawnPos;
    }

    void StartSpawning()
    {
        InvokeRepeating("SpawnRandomObject", spawnTimeInterval, spawnTimeInterval);
    }

    public void RestartSpawning()
    {
        CancelInvoke();
        spawnTimeInterval /= GameManager.GameAccelerator;
        StartSpawning();
    }
}

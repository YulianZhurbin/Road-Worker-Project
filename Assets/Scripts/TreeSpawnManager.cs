using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TreeSpawnManager : MonoBehaviour
{
    //private static TreeSpawnManager sharedInstance;

    [SerializeField] GameObject[] treeLines;

    private TreePooler treePooler;
    private readonly float zSpawn = 170.0f;
    private float[][] xTreeCoordinates;
    private float treeSpawnInterval = 0.5f;

    //public static TreeSpawnManager SharedInstance
    //{
    //    get { return sharedInstance; }
    //}

    void Start()
    {
        //if (sharedInstance == null)
        //{
        //    sharedInstance = this;
        //}

        treePooler = TreePooler.SharedInstance;

        //GameManager.GameAcceleration += RestartSpawning;
        CreateTreeSpawnXCoordinates();
        MakeStartTrees();
        StartSpawning();
    }

    private void MakeStartTrees()
    {
        int treeRowStep = 6;
        GameObject tree;

        for (int treeRowZCoordinate = 0; treeRowZCoordinate < zSpawn; treeRowZCoordinate += treeRowStep)
        {
            for (int treeLineNumber = 0; treeLineNumber < xTreeCoordinates.Length; treeLineNumber++)
            {
                tree = treePooler.GetPooledTree();

                if (tree == null)
                {
                    continue;
                }

                int randomXIndex = Random.Range(0, xTreeCoordinates[treeLineNumber].Length);
                tree.transform.position = new Vector3(xTreeCoordinates[treeLineNumber][randomXIndex], 0, treeRowZCoordinate);
                tree.SetActive(true);
            }
        }
    }

    void SpawnTree()
    {
        for (int treeLineNumber = 0; treeLineNumber < treeLines.Length; treeLineNumber++)
        {
            GameObject tree = treePooler.GetPooledTree();
            if (tree == null)
            {
                continue;
            }
            tree.transform.position = GetRandomTreePosition(treeLineNumber);
            tree.SetActive(true);
        }

    }

    Vector3 GetRandomTreePosition(int treeLineNumber)
    {
        int randomXIndex = Random.Range(0, xTreeCoordinates[treeLineNumber].Length);
        Vector3 spawnPos = new Vector3(xTreeCoordinates[treeLineNumber][randomXIndex], 0, zSpawn);
        return spawnPos;
    }

    void CreateTreeSpawnXCoordinates()
    {
        xTreeCoordinates = new float[treeLines.Length][];
        const int ROWS_IN_TREE_LINE_AMOUNT = 5;

        for (int i = 0; i < treeLines.Length; i++)
        {
            xTreeCoordinates[i] = new float[ROWS_IN_TREE_LINE_AMOUNT];
            float treeLineXCoordinate = treeLines[i].transform.position.x;

            for (int j = 0; j < ROWS_IN_TREE_LINE_AMOUNT; j++)
            {
                xTreeCoordinates[i][j] = j + treeLineXCoordinate;
            }
        }
    }

    void StartSpawning() => InvokeRepeating("SpawnTree", treeSpawnInterval, treeSpawnInterval);

    public void RestartSpawning()
    {
        CancelInvoke();
        treeSpawnInterval /= GameManager.GameAccelerator;
        StartSpawning();
    }
}

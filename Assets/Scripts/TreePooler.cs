using System.Collections.Generic;
using UnityEngine;

public class TreePooler : MonoBehaviour
{
    private static TreePooler sharedInstance;
    private List<GameObject> pooledTrees;

    [SerializeField] List<GameObject> treePrefabs;
    [SerializeField] int amountToPool;

    public static TreePooler SharedInstance
    {
        get { return sharedInstance; }
    }

    void Awake()
    {
        sharedInstance = this;


        pooledTrees = new List<GameObject>();
        int treesToPoolIndex = 0;

        for (int i = 0; i < amountToPool; i++)
        {
            GameObject tree = Instantiate(treePrefabs[treesToPoolIndex]);
            tree.SetActive(false);
            pooledTrees.Add(tree);
            tree.transform.SetParent(transform);
            treesToPoolIndex = treesToPoolIndex != treePrefabs.Count - 1 ? treesToPoolIndex + 1 : 0;
        }
    }

    void Start()
    {
    }

    public GameObject GetPooledTree()
    {
        for (int i = 0; i < pooledTrees.Count; i++)
        {
            if (!pooledTrees[i].activeInHierarchy)
            {
                return pooledTrees[i];
            }
        }
        return null;
    }

}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public static Storage instance;
    public string storedName = "Player1";

    RecordContainer recordContainer;

    public RecordContainer RecordContainer
    {
        get { return recordContainer; }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Set a new record if the current one is broken
    /// </summary>
    public void CheckRecord()
    {
        LoadRecord();

        if (recordContainer == null || GameManager.Points > recordContainer.score)
        {
            WriteNewRecord();
        }
    }

    public void LoadRecord()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            recordContainer = JsonUtility.FromJson<RecordContainer>(json);
        }
    }

    void WriteNewRecord()
    {
        RecordContainer newRecordContainer = new RecordContainer();
        newRecordContainer.name = storedName;
        newRecordContainer.score = GameManager.Points;

        string json = JsonUtility.ToJson(newRecordContainer);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
}

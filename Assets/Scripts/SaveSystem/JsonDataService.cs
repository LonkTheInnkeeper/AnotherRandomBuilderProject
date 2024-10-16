using System;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class JsonDataService : IDataService
{
    public bool SaveData<T>(string relativePath, T data)
    {
        string path = Application.dataPath + "/" + relativePath;

        try
        {
            if (File.Exists(path))
            {
                Debug.Log("Data exists. Deleting old file and creating a new one.");
                File.Delete(path);
            }
            else
            {
                Debug.Log("Creating a new file");
            }

            FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(data));
            return true;
        }

        catch (Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    public T LoadData<T>(string relativePath)
    {
        string path = Application.dataPath + "/" + relativePath;

        if (!File.Exists(path))
        {
            Debug.Log("Cannot load data. The file doesn't exist");
            throw new Exception($"{path} file doesn't exist");
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
            throw e;
        }
    }

    public bool CheckData(string relativePath)
    {

        string path = Application.dataPath + "/" + relativePath;

        if (File.Exists(path)) return true;
        else return false;
    }
}

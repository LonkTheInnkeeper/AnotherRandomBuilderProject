using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveLoadSystem
{
    static IDataService dataService = new JsonDataService();

    public static void SaveMapResources(List<ResourceData> resourceDatas)
    {
        if (dataService.SaveData("Map/MapData/Resources.json", resourceDatas))
        {
            return;
        }
        else
        {
            Debug.LogError("Could not save file");
        }
    }

    public static List<ResourceData> LoadMapResources()
    {
        string path = "Map/MapData/Resources.json";

        if (File.Exists(Application.dataPath + "/" + path))
        {
            List<ResourceData> resources = new List<ResourceData>();
            resources = dataService.LoadData<List<ResourceData>>(path);
            return resources;
        }
        else
        {
            Debug.Log("Null data");
            return null;
        };
    }

    public static List<string> LoadStringList(string path)
    {
        if (File.Exists(Application.dataPath + "/" + path))
        {
            List<string> strings = new List<string>();
            strings = dataService.LoadData<List<string>>(path);
            return strings;
        }
        else
        {
            Debug.Log("Null data");
            return null;
        };
    }

    public static void SaveStringList(List<string> list, string path)
    {
        if (dataService.SaveData(path, list))
        {
            return;
        }
        else
        {
            Debug.LogError("Could not save file");
        }
    }
}

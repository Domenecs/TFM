using System.IO;
using UnityEngine;

public class LoadSaveManager : MonoBehaviour {


    public static LoadSaveManager Instance { get;private set; }

    private static string filePath => Application.persistentDataPath + "/playerProgress.json";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }


    




    public  void SaveProgress(Unlockables data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    public  Unlockables LoadProgress()
    {
        string json = "";
        if (File.Exists(filePath))
        {
            json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<Unlockables>(json);
        }
        else
        {
            //If it's the first time create a file with basic data.
            Unlockables currentProgress = new Unlockables();
            SaveProgress(currentProgress);
        }
        json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<Unlockables>(json);

    }
}

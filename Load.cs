using System.Collections;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class Load : MonoBehaviour
{
    [SerializeField] string fileName;
    [SerializeField] private SaveClass objectsList;
    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.streamingAssetsPath, fileName + ".json");
    }

    private void Start()
    {
        LoadEventsListData();
    }

    public void SaveEventsListData()
    {
        string json = JsonConvert.SerializeObject(objectsList, Formatting.Indented);
        File.WriteAllText(savePath, json);
        Debug.Log($"Saved to {savePath}");
    }

    public void LoadEventsListData()
    {
        StartCoroutine(LoadCorut());
    }

    IEnumerator LoadCorut()
    {
        string json;

        if (savePath.Contains("://") || savePath.Contains(":///"))
        {
            UnityWebRequest www = UnityWebRequest.Get(savePath);
            yield return www.SendWebRequest();
            json = www.downloadHandler.text;
        }
        else
        {
            json = File.ReadAllText(savePath);
        }
        objectsList = JsonConvert.DeserializeObject<SaveClass>(json);

        Debug.Log($"Loaded from {savePath}");
        Debug.Log($"Result: {json}");
    }
}

[System.Serializable]
public class SaveClass
{
    public GameObject[] objects;
}

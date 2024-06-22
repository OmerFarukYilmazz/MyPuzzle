using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonController : MonoBehaviour
{    
    public LevelSystemManager _registeredLevel = new LevelSystemManager();
    //public LevelSystemManager _retryLevel = new LevelSystemManager();
    // Start is called before the first frame update
    //public string Jsonnn;
    public static string directory = "/Saves/";
    
    void Start()
    {
        
    }
    
    public void JsonSave(LevelSystemManager _level)
    {
        // 1. merthod -> to our computer
        /*string Jsonnn = JsonUtility.ToJson(_level);        
        File.WriteAllText(Application.dataPath + "Saves/UsersInfo.json", Jsonnn);*/

        // 2. merthod -> to our device
        string dir = Application.persistentDataPath + directory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        string JsonnRoad = JsonUtility.ToJson(_level);
        File.WriteAllText(dir + "UsersInfo.json", JsonnRoad);
    }
    public void JsonLoad()
    {
        /*LevelSystemManager temp = new LevelSystemManager();
        temp = JsonUtility.FromJson<LevelSystemManager>(Jsonnn);
        for(int i = 0; i < arrayX.Length; i++)
        {
            Debug.Log(temp._arrayX[i]);
        }*/
        string JsonnRoad = (Application.persistentDataPath + directory + "UsersInfo.json");
        if (File.Exists(JsonnRoad))
        {
            string JsonnRead = File.ReadAllText(JsonnRoad);
            _registeredLevel = JsonUtility.FromJson<LevelSystemManager>(JsonnRead);

        }
        else
        {
            Debug.Log("error");
        }
    }
    public void JsonSaveFirst(LevelSystemManager _level)
    {
        string dir = Application.persistentDataPath + directory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        string JsonnRoad = JsonUtility.ToJson(_level);        
        File.WriteAllText(dir + "UsersInfoFirst.json", JsonnRoad);
    }
    public void JsonLoadFirst()
    {       
        string JsonnRoad = (Application.persistentDataPath + directory +"UsersInfoFirst.json");
        if (File.Exists(JsonnRoad))
        {
            string JsonnRead = File.ReadAllText(JsonnRoad);
            _registeredLevel = JsonUtility.FromJson<LevelSystemManager>(JsonnRead);

        }
        else
        {
            Debug.Log("error");
        }
    }
    public void JsonSaveShape(LevelSystemManager _level)
    {
        string dir = Application.persistentDataPath + directory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        string JsonnRoad = JsonUtility.ToJson(_level);
        File.WriteAllText(dir + "UsersInfoShape.json", JsonnRoad);
    }
    public void JsonLoadShape()
    {
        string JsonnRoad = (Application.persistentDataPath + directory + "UsersInfoShape.json");
        if (File.Exists(JsonnRoad))
        {
            string JsonnRead = File.ReadAllText(JsonnRoad);
            _registeredLevel = JsonUtility.FromJson<LevelSystemManager>(JsonnRead);

        }
        else
        {
            Debug.Log("error");
        }
    }
    public void JsonSaveQuestion(LevelSystemManager _level)
    {
        string dir = Application.persistentDataPath + directory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        string JsonnRoad = JsonUtility.ToJson(_level);
        File.WriteAllText(dir + "UsersInfoQuestion.json", JsonnRoad);
    }
    public void JsonLoadQuestion()
    {
        string JsonnRoad = (Application.persistentDataPath + directory + "UsersInfoQuestion.json");
        if (File.Exists(JsonnRoad))
        {
            string JsonnRead = File.ReadAllText(JsonnRoad);
            _registeredLevel = JsonUtility.FromJson<LevelSystemManager>(JsonnRead);

        }
        else
        {
            Debug.Log("error");
        }
    }


}

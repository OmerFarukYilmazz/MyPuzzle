using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class GameData 
{
    GameManager _gameManager;
    //public List<Vector2> _vectorlist;
    public List<Vector2> _vector2list = new List<Vector2>();
    public List<Vector2> _vectortemp = new List<Vector2>();
    public string _vectorlist;

    public int[] arrayX;
    public int[] arrayY;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        _gameManager = Object.FindObjectOfType<GameManager>();
    }
    // Update is called once per frame
    public void saveData()
    {









        //var json = JsonConvert.SerializeObject(_vector2list); // To Serialise
        //var myVector3s = JsonConvert.DeserializeObject<List<Vector3>>(json); // To Deserialise
        /*_vectorlist = JsonConvert.SerializeObject(_gameManager._vector2positionlist,
           new JsonSerializerSettings()
           {
               ReferenceLoopHandling = ReferenceLoopHandling.Ignore
           });

        

        _vectortemp = JsonConvert.DeserializeObject<List<Vector2>>(_vectorlist);
        
        foreach (var x in _vectortemp)
        {
            Debug.Log(x.ToString());            
        }
        print("saveData");
        */


    }
    public void updateData()
    {

        /*var _vectorlist = JsonConvert.SerializeObject(_vector2list,
           new JsonSerializerSettings()
           {
               ReferenceLoopHandling = ReferenceLoopHandling.Ignore
           });

        foreach (var x in _vector2list)
        {
            Debug.Log(x.ToString());
        }*/


    }
    public void printData()
    {
        /*foreach (var x in _vector2list)
        {
            Debug.Log(x.ToString());
        }*/
    }
    public void loadData()
    {
       

        /*print("loadData");
        //_vectortemp = JsonConvert.DeserializeObject<List<Vector2>>(_vectorlist);
        foreach (var x in _vectorlist)
        {
            Debug.Log(x.ToString());
            print("22");
        }*/
        
    }
}

public class PositionBlock
{
    
    
}
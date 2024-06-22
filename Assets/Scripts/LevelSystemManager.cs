using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSystemManager
{

    /*public static LevelSystemManager instance;
    public int activeLevel;*/
    public int[] _arrayX;
    public int[] _arrayY;
    public int[] _arrayZ;
    // Start is called before the first frame update
    public LevelSystemManager()
    {

    }
   
    public LevelSystemManager(int[] arrayX,int[] arrayY)
    {
        this._arrayX = arrayX;
        this._arrayY = arrayY;
        
    }

    /*private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }*/
}
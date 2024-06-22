using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
[System.Serializable]
public class JsonTest : MonoBehaviour
{
    GameManager _gameManager;
    // JSON'a dahil olur: tek boyutlu array desteklenir ve de�i�ken public
    public int[] array = new int[2] { 11, 13 };

    // JSON'a dahil olur: temel Unity struct'lar� desteklenir ve
    // de�i�ken private olmas�na ra�men SerializeField attribute'�ne sahip
    [SerializeField]
    private Vector3 vektor = new Vector3(1f, 0f, 2f);
    
    

    // JSON'a dahil olmaz: primitive t�rler desteklenir ancak de�i�ken private
    private float sayi = 3.14f;

    // JSON'a dahil olmaz: Dictionary desteklenmez ve de�i�ken private
    public Dictionary<string, int> dictionary = new Dictionary<string, int>();
    
    // JSON'a dahil olmaz: string desteklenir ve de�i�ken public ancak de�i�ken
    // System.NonSerialized attribute'�ne sahip
    [System.NonSerialized]
    public string yazi = "Test";
    public List<int> _list = new List<int>();
    
    public List<Vector3> _vector2list = new List<Vector3>();

    private void Awake()
    {
        _gameManager = Object.FindObjectOfType<GameManager>();
        _list.Add(1);
        _list.Add(2);
        _list.Add(3);
        _vector2list.Add(new Vector2(1f, 2f));
        _vector2list.Add(new Vector2(2f, 3f));
        _vector2list.Add(new Vector2(5f, 1f));
    }
    public void SavetoJson()
    {
        
    }
    private void Start()
    {
        // Yeni bir JSONTest objesi olu�tur ve bunu JSON'a �evir
        string json = JsonUtility.ToJson(new JsonTest());
        var _slist = JsonConvert.SerializeObject(_list);        
        var _vectorlist = JsonConvert.SerializeObject(_vector2list, 
            new JsonSerializerSettings()
            {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        // Olu�turulan JSON'u konsola yazd�r
        Debug.Log(json);
        Debug.Log(_slist);
        foreach (var x in _vector2list)
        {
            Debug.Log(x.ToString());
        }
        //Debug.Log(_vectorlist);
    }

}


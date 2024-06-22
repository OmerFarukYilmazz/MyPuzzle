using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
[System.Serializable]
public class JsonTest : MonoBehaviour
{
    GameManager _gameManager;
    // JSON'a dahil olur: tek boyutlu array desteklenir ve deðiþken public
    public int[] array = new int[2] { 11, 13 };

    // JSON'a dahil olur: temel Unity struct'larý desteklenir ve
    // deðiþken private olmasýna raðmen SerializeField attribute'üne sahip
    [SerializeField]
    private Vector3 vektor = new Vector3(1f, 0f, 2f);
    
    

    // JSON'a dahil olmaz: primitive türler desteklenir ancak deðiþken private
    private float sayi = 3.14f;

    // JSON'a dahil olmaz: Dictionary desteklenmez ve deðiþken private
    public Dictionary<string, int> dictionary = new Dictionary<string, int>();
    
    // JSON'a dahil olmaz: string desteklenir ve deðiþken public ancak deðiþken
    // System.NonSerialized attribute'üne sahip
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
        // Yeni bir JSONTest objesi oluþtur ve bunu JSON'a çevir
        string json = JsonUtility.ToJson(new JsonTest());
        var _slist = JsonConvert.SerializeObject(_list);        
        var _vectorlist = JsonConvert.SerializeObject(_vector2list, 
            new JsonSerializerSettings()
            {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        // Oluþturulan JSON'u konsola yazdýr
        Debug.Log(json);
        Debug.Log(_slist);
        foreach (var x in _vector2list)
        {
            Debug.Log(x.ToString());
        }
        //Debug.Log(_vectorlist);
    }

}


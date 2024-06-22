using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [HideInInspector] public bool brainEffect;
    [HideInInspector] public bool bonusEffect;
    [HideInInspector] public bool giftEffect;
    public SpriteRenderer _spriteRenderer; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        _spriteRenderer = Object.FindObjectOfType<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
   
}
